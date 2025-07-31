using LMS.System.Domain.Enums;
using LMS.System.Domain.Model.Tests;

namespace LMS.System.Domain.Model.Task
{
    /// <summary>
    /// Assignment.
    /// </summary>
    public class Assignment : Entity
    {
        /// <summary>
        /// Title assignment.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Description assignment.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Deadline assignment.
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Course Id assignment.
        /// </summary>
        public required Guid CourseId { get; set; }

        /// <summary>
        /// Course assignment.
        /// </summary>
        public Course? Course { get; set; }

        /// <summary>
        /// Type assignment.
        /// </summary>
        public required AssignmentType AssignmentType { get; set; }

        /// <summary>
        /// Tests Question.
        /// </summary>
        public List<TestQuestion>? TestQuestions { get; set; } = new List<TestQuestion>();

        /// <summary>
        /// Submissions.
        /// </summary>
        public List<Submission>? Submissions { get; set; } = new List<Submission>();
    }
}
