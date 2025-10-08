namespace BudgetEase.Core.Interfaces;

public interface IBackupCleanupService
{
    Task CleanupOldBackupsAsync(CancellationToken cancellationToken = default);
}
