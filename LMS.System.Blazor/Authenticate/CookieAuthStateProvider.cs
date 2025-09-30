using System.Security.Claims;
using global::System.Security.Claims;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieAuthStateProvider"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">Provides access to the current HTTP context for reading/writing authentication cookies.</param>
        /// <param name="authService">Authentication service handling core authentication logic and token operations.</param>
        public CookieAuthStateProvider(IHttpContextAccessor httpContextAccessor, AuthService authService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        /// <summary>
        /// Gets the current authentication state from cookies.
        /// </summary>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }

            // Try to get access token from cookie
            var accessToken = httpContext.Request.Cookies["accessToken"];
            var refreshToken = httpContext.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }

            try
            {
                var principal = await GetPrincipalFromTokensAsync(accessToken, refreshToken);
                return new AuthenticationState(principal);
            }
            catch
            {
                // Tokens are invalid, clear cookies
                httpContext.Response.Cookies.Delete("accessToken");
                httpContext.Response.Cookies.Delete("refreshToken");
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        /// <summary>
        /// Gets principal from access and refresh tokens.
        /// </summary>
        private async Task<ClaimsPrincipal> GetPrincipalFromTokensAsync(string accessToken, string refreshToken)
        {
            var tokenService = new JwtTokenService(
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(),
                TimeProvider.System);

            var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
            if (principal != null)
            {
                return principal;
            }

            // Try to refresh tokens
            var authResult = await _authService.RefreshTokenAsync(accessToken, refreshToken);
            if (authResult != null)
            {
                // Set new cookies
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    SetAuthCookies(httpContext.Response, authResult.AccessToken, authResult.RefreshToken);
                }

                principal = tokenService.GetPrincipalFromExpiredToken(authResult.AccessToken);
            }

            return principal ?? new ClaimsPrincipal();
        }

        /// <summary>
        /// Sets authentication cookies for the user.
        /// </summary>
        public void SetAuthCookies(HttpResponse response, string accessToken, string refreshToken)
        {
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            response.Cookies.Append("accessToken", accessToken, accessTokenOptions);
            response.Cookies.Append("refreshToken", refreshToken, refreshTokenOptions);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Clears authentication cookies (logout).
        /// </summary>
        public void ClearAuthCookies()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var refreshToken = httpContext.Request.Cookies["refreshToken"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    _ = _authService.RevokeRefreshTokenAsync(refreshToken);
                }

                httpContext.Response.Cookies.Delete("accessToken");
                httpContext.Response.Cookies.Delete("refreshToken");
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Marks the user as authenticated.
        /// </summary>
        public void MarkUserAsAuthenticated(string accessToken, string refreshToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                SetAuthCookies(httpContext.Response, accessToken, refreshToken);
            }
        }
    }
}
