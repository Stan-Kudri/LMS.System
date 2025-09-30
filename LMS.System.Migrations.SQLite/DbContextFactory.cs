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

            return new AppDbContext(options, dateTimeProvider);
        }
    }
}
