using BudgetEase.Core.Interfaces;

namespace BudgetEase.Api.BackgroundServices;

/// <summary>
/// Background service that runs daily database backups
/// </summary>
public class DatabaseBackupBackgroundService : BackgroundService
{
    private readonly ILogger<DatabaseBackupBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly int _intervalHours;

    public DatabaseBackupBackgroundService(
        ILogger<DatabaseBackupBackgroundService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _intervalHours = int.TryParse(configuration["DatabaseBackup:IntervalHours"], out var hours) ? hours : 24;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Database Backup Background Service started. Will run every {IntervalHours} hour(s)", _intervalHours);

        // Run initial backup after a short delay to allow application startup
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Running database backup task at {Time}", DateTime.UtcNow);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var backupService = scope.ServiceProvider.GetRequiredService<IDatabaseBackupService>();
                    var backupPath = await backupService.CreateBackupAsync(stoppingToken);
                    
                    if (!string.IsNullOrEmpty(backupPath))
                    {
                        _logger.LogInformation("Database backup completed: {BackupPath}", backupPath);
                    }
                }

                _logger.LogInformation("Next database backup scheduled in {IntervalHours} hour(s)", _intervalHours);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Database Backup Background Service");
            }

            // Wait for the configured interval before running again
            await Task.Delay(TimeSpan.FromHours(_intervalHours), stoppingToken);
        }

        _logger.LogInformation("Database Backup Background Service stopped");
    }
}
