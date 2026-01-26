using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OOAD_Project.Components;

namespace OOAD_Project
{
    public partial class DiningForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private string currentUser;

        public DiningForm(string username)
        {
            InitializeComponent();
            currentUser = username;
            LoadTableCards();
        }

        private void LoadTableCards()
        {
            flpTable.Controls.Clear();
            flpTable.Padding = new Padding(20);

            string query = "SELECT table_id, table_name, capacity, status FROM tables ORDER BY table_id;";
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                int capacity = reader.GetInt32(2);
                string status = reader.GetString(3);

                TableCard card = new TableCard();
                card.Margin = new Padding(10);
                card.SetTableData(id, name, capacity, status);

                string username = this.currentUser;

                card.Click += (s, e) =>
                {
                    string username = this.currentUser;

                    if (card.TableStatus.Equals("Available", StringComparison.OrdinalIgnoreCase))
                    {
                        // ✅ Case 1: Available → create new order
                        var result = MessageBox.Show($"Do you want to create a new order for {card.TableName}?",
                                                     "New Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                using var conn2 = Database.GetConnection();
                                conn2.Open();

                                int userId = GetCurrentUserId();
                                if (userId == -1)
                                {
                                    MessageBox.Show("User not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                string insertOrder = @"
                    INSERT INTO orders (table_id, user_id, order_type, status)
                    VALUES (@table_id, @user_id, 'Dine-in', 'Unpaid')
                    RETURNING order_id;";

                                using var cmd2 = new NpgsqlCommand(insertOrder, conn2);
                                cmd2.Parameters.AddWithValue("table_id", card.TableId);
                                cmd2.Parameters.AddWithValue("user_id", userId);
                                int orderId = Convert.ToInt32(cmd2.ExecuteScalar());

                                string updateTable = "UPDATE tables SET status='Taken' WHERE table_id=@table_id;";
                                using var cmd3 = new NpgsqlCommand(updateTable, conn2);
                                cmd3.Parameters.AddWithValue("table_id", card.TableId);
                                cmd3.ExecuteNonQuery();

                                card.SetTableData(card.TableId, card.TableName, card.TableCapacity, "Taken");

                                POSForm pos = new POSForm(this, username, "Eat Here", card.TableName, card.TableId, orderId);
                                pos.Show();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error creating order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else if (card.TableStatus.Equals("Taken", StringComparison.OrdinalIgnoreCase))
                    {
                        // ✅ Case 2: Taken → open existing unpaid order
                        try
                        {
                            using var conn = Database.GetConnection();
                            conn.Open();

                            string query = @"
                SELECT order_id 
                FROM orders 
                WHERE table_id = @tid AND status = 'Unpaid'
                ORDER BY order_date DESC LIMIT 1;";

                            using var cmd = new NpgsqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("tid", card.TableId);
                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                int existingOrderId = Convert.ToInt32(result);
                                POSForm pos = new POSForm(this, username, "Eat Here", card.TableName, card.TableId, existingOrderId);
                                pos.Show();
                            }
                            else
                            {
                                MessageBox.Show("No unpaid order found for this table.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error loading existing order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // Optional: if status = Paid or Reserved etc.
                        MessageBox.Show($"This table is {card.TableStatus}.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                };


                flpTable.Controls.Add(card);
            }
        }

        private int GetCurrentUserId()
        {
            int userId = -1;
            string query = "SELECT id FROM users WHERE username = @username LIMIT 1;";

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("username", currentUser);
                var result = cmd.ExecuteScalar();
                if (result != null)
                    userId = Convert.ToInt32(result);
            }
            return userId;
        }
    }
}
