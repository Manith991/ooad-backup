using System;
using System.Collections.Generic;
using Npgsql;
using OOAD_Project.Domain;

namespace OOAD_Project.Patterns.Repository
{
    /// <summary>
    /// REPOSITORY PATTERN - User/Staff data access
    /// Centralizes all user-related database operations
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        public User GetById(int id)
        {
            string query = "SELECT id, username, name, role, status, image FROM users WHERE id = @id LIMIT 1;";

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
                                return MapReaderToUser(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserRepository] Error getting user by ID: {ex.Message}");
            }

            return null;
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            string query = "SELECT id, username, name, role, status, image FROM users ORDER BY id ASC;";

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
                            users.Add(MapReaderToUser(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserRepository] Error getting all users: {ex.Message}");
            }

            return users;
        }

        public IEnumerable<User> GetByRole(string role)
        {
            var users = new List<User>();
            string query = "SELECT id, username, name, role, status, image FROM users WHERE role = @role ORDER BY id ASC;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@role", role);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(MapReaderToUser(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserRepository] Error getting users by role: {ex.Message}");
            }

            return users;
        }

        public int Add(User entity)
        {
            string query = @"
                INSERT INTO users (username, name, role, status, image)
                VALUES (@username, @name, @role, @status, @image)
                RETURNING id;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", entity.Username ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@name", entity.Name);
                        cmd.Parameters.AddWithValue("@role", entity.Role);
                        cmd.Parameters.AddWithValue("@status", entity.Status ?? "Active");
                        cmd.Parameters.AddWithValue("@image", entity.Image ?? (object)DBNull.Value);

                        var result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserRepository] Error adding user: {ex.Message}");
                throw;
            }
        }

        public void Update(User entity)
        {
            string query = @"
                UPDATE users 
                SET name = @name, 
                    role = @role, 
                    status = @status, 
                    image = @image
                WHERE id = @id;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", entity.Name);
                        cmd.Parameters.AddWithValue("@role", entity.Role);
                        cmd.Parameters.AddWithValue("@status", entity.Status ?? "Active");
                        cmd.Parameters.AddWithValue("@image", entity.Image ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@id", entity.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserRepository] Error updating user: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM users WHERE id = @id;";

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
                Console.WriteLine($"[UserRepository] Error deleting user: {ex.Message}");
                throw;
            }
        }

        public bool Exists(int id)
        {
            string query = "SELECT COUNT(*) FROM users WHERE id = @id;";

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
                Console.WriteLine($"[UserRepository] Error checking user existence: {ex.Message}");
                return false;
            }
        }

        // Helper method to map database reader to User object
        private User MapReaderToUser(NpgsqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.IsDBNull(1) ? null : reader.GetString(1),
                Name = reader.GetString(2),
                Role = reader.GetString(3),
                Status = reader.IsDBNull(4) ? "Active" : reader.GetString(4),
                Image = reader.IsDBNull(5) ? null : reader.GetString(5)
            };
        }
    }
}