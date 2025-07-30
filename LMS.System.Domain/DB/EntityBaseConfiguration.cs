using LMS.System.Domain.Enums;
using LMS.System.Domain.Extension;
using LMS.System.Domain.Model;
using LMS.System.Domain.Model.Task;
using LMS.System.Domain.Model.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.System.Domain.DB
{
    /// <summary>
    /// Base entity configuration.
    /// </summary>
    /// <typeparam name="T">Model configuration.</typeparam>
    public abstract class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        /// <summary>
        /// Configure model.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id")
                                       .ValueGeneratedOnAdd();
            builder.HasKey(e => e.CreatedAt);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at")
                                              .HasDefaultValue(DateTime.Now)
                                              .ValueGeneratedOnAddOrUpdate();
            builder.HasKey(e => e.UpdatedAt);
            builder.Property(e => e.UpdatedAt).HasColumnName("update_at")
                                              .HasDefaultValue(DateTime.Now)
                                              .ValueGeneratedOnAddOrUpdate();
            ConfigureModel(builder);
        }

        /// <summary>
        /// Configure model.
        /// </summary>
        /// <param name="builder">Entity type builder.</param>
        protected abstract void ConfigureModel(EntityTypeBuilder<T> builder);
    }

    /// <summary>
    /// Category configuration.
    /// </summary>
    /// <typeparam name="T">Category.</typeparam>
    public sealed class CategoryConfiguration : EntityBaseConfiguration<Category>
    {
        /// <summary>
        /// Configure category.
        /// </summary>
        /// <param name="builder">Builder DB.</param>
        protected override void ConfigureModel(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");
            builder.HasIndex(e => e.Name).IsUnique();
            builder.Property(e => e.Name).IsRequired()
                                         .HasMaxLength(128)
                                         .HasColumnName("name");

            builder.HasMany(e => e.Courses)
                   .WithOne(e => e.Category)
                   .HasForeignKey(e => e.CategoryId);
        }
    }

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
                                          .HasColumnName("is_published")
                                          .ValueGeneratedOnAdd();
            builder.Property(e => e.CourseId).IsRequired()
                                             .HasColumnName("course_id");
            builder.Property(e => e.Type).IsRequired()
                                         .HasColumnName("type")
                                         .HasDefaultValue(LessonType.Text)
                                         .SmartEnumNameConversion();
        }
    }

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
            builder.HasOne(e => e.Course)
                   .WithMany(e => e.Enrollments)
                   .HasForeignKey(e => e.CourseId);
        }
    }

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
                                                   .HasColumnName("type")
                                                   .HasDefaultValue(AssignmentType.Text)
                                                   .SmartEnumNameConversion();

            builder.HasOne(p => p.Course)
                   .WithMany(p => p.Assignments)
                   .HasForeignKey(p => p.CourseId);
            builder.HasMany(p => p.TestQuestions)
                   .WithOne(p => p.Assignment)
                   .HasForeignKey(p => p.AssignmentId);
            builder.HasMany(p => p.Submissions)
                   .WithOne(p => p.Assignment)
                   .HasForeignKey(p => p.AssignmentId);
        }
    }

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
        }
    }

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
                                                 .HasColumnName("type")
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
