using LMS.System.Domain.Model.Tests;
using LMS.System.Domain.Model.Users;

namespace LMS.System.Domain.Model.Task
{
    /// <summary>
    /// Submission.
    /// </summary>
    public class Submission : Entity
    {
        /// <summary>
        /// AnswerText Submission.
        /// </summary>
        public required string AnswerText { get; set; }

        /// <summary>
        /// File Path Submission.
        /// </summary>
        public required string FilePath { get; set; }

        /// <summary>
        /// Grade Submission.
        /// </summary>
        public required byte Grade { get; set; }

        /// <summary>
        /// Feedback Submission.
        /// </summary>
        public required string Feedback { get; set; }

        /// <summary>
        /// Date time Submission.
        /// </summary>
        public required DateTime SubmittedAt { get; set; }

        /// <summary>
        /// Assignment Id Submission.
        /// </summary>
        public required Guid AssignmentId { get; set; }

        /// <summary>
        /// Assignment Id Submission.
        /// </summary>
        public Assignment? Assignment { get; set; }

        /// <summary>
        /// Student Id Submission.
        /// </summary>
        public required Guid StudentId { get; set; }

        /// <summary>
        /// Student Submission.
        /// </summary>
        public User? Student { get; set; }

        /// <summary>
        /// TestSubmission List.
        /// </summary>
        public List<TestSubmission> TestSubmissions { get; set; } = new List<TestSubmission>();
    }
}
