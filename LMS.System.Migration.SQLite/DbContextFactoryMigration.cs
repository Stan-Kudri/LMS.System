using LMS.System.Domain.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LMS.System.Migration.SQLite
{
    public class DbContextFactoryMigration : IDesignTimeDbContextFactory<AppDbContext>
    {
        private const string ConnectionName = "LMSDbConnection";

        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder().UseSqlite(
                $"Data Source={ConnectionName}.db",
                b => b.MigrationsAssembly(typeof(DbContextFactoryMigration).Assembly.FullName));

            return new AppDbContext(builder.Options);
        }
    }
}
