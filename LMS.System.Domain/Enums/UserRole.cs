using Ardalis.SmartEnum;

namespace LMS.System.Domain.Enums
{
    /// <summary>
    /// User Role LMS.
    /// </summary>
    public class UserRole : SmartEnum<UserRole>
    {
        /// <summary>
        /// Adminisrator.
        /// </summary>
        public static readonly UserRole Admin = new("Administrator", 0);

        /// <summary>
        /// Teacher.
        /// </summary>
        public static readonly UserRole Teacher = new("Teacher", 1);

        /// <summary>
        /// Student.
        /// </summary>
        public static readonly UserRole Student = new("Student", 2);

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRole"/> class.
        /// Base constructor UserRole.
        /// </summary>
        /// <param name="name">Name user role.</param>
        /// <param name="value">Value user role enum.</param>
        private UserRole(string name, int value)
            : base(name, value)
        {
        }
    }
}
