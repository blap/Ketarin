using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ketarin
{
    /// <summary>
    /// Provides database migration utilities for .NET 6 migration.
    /// Handles SQLite database compatibility and migration tasks.
    /// </summary>
    internal static class DatabaseMigrationHelper
    {
        private const string DatabaseVersionKey = "DatabaseVersion";
        private const int CurrentDatabaseVersion = 2;

        #region Migration Management

        /// <summary>
        /// Checks if database migration is needed and performs it if necessary.
        /// </summary>
        public static async Task<bool> MigrateDatabaseIfNeededAsync(string databasePath)
        {
            if (!File.Exists(databasePath))
            {
                // Create new database with current schema
                await CreateDatabaseAsync(databasePath);
                return true;
            }

            var currentVersion = await GetDatabaseVersionAsync(databasePath);
            if (currentVersion >= CurrentDatabaseVersion)
            {
                return false; // No migration needed
            }

            // Perform migration
            await PerformMigrationAsync(databasePath, currentVersion);
            return true;
        }

        /// <summary>
        /// Gets the current database version.
        /// </summary>
        public static async Task<int> GetDatabaseVersionAsync(string databasePath)
        {
            try
            {
                using var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT SettingValue FROM settings WHERE SettingPath = @VersionKey";
                command.Parameters.Add(new SQLiteParameter("@VersionKey", DatabaseVersionKey));

                var result = await command.ExecuteScalarAsync();
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch
            {
                // If we can't read version, assume version 0
                return 0;
            }
        }

        /// <summary>
        /// Sets the database version.
        /// </summary>
        private static async Task SetDatabaseVersionAsync(SQLiteConnection connection, int version)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT OR REPLACE INTO settings (SettingPath, SettingValue)
                VALUES (@VersionKey, @Version)";

            command.Parameters.Add(new SQLiteParameter("@VersionKey", DatabaseVersionKey));
            command.Parameters.Add(new SQLiteParameter("@Version", version.ToString()));

            await command.ExecuteNonQueryAsync();
        }

        #endregion

        #region Database Creation

        /// <summary>
        /// Creates a new database with the current schema.
        /// </summary>
        public static async Task CreateDatabaseAsync(string databasePath)
        {
            // Ensure directory exists
            var directory = Path.GetDirectoryName(databasePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
            await connection.OpenAsync();

            // Create tables
            await CreateTablesAsync(connection);

            // Set database version
            await SetDatabaseVersionAsync(connection, CurrentDatabaseVersion);
        }

        /// <summary>
        /// Creates all necessary database tables.
        /// </summary>
        private static async Task CreateTablesAsync(SQLiteConnection connection)
        {
            var createTableCommands = new[]
            {
                // Applications table
                @"CREATE TABLE IF NOT EXISTS applications (
                    ApplicationId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ApplicationName TEXT NOT NULL,
                    ApplicationGuid TEXT UNIQUE NOT NULL,
                    ApplicationUrl TEXT,
                    DownloadUrl TEXT,
                    FileHippoId TEXT,
                    PreviousLocation TEXT,
                    ShareApplication INTEGER DEFAULT 0,
                    CanBeShared INTEGER DEFAULT 1,
                    Enabled INTEGER DEFAULT 1,
                    CheckForUpdatesOnly INTEGER DEFAULT 0,
                    LastUpdated DATETIME,
                    SaveToFile TEXT,
                    ExecuteCommand TEXT,
                    ExecutePreCommand TEXT,
                    ExecutePostCommand TEXT,
                    Category TEXT,
                    PreviousVersion TEXT,
                    IgnoreFileInformation INTEGER DEFAULT 0,
                    DownloadBeta INTEGER DEFAULT 0,
                    DownloadDate DATETIME,
                    FixedDownloadUrl TEXT,
                    ExecuteCommandType INTEGER DEFAULT 0,
                    ExecutePreCommandType INTEGER DEFAULT 0,
                    ExecutePostCommandType INTEGER DEFAULT 0,
                    FailureCount INTEGER DEFAULT 0,
                    SearchUrl TEXT,
                    SearchExpression TEXT,
                    SearchReplace TEXT,
                    TargetPath TEXT,
                    DeletePreviousFile INTEGER DEFAULT 0,
                    DownloadSource INTEGER DEFAULT 0,
                    UserAgent TEXT,
                    UserNotes TEXT,
                    VariableChangeIndicator TEXT,
                    VariableChangeIndicatorUrl TEXT,
                    SetupInstructionId INTEGER,
                    HashType TEXT,
                    Hash TEXT,
                    HashVariable TEXT,
                    StartInterval INTEGER DEFAULT 0,
                    EndInterval INTEGER DEFAULT 0,
                    EmbeddedSetupInstruction TEXT,
                    RegexRightToLeft INTEGER DEFAULT 0,
                    WebsiteUrl TEXT,
                    CanBeSharedComputed INTEGER DEFAULT 1,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                )",

                // Variables table
                @"CREATE TABLE IF NOT EXISTS variables (
                    VariableId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ApplicationId INTEGER NOT NULL,
                    VariableName TEXT NOT NULL,
                    VariableValue TEXT,
                    VariableType INTEGER DEFAULT 0,
                    VariableUrl TEXT,
                    VariablePostData TEXT,
                    VariableRegex TEXT,
                    VariableStartText TEXT,
                    VariableEndText TEXT,
                    VariableContentType TEXT,
                    VariableCachedContent TEXT,
                    Enabled INTEGER DEFAULT 1,
                    FOREIGN KEY (ApplicationId) REFERENCES applications(ApplicationId) ON DELETE CASCADE
                )",

                // Settings table
                @"CREATE TABLE IF NOT EXISTS settings (
                    SettingId INTEGER PRIMARY KEY AUTOINCREMENT,
                    SettingPath TEXT UNIQUE NOT NULL,
                    SettingValue TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                )",

                // Setup instructions table
                @"CREATE TABLE IF NOT EXISTS setup_instructions (
                    SetupInstructionId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Type TEXT NOT NULL,
                    Data TEXT,
                    Enabled INTEGER DEFAULT 1,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                )",

                // Categories table
                @"CREATE TABLE IF NOT EXISTS categories (
                    CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
                    CategoryName TEXT UNIQUE NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                )"
            };

            foreach (var commandText in createTableCommands)
            {
                using var command = connection.CreateCommand();
                command.CommandText = commandText;
                await command.ExecuteNonQueryAsync();
            }

            // Create indexes for better performance
            await CreateIndexesAsync(connection);
        }

        /// <summary>
        /// Creates database indexes for better performance.
        /// </summary>
        private static async Task CreateIndexesAsync(SQLiteConnection connection)
        {
            var indexCommands = new[]
            {
                "CREATE INDEX IF NOT EXISTS idx_applications_guid ON applications(ApplicationGuid)",
                "CREATE INDEX IF NOT EXISTS idx_applications_name ON applications(ApplicationName)",
                "CREATE INDEX IF NOT EXISTS idx_applications_category ON applications(Category)",
                "CREATE INDEX IF NOT EXISTS idx_variables_app_id ON variables(ApplicationId)",
                "CREATE INDEX IF NOT EXISTS idx_variables_name ON variables(VariableName)",
                "CREATE INDEX IF NOT EXISTS idx_settings_path ON settings(SettingPath)"
            };

            foreach (var commandText in indexCommands)
            {
                using var command = connection.CreateCommand();
                command.CommandText = commandText;
                await command.ExecuteNonQueryAsync();
            }
        }

        #endregion

        #region Migration Logic

        /// <summary>
        /// Performs database migration from the specified version.
        /// </summary>
        private static async Task PerformMigrationAsync(string databasePath, int fromVersion)
        {
            using var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                for (int version = fromVersion + 1; version <= CurrentDatabaseVersion; version++)
                {
                    await ApplyMigrationStepAsync(connection, version);
                }

                await SetDatabaseVersionAsync(connection, CurrentDatabaseVersion);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Applies a specific migration step.
        /// </summary>
        private static async Task ApplyMigrationStepAsync(SQLiteConnection connection, int targetVersion)
        {
            switch (targetVersion)
            {
                case 1:
                    await ApplyMigrationV1Async(connection);
                    break;
                case 2:
                    await ApplyMigrationV2Async(connection);
                    break;
                default:
                    throw new NotSupportedException($"Migration to version {targetVersion} is not supported.");
            }
        }

        /// <summary>
        /// Migration to version 1: Add new columns and indexes.
        /// </summary>
        private static async Task ApplyMigrationV1Async(SQLiteConnection connection)
        {
            var migrationCommands = new[]
            {
                "ALTER TABLE applications ADD COLUMN CanBeSharedComputed INTEGER DEFAULT 1",
                "ALTER TABLE applications ADD COLUMN CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP",
                "ALTER TABLE applications ADD COLUMN UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP",
                "ALTER TABLE variables ADD COLUMN VariableCachedContent TEXT",
                "CREATE INDEX IF NOT EXISTS idx_applications_created ON applications(CreatedAt)",
                "CREATE INDEX IF NOT EXISTS idx_variables_cached ON variables(VariableCachedContent)"
            };

            foreach (var commandText in migrationCommands)
            {
                using var command = connection.CreateCommand();
                command.CommandText = commandText;
                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Migration to version 2: Performance optimizations and new features.
        /// </summary>
        private static async Task ApplyMigrationV2Async(SQLiteConnection connection)
        {
            var migrationCommands = new[]
            {
                "ALTER TABLE applications ADD COLUMN RegexRightToLeft INTEGER DEFAULT 0",
                "ALTER TABLE applications ADD COLUMN WebsiteUrl TEXT",
                "CREATE INDEX IF NOT EXISTS idx_applications_website ON applications(WebsiteUrl)",
                "CREATE INDEX IF NOT EXISTS idx_applications_regex_rtl ON applications(RegexRightToLeft)"
            };

            foreach (var commandText in migrationCommands)
            {
                using var command = connection.CreateCommand();
                command.CommandText = commandText;
                await command.ExecuteNonQueryAsync();
            }
        }

        #endregion

        #region Database Maintenance

        /// <summary>
        /// Performs database maintenance operations.
        /// </summary>
        public static async Task PerformMaintenanceAsync(string databasePath)
        {
            using var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
            await connection.OpenAsync();

            // Vacuum database to reclaim space
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "VACUUM";
                await command.ExecuteNonQueryAsync();
            }

            // Analyze database for query optimization
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "ANALYZE";
                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Validates database integrity.
        /// </summary>
        public static async Task<bool> ValidateDatabaseAsync(string databasePath)
        {
            try
            {
                using var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "PRAGMA integrity_check";
                var result = await command.ExecuteScalarAsync();

                return result?.ToString() == "ok";
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a backup of the database.
        /// </summary>
        public static async Task CreateBackupAsync(string databasePath, string backupPath)
        {
            // Ensure backup directory exists
            var directory = Path.GetDirectoryName(backupPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create backup using SQLite backup API
            using var sourceConnection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
            await sourceConnection.OpenAsync();

            using var backupConnection = new SQLiteConnection($"Data Source={backupPath};Version=3;");
            await backupConnection.OpenAsync();

            sourceConnection.BackupDatabase(backupConnection, "main", "main", -1, null, 0);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Gets database statistics.
        /// </summary>
        public static async Task<DatabaseStats> GetDatabaseStatsAsync(string databasePath)
        {
            using var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
            await connection.OpenAsync();

            var stats = new DatabaseStats();

            // Get table counts
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM applications";
                stats.ApplicationCount = Convert.ToInt32(await command.ExecuteScalarAsync());
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM variables";
                stats.VariableCount = Convert.ToInt32(await command.ExecuteScalarAsync());
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM settings";
                stats.SettingCount = Convert.ToInt32(await command.ExecuteScalarAsync());
            }

            // Get database file size
            stats.DatabaseSize = new FileInfo(databasePath).Length;

            return stats;
        }

        #endregion
    }

    /// <summary>
    /// Database statistics structure.
    /// </summary>
    public class DatabaseStats
    {
        public int ApplicationCount { get; set; }
        public int VariableCount { get; set; }
        public int SettingCount { get; set; }
        public long DatabaseSize { get; set; }
    }
}