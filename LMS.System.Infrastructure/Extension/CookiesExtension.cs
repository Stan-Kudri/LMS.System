using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LMS.System.Infrastructure.Authenticates;
using Microsoft.AspNetCore.Http;

namespace LMS.System.Infrastructure.Extension
{
    /// <summary>
    /// Extension cookies.
    /// </summary>
    public static class CookiesExtension
    {
        private static readonly CookieOptions _optionToken = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(AuthConst.AccessTokenExpiryMinute),
        };

        private static readonly CookieOptions _optionRefreshToken = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(AuthConst.RefreshTokenExpiryDays),
        };

        /// <summary>
        /// Add a new cookie.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        /// <param name="token">Short-lived token for accessing protected resources.</param>
        public static void AppendAccessToken(this HttpContext httpContext, string token)
            => httpContext.Response.Cookies.Append(AuthConst.AccessToken, token, _optionToken);

        /// <summary>
        /// Add a new cookie.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        /// <param name="token">Long-lived token for obtaining new access tokens.</param>
        public static void AppendRefreshToken(this HttpContext httpContext, string token)
            => httpContext.Response.Cookies.Append(AuthConst.RefreshToken, token, _optionRefreshToken);

        /// <summary>
        /// Read cookie access token.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        public static string GetAccessToken(this HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue(AuthConst.AccessToken, out string token);
            return token;
        }

        /// <summary>
        /// Read cookie refresh token.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        public static string GetRefreshToken(this HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue(AuthConst.RefreshToken, out string token);
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
