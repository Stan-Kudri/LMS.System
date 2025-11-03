using LMS.System.Domain.DB.Configuration;
using LMS.System.Domain.Model;
using LMS.System.Domain.Model.Task;
using LMS.System.Domain.Model.Tests;
using LMS.System.Domain.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace LMS.System.Domain.DB
{
    /// <summary>
    /// Main database context for the application using Entity Framework Core.
    /// Manages entity sets, database connections, and automatic timestamp updates.
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly TimeProvider _dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// Initializes the database context with specified options and time provider.
        /// </summary>
        /// <param name="options">The database context options.</param>
        /// <param name="dateTimeProvider">The time provider for timestamp generation.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options, TimeProvider dateTimeProvider)
            : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
            SavingChanges += UpdateTimestamps;
        }

        /// <summary>
        /// Users table for application users.
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Refresh tokens for authentication.
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        /// <summary>
        /// Course categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        /// Educational courses.
        /// </summary>
        public DbSet<Course> Courses { get; set; } = null!;

        /// <summary>
        /// Lessons within courses.
        /// </summary>
        public DbSet<Lesson> Lessons { get; set; } = null!;

        /// <summary>
        /// Course assignments.
        /// </summary>
        public DbSet<Assignment> Assignments { get; set; } = null!;

        /// <summary>
        /// Assignment submissions.
        /// </summary>
        public DbSet<Submission> Submissions { get; set; } = null!;

        /// <summary>
        /// User course enrollments.
        /// </summary>
        public DbSet<Enrollment> Enrollments { get; set; } = null!;

        /// <summary>
        /// Test questions.
        /// </summary>
        public DbSet<TestQuestion> TestQuestions { get; set; } = null!;

        /// <summary>
        /// Answer options for test questions.
        /// </summary>
        public DbSet<TestAnswerOption> TestAnswerOptions { get; set; } = null!;

        /// <summary>
        /// Test submissions and attempts.
        /// </summary>
        public DbSet<TestSubmission> TestSubmissions { get; set; } = null!;

        /// <summary>
        /// Is typically called only once when the first instance of a derived DbContext is created.
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }

        /// <summary>
        /// Updates CreatedAt and UpdatedAt for new and modified entities.
        /// </summary>
        private void UpdateTimestamps(object? sender, SavingChangesEventArgs e)
        {
            var now = _dateTimeProvider.GetUtcNow().DateTime;

            var changedEntries = ChangeTracker.Entries().Where(x => x.Entity is Entity
                                                                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (Entity)entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = now;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedAt = now;
                        break;
                }
            }
        }
    }
}
