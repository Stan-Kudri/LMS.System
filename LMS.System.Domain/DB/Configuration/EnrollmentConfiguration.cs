using LMS.System.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB
{
    /// <summary>
    /// Enrollment configuration.
    /// </summary>
    /// <typeparam name="Enrollment">Enrollment.</typeparam>
    public sealed class EnrollmentConfiguration : EntityBaseConfiguration<Enrollment>
    {
        /// <summary>
        /// Configure enrollment.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("enrollement");
            builder.Property(e => e.Completed).IsRequired()
                                              .HasDefaultValue(false)
                                              .HasColumnName("is_completed");
            builder.Property(e => e.StudentId).IsRequired()
                                              .HasColumnName("student_id");
            builder.Property(e => e.CourseId).IsRequired()
                                             .HasColumnName("course_id");
            builder.Property(e => e.EnrollmentDate).IsRequired()
                                                   .HasColumnName("enrollement_date")
                                                   .ValueGeneratedOnAdd()
                                                   .HasDefaultValue(DateTime.UtcNow);

            builder.HasOne(e => e.Student)
                   .WithMany(e => e.Enrollments)
                   .HasForeignKey(e => e.StudentId);
        }
    }
}
