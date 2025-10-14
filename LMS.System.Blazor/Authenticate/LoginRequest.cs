namespace LMS.System.Blazor.Authenticate
{
    /// <summary>
    /// Request for login User.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequest"/> class.
        /// Base constructor.
        /// </summary>
        public LoginRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequest"/> class.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
