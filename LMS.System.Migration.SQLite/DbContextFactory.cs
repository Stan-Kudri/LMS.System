using LMS.System.Domain.DB;
using LMS.System.Domain.Model.Dates;
using Microsoft.EntityFrameworkCore;

namespace LMS.System.Migration.SQLite
{
    public class DbContextFactory(string connectionName, IDateTimeProvider dateTimeProvider)
    {
        public AppDbContext Create()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={connectionName}.db", x =>
            {
                x.MigrationsAssembly(typeof(DbContextFactoryMigration).Assembly.FullName);
            });

            var dbContext = new AppDbContext(builder.Options, dateTimeProvider);
            dbContext.Migrate();

            return dbContext;
        }
    }
}
