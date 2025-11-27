using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LMS.System.Domain.DB;
using LMS.System.Domain.Extension;
using LMS.System.Domain.Model.Users;
using LMS.System.Infrastructure.Authenticates.Model;
using LMS.System.Infrastructure.Extension;
using LMS.System.Infrastructure.Service.Validation;
using Microsoft.EntityFrameworkCore;

namespace LMS.System.Infrastructure.Service
{
    /// <summary>
    /// Authentication service for handling user authentication and token management.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </remarks>
    /// <param name="context">Database context for accessing user data and authentication records.</param>
    /// <param name="tokenService">Service responsible for JWT token generation and validation.</param>
    /// <param name="passwordHasher">Service handling password hashing and verification.</param>
    /// <param name="timeProvider">Time provider abstraction for time-based operations.</param>
    public class AuthService(AppDbContext context, JwtTokenService tokenService, PasswordHasher passwordHasher, TimeProvider timeProvider, IValidationService validationService)
    {
        private const string ErrorEmailAddressExist = "A user with that email address already exists.";

        private readonly AppDbContext _context = context;
        private readonly JwtTokenService _tokenService = tokenService;
        private readonly PasswordHasher _passwordHasher = passwordHasher;
        private readonly TimeProvider _timeProvider = timeProvider;
        private readonly IValidationService _validationService = validationService;

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRequest">User registration data.</param>
        /// <returns>User entity if successful, null if email exists.</returns>
        public async Task<(User? User, RegistrationResult? ResultErrors)> RegisterUserAsync(UserRequest userRequest)
        {
            var result = new RegistrationResult();

            var validationResult = _validationService.Validate(userRequest);
            if (!validationResult.IsValid)
            {
                result.Errors.AddRange(validationResult.Errors!);
                return (null, result);
            }

            var ress = await _context.Users.AnyAsync(u => u.Email == userRequest.Email);
            if (await _context.Users.AnyAsync(u => u.Email == userRequest.Email))
            {
                result.Errors.Add(ErrorEmailAddressExist);
                return (null, result);
            }

            var user = new User
            {
                Email = userRequest.Email,
                PasswordHash = _passwordHasher.Generate(userRequest.Password!),
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                UserRole = userRequest.UserRole,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return (user, null);
        }

        /// <summary>
        /// Authenticates a user and generates tokens.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>Auth result with tokens and user info.</returns>
        public async Task<AuthResult?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email).ConfigureAwait(false);
            if (user == null || !_passwordHasher.Verification(password, user.PasswordHash!))
            {
                return null;
            }

            // Generate tokens
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user);

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                User = user,
            };
        }

        /// <summary>
        /// Refreshes the access token using a refresh token.
        /// </summary>
        /// <param name="accessToken">Expired access token.</param>
        /// <param name="refreshToken">Valid refresh token.</param>
        /// <returns>New auth result or null if invalid.</returns>
        public async Task<AuthResult?> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = _tokenService.ValidateAccessToken(accessToken);
            var userId = principal.TryParseIdUserClaimsPrincipal();

            var storedRefreshToken = await _context.RefreshTokens.Include(rt => rt.User)
                                                                 .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId)
                                                                 .ConfigureAwait(false);

            if (storedRefreshToken == null || !storedRefreshToken.IsActive)
            {
                return null;
            }

            var user = storedRefreshToken.User!;

            // Generate new tokens
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user);

            // Revoke old refresh token and save new one
            storedRefreshToken.IsRevoked = true;
            _context.RefreshTokens.Add(newRefreshToken);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            // Clean up expired tokens
            await CleanUpExpiredTokensAsync().ConfigureAwait(false);

            return new AuthResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                User = storedRefreshToken.User!,
            };
        }

        /// <summary>
        /// Revokes a refresh token (logout).
        /// </summary>
        /// <param name="refreshToken">Refresh token to revoke.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken)
                                                    .ConfigureAwait(false);

            if (token != null)
            {
                token.IsRevoked = true;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Cleans up expired refresh tokens from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CleanUpExpiredTokensAsync()
        {
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            var expiredTokens = _context.RefreshTokens.Where(rt => rt.Expires < now);
            _context.RefreshTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets user by ID.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User entity or null.</returns>
        public async Task<User?> GetUserByIdAsync(Guid userId)
            => await _context.Users.FirstOrDefaultAsync(u => u.Id == userId)
                                   .ConfigureAwait(false);
    }
}
