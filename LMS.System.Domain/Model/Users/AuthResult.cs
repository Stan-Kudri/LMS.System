using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.System.Domain.Model.Users
{
    /// <summary>
    /// Authentication result containing tokens and user information.
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// JWT (JSON Web Token) used for authenticating API requests.
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// Token used to obtain a new access token when it expires.
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// Authenticated user's profile information.
        /// </summary>
        public required User User { get; set; }
    }
}
