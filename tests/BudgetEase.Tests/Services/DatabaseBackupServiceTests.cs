using BudgetEase.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BudgetEase.Tests.Services;

public class DatabaseBackupServiceTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly string _testDbPath;
    private readonly string _backupDirectory;
    private readonly IConfiguration _configuration;
    private readonly Mock<ILogger<DatabaseBackupService>> _loggerMock;

    public DatabaseBackupServiceTests()
    {
        // Create a unique test directory for this test run
        _testDirectory = Path.Combine(Path.GetTempPath(), "BudgetEaseTests_" + Guid.NewGuid().ToString());
        _testDbPath = Path.Combine(_testDirectory, "test.db");
        _backupDirectory = Path.Combine(_testDirectory, "Backups");
        
        Directory.CreateDirectory(_testDirectory);
        
        // Create real configuration object
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "ConnectionStrings:DefaultConnection", $"Data Source={_testDbPath}" },
            { "DatabaseBackup:BackupDirectory", _backupDirectory },
            { "DatabaseBackup:RetentionDays", "7" }
        });
        _configuration = configBuilder.Build();
        
        _loggerMock = new Mock<ILogger<DatabaseBackupService>>();
    }

    [Fact]
    public async Task BackupDatabaseAsync_CreatesBackupFile_WhenDatabaseExists()
    {
        // Arrange
        File.WriteAllText(_testDbPath, "test database content");
        var service = new DatabaseBackupService(_configuration, _loggerMock.Object);

        // Act
        var backupPath = await service.BackupDatabaseAsync();

        // Assert
        Assert.False(string.IsNullOrEmpty(backupPath));
        Assert.True(File.Exists(backupPath));
        Assert.Contains("budgetease_backup_", backupPath);
        Assert.Equal("test database content", File.ReadAllText(backupPath));
    }

    [Fact]
    public async Task BackupDatabaseAsync_ReturnsEmpty_WhenDatabaseDoesNotExist()
    {
        // Arrange
        var service = new DatabaseBackupService(_configuration, _loggerMock.Object);

        // Act
        var backupPath = await service.BackupDatabaseAsync();

        // Assert
        Assert.Empty(backupPath);
    }

    [Fact]
    public async Task RestoreLatestBackupAsync_RestoresDatabase_WhenBackupExists()
    {
        // Arrange
        Directory.CreateDirectory(_backupDirectory);
        var backupPath = Path.Combine(_backupDirectory, "budgetease_backup_20240101_120000.db");
        File.WriteAllText(backupPath, "backup content");
        
        var service = new DatabaseBackupService(_configuration, _loggerMock.Object);

        // Act
        var restored = await service.RestoreLatestBackupAsync();

        // Assert
        Assert.True(restored);
        Assert.True(File.Exists(_testDbPath));
        Assert.Equal("backup content", File.ReadAllText(_testDbPath));
    }

    [Fact]
    public async Task RestoreLatestBackupAsync_ReturnsFalse_WhenDatabaseAlreadyExists()
    {
        // Arrange
        File.WriteAllText(_testDbPath, "existing database");
        Directory.CreateDirectory(_backupDirectory);
        var backupPath = Path.Combine(_backupDirectory, "budgetease_backup_20240101_120000.db");
        File.WriteAllText(backupPath, "backup content");
        
        var service = new DatabaseBackupService(_configuration, _loggerMock.Object);

        // Act
        var restored = await service.RestoreLatestBackupAsync();

        // Assert
        Assert.False(restored);
        Assert.Equal("existing database", File.ReadAllText(_testDbPath));
    }

    [Fact]
    public async Task RestoreLatestBackupAsync_ReturnsFalse_WhenNoBackupsExist()
    {
        // Arrange
        var service = new DatabaseBackupService(_configuration, _loggerMock.Object);

        // Act
        var restored = await service.RestoreLatestBackupAsync();

        // Assert
        Assert.False(restored);
        Assert.False(File.Exists(_testDbPath));
    }

    [Fact]
    public async Task GetAvailableBackupsAsync_ReturnsBackupsInDescendingOrder()
    {
        // Arrange
        Directory.CreateDirectory(_backupDirectory);
        
        var backup1 = Path.Combine(_backupDirectory, "budgetease_backup_20240101_120000.db");
        var backup2 = Path.Combine(_backupDirectory, "budgetease_backup_20240102_120000.db");
        var backup3 = Path.Combine(_backupDirectory, "budgetease_backup_20240103_120000.db");
        
        File.WriteAllText(backup1, "backup1");
        await Task.Delay(10);
        File.WriteAllText(backup2, "backup2");
        await Task.Delay(10);
        File.WriteAllText(backup3, "backup3");
        
        var service = new DatabaseBackupService(_configuration, _loggerMock.Object);

        // Act
        var backups = (await service.GetAvailableBackupsAsync()).ToList();

        // Assert
        Assert.Equal(3, backups.Count);
        // Most recent should be first
        Assert.Contains("20240103", backups[0]);
    }

    [Fact]
    public async Task CleanupOldBackupsAsync_DeletesOldBackups()
    {
        // Arrange
        Directory.CreateDirectory(_backupDirectory);
        
        var oldBackup = Path.Combine(_backupDirectory, "budgetease_backup_20200101_120000.db");
        var recentBackup = Path.Combine(_backupDirectory, "budgetease_backup_20991231_120000.db");
        
        File.WriteAllText(oldBackup, "old backup");
        File.SetCreationTimeUtc(oldBackup, DateTime.UtcNow.AddDays(-10));
        
        File.WriteAllText(recentBackup, "recent backup");
        
        var service = new DatabaseBackupService(_configuration, _loggerMock.Object);

        // Act
        await service.CleanupOldBackupsAsync(keepDays: 7);

        // Assert
        Assert.False(File.Exists(oldBackup));
        Assert.True(File.Exists(recentBackup));
    }

    public void Dispose()
    {
        // Cleanup test directory
        try
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, recursive: true);
            }
        }
        catch
        {
            // Ignore cleanup errors
        }
    }
}
