using LMS.System.Domain.Model.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// Test Submission configuration.
    /// </summary>
    /// <typeparam name="TestSubmission">Test Submission.</typeparam>
    public sealed class TestSubmissionConfiguration : EntityBaseConfiguration<TestSubmission>
    {
        /// <summary>
        /// Configure Test Submission.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<TestSubmission> builder)
        {
            builder.ToTable("test_submission");
            builder.Property(e => e.TestQuestionId).IsRequired()
                                                   .HasColumnName("test_question_id");
            builder.Property(e => e.SubmissionId).IsRequired()
                                                 .HasColumnName("submission_id");
            builder.Property(e => e.SelectedOptionId).IsRequired()
                                                     .HasColumnName("selected_option_id");
        }
    }
}
