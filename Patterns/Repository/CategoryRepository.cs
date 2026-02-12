using System;
using System.Collections.Generic;
using Npgsql;
using OOAD_Project.Domain;

namespace OOAD_Project.Patterns.Repository
{
    /// <summary>
    /// REPOSITORY PATTERN - Category data access
    /// Centralizes all category-related database operations
    /// </summary>
    public class CategoryRepository : IRepository<Category>
    {
        public Category GetById(int id)
        {
            string query = "SELECT categoryid, categoryname, imagepath FROM categories WHERE categoryid = @id LIMIT 1;";

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
                                return new Category
                                {
                                    CategoryId = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1),
                                    ImagePath = reader.IsDBNull(2) ? null : reader.GetString(2)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoryRepository] Error getting category by ID: {ex.Message}");
            }

            return null;
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = new List<Category>();
            string query = "SELECT categoryid, categoryname, imagepath FROM categories ORDER BY categoryid ASC;";

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
                            categories.Add(new Category
                            {
                                CategoryId = reader.GetInt32(0),
                                CategoryName = reader.GetString(1),
                                ImagePath = reader.IsDBNull(2) ? null : reader.GetString(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoryRepository] Error getting all categories: {ex.Message}");
            }

            return categories;
        }

        public int Add(Category entity)
        {
            string query = @"
                INSERT INTO categories (categoryname, imagepath)
                VALUES (@name, @image)
                RETURNING categoryid;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", entity.CategoryName);
                        cmd.Parameters.AddWithValue("@image", entity.ImagePath ?? (object)DBNull.Value);

                        var result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoryRepository] Error adding category: {ex.Message}");
                throw;
            }
        }

        public void Update(Category entity)
        {
            string query = @"
                UPDATE categories 
                SET categoryname = @name, 
                    imagepath = @image 
                WHERE categoryid = @id;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", entity.CategoryName);
                        cmd.Parameters.AddWithValue("@image", entity.ImagePath ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@id", entity.CategoryId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CategoryRepository] Error updating category: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM categories WHERE categoryid = @id;";

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
                Console.WriteLine($"[CategoryRepository] Error deleting category: {ex.Message}");
                throw;
            }
        }

        public bool Exists(int id)
        {
            string query = "SELECT COUNT(*) FROM categories WHERE categoryid = @id;";

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
                Console.WriteLine($"[CategoryRepository] Error checking category existence: {ex.Message}");
                return false;
            }
        }
    }
}