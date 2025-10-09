using BudgetEase.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetEase.Tests.Services;

public class DatabaseBackupServiceTests : IDisposable
{
    private readonly Mock<ILogger<DatabaseBackupService>> _mockLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IConfigurationSection> _mockConnectionStringsSection;
    private readonly string _testBackupDirectory;
    private readonly string _testDatabasePath;

    public DatabaseBackupServiceTests()
    {
        _mockLogger = new Mock<ILogger<DatabaseBackupService>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConnectionStringsSection = new Mock<IConfigurationSection>();
        _testBackupDirectory = Path.Combine(Path.GetTempPath(), $"TestDatabaseBackups_{Guid.NewGuid()}");
        _testDatabasePath = Path.Combine(Path.GetTempPath(), $"test_budgetease_{Guid.NewGuid()}.db");
        
        // Setup configuration
        _mockConfiguration.Setup(c => c["DatabaseBackup:BackupDirectory"]).Returns(_testBackupDirectory);
        _mockConfiguration.Setup(c => c["ConnectionStrings:DefaultConnection"]).Returns($"Data Source={_testDatabasePath}");
    }

    [Fact]
    public async Task CreateBackupAsync_CreatesBackupFile()
    {
        // Arrange
        Directory.CreateDirectory(_testBackupDirectory);
        File.WriteAllText(_testDatabasePath, "test database content");
        
        var service = new DatabaseBackupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        var backupPath = await service.CreateBackupAsync();

        // Assert
        Assert.NotEmpty(backupPath);
        Assert.True(File.Exists(backupPath), "Backup file should exist");
        Assert.Contains(_testBackupDirectory, backupPath);
        Assert.Contains("budgetease_backup_", Path.GetFileName(backupPath));
    }

    [Fact]
    public async Task CreateBackupAsync_ReturnsEmptyString_WhenDatabaseDoesNotExist()
    {
        // Arrange
        var service = new DatabaseBackupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        var backupPath = await service.CreateBackupAsync();

        // Assert
        Assert.Empty(backupPath);
    }

    [Fact]
    public async Task CreateBackupAsync_CreatesBackupDirectory_IfNotExists()
    {
        // Arrange
        File.WriteAllText(_testDatabasePath, "test database content");
        var service = new DatabaseBackupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        await service.CreateBackupAsync();

        // Assert
        Assert.True(Directory.Exists(_testBackupDirectory), "Backup directory should be created");
    }

    [Fact]
    public async Task RestoreLatestBackupAsync_ReturnsFalse_WhenDatabaseExists()
    {
        // Arrange
        File.WriteAllText(_testDatabasePath, "existing database");
        var service = new DatabaseBackupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        var restored = await service.RestoreLatestBackupAsync();

        // Assert
        Assert.False(restored, "Should not restore when database exists");
    }

    [Fact]
    public async Task RestoreLatestBackupAsync_ReturnsFalse_WhenNoBackupsAvailable()
    {
        // Arrange
        var service = new DatabaseBackupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        var restored = await service.RestoreLatestBackupAsync();

        // Assert
        Assert.False(restored, "Should return false when no backups available");
        Assert.False(File.Exists(_testDatabasePath), "Database should not be created");
    }

    [Fact]
    public async Task RestoreLatestBackupAsync_RestoresFromLatestBackup()
    {
        // Arrange
        Directory.CreateDirectory(_testBackupDirectory);
        
        // Create multiple backup files
        var oldBackup = Path.Combine(_testBackupDirectory, "budgetease_backup_20240101_120000.db");
        var newBackup = Path.Combine(_testBackupDirectory, "budgetease_backup_20240102_120000.db");
        
        File.WriteAllText(oldBackup, "old backup content");
        File.WriteAllText(newBackup, "new backup content");
        
        // Set creation times
        File.SetCreationTimeUtc(oldBackup, DateTime.UtcNow.AddDays(-2));
        File.SetCreationTimeUtc(newBackup, DateTime.UtcNow.AddDays(-1));
        
        var service = new DatabaseBackupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        var restored = await service.RestoreLatestBackupAsync();

        // Assert
        Assert.True(restored, "Should restore successfully");
        Assert.True(File.Exists(_testDatabasePath), "Database should be restored");
        var restoredContent = await File.ReadAllTextAsync(_testDatabasePath);
        Assert.Equal("new backup content", restoredContent);
    }

    [Fact]
    public async Task GetAvailableBackupsAsync_ReturnsBackupsOrderedByCreationDate()
    {
        // Arrange
        Directory.CreateDirectory(_testBackupDirectory);
        
        var backup1 = Path.Combine(_testBackupDirectory, "budgetease_backup_1.db");
        var backup2 = Path.Combine(_testBackupDirectory, "budgetease_backup_2.db");
        var backup3 = Path.Combine(_testBackupDirectory, "budgetease_backup_3.db");
        
        File.WriteAllText(backup1, "backup1");
        File.WriteAllText(backup2, "backup2");
        File.WriteAllText(backup3, "backup3");
        
        File.SetCreationTimeUtc(backup1, DateTime.UtcNow.AddDays(-3));
        File.SetCreationTimeUtc(backup2, DateTime.UtcNow.AddDays(-1));
        File.SetCreationTimeUtc(backup3, DateTime.UtcNow.AddDays(-2));
        
        var service = new DatabaseBackupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        var backups = await service.GetAvailableBackupsAsync();

        // Assert
        var backupList = backups.ToList();
        Assert.Equal(3, backupList.Count);
        Assert.Contains("backup_2.db", backupList[0]); // Most recent
        Assert.Contains("backup_3.db", backupList[1]);
        Assert.Contains("backup_1.db", backupList[2]); // Oldest
    }

    [Fact]
    public async Task GetAvailableBackupsAsync_ReturnsEmpty_WhenNoBackups()
    {
        // Arrange
        var service = new DatabaseBackupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        var backups = await service.GetAvailableBackupsAsync();

        // Assert
        Assert.Empty(backups);
    }

    [Fact]
    public async Task CreateBackupAsync_WithMultipleBackups_CreatesUniqueFiles()
    {
        // Arrange
        Directory.CreateDirectory(_testBackupDirectory);
        File.WriteAllText(_testDatabasePath, "test database content");
        
        var service = new DatabaseBackupService(_mockLogger.Object, _mockConfiguration.Object);

        // Act
        var backup1 = await service.CreateBackupAsync();
        await Task.Delay(1100); // Ensure different timestamp (seconds precision)
        var backup2 = await service.CreateBackupAsync();

        // Assert
        Assert.NotEmpty(backup1);
        Assert.NotEmpty(backup2);
        Assert.NotEqual(backup1, backup2);
        Assert.True(File.Exists(backup1));
        Assert.True(File.Exists(backup2));
    }

    public void Dispose()
    {
        // Cleanup test files and directories
        if (File.Exists(_testDatabasePath))
        {
            try
            {
                File.Delete(_testDatabasePath);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

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
