using ISC_Project.Hubs;

namespace ISC_Project.Services
{
    public class PrivateChatCleanupService : BackgroundService
    {
        private readonly ILogger<PrivateChatCleanupService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(5); // Chạy mỗi 5 phút

        public PrivateChatCleanupService(ILogger<PrivateChatCleanupService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PrivateChatCleanupService started");

            using var timer = new PeriodicTimer(_period);

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    try
                    {
                        PrivateChatHub.CleanupInactiveConnections();
                        _logger.LogDebug("Cleaned up inactive connections");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during cleanup process");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("PrivateChatCleanupService stopped");
            }
        }
    }
}
