using System.ComponentModel;
using LMS.System.Domain.DB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LMS.System.Infrastructure.Service
{
    /// <summary>
    /// Token cleaning service.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RefreshTokenCleanupService"/> class.
    /// </remarks>
    /// <param name="serviceProvider">Provider.</param>
    /// <param name="logger">Logger</param>
    public class RefreshTokenCleanupService(IServiceProvider serviceProvider, ILogger<RefreshTokenCleanupService> logger) 
        : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<RefreshTokenCleanupService> _logger = logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(24); // Check every 24 hours.

        /// <summary>
        /// Clean expired Refresh Token execute.
        /// </summary>
        /// <param name="stoppingToken">CancellationToken.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupExpiredTokensAsync();
                    await Task.Delay(_cleanupInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, message: "Error clean RefreshToken.");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }

        /// <summary>
        /// Clean expired Refresh Tokens.
        /// </summary>
        private async Task CleanupExpiredTokensAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var expiredTokens = dbContext.RefreshTokens.Where(rt => rt.Expires < DateTime.UtcNow || rt.IsRevoked);

            dbContext.RefreshTokens.RemoveRange(expiredTokens);
            var deletedCount = await dbContext.SaveChangesAsync();

            if (deletedCount > 0)
            {
                _logger.LogInformation("Removed {deletedCount} inactive RefreshTokens.", deletedCount);
            }
        }
    }
}
