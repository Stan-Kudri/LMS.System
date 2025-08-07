using LMS.System.Domain.Enums;
using LMS.System.Domain.Extension;
using LMS.System.Domain.Model.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// Assignment configuration.
    /// </summary>
    /// <typeparam name="Assignment">Assignment.</typeparam>
    public sealed class AssignmentConfiguration : EntityBaseConfiguration<Assignment>
    {
        /// <summary>
        /// Configure assignment.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("assignment");
            builder.HasIndex(e => e.Title).IsUnique();
            builder.Property(e => e.Title).IsRequired()
                                          .HasMaxLength(128)
                                          .HasColumnName("title");
            builder.Property(e => e.Description).HasMaxLength(256)
                                                .HasColumnName("description");
            builder.Property(e => e.Deadline).IsRequired()
                                             .HasColumnName("enrollement_date")
                                             .ValueGeneratedOnAdd()
                                             .HasDefaultValue(DateTime.UtcNow);
            builder.Property(e => e.CourseId).IsRequired()
                                             .HasColumnName("course_id");
            builder.Property(e => e.AssignmentType).IsRequired()
                                                   .HasColumnName("assignment_type")
                                                   .HasDefaultValue(AssignmentType.Text)
                                                   .SmartEnumNameConversion();

            builder.HasMany(p => p.TestQuestions)
                   .WithOne(p => p.Assignment)
                   .HasForeignKey(p => p.AssignmentId);
            builder.HasMany(p => p.Submissions)
                   .WithOne(p => p.Assignment)
                   .HasForeignKey(p => p.AssignmentId);
        }
    }
}
