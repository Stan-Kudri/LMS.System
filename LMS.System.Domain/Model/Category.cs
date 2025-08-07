namespace LMS.System.Domain.Model
{
    /// <summary>
    /// Category.
    /// </summary>
    public class Category : Entity
    {
        /// <summary>
        /// Name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Courses.
        /// </summary>
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
