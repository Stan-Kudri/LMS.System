using System.Security.Claims;
using LMS.System.Domain.Service;
using LMS.System.Infrastructure.Authenticates;
using LMS.System.Infrastructure.Extension;
using Microsoft.AspNetCore.Components.Authorization;

namespace LMS.System.Blazor.Authenticate
{
    /// <summary>
    /// Custom authentication state provider that uses cookies for token storage.
    /// </summary>
    public class CookieAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpContext _httpContext;
        private readonly AuthService _authService;
        private readonly JwtTokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieAuthStateProvider"/> class.
        /// </summary>
        /// <param name="httpContext">DB context accessor.</param>
        /// <param name="authService">Authenticate service.</param>
        public CookieAuthStateProvider(HttpContext httpContext, AuthService authService)
        {
            _httpContext = httpContext;
            _authService = authService;

            // Initialize JWT token service with configuration
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _tokenService = new JwtTokenService(configuration, TimeProvider.System);
        }

        /// <summary>
        /// Get current authentication state from cookies.
        /// </summary>
        /// <returns>AuthenticationState.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var access = _httpContext.GetAccessToken();
                var refresh = _httpContext.GetRefreshToken();
                // Read tokens from cookies
                var accessToken = _httpContext.Request.Cookies[AuthConst.AccessToken];
                var refreshToken = _httpContext.Request.Cookies[AuthConst.RefreshToken];

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal());
                }

                // Validate tokens and create authentication state
                var principal = await GetPrincipalFromTokensAsync(accessToken, refreshToken);
                return new AuthenticationState(principal);
            }
            catch
            {
                // Clear invalid tokens and return empty principal
                ClearAuthCookies();
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        /// <summary>
        /// Extract principal from access and refresh tokens.
        /// </summary>
        /// <param name="accessToken">Access Token.</param>
        /// <param name="refreshToken">Refresh Token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<ClaimsPrincipal> GetPrincipalFromTokensAsync(string accessToken, string refreshToken)
        {
            // First try to validate the access token
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            if (principal != null && principal.Identity?.IsAuthenticated == true)
            {
                return principal;
            }

            // If access token is invalid, try to refresh using refresh token
            var authResult = await _authService.RefreshTokenAsync(accessToken, refreshToken);
            if (authResult != null)
            {
                if (_httpContext != null)
                {
                    SetAuthCookies(_httpContext, authResult.AccessToken, authResult.RefreshToken);
                }

                principal = _tokenService.GetPrincipalFromExpiredToken(authResult.AccessToken);
            }

            return principal ?? new ClaimsPrincipal();
        }

        /// <summary>
        /// Clear authentication cookies (logout).
        /// </summary>
        public void ClearAuthCookies()
        {
            if (_httpContext != null)
            {
                // Revoke refresh token from database
                var refreshToken = _httpContext.Request.Cookies[AuthConst.RefreshToken];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    _ = _authService.RevokeRefreshTokenAsync(refreshToken);
                }

                // Remove cookies from response
                _httpContext.RemoveAllTokens();
            }

            // Notify authentication state change
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Set authentication cookies in HTTP response.
        /// </summary>
        /// <param name="httpContext">HTTP request context.</param>
        /// <param name="accessToken">Access Token.</param>
        /// <param name="refreshToken">Refresh Token.</param>
        /// <exception cref="InvalidOperationException">The response has already started, cannot set cookies.</exception>
        private void SetAuthCookies(HttpContext httpContext, string accessToken, string refreshToken)
        {
            // Set cookies.
            httpContext.AppendAccessToken(accessToken);
            httpContext.AppendRefreshToken(refreshToken);

            // Notify authentication state change
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
