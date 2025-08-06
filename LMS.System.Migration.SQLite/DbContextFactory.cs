using LMS.System.Domain.DB;
using LMS.System.Domain.Model.Dates;
using Microsoft.EntityFrameworkCore;

namespace LMS.System.Migration.SQLite
{
    public class DbContextFactory(string connectionName, ISystemClock dateTimeProvider)
    {
        public AppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                                .UseSqlite($"Data Source={connectionName}.db")
                                .Options;

            var dbContext = new AppDbContext(options, dateTimeProvider);
            dbContext.Database.Migrate();
            return dbContext;
        }
    }
}
