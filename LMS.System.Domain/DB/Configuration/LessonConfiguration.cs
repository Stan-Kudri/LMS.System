using LMS.System.Domain.Enums;
using LMS.System.Domain.Extension;
using LMS.System.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// Lesson configuration.
    /// </summary>
    /// <typeparam name="Lesson">Lesson.</typeparam>
    public sealed class LessonConfiguration : EntityBaseConfiguration<Lesson>
    {
        /// <summary>
        /// Configure lesson.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("lesson");
            builder.HasIndex(e => e.Title).IsUnique();
            builder.Property(e => e.Title).IsRequired()
                                          .HasMaxLength(128)
                                          .HasColumnName("title");
            builder.Property(e => e.Content).IsRequired()
                                            .HasColumnName("content");
            builder.Property(e => e.Order).IsRequired()
                                          .HasColumnName("order")
                                          .ValueGeneratedOnAdd();
            builder.Property(e => e.CourseId).IsRequired()
                                             .HasColumnName("course_id");
            builder.Property(e => e.Type).IsRequired()
                                         .HasColumnName("lesson_type")
                                         .HasDefaultValue(LessonType.Text)
                                         .SmartEnumNameConversion();
        }
    }
}
