using Ardalis.SmartEnum;

namespace LMS.System.Domain.Enums
{
    /// <summary>
    /// Assignment Type.
    /// </summary>
    public class AssignmentType : SmartEnum<AssignmentType>
    {
        /// <summary>
        /// Text.
        /// </summary>
        public static readonly AssignmentType Test = new("Test", 0);

        /// <summary>
        /// Text.
        /// </summary>
        public static readonly AssignmentType FileUpload = new("File Upload", 1);

        /// <summary>
        /// Text.
        /// </summary>
        public static readonly AssignmentType Text = new("Text", 2);

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentType"/> class.
        /// </summary>
        /// <param name="name">Name assignment type.</param>
        /// <param name="value">Value Assignment type enum.</param>
        private AssignmentType(string name, int value)
            : base(name, value)
        {
        }
    }
}
