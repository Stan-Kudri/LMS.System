using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.System.Infrastructure.Authenticates
{
    /// <summary>
    /// Constant class Authenticate.
    /// </summary>
    public static class AuthConst
    {
        /// <summary>
        /// AccessToken.
        /// </summary>
        public const string AccessToken = "accessToken";

        /// <summary>
        /// RefreshToken.
        /// </summary>
        public const string RefreshToken = "refreshToken";

        /// <summary>
        /// Expiry minute AccessToken.
        /// </summary>
        public const int AccessTokenExpiryMinute = 15;

        /// <summary>
        /// Expiry days RefreshToken.
        /// </summary>
        public const int RefreshTokenExpiryDays = 7;
    }
}
