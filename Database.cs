using Npgsql;
using System.Data;

namespace OOAD_Project
{
    public static class Database
    {
        private static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=db22056#;Database=postgres;";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        public static DataTable GetData(string query)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var da = new NpgsqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static void Execute(string query, params NpgsqlParameter[] parameters)
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
    }
}