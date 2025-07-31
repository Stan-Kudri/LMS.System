namespace LMS.System.Domain.Model
{
    /// <summary>
    /// Entity (Id item).
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Id item from DB.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Created item from DB.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Update item from DB.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
