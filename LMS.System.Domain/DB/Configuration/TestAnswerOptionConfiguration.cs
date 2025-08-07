using LMS.System.Domain.Model.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// Test Answer Option configuration.
    /// </summary>
    /// <typeparam name="TestAnswerOption">Test Answer Option.</typeparam>
    public sealed class TestAnswerOptionConfiguration : EntityBaseConfiguration<TestAnswerOption>
    {
        /// <summary>
        /// Configure Test Answer Option.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<TestAnswerOption> builder)
        {
            builder.ToTable("test_answer_option");
            builder.Property(e => e.OptionText).IsRequired()
                                               .HasMaxLength(128)
                                               .HasColumnName("option_text");
            builder.Property(e => e.IsCorrect).IsRequired()
                                              .HasDefaultValue(true)
                                              .HasColumnName("is_correct");
            builder.Property(e => e.TestQuestionId).IsRequired()
                                                   .HasColumnName("test_question_id");
        }
    }
}
