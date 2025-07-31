using Ardalis.SmartEnum;

namespace LMS.System.Domain.Enums
{
    /// <summary>
    /// Type lesson.
    /// </summary>
    public class LessonType : SmartEnum<LessonType>
    {
        /// <summary>
        /// Text.
        /// </summary>
        public static readonly LessonType Text = new("Text", 0);

        /// <summary>
        /// Text.
        /// </summary>
        public static readonly LessonType Video = new("Video", 1);

        /// <summary>
        /// Text.
        /// </summary>
        public static readonly LessonType PDF = new("PDF", 2);

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonType"/> class.
        /// </summary>
        /// <param name="name">Name lesson type.</param>
        /// <param name="value">Value lesson type enum.</param>
        private LessonType(string name, int value)
            : base(name, value)
        {
        }
    }
}
