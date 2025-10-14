using System.Security.Claims;
using LMS.System.Domain.Service;
using Microsoft.AspNetCore.Components.Authorization;

namespace LMS.System.Blazor.Authenticate
{
    /// <summary>
    /// Custom authentication state provider that uses cookies for token storage.
    /// </summary>
    public class CookieAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthService _authService;
        private readonly JwtTokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieAuthStateProvider"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">DB Context.</param>
        /// <param name="authService">Authenticate service.</param>
        public CookieAuthStateProvider(IHttpContextAccessor httpContextAccessor, AuthService authService)
        {
            _httpContextAccessor = httpContextAccessor;
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
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }

            // Read tokens from cookies
            var accessToken = httpContext.Request.Cookies[AuthConst.AccessToken];
            var refreshToken = httpContext.Request.Cookies[AuthConst.RefreshToken];

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }

            try
            {
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
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    SetAuthCookies(httpContext.Response, authResult.AccessToken, authResult.RefreshToken);
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
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // Revoke refresh token from database
                var refreshToken = httpContext.Request.Cookies[AuthConst.RefreshToken];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    _ = _authService.RevokeRefreshTokenAsync(refreshToken);
                }

                // Remove cookies from response
                httpContext.Response.Cookies.Delete(AuthConst.AccessToken);
                httpContext.Response.Cookies.Delete(AuthConst.RefreshToken);
            }

            // Notify authentication state change
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Set authentication cookies in HTTP response.
        /// </summary>
        /// <param name="response">HttpResponse.</param>
        /// <param name="accessToken">Access Token.</param>
        /// <param name="refreshToken">Refresh Token.</param>
        /// <exception cref="InvalidOperationException">The response has already started, cannot set cookies.</exception>
        private void SetAuthCookies(HttpResponse response, string accessToken, string refreshToken)
        {
            // Check if response has already started to avoid exceptions
            if (response.HasStarted)
            {
                throw new InvalidOperationException("The response has already started, cannot set cookies.");
            }

            // Configure access token cookie options
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15),
            };

            // Configure refresh token cookie options
            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
            };

            // Append cookies to response
            response.Cookies.Append(AuthConst.AccessToken, accessToken, accessTokenOptions);
            response.Cookies.Append(AuthConst.RefreshToken, refreshToken, refreshTokenOptions);

            // Notify authentication state change
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
