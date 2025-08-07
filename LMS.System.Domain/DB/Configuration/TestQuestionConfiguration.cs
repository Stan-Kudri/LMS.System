using LMS.System.Domain.Enums;
using LMS.System.Domain.Extension;
using LMS.System.Domain.Model.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB.Configuration
{
    /// <summary>
    /// Test Question configuration.
    /// </summary>
    /// <typeparam name="TestQuestion">Test Question.</typeparam>
    public sealed class TestQuestionConfiguration : EntityBaseConfiguration<TestQuestion>
    {
        /// <summary>
        /// Configure Test Question.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<TestQuestion> builder)
        {
            builder.ToTable("test_question");
            builder.Property(e => e.QuestionText).IsRequired()
                                                 .HasMaxLength(128)
                                                 .HasColumnName("question_text");
            builder.Property(e => e.QuestionType).IsRequired()
                                                 .HasColumnName("question_type")
                                                 .HasDefaultValue(QuestionType.SingleChoice)
                                                 .SmartEnumNameConversion();
            builder.Property(e => e.AssignmentId).IsRequired()
                                                 .HasColumnName("assignment_id");

            builder.HasMany(e => e.AnswerOptions)
                   .WithOne(e => e.TestQuestion)
                   .HasForeignKey(e => e.TestQuestionId);
            builder.HasMany(e => e.TestSubmissions)
                   .WithOne(e => e.TestQuestion)
                   .HasForeignKey(e => e.TestQuestionId);
        }
    }
}
