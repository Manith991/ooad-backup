using Npgsql;
using System.Data;
using OOAD_Project.Patterns;

namespace OOAD_Project
{
    /// <summary>
    /// Static Facade Pattern - Provides convenient static methods
    /// while internally using the DatabaseManager Singleton.
    /// 
    /// ✅ NO HARDCODED PASSWORD - delegates to Singleton which reads from appsettings.json
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Get a database connection via the Singleton
        /// </summary>
        public static NpgsqlConnection GetConnection()
        {
            // ✅ Delegates to Singleton (which reads from appsettings.json)
            return DatabaseManager.Instance.GetConnection();
        }

        /// <summary>
        /// Execute a SELECT query and return DataTable
        /// </summary>
        public static DataTable GetData(string query)
        {
            // ✅ Delegates to Singleton
            return DatabaseManager.Instance.GetData(query);
        }

        /// <summary>
        /// Execute INSERT, UPDATE, DELETE queries
        /// </summary>
        public static void Execute(string query, params NpgsqlParameter[] parameters)
        {
            // ✅ Delegates to Singleton
            DatabaseManager.Instance.Execute(query, parameters);
        }

        /// <summary>
        /// Execute query and return scalar result
        /// </summary>
        public static object? ExecuteScalar(string query, params NpgsqlParameter[] parameters)
        {
            // ✅ Delegates to Singleton
            return DatabaseManager.Instance.ExecuteScalar(query, parameters);
        }

        /// <summary>
        /// Test database connection
        /// </summary>
        public static bool TestConnection()
        {
            // ✅ Delegates to Singleton
            return DatabaseManager.Instance.TestConnection();
        }
    }
}