using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LMS.System.Domain.Enums;

namespace LMS.System.Infrastructure.Authenticates.Model
{
    /// <summary>
    /// Request for creating or updating a User.
    /// </summary>
    public class UserRequest
    {
        private const string PasswordRegularExpres = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).+$";
        private const string NameRegularExpres = @"^[a-zA-Zа-яА-ЯёЁ\s\-']+$";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRequest"/> class with default values.
        /// </summary>
        public UserRequest()
        {
        }

        /// <summary>
        /// Email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password (for creation only).
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        [RegularExpression(PasswordRegularExpres, ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// First name.
        /// </summary>
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [RegularExpression(NameRegularExpres, ErrorMessage = "First name can only contain letters, spaces, hyphens and apostrophes.")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name.
        /// </summary>
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [RegularExpression(NameRegularExpres, ErrorMessage = "Last name can only contain letters, spaces, hyphens and apostrophes.")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// User role.
        /// </summary>
        [Required(ErrorMessage = "User role is required.")]
        public UserRole UserRole { get; set; } = UserRole.Student;
    }
}
