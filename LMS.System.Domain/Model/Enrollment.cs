using LMS.System.Domain.Model.Users;

namespace LMS.System.Domain.Model
{
    /// <summary>
    /// Enrollment.
    /// </summary>
    public class Enrollment : Entity
    {
        /// <summary>
        /// Enrollment Date.
        /// </summary>
        public required DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Is completed studies.
        /// </summary>
        public required bool Completed { get; set; }

        /// <summary>
        /// Student Id Enrollment.
        /// </summary>
        public required Guid StudentId { get; set; }

        /// <summary>
        /// Student Enrollment.
        /// </summary>
        public User? Student { get; set; }

        /// <summary>
        /// Course Id Enrollment.
        /// </summary>
        public required Guid CourseId { get; set; }

        /// <summary>
        /// Course Enrollment.
        /// </summary>
        public Course? Course { get; set; }
    }
}
