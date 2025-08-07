using LMS.System.Domain.DB.Configuration;
using LMS.System.Domain.Model;
using LMS.System.Domain.Model.Dates;
using LMS.System.Domain.Model.Task;
using LMS.System.Domain.Model.Tests;
using LMS.System.Domain.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace LMS.System.Domain.DB
{
    /// <summary>
    /// DbContext.
    /// </summary>
    public class AppDbContext(DbContextOptions options, ISystemClock dateTimeProvider)
        : DbContext(options)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public AppDbContext(DbContextOptions options)
            : this(options, new SystemDateTimeProvider())
        {
        }

        /// <summary>
        /// DBset User.
        /// </summary>
        public DbSet<User>? Users { get; set; }

        /// <summary>
        /// DBset Category.
        /// </summary>
        public DbSet<Category>? Categories { get; set; }

        /// <summary>
        /// DBset Course.
        /// </summary>
        public DbSet<Course>? Courses { get; set; }

        /// <summary>
        /// DBset Lesson.
        /// </summary>
        public DbSet<Lesson>? Lessons { get; set; }

        /// <summary>
        /// DBset Assignment.
        /// </summary>
        public DbSet<Assignment>? Assignments { get; set; }

        /// <summary>
        /// DBset Submission.
        /// </summary>
        public DbSet<Submission>? Submissions { get; set; }

        /// <summary>
        /// DBset Enrollment.
        /// </summary>
        public DbSet<Enrollment>? Enrollments { get; set; }

        /// <summary>
        /// DBset TestQuestion.
        /// </summary>
        public DbSet<TestQuestion>? TestQuestions { get; set; }

        /// <summary>
        /// DBset TestAnswerOption.
        /// </summary>
        public DbSet<TestAnswerOption>? TestAnswerOptions { get; set; }

        /// <summary>
        /// DBset TestSubmission.
        /// </summary>
        public DbSet<TestSubmission>? TestSubmissions { get; set; }

        private void OnSavingChanges(object? sender, SavingChangesEventArgs e)
        {
            var changedEntries = ChangeTracker.Entries()
                                              .Where(x => x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var now = dateTimeProvider.UtcNow;
            foreach (var entry in changedEntries)
            {
                var entity = (Entity)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                }

                entity.UpdatedAt = now;
            }
        }

        /// <summary>
        /// Override "OnModelCreating".
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}
