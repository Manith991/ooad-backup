using Npgsql;
using System;
using System.Data;
using System.IO;
using System.Text.Json;

namespace OOAD_Project.Patterns
{
    /// <summary>
    /// SINGLETON PATTERN - Secure Implementation
    /// Reads connection string from appsettings.json instead of hardcoding
    /// </summary>
    public sealed class DatabaseManager
    {
        #region Singleton Implementation

        private static DatabaseManager? _instance = null;
        private static readonly object _lock = new object();
        private readonly string _connectionString;

        /// <summary>
        /// Private constructor - loads connection string from config file
        /// </summary>
        private DatabaseManager()
        {
            _connectionString = LoadConnectionString();
        }

        /// <summary>
        /// Thread-safe singleton instance
        /// </summary>
        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Configuration Loading

        /// <summary>
        /// Load connection string from appsettings.json
        /// Falls back to default if file not found (for development)
        /// </summary>
        private string LoadConnectionString()
        {
            try
            {
                // Get the application directory
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // ✅ CORRECT - Use relative path
                string configPath = Path.Combine(baseDirectory, "appsettings.json");

                // If not found in base directory, try project root
                if (!File.Exists(configPath))
                {
                    // Go up to find project root
                    string? projectRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
                    if (projectRoot != null)
                    {
                        configPath = Path.Combine(projectRoot, "appsettings.json");
                    }
                }

                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    using JsonDocument doc = JsonDocument.Parse(json);

                    var connectionString = doc.RootElement
                        .GetProperty("ConnectionStrings")
                        .GetProperty("DefaultConnection")
                        .GetString();

                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        Console.WriteLine("[DatabaseManager] ✅ Connection string loaded from appsettings.json");
                        return connectionString;
                    }
                }

                // Config file not found - show warning
                Console.WriteLine("[DatabaseManager] ⚠️ appsettings.json not found, using fallback connection");
                System.Windows.Forms.MessageBox.Show(
                    "Configuration file 'appsettings.json' not found.\n\n" +
                    "Please copy 'appsettings.example.json' to 'appsettings.json' " +
                    "and update with your database credentials.",
                    "Configuration Missing",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DatabaseManager] ❌ Error loading config: {ex.Message}");
            }

            // ⚠️ FALLBACK - Only for local development
            return "Host=localhost;Port=5432;Username=postgres;Password=YOUR_PASSWORD;Database=postgres;";
        }

        #endregion

        #region Database Operations

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public DataTable GetData(string query)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        public void Execute(string query, params NpgsqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public object? ExecuteScalar(string query, params NpgsqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}