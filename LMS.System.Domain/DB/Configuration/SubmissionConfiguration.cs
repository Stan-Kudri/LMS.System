using LMS.System.Domain.Model.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// Submission configuration.
    /// </summary>
    /// <typeparam name="Submission">Submission.</typeparam>
    public sealed class SubmissionConfiguration : EntityBaseConfiguration<Submission>
    {
        /// <summary>
        /// Configure submission.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<Submission> builder)
        {
            builder.ToTable("submission");
            builder.Property(e => e.AnswerText).IsRequired()
                                               .HasMaxLength(128)
                                               .HasColumnName("answer_text");
            builder.Property(e => e.FilePath).IsRequired()
                                             .HasMaxLength(64)
                                             .HasColumnName("file_path");
            builder.Property(e => e.Grade).IsRequired()
                                          .HasColumnName("grade");
            builder.Property(e => e.Feedback).IsRequired()
                                             .HasMaxLength(128)
                                             .HasColumnName("feedback");
            builder.Property(e => e.SubmittedAt).IsRequired()
                                                .HasColumnName("submitted_at")
                                                .ValueGeneratedOnAdd()
                                                .HasDefaultValue(DateTime.UtcNow);
            builder.Property(e => e.AssignmentId).IsRequired()
                                                 .HasColumnName("assignment_id");
            builder.Property(e => e.StudentId).IsRequired()
                                              .HasColumnName("student_id");

            builder.HasOne(e => e.Student)
                   .WithMany(e => e.Submissions)
                   .HasForeignKey(e => e.StudentId);
        }
    }
}
