namespace BudgetEase.Core.Interfaces;

public interface IDatabaseBackupService
{
    Task<string> BackupDatabaseAsync(CancellationToken cancellationToken = default);
    Task<bool> RestoreLatestBackupAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetAvailableBackupsAsync();
    Task CleanupOldBackupsAsync(int keepDays = 30);
}
