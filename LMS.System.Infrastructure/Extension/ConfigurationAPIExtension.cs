using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LMS.System.Infrastructure.Extension
{
    /// <summary>
    /// Getter Secret, Issuer and Audience key.
    /// </summary>
    public static class ConfigurationAPIExtension
    {
        /// <summary>
        /// Get secret key app.
        /// </summary>
        /// <param name="configuration">appsettings config.</param>
        /// <returns>Secret key.</returns>
        /// <exception cref="InvalidOperationException">JWT Secret is not configured.</exception>
        public static string GetSecretKey(this IConfiguration configuration)
            => configuration["Jwt:Secret"]
               ?? throw new InvalidOperationException("JWT Secret is not configured.");

        /// <summary>
        /// Get issuer key app.
        /// </summary>
        /// <param name="configuration">appsettings config.</param>
        /// <returns>Issuer key.</returns>
        /// <exception cref="InvalidOperationException">JWT Issuer is not configured.</exception>
        public static string GetIssuer(this IConfiguration configuration)
            => configuration["Jwt:Issuer"]
               ?? throw new InvalidOperationException("JWT Issuer is not configured.");

        /// <summary>
        /// Get audience key app.
        /// </summary>
        /// <param name="configuration">appsettings config.</param>
        /// <returns>Audience key.</returns>
        /// <exception cref="InvalidOperationException">JWT Audience is not configured.</exception>
        public static string GetAudience(this IConfiguration configuration)
            => configuration["Jwt:Audience"]
               ?? throw new InvalidOperationException("JWT Audience is not configured.");
    }
}
