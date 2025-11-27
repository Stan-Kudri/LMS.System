using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LMS.System.Infrastructure.Authenticates.Constants;
using Microsoft.AspNetCore.Http;

namespace LMS.System.Infrastructure.Extension
{
    /// <summary>
    /// Extension cookies.
    /// </summary>
    public static class CookiesExtension
    {
        private static CookieOptions CreateTokenCookieOptions()
        {
            return new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(AuthConst.AccessTokenExpiryMinute),
            };
        }

        private static CookieOptions CreateRefreshTokenCookieOptions()
        {
            return new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(AuthConst.RefreshTokenExpiryDays),
            };
        }

        /// <summary>
        /// Add a new cookie AccessToken.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        /// <param name="token">Short-lived token for accessing protected resources.</param>
        public static void AppendAccessToken(this HttpContext httpContext, string token)
            => httpContext.Response.Cookies.Append(AuthConst.AccessToken, token, CreateTokenCookieOptions());

        /// <summary>
        /// Add a new cookie RefreshToken.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        /// <param name="token">Long-lived token for obtaining new access tokens.</param>
        public static void AppendRefreshToken(this HttpContext httpContext, string token)
            => httpContext.Response.Cookies.Append(AuthConst.RefreshToken, token, CreateRefreshTokenCookieOptions());

        /// <summary>
        /// Read cookie access token.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        /// <returns>Access token.</returns>
        public static string? GetAccessToken(this HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue(AuthConst.AccessToken, out string? token);
            return token;
        }

        /// <summary>
        /// Read cookie refresh token.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        /// <returns>Refresh token.</returns>
        public static string? GetRefreshToken(this HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue(AuthConst.RefreshToken, out string? token);
            return token;
        }

        /// <summary>
        /// Remove all cookies.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        public static void RemoveAllTokens(this HttpContext httpContext)
        {
            RemoveToken(httpContext);
            RemoveRefreshToken(httpContext);
        }

        /// <summary>
        /// Remove all cookie RefreshToken.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        public static void RemoveToken(this HttpContext httpContext)
            => httpContext.Response.Cookies.Delete(AuthConst.AccessToken);

        /// <summary>
        /// Remove all cookie RefreshToken.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        public static void RemoveRefreshToken(this HttpContext httpContext)
            => httpContext.Response.Cookies.Delete(AuthConst.RefreshToken);
    }
}
