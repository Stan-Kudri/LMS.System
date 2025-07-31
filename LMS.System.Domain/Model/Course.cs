using LMS.System.Domain.Model.Task;
using LMS.System.Domain.Model.Users;

namespace LMS.System.Domain.Model
{
    /// <summary>
    /// Course.
    /// </summary>
    public class Course : Entity
    {
        /// <summary>
        /// Title course.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Descriprion course.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Category ID course.
        /// </summary>
        public required int CategoryId { get; set; }

        /// <summary>
        /// Category course.
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// ID Instructor courses (user id).
        /// </summary>
        public required Guid InstructorId { get; set; }

        /// <summary>
        /// Instructor courses (user).
        /// </summary>
        public User? Instructor { get; set; }

        /// <summary>
        /// Is publishe courses.
        /// </summary>
        public required bool IsPublished { get; set; }

        /// <summary>
        /// Enrollments in courses.
        /// </summary>
        public List<Enrollment>? Enrollments { get; set; } = new List<Enrollment>();

        /// <summary>
        /// Lessons in courses.
        /// </summary>
        public List<Lesson>? Lessons { get; set; } = new List<Lesson>();

        /// <summary>
        /// Assignments in courses.
        /// </summary>
        public List<Assignment>? Assignments { get; set; } = new List<Assignment>();
    }
}
