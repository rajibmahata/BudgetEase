using BudgetEase.Core.Interfaces;

namespace BudgetEase.Api.BackgroundServices;

public class BackupCleanupBackgroundService : BackgroundService
{
    private readonly ILogger<BackupCleanupBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly int _intervalHours;

    public BackupCleanupBackgroundService(
        ILogger<BackupCleanupBackgroundService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _intervalHours = int.TryParse(configuration["BackupSettings:CleanupIntervalHours"], out var hours) ? hours : 24;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Backup Cleanup Background Service started. Will run every {IntervalHours} hour(s)", _intervalHours);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Running backup cleanup task at {Time}", DateTime.UtcNow);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var backupCleanupService = scope.ServiceProvider.GetRequiredService<IBackupCleanupService>();
                    await backupCleanupService.CleanupOldBackupsAsync(stoppingToken);
                }

                _logger.LogInformation("Next backup cleanup scheduled in {IntervalHours} hour(s)", _intervalHours);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Backup Cleanup Background Service");
            }

            // Wait for the configured interval before running again
            await Task.Delay(TimeSpan.FromHours(_intervalHours), stoppingToken);
        }

        _logger.LogInformation("Backup Cleanup Background Service stopped");
    }
}
