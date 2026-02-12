using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using OOAD_Project.Domain;

namespace OOAD_Project.Patterns.Repository
{
    /// <summary>
    /// REPOSITORY PATTERN - Product data access
    /// Centralizes all product-related database operations
    /// </summary>
    public class ProductRepository : IRepository<Product>
    {
        public Product GetById(int id)
        {
            string query = @"
                SELECT p.productid, p.productname, p.price, p.categoryid, 
                       c.categoryname, p.imagepath
                FROM products p
                LEFT JOIN categories c ON p.categoryid = c.categoryid
                WHERE p.productid = @id
                LIMIT 1;";

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
                                return MapReaderToProduct(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductRepository] Error getting product by ID: {ex.Message}");
            }

            return null;
        }

        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();
            string query = @"
                SELECT p.productid, p.productname, p.price, p.categoryid, 
                       c.categoryname, p.imagepath
                FROM products p
                LEFT JOIN categories c ON p.categoryid = c.categoryid
                ORDER BY p.productid ASC;";

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
                            products.Add(MapReaderToProduct(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductRepository] Error getting all products: {ex.Message}");
            }

            return products;
        }

        public IEnumerable<Product> GetByCategory(string categoryName)
        {
            var products = new List<Product>();
            string query = @"
                SELECT p.productid, p.productname, p.price, p.categoryid, 
                       c.categoryname, p.imagepath
                FROM products p
                LEFT JOIN categories c ON p.categoryid = c.categoryid
                WHERE c.categoryname = @category
                ORDER BY p.productid ASC;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@category", categoryName);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(MapReaderToProduct(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductRepository] Error getting products by category: {ex.Message}");
            }

            return products;
        }

        public int Add(Product entity)
        {
            string query = @"
                INSERT INTO products (productname, price, categoryid, imagepath)
                VALUES (@name, @price, @catid, @image)
                RETURNING productid;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", entity.ProductName);
                        cmd.Parameters.AddWithValue("@price", entity.Price);
                        cmd.Parameters.AddWithValue("@catid", entity.CategoryId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@image", entity.ImagePath ?? (object)DBNull.Value);

                        var result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductRepository] Error adding product: {ex.Message}");
                throw;
            }
        }

        public void Update(Product entity)
        {
            string query = @"
                UPDATE products 
                SET productname = @name, 
                    price = @price, 
                    categoryid = @catid, 
                    imagepath = @image
                WHERE productid = @id;";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", entity.ProductName);
                        cmd.Parameters.AddWithValue("@price", entity.Price);
                        cmd.Parameters.AddWithValue("@catid", entity.CategoryId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@image", entity.ImagePath ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@id", entity.ProductId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ProductRepository] Error updating product: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM products WHERE productid = @id;";

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
                Console.WriteLine($"[ProductRepository] Error deleting product: {ex.Message}");
                throw;
            }
        }

        public bool Exists(int id)
        {
            string query = "SELECT COUNT(*) FROM products WHERE productid = @id;";

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
                Console.WriteLine($"[ProductRepository] Error checking product existence: {ex.Message}");
                return false;
            }
        }

        // Helper method to map database reader to Product object
        private Product MapReaderToProduct(NpgsqlDataReader reader)
        {
            return new Product
            {
                ProductId = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Price = reader.GetDecimal(2),
                CategoryId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                CategoryName = reader.IsDBNull(4) ? "Unknown" : reader.GetString(4),
                ImagePath = reader.IsDBNull(5) ? null : reader.GetString(5)
            };
        }
    }
}