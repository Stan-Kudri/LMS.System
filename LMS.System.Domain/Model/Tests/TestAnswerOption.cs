namespace LMS.System.Domain.Model.Tests
{
    /// <summary>
    /// Test Answer Option entity.
    /// </summary>
    public class TestAnswerOption : Entity
    {
        /// <summary>
        /// Question Text Test Answer Option.
        /// </summary>
        public required string OptionText { get; set; }

        /// <summary>
        /// Is Correct Test Answer Option.
        /// </summary>
        public required bool IsCorrect { get; set; }

        /// <summary>
        /// Test Question ID Test Answer Option.
        /// </summary>
        public required Guid TestQuestionId { get; set; }

        /// <summary>
        /// Test Question Test Answer Option.
        /// </summary>
        public TestQuestion? TestQuestion { get; set; }
    }
}
