using BudgetEase.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BudgetEase.Infrastructure.Services;

public class DatabaseBackupBackgroundService : BackgroundService
{
    private readonly IDatabaseBackupService _backupService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseBackupBackgroundService> _logger;
    private readonly TimeSpan _backupInterval;

    public DatabaseBackupBackgroundService(
        IDatabaseBackupService backupService,
        IConfiguration configuration,
        ILogger<DatabaseBackupBackgroundService> logger)
    {
        _backupService = backupService;
        _configuration = configuration;
        _logger = logger;
        
        // Get backup interval from configuration (default to 24 hours)
        var intervalHours = int.TryParse(_configuration["DatabaseBackup:IntervalHours"], out var hours) ? hours : 24;
        _backupInterval = TimeSpan.FromHours(intervalHours);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Database Backup Background Service started. Backup interval: {Interval} hours", 
            _backupInterval.TotalHours);

        // Wait a short time before first backup to ensure app is fully initialized
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Starting scheduled database backup");
                var backupPath = await _backupService.BackupDatabaseAsync(stoppingToken);
                
                if (!string.IsNullOrEmpty(backupPath))
                {
                    _logger.LogInformation("Scheduled backup completed successfully: {BackupPath}", backupPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during scheduled database backup");
            }

            // Wait for next backup interval
            try
            {
                await Task.Delay(_backupInterval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                // Service is stopping
                break;
            }
        }

        _logger.LogInformation("Database Backup Background Service stopped");
    }
}
