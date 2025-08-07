using LMS.System.Domain.Enums;
using LMS.System.Domain.Model.Task;

namespace LMS.System.Domain.Model.Tests
{
    /// <summary>
    /// Test Question.
    /// </summary>
    public class TestQuestion : Entity
    {
        /// <summary>
        /// Question Text Question Test.
        /// </summary>
        public required string QuestionText { get; set; }

        /// <summary>
        /// Test Question Question Type.
        /// </summary>
        public required QuestionType QuestionType { get; set; }

        /// <summary>
        /// Assignment ID Test Question.
        /// </summary>
        public required Guid AssignmentId { get; set; }

        /// <summary>
        /// Assignment Test Question.
        /// </summary>
        public Assignment? Assignment { get; set; }

        /// <summary>
        /// Answer Options Test Question.
        /// </summary>
        public List<TestAnswerOption> AnswerOptions { get; set; } = new List<TestAnswerOption>();

        /// <summary>
        /// Test Submissions Test Question.
        /// </summary>
        public List<TestSubmission> TestSubmissions { get; set; } = new List<TestSubmission>();
    }
}
