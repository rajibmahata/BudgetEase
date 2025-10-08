using BudgetEase.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetEase.Infrastructure.Services;

public class BackupCleanupService : IBackupCleanupService
{
    private readonly ILogger<BackupCleanupService> _logger;
    private readonly string _backupDirectory;
    private readonly int _retentionDays;

    public BackupCleanupService(
        ILogger<BackupCleanupService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _backupDirectory = configuration["BackupSettings:BackupDirectory"] ?? "Backups";
        _retentionDays = int.TryParse(configuration["BackupSettings:RetentionDays"], out var days) ? days : 30;
    }

    public async Task CleanupOldBackupsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!Directory.Exists(_backupDirectory))
            {
                _logger.LogInformation("Backup directory '{BackupDirectory}' does not exist. No cleanup needed.", _backupDirectory);
                return;
            }

            var cutoffDate = DateTime.UtcNow.AddDays(-_retentionDays);
            _logger.LogInformation("Starting backup cleanup for files older than {CutoffDate} ({RetentionDays} days)", 
                cutoffDate, _retentionDays);

            var files = Directory.GetFiles(_backupDirectory, "*", SearchOption.AllDirectories);
            var deletedCount = 0;

            foreach (var file in files)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning("Backup cleanup cancelled");
                    break;
                }

                var fileInfo = new FileInfo(file);
                if (fileInfo.LastWriteTimeUtc < cutoffDate)
                {
                    try
                    {
                        await Task.Run(() => File.Delete(file), cancellationToken);
                        deletedCount++;
                        _logger.LogInformation("Deleted old backup file: {FileName}, Last modified: {LastModified}", 
                            fileInfo.Name, fileInfo.LastWriteTimeUtc);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete backup file: {FileName}", fileInfo.Name);
                    }
                }
            }

            _logger.LogInformation("Backup cleanup completed. Deleted {DeletedCount} file(s)", deletedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during backup cleanup");
            throw;
        }
    }
}
