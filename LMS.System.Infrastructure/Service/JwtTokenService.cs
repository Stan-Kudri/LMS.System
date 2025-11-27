using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LMS.System.Domain.Extension;
using LMS.System.Domain.Model.Users;
using LMS.System.Infrastructure.Authenticates.Constants;
using LMS.System.Infrastructure.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LMS.System.Infrastructure.Service
{
    /// <summary>
    /// Service for generating and validating JWT tokens.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="JwtTokenService"/> class.
    /// </remarks>
    /// <param name="configuration">Configuration.</param>
    /// <param name="timeProvider">Time provider.</param>
    public class JwtTokenService(IConfiguration configuration, TimeProvider timeProvider)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly TimeProvider _timeProvider = timeProvider;

        /// <summary>
        /// Generates an access token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>The generated JWT access token.</returns>
        public string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSecretKey());

            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Role, user.UserRole.Name),
            new(JwtRegisteredClaimNames.Email, user.Email),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = _timeProvider.GetUtcNow().DateTime.AddMinutes(AuthConst.AccessTokenExpiryMinute),
                Issuer = _configuration.GetIssuer(),
                Audience = _configuration.GetAudience(),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generates a new refresh token.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>A new refresh token.</returns>
        public RefreshToken GenerateRefreshToken(User user)
        {
            var randomBytes = new byte[64];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);

            return new RefreshToken
            {
                UserId = user.Id,
                Token = Convert.ToBase64String(randomBytes),
                Expires = _timeProvider.GetUtcNow().DateTime.AddDays(AuthConst.RefreshTokenExpiryDays),
            };
        }

        /// <summary>
        /// Validates the access token and returns the principal if valid.
        /// </summary>
        /// <param name="token">The access token to validate.</param>
        /// <returns>The ClaimsPrincipal if token is valid, null otherwise.</returns>
        public ClaimsPrincipal? ValidateAccessToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration.GetSecretKey());

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration.GetIssuer(),
                    ValidateAudience = true,
                    ValidAudience = _configuration.GetAudience(),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the user ID from an expired access token (useful for refresh token flow).
        /// </summary>
        /// <param name="token">The expired access token.</param>
        /// <returns>The user ID if token can be read, null otherwise.</returns>
        public Guid? GetUserIdFromExpiredToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

                return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId)
                       ? userId
                       : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Finds a valid refresh token for a user.
        /// </summary>
        /// <param name="user">The user to search for.</param>
        /// <param name="refreshToken">The refresh token value.</param>
        /// <returns>The refresh token if found and valid, null otherwise.</returns>
        public RefreshToken? FindValidRefreshToken(User user, string refreshToken)
            => user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken
                                                       && !rt.IsRevoked
                                                       && rt.Expires > _timeProvider.GetUtcNow().DateTime);
    }
}
