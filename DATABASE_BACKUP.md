# Database Backup and Restore

## Overview

BudgetEase includes an automated database backup and restore system that ensures your SQLite database is protected against data loss. The system automatically creates daily backups and can restore from the latest backup when the application starts.

## Features

### Automatic Daily Backups
- **Scheduled Backups**: The system automatically backs up the database every 24 hours (configurable)
- **Background Service**: Runs as a background service, so backups happen without user intervention
- **Timestamped Files**: Each backup is saved with a timestamp (format: `budgetease_backup_YYYYMMDD_HHmmss.db`)
- **First Backup**: The first backup is created 1 minute after application startup, subsequent backups run at the configured interval

### Automatic Restore on Startup
- **Smart Restore**: If the database file doesn't exist on startup, the system automatically restores from the most recent backup
- **Safe Operation**: Only restores when the database is missing - won't overwrite an existing database
- **Seamless Recovery**: Application continues normally after restore with all data intact

### Automatic Cleanup
- **Retention Policy**: Old backups are automatically deleted based on retention period
- **Default Retention**: 30 days (configurable)
- **Runs After Each Backup**: Cleanup happens automatically after each successful backup

## Configuration

Configure the backup system in `appsettings.json`:

```json
{
  "DatabaseBackup": {
    "BackupDirectory": "DatabaseBackups",
    "IntervalHours": 24,
    "RetentionDays": 30
  }
}
```

### Configuration Options

| Option | Description | Default |
|--------|-------------|---------|
| `BackupDirectory` | Directory where backups are stored (relative to database location or absolute path) | `DatabaseBackups` |
| `IntervalHours` | How often to create backups (in hours) | `24` |
| `RetentionDays` | How many days to keep old backups | `30` |

## Backup Files

### Location
By default, backups are stored in the `DatabaseBackups` directory in the same location as the main database file:
- Database: `src/BudgetEase.Api/budgetease.db`
- Backups: `src/BudgetEase.Api/DatabaseBackups/`

### File Naming
Backup files follow this naming convention:
```
budgetease_backup_20251008_143915.db
                  ^^^^^^^^ ^^^^^^
                  Date     Time (UTC)
```

### File Size
Backup files are exact copies of the database at the time of backup, so their size matches the database size.

## How It Works

### Startup Process
1. Application starts
2. Restore service checks if database exists
3. If database is missing:
   - Finds the most recent backup file
   - Copies it to the database location
   - Logs the restore operation
4. Database migrations are applied (if needed)
5. Application continues normal startup

### Background Backup Process
1. Background service starts with application
2. Waits 1 minute for application to initialize
3. Creates first backup
4. Cleans up old backups (older than retention period)
5. Waits for configured interval (default: 24 hours)
6. Repeats steps 3-5 until application stops

### Backup Process Details
The backup process:
1. Checks if database file exists
2. Creates backup directory if needed
3. Generates timestamped filename
4. Copies database file to backup location
5. Logs successful backup
6. Triggers cleanup of old backups

## Monitoring

The backup system provides detailed logging at the `Information` level:

```
info: BudgetEase.Infrastructure.Services.DatabaseBackupBackgroundService[0]
      Database Backup Background Service started. Backup interval: 24 hours

info: BudgetEase.Infrastructure.Services.DatabaseBackupService[0]
      Starting database backup to /path/to/DatabaseBackups/budgetease_backup_20251008_143915.db

info: BudgetEase.Infrastructure.Services.DatabaseBackupService[0]
      Database backup completed successfully: /path/to/DatabaseBackups/budgetease_backup_20251008_143915.db

info: BudgetEase.Infrastructure.Services.DatabaseBackupService[0]
      Restoring database from backup: /path/to/DatabaseBackups/budgetease_backup_20251008_143915.db

info: BudgetEase.Infrastructure.Services.DatabaseBackupService[0]
      Database restored successfully from /path/to/DatabaseBackups/budgetease_backup_20251008_143915.db
```

## Manual Operations

### Manual Backup
To manually trigger a backup, you can inject `IDatabaseBackupService` into your code:

```csharp
public class MyController : ControllerBase
{
    private readonly IDatabaseBackupService _backupService;

    public MyController(IDatabaseBackupService backupService)
    {
        _backupService = backupService;
    }

    [HttpPost("manual-backup")]
    public async Task<IActionResult> CreateBackup()
    {
        var backupPath = await _backupService.BackupDatabaseAsync();
        return Ok(new { backupPath });
    }
}
```

### Manual Restore
The restore process only runs automatically when the database is missing. To manually restore from a backup:

1. Stop the application
2. Delete or rename the existing database file
3. (Optional) Delete/rename specific backup file you want to restore to make it the most recent
4. Start the application - it will automatically restore from the latest backup

### List Available Backups
```csharp
var backups = await _backupService.GetAvailableBackupsAsync();
foreach (var backup in backups)
{
    Console.WriteLine($"Backup: {backup}");
}
```

## Troubleshooting

### Backups Not Being Created

1. **Check logs** - Look for error messages from `DatabaseBackupService` or `DatabaseBackupBackgroundService`
2. **Verify permissions** - Ensure the application has write permissions for the backup directory
3. **Check disk space** - Ensure sufficient disk space for backup files
4. **Verify configuration** - Check `appsettings.json` for correct backup settings

### Restore Not Working

1. **Check backup files exist** - Verify backup files are present in the `DatabaseBackups` directory
2. **Check database doesn't exist** - Restore only happens when database file is missing
3. **Verify file permissions** - Ensure application can read backup files and write to database location
4. **Check logs** - Look for restore-related log messages

### Old Backups Not Being Cleaned Up

1. **Check retention configuration** - Verify `RetentionDays` is set correctly in `appsettings.json`
2. **Verify file timestamps** - Cleanup is based on file creation time
3. **Check logs** - Look for cleanup-related log messages

## Best Practices

1. **Regular Monitoring**: Check logs regularly to ensure backups are completing successfully
2. **Test Restores**: Periodically test the restore process to ensure backups are valid
3. **External Backups**: For production, consider copying backups to external storage (cloud, network drive)
4. **Adjust Retention**: Set retention period based on your data change frequency and storage capacity
5. **Backup Location**: For production, consider using an absolute path for backups on a separate drive

## Security Considerations

1. **File Permissions**: Backup files contain all database data - protect them with appropriate file permissions
2. **Sensitive Data**: If database contains sensitive data, consider encrypting backup files
3. **Backup Location**: Store backups in a secure location with restricted access
4. **Network Backups**: If backing up over network, use secure protocols (SFTP, encrypted shares)

## Production Recommendations

For production deployments:

1. **External Storage**: Configure backups to write to a separate physical drive or network location
2. **Redundancy**: Implement additional backup strategies (e.g., cloud storage sync)
3. **Monitoring**: Set up alerts for backup failures
4. **Testing**: Regularly test restore procedures
5. **Frequency**: Adjust backup frequency based on data change rate (consider hourly backups for high-activity systems)
6. **Encryption**: Implement encryption for backup files containing sensitive data

Example production configuration:

```json
{
  "DatabaseBackup": {
    "BackupDirectory": "/mnt/backup-drive/budgetease-backups",
    "IntervalHours": 6,
    "RetentionDays": 90
  }
}
```

## Technical Details

### Implementation

The backup system consists of three main components:

1. **IDatabaseBackupService** (`BudgetEase.Core.Interfaces`)
   - Interface defining backup operations
   - Located in Core layer for clean architecture

2. **DatabaseBackupService** (`BudgetEase.Infrastructure.Services`)
   - Implementation of backup, restore, and cleanup logic
   - Handles file operations and configuration
   - Registered as singleton service

3. **DatabaseBackupBackgroundService** (`BudgetEase.Infrastructure.Services`)
   - Background service that runs backup on schedule
   - Inherits from `BackgroundService`
   - Registered as hosted service

### Service Registration

Services are registered in `Program.cs`:

```csharp
// Register database backup service
builder.Services.AddSingleton<IDatabaseBackupService, DatabaseBackupService>();
builder.Services.AddHostedService<DatabaseBackupBackgroundService>();
```

### Dependencies

- `Microsoft.Extensions.Hosting.Abstractions` - For background service support
- `Microsoft.Extensions.Configuration` - For configuration access
- `Microsoft.Extensions.Logging` - For logging

## Future Enhancements

Potential improvements for future versions:

- [ ] Compression of backup files to save space
- [ ] Encryption support for backup files
- [ ] Cloud storage integration (Azure Blob, AWS S3)
- [ ] Email notifications for backup success/failure
- [ ] REST API endpoints for backup management
- [ ] Backup verification (checksum validation)
- [ ] Incremental backups for large databases
- [ ] Backup scheduling with cron expressions
- [ ] Multi-backup strategies (hourly, daily, weekly)
