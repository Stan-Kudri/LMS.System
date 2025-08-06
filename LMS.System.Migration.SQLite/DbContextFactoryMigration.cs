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
            var options = new DbContextOptionsBuilder<AppDbContext>()
                                .UseSqlite($"Data Source={ConnectionName}.db")
                                .Options;

            return new AppDbContext(options);
        }
    }
}
