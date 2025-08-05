namespace LMS.System.Domain.Model.Dates
{
    /// <summary>
    /// Default interface implementation.
    /// </summary>
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Current date.
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
