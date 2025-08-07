using LMS.System.Domain.Enums;

namespace LMS.System.Domain.Model
{
    /// <summary>
    /// Lesson.
    /// </summary>
    public class Lesson : Entity
    {
        /// <summary>
        /// Title lesson.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Content lesson.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Course ID lesson.
        /// </summary>
        public required Guid CourseId { get; set; }

        /// <summary>
        /// Course lesson.
        /// </summary>
        public Course? Course { get; set; }

        /// <summary>
        /// Order lesson.
        /// </summary>
        public required int Order { get; set; }

        /// <summary>
        /// Lesson type.
        /// </summary>
        public required LessonType Type { get; set; }
    }
}
