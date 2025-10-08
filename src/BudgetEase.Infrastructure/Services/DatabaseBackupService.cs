using BudgetEase.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetEase.Infrastructure.Services;

public class DatabaseBackupService : IDatabaseBackupService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseBackupService> _logger;
    private readonly string _databasePath;
    private readonly string _backupDirectory;

    public DatabaseBackupService(
        IConfiguration configuration,
        ILogger<DatabaseBackupService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        // Get database path from connection string
        var connectionString = _configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=budgetease.db";
        
        // Extract database file path from connection string
        _databasePath = ExtractDatabasePath(connectionString);
        
        // Set backup directory
        var backupPath = _configuration["DatabaseBackup:BackupDirectory"] ?? "DatabaseBackups";
        _backupDirectory = Path.IsPathRooted(backupPath) 
            ? backupPath 
            : Path.Combine(Path.GetDirectoryName(_databasePath) ?? ".", backupPath);
        
        // Ensure backup directory exists
        Directory.CreateDirectory(_backupDirectory);
    }

    public async Task<string> BackupDatabaseAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!File.Exists(_databasePath))
            {
                _logger.LogWarning("Database file not found at {Path}. Skipping backup.", _databasePath);
                return string.Empty;
            }

            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var backupFileName = $"budgetease_backup_{timestamp}.db";
            var backupFilePath = Path.Combine(_backupDirectory, backupFileName);

            _logger.LogInformation("Starting database backup to {BackupPath}", backupFilePath);

            // Copy database file to backup location
            await Task.Run(() => File.Copy(_databasePath, backupFilePath, overwrite: false), cancellationToken);

            _logger.LogInformation("Database backup completed successfully: {BackupPath}", backupFilePath);

            // Cleanup old backups
            await CleanupOldBackupsAsync();

            return backupFilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error backing up database");
            throw;
        }
    }

    public async Task<bool> RestoreLatestBackupAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Only restore if database doesn't exist
            if (File.Exists(_databasePath))
            {
                _logger.LogInformation("Database already exists. Skipping restore.");
                return false;
            }

            var backups = await GetAvailableBackupsAsync();
            var latestBackup = backups.FirstOrDefault();

            if (string.IsNullOrEmpty(latestBackup))
            {
                _logger.LogInformation("No backup files found. Skipping restore.");
                return false;
            }

            _logger.LogInformation("Restoring database from backup: {BackupPath}", latestBackup);

            // Ensure the database directory exists
            var dbDirectory = Path.GetDirectoryName(_databasePath);
            if (!string.IsNullOrEmpty(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }

            // Copy backup file to database location
            await Task.Run(() => File.Copy(latestBackup, _databasePath, overwrite: true), cancellationToken);

            _logger.LogInformation("Database restored successfully from {BackupPath}", latestBackup);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring database from backup");
            return false;
        }
    }

    public Task<IEnumerable<string>> GetAvailableBackupsAsync()
    {
        try
        {
            if (!Directory.Exists(_backupDirectory))
            {
                return Task.FromResult(Enumerable.Empty<string>());
            }

            var backupFiles = Directory.GetFiles(_backupDirectory, "budgetease_backup_*.db")
                .OrderByDescending(f => File.GetCreationTimeUtc(f))
                .ToList();

            return Task.FromResult<IEnumerable<string>>(backupFiles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available backups");
            return Task.FromResult(Enumerable.Empty<string>());
        }
    }

    public async Task CleanupOldBackupsAsync(int keepDays = 30)
    {
        try
        {
            var retentionDays = int.TryParse(_configuration["DatabaseBackup:RetentionDays"], out var days) ? days : keepDays;

            var backups = await GetAvailableBackupsAsync();
            var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);

            foreach (var backup in backups)
            {
                var fileInfo = new FileInfo(backup);
                if (fileInfo.CreationTimeUtc < cutoffDate)
                {
                    _logger.LogInformation("Deleting old backup: {BackupPath} (created {CreatedDate})", 
                        backup, fileInfo.CreationTimeUtc);
                    File.Delete(backup);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up old backups");
        }
    }

    private static string ExtractDatabasePath(string connectionString)
    {
        // Parse SQLite connection string to extract Data Source
        var parts = connectionString.Split(';');
        foreach (var part in parts)
        {
            var keyValue = part.Trim().Split('=', 2);
            if (keyValue.Length == 2 && 
                keyValue[0].Trim().Equals("Data Source", StringComparison.OrdinalIgnoreCase))
            {
                var path = keyValue[1].Trim();
                return Path.IsPathRooted(path) ? path : Path.GetFullPath(path);
            }
        }
        
        // Default fallback
        return Path.GetFullPath("budgetease.db");
    }
}
