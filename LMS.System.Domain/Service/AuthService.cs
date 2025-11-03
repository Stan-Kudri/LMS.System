using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LMS.System.Domain.DB;
using LMS.System.Domain.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace LMS.System.Domain.Service
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
    public class AuthService(AppDbContext context, JwtTokenService tokenService, PasswordHasher passwordHasher, TimeProvider timeProvider)
    {
        private readonly AppDbContext _context = context;
        private readonly JwtTokenService _tokenService = tokenService;
        private readonly PasswordHasher _passwordHasher = passwordHasher;
        private readonly TimeProvider _timeProvider = timeProvider;

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRequest">User registration data.</param>
        /// <returns>User entity if successful, null if email exists.</returns>
        public async Task<User?> RegisterAsync(UserRequest userRequest)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userRequest.Email).ConfigureAwait(false))
            {
                return null;
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

            return user;
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
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Save refresh token
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expires = _tokenService.GetRefreshTokenExpiryTime(),
                UserId = user.Id,
                IsRevoked = false,
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
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
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            if (principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString)
            {
                return null;
            }

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return null;
            }

            var storedRefreshToken = await _context.RefreshTokens.Include(rt => rt.User)
                                                                 .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId)
                                                                 .ConfigureAwait(false);

            if (storedRefreshToken == null || !storedRefreshToken.IsActive)
            {
                return null;
            }

            // Generate new tokens
            var newAccessToken = _tokenService.GenerateAccessToken(storedRefreshToken.User!);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Revoke old refresh token and save new one
            storedRefreshToken.IsRevoked = true;

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                Expires = _tokenService.GetRefreshTokenExpiryTime(),
                UserId = userId,
                IsRevoked = false,
            };

            _context.RefreshTokens.Add(newRefreshTokenEntity);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            // Clean up expired tokens
            await CleanUpExpiredTokensAsync().ConfigureAwait(false);

            return new AuthResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
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
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId)
                                       .ConfigureAwait(false);
        }
    }
}
