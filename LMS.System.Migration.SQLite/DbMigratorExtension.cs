using LMS.System.Domain.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.System.Migration.SQLite
{
    public static class DbMigratorExtension
    {
        /// <summary>
        /// Apply migrations based on model configurations.
        /// </summary>
        /// <param name="dbContext">Basic data session.</param>
        /// <exception cref="InvalidOperationException">Invalid Operation.</exception>
        public static void Migrate(this AppDbContext dbContext)
        {
            var migrate = dbContext.Database.GetInfrastructure().GetService<IMigrator>()
                                            ?? throw new InvalidOperationException("Unable to not found migrator service.");

            foreach (var migrationName in dbContext.Database.GetPendingMigrations())
            {
                migrate.Migrate(migrationName);
            }
        }
    }
}
