using LMS.System.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// Course configuration.
    /// </summary>
    /// <typeparam name="Course">Course.</typeparam>
    public sealed class CourseConfiguration : EntityBaseConfiguration<Course>
    {
        /// <summary>
        /// Configure course.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("course");
            builder.HasIndex(e => e.Title).IsUnique();
            builder.Property(e => e.Title).IsRequired()
                                          .HasMaxLength(128)
                                          .HasColumnName("title");
            builder.Property(e => e.Description).HasMaxLength(256)
                                                .HasColumnName("description");
            builder.Property(e => e.IsPublished).IsRequired()
                                                .HasDefaultValue(false)
                                                .HasColumnName("is_published");
            builder.Property(e => e.CategoryId).IsRequired()
                                               .HasColumnName("category_id");
            builder.Property(e => e.InstructorId).IsRequired()
                                                 .HasColumnName("instructor_id");

            builder.HasMany(e => e.Enrollments)
                   .WithOne(e => e.Course)
                   .HasForeignKey(e => e.CourseId);
            builder.HasMany(e => e.Assignments)
                   .WithOne(e => e.Course)
                   .HasForeignKey(e => e.CourseId);
            builder.HasMany(e => e.Lessons)
                   .WithOne(e => e.Course)
                   .HasForeignKey(e => e.CourseId);
        }
    }
}
