using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.System.Domain.Enums;

namespace LMS.System.Domain.Model.Users
{
    /// <summary>
    /// Request for creating or updating a User.
    /// </summary>
    public class UserRequest
    {
        private const string PasswordRegularExpres = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
        private const string NameRegularExpres = @"^[a-zA-Zа-яА-ЯёЁ\s\-']+$";

        /// <summary>
        /// Email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
        public required string Email { get; set; }

        /// <summary>
        /// Password (for creation only).
        /// </summary>
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        [RegularExpression(PasswordRegularExpres, ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.")]
        public string? Password { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [RegularExpression(NameRegularExpres, ErrorMessage = "First name can only contain letters, spaces, hyphens and apostrophes.")]
        public required string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [RegularExpression(NameRegularExpres, ErrorMessage = "Last name can only contain letters, spaces, hyphens and apostrophes.")]
        public required string LastName { get; set; }

        /// <summary>
        /// User role.
        /// </summary>
        [Required(ErrorMessage = "User role is required.")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid user role.")]
        public required UserRole UserRole { get; set; }
    }
}
