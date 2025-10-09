using BudgetEase.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetEase.Infrastructure.Services;

public class DatabaseBackupService : IDatabaseBackupService
{
    private readonly ILogger<DatabaseBackupService> _logger;
    private readonly string _backupDirectory;
    private readonly string _databasePath;

    public DatabaseBackupService(
        ILogger<DatabaseBackupService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _backupDirectory = configuration["DatabaseBackup:BackupDirectory"] ?? "DatabaseBackups";
        
        // Get database path from connection string
        var connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? "Data Source=budgetease.db";
        _databasePath = ExtractDatabasePath(connectionString);
    }

    public async Task<string> CreateBackupAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Ensure backup directory exists
            if (!Directory.Exists(_backupDirectory))
            {
                Directory.CreateDirectory(_backupDirectory);
                _logger.LogInformation("Created backup directory: {BackupDirectory}", _backupDirectory);
            }

            // Check if database file exists
            if (!File.Exists(_databasePath))
            {
                _logger.LogWarning("Database file not found at {DatabasePath}. Skipping backup.", _databasePath);
                return string.Empty;
            }

            // Create backup filename with timestamp
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var backupFileName = $"budgetease_backup_{timestamp}.db";
            var backupPath = Path.Combine(_backupDirectory, backupFileName);

            // Copy database file to backup location
            await Task.Run(() => File.Copy(_databasePath, backupPath, overwrite: false), cancellationToken);

            _logger.LogInformation("Database backup created successfully: {BackupPath}", backupPath);
            return backupPath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create database backup");
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
                _logger.LogInformation("Database already exists at {DatabasePath}. Skipping restore.", _databasePath);
                return false;
            }

            // Get latest backup
            var backups = await GetAvailableBackupsAsync();
            var latestBackup = backups.FirstOrDefault();

            if (string.IsNullOrEmpty(latestBackup))
            {
                _logger.LogInformation("No backup files found. Starting with fresh database.");
                return false;
            }

            // Ensure database directory exists
            var databaseDirectory = Path.GetDirectoryName(_databasePath);
            if (!string.IsNullOrEmpty(databaseDirectory) && !Directory.Exists(databaseDirectory))
            {
                Directory.CreateDirectory(databaseDirectory);
            }

            // Restore from latest backup
            await Task.Run(() => File.Copy(latestBackup, _databasePath, overwrite: false), cancellationToken);

            _logger.LogInformation("Database restored from backup: {BackupPath}", latestBackup);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore database from backup");
            throw;
        }
    }

    public async Task<IEnumerable<string>> GetAvailableBackupsAsync()
    {
        return await Task.Run(() =>
        {
            if (!Directory.Exists(_backupDirectory))
            {
                return Enumerable.Empty<string>();
            }

            return Directory.GetFiles(_backupDirectory, "*.db")
                .OrderByDescending(f => File.GetCreationTimeUtc(f))
                .ToList();
        });
    }

    private string ExtractDatabasePath(string connectionString)
    {
        // Parse SQLite connection string to extract database path
        var parts = connectionString.Split(';');
        foreach (var part in parts)
        {
            var keyValue = part.Split('=');
            if (keyValue.Length == 2 && keyValue[0].Trim().Equals("Data Source", StringComparison.OrdinalIgnoreCase))
            {
                return keyValue[1].Trim();
            }
        }

        // Default fallback
        return "budgetease.db";
    }
}
