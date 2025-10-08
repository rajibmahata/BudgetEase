using BudgetEase.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetEase.Tests.Services;

public class BackupCleanupServiceTests : IDisposable
{
    private readonly Mock<ILogger<BackupCleanupService>> _mockLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly string _testBackupDirectory;

    public BackupCleanupServiceTests()
    {
        _mockLogger = new Mock<ILogger<BackupCleanupService>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _testBackupDirectory = Path.Combine(Path.GetTempPath(), $"TestBackups_{Guid.NewGuid()}");
        
        // Setup default configuration
        _mockConfiguration.Setup(c => c["BackupSettings:BackupDirectory"]).Returns(_testBackupDirectory);
        _mockConfiguration.Setup(c => c["BackupSettings:RetentionDays"]).Returns("30");
    }

    [Fact]
    public async Task CleanupOldBackupsAsync_DeletesFilesOlderThan30Days()
    {
        // Arrange
        Directory.CreateDirectory(_testBackupDirectory);
        
        // Create test files with different ages
        var oldFile = Path.Combine(_testBackupDirectory, "old_backup.db");
        var recentFile = Path.Combine(_testBackupDirectory, "recent_backup.db");
        
        File.WriteAllText(oldFile, "old data");
        File.WriteAllText(recentFile, "recent data");
        
        // Set file dates
        File.SetLastWriteTimeUtc(oldFile, DateTime.UtcNow.AddDays(-35));
        File.SetLastWriteTimeUtc(recentFile, DateTime.UtcNow.AddDays(-5));
        
        var service = new BackupCleanupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        await service.CleanupOldBackupsAsync();

        // Assert
        Assert.False(File.Exists(oldFile), "Old file should be deleted");
        Assert.True(File.Exists(recentFile), "Recent file should not be deleted");
    }

    [Fact]
    public async Task CleanupOldBackupsAsync_DoesNotDeleteFilesWithinRetentionPeriod()
    {
        // Arrange
        Directory.CreateDirectory(_testBackupDirectory);
        
        var file1 = Path.Combine(_testBackupDirectory, "backup1.db");
        var file2 = Path.Combine(_testBackupDirectory, "backup2.db");
        
        File.WriteAllText(file1, "data1");
        File.WriteAllText(file2, "data2");
        
        File.SetLastWriteTimeUtc(file1, DateTime.UtcNow.AddDays(-10));
        File.SetLastWriteTimeUtc(file2, DateTime.UtcNow.AddDays(-20));
        
        var service = new BackupCleanupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        await service.CleanupOldBackupsAsync();

        // Assert
        Assert.True(File.Exists(file1), "File 1 should not be deleted");
        Assert.True(File.Exists(file2), "File 2 should not be deleted");
    }

    [Fact]
    public async Task CleanupOldBackupsAsync_HandlesNonExistentDirectory()
    {
        // Arrange
        var nonExistentDir = Path.Combine(Path.GetTempPath(), $"NonExistent_{Guid.NewGuid()}");
        _mockConfiguration.Setup(c => c["BackupSettings:BackupDirectory"]).Returns(nonExistentDir);
        
        var service = new BackupCleanupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act & Assert - Should not throw
        await service.CleanupOldBackupsAsync();
    }

    [Fact]
    public async Task CleanupOldBackupsAsync_DeletesMultipleOldFiles()
    {
        // Arrange
        Directory.CreateDirectory(_testBackupDirectory);
        
        var oldFile1 = Path.Combine(_testBackupDirectory, "old_backup1.db");
        var oldFile2 = Path.Combine(_testBackupDirectory, "old_backup2.db");
        var oldFile3 = Path.Combine(_testBackupDirectory, "old_backup3.db");
        var recentFile = Path.Combine(_testBackupDirectory, "recent_backup.db");
        
        File.WriteAllText(oldFile1, "old data 1");
        File.WriteAllText(oldFile2, "old data 2");
        File.WriteAllText(oldFile3, "old data 3");
        File.WriteAllText(recentFile, "recent data");
        
        File.SetLastWriteTimeUtc(oldFile1, DateTime.UtcNow.AddDays(-31));
        File.SetLastWriteTimeUtc(oldFile2, DateTime.UtcNow.AddDays(-45));
        File.SetLastWriteTimeUtc(oldFile3, DateTime.UtcNow.AddDays(-60));
        File.SetLastWriteTimeUtc(recentFile, DateTime.UtcNow.AddDays(-5));
        
        var service = new BackupCleanupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        await service.CleanupOldBackupsAsync();

        // Assert
        Assert.False(File.Exists(oldFile1), "Old file 1 should be deleted");
        Assert.False(File.Exists(oldFile2), "Old file 2 should be deleted");
        Assert.False(File.Exists(oldFile3), "Old file 3 should be deleted");
        Assert.True(File.Exists(recentFile), "Recent file should not be deleted");
    }

    [Fact]
    public async Task CleanupOldBackupsAsync_HandlesEmptyDirectory()
    {
        // Arrange
        Directory.CreateDirectory(_testBackupDirectory);
        
        var service = new BackupCleanupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act & Assert - Should not throw
        await service.CleanupOldBackupsAsync();
    }

    [Fact]
    public async Task CleanupOldBackupsAsync_UsesCustomRetentionDays()
    {
        // Arrange
        Directory.CreateDirectory(_testBackupDirectory);
        _mockConfiguration.Setup(c => c["BackupSettings:RetentionDays"]).Returns("7");
        
        var oldFile = Path.Combine(_testBackupDirectory, "old_backup.db");
        var recentFile = Path.Combine(_testBackupDirectory, "recent_backup.db");
        
        File.WriteAllText(oldFile, "old data");
        File.WriteAllText(recentFile, "recent data");
        
        File.SetLastWriteTimeUtc(oldFile, DateTime.UtcNow.AddDays(-10));
        File.SetLastWriteTimeUtc(recentFile, DateTime.UtcNow.AddDays(-3));
        
        var service = new BackupCleanupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        await service.CleanupOldBackupsAsync();

        // Assert
        Assert.False(File.Exists(oldFile), "File older than 7 days should be deleted");
        Assert.True(File.Exists(recentFile), "File within 7 days should not be deleted");
    }

    public void Dispose()
    {
        // Cleanup test directory
        if (Directory.Exists(_testBackupDirectory))
        {
            try
            {
                Directory.Delete(_testBackupDirectory, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }
}
