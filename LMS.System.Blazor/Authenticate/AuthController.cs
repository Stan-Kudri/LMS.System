using LMS.System.Blazor.Components.Pages;
using LMS.System.Domain.Service;
using LMS.System.Infrastructure.Authenticates;
using LMS.System.Infrastructure.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LMS.System.Blazor.Authenticate
{
    /// <summary>
    /// Authentication Controller.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">Service.</param>
        public AuthController(AuthService authService) => _authService = authService;

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="request">Request login.</param>
        /// <returns>A <see cref="Task{TResult}"/> Representing the result of the asynchronous operation.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            // Add model.
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid request data",
                    Errors = ModelState.Values.SelectMany(request => request.Errors),
                });
            }

            // Is Required email and passwprd.
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Success = false, Message = "Email and password are required" });
            }

            var result = await _authService.LoginAsync(request.Email, request.Password);
            if (result == null)
            {
                return Unauthorized(new { Success = false, Message = "Invalid email or password" });
            }

            // Set cookies.
            HttpContext.AppendAccessToken(result.AccessToken);
            HttpContext.AppendRefreshToken(result.RefreshToken);

            return Ok(new { Success = true, User = new { result.User.Id, result.User.Email, result.User.FirstName } });
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.RemoveAllTokens();
            return Ok();
        }
    }
}
