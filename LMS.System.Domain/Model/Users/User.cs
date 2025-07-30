using LMS.System.Domain.Enums;
using LMS.System.Domain.Model.Task;

namespace LMS.System.Domain.Model.Users
{
    /// <summary>
    /// User class.
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// Email.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// PasswordHash.
        /// </summary>
        public required string PasswordHash { get; set; }

        /// <summary>
        /// FirstName.
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// LastName.
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        public required UserRole UserRole { get; set; }

        /// <summary>
        /// Submissions user.
        /// </summary>
        public List<Submission>? Submissions { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        public List<Enrollment>? Enrollments { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        public List<Submission>? Role { get; set; }
    }
}
