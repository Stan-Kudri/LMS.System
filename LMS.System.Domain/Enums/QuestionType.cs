using Ardalis.SmartEnum;

namespace LMS.System.Domain.Enums
{
    /// <summary>
    /// Question Type.
    /// </summary>
    public class QuestionType : SmartEnum<QuestionType>
    {
        /// <summary>
        /// Single Choice.
        /// </summary>
        public static readonly QuestionType SingleChoice = new("Single Choice", 0);

        /// <summary>
        /// Multi Choice.
        /// </summary>
        public static readonly QuestionType MultiChoice = new("Multi Choice", 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionType"/> class.
        /// </summary>
        /// <param name="name">Name question type.</param>
        /// <param name="value">Value question type.</param>
        private QuestionType(string name, int value)
            : base(name, value)
        {
        }
    }
}
