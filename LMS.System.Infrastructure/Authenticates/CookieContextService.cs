using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.System.Domain.Model.Users;
using LMS.System.Infrastructure.Extension;
using LMS.System.Infrastructure.Service;
using Microsoft.AspNetCore.Http;

namespace LMS.System.Infrastructure.Authenticates
{
    /// <summary>
    /// Set and remove Cookies.
    /// </summary>
    /// <param name="tokenService">Token service project.</param>
    /// <param name="refreshTokenService">Refresh token service.</param>
    public class CookieContextService(JwtTokenService tokenService, RefreshTokenService refreshTokenService)
    {
        /// <summary>
        /// Sets access and refresh tokens in the HTTP context and persists the refresh token
        /// </summary>
        /// <param name="httpContext">Current HTTP context.</param>
        /// <param name="user">User for whom tokens are generated.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        public async Task SetTokensAsync(HttpContext httpContext, User user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(httpContext);
            ArgumentNullException.ThrowIfNull(user);

            var token = tokenService.GenerateAccessToken(user);
            var refreshToken = tokenService.GenerateRefreshToken(user);

            await refreshTokenService.UpsertAsync(refreshToken, cancellationToken);

            httpContext.AppendAccessToken(token);
            httpContext.AppendRefreshToken(refreshToken.Token);
        }

        /// <summary>
        /// Removes both tokens from HTTP context and revokes the refresh token
        /// </summary>
        /// <param name="httpContext">Current HTTP context.</param>
        /// <param name="refreshToken">Refresh token to revoke.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        public async Task RemoveTokensAsync(HttpContext httpContext, RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(httpContext);
            ArgumentNullException.ThrowIfNull(refreshToken);

            if (refreshToken.User != null)
            {
                await refreshTokenService.RemoveAsync(refreshToken, cancellationToken);
            }

            httpContext.RemoveAllTokens();
        }
    }
}
