using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.System.Infrastructure.Authenticates.Constants
{
    /// <summary>
    /// Token cookie constants.
    /// </summary>
    public static class TokensConst
    {
        /// <summary>
        /// Name of the access token cookie.
        /// </summary>
        public static string GetTokenName => "JWTBearer";

        /// <summary>
        /// Name of the refresh token cookie.
        /// </summary>
        public static string GetRefreshTokenName => "RefreshToken";

        /// <summary>
        /// Time interval for updating access tokens.
        /// </summary>
        public static TimeSpan GetUpdateTimeToken => TimeSpan.FromMinutes(30);

        /// <summary>
        /// Creation timestamp for refresh tokens.
        /// </summary>
        public static DateTime GetDateCreateRefreshToken => DateTime.UtcNow;

        /// <summary>
        /// Expiration timestamp for refresh tokens.
        /// </summary>
        public static DateTime GetExpiresRefreshToken => DateTime.UtcNow.AddHours(24);

        /// <summary>
        /// Time interval for updating refresh tokens.
        /// </summary>
        public static TimeSpan GetUpdateTimeRefreshToken => TimeSpan.FromMinutes(60);
    }
}
