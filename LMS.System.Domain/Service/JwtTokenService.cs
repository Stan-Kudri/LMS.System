using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LMS.System.Domain.Model.Users;
using LMS.System.Infrastructure.Authenticates;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LMS.System.Domain.Service
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
        /// <param name="user">User entity.</param>
        /// <returns>Access token string.</returns>
        public string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.GivenName, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.Role, user.UserRole.Name),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = _timeProvider.GetUtcNow().UtcDateTime.AddMinutes(AuthConst.AccessTokenExpiryMinute),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generates a refresh token.
        /// </summary>
        /// <returns>Refresh token string.</returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Gets the principal from an expired access token.
        /// </summary>
        /// <param name="token">Expired access token.</param>
        /// <returns>Claims principal or null if invalid.</returns>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }

        /// <summary>
        /// Gets the expiration time for refresh tokens.
        /// </summary>
        /// <returns>Expiration datetime.</returns>
        public DateTime GetRefreshTokenExpiryTime()
            => _timeProvider.GetUtcNow().UtcDateTime.AddDays(AuthConst.AccessTokenExpiryMinute);
    }
}
