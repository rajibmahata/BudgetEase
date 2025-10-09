namespace BudgetEase.Core.Interfaces;

/// <summary>
/// Service for managing database backups and restores
/// </summary>
public interface IDatabaseBackupService
{
    /// <summary>
    /// Creates a backup of the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Path to the created backup file</returns>
    Task<string> CreateBackupAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores the database from the latest backup if database doesn't exist
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if restore was performed, false if database already exists</returns>
    Task<bool> RestoreLatestBackupAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all available backup files ordered by creation date descending
    /// </summary>
    /// <returns>List of backup file paths</returns>
    Task<IEnumerable<string>> GetAvailableBackupsAsync();
}
