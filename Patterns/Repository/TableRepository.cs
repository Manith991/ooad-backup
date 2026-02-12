using System;
using System.Collections.Generic;
using Npgsql;
using OOAD_Project.Domain;

namespace OOAD_Project.Patterns.Repository
{
    /// <summary>
    /// REPOSITORY PATTERN - Table data access
    /// Centralizes all table-related database operations
    /// </summary>
    public class TableRepository : IRepository<Table>
    {
        public Table GetById(int id)
        {
            string query = "SELECT table_id, table_name, capacity, status FROM tables WHERE table_id = @id LIMIT 1;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Table
                                {
                                    TableId = reader.GetInt32(0),
                                    TableName = reader.GetString(1),
                                    Capacity = reader.GetInt32(2),
                                    Status = reader.GetString(3)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TableRepository] Error getting table by ID: {ex.Message}");
            }

            return null;
        }

        public IEnumerable<Table> GetAll()
        {
            var tables = new List<Table>();
            string query = "SELECT table_id, table_name, capacity, status FROM tables ORDER BY table_id ASC;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(new Table
                            {
                                TableId = reader.GetInt32(0),
                                TableName = reader.GetString(1),
                                Capacity = reader.GetInt32(2),
                                Status = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TableRepository] Error getting all tables: {ex.Message}");
            }

            return tables;
        }

        public int Add(Table entity)
        {
            string query = @"
                INSERT INTO tables (table_name, capacity, status)
                VALUES (@name, @cap, @status)
                RETURNING table_id;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", entity.TableName);
                        cmd.Parameters.AddWithValue("@cap", entity.Capacity);
                        cmd.Parameters.AddWithValue("@status", entity.Status);

                        var result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TableRepository] Error adding table: {ex.Message}");
                throw;
            }
        }

        public void Update(Table entity)
        {
            string query = @"
                UPDATE tables 
                SET table_name = @name, 
                    capacity = @cap, 
                    status = @status
                WHERE table_id = @id;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", entity.TableName);
                        cmd.Parameters.AddWithValue("@cap", entity.Capacity);
                        cmd.Parameters.AddWithValue("@status", entity.Status);
                        cmd.Parameters.AddWithValue("@id", entity.TableId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TableRepository] Error updating table: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM tables WHERE table_id = @id;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TableRepository] Error deleting table: {ex.Message}");
                throw;
            }
        }

        public bool Exists(int id)
        {
            string query = "SELECT COUNT(*) FROM tables WHERE table_id = @id;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TableRepository] Error checking table existence: {ex.Message}");
                return false;
            }
        }

        public void UpdateStatus(int tableId, string status)
        {
            string query = "UPDATE tables SET status = @status WHERE table_id = @id;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@id", tableId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TableRepository] Error updating table status: {ex.Message}");
                throw;
            }
        }
    }
}