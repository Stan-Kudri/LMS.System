using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.System.Infrastructure.Authenticates.Model
{
    /// <summary>
    /// Represents the result of a user registration operation.
    /// </summary>
    public class RegistrationResult
    {
        /// <summary>
        /// Indicates whether the registration was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Contains validation errors if registration failed.
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Specifies the URL to redirect to after registration.
        /// </summary>
        public string RedirectUrl { get; set; } = "/login";
    }
}
