using LMS.System.Domain.Model.Task;

namespace LMS.System.Domain.Model.Tests
{
    /// <summary>
    /// Test Submission.
    /// </summary>
    public class TestSubmission : Entity
    {
        /// <summary>
        /// Test Question ID Test Submission.
        /// </summary>
        public required Guid TestQuestionId { get; set; }

        /// <summary>
        /// Test Question Test Submission.
        /// </summary>
        public TestQuestion? TestQuestion { get; set; }

        /// <summary>
        /// Submission ID Test Submission.
        /// </summary>
        public required Guid SubmissionId { get; set; }

        /// <summary>
        /// Submission Test Submission.
        /// </summary>
        public Submission? Submission { get; set; }

        /// <summary>
        /// Test Answer Option ID Test Submission.
        /// </summary>
        public required Guid SelectedOptionId { get; set; }

        /// <summary>
        /// Test Answer Option Test Submission.
        /// </summary>
        public TestAnswerOption? SelectedOption { get; set; }
    }
}
