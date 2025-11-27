using System;
using LMS.System.Domain.DB;
using Microsoft.EntityFrameworkCore;

namespace LMS.System.Migrations.SQLite
{
    public class DbContextFactory(string connectionName, TimeProvider dateTimeProvider)
    {
        public AppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                                .UseSqlite($"Data Source={connectionName}.db",
                                           e => e.MigrationsAssembly(typeof(DbContextFactoryMigration).Assembly.FullName))
                                .Options;
            var dbContext = new AppDbContext(options, dateTimeProvider);
            dbContext.Database.Migrate();
            return dbContext;
        }
    }
}
