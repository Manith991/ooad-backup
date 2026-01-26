using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace OOAD_Project
{
    public partial class RecordForm : Form
    {
        private readonly string currentRole; // "admin" or "staff"

        public RecordForm(string role)
        {
            InitializeComponent();
            currentRole = role;

            // Hide Delete column if not admin
            dgvRecord.Columns["colDelete"].Visible = currentRole.ToLower() == "(admin)";

            LoadRecords();
        }

        private void LoadRecords()
        {
            dgvRecord.Rows.Clear();

            string query = @"
                SELECT 
                    o.order_id,
                    t.table_name,
                    u.name AS staff_name,
                    o.order_type,
                    o.order_date,
                    o.total_amount,
                    o.status,
                    o.payment_method
                FROM orders o
                LEFT JOIN tables t ON o.table_id = t.table_id
                LEFT JOIN users u ON o.user_id = u.id
                ORDER BY o.order_date DESC;
            ";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            i++;
                            int rowIndex = dgvRecord.Rows.Add();
                            DataGridViewRow row = dgvRecord.Rows[rowIndex];

                            row.Cells["colNo"].Value = i;
                            row.Cells["colTable"].Value = reader["table_name"]?.ToString();
                            row.Cells["colStaff"].Value = reader["staff_name"]?.ToString();
                            row.Cells["colOrder"].Value = reader["order_type"]?.ToString();
                            row.Cells["colDate"].Value = Convert.ToDateTime(reader["order_date"]).ToString("yyyy-MM-dd HH:mm");
                            row.Cells["colTotal"].Value = Convert.ToDecimal(reader["total_amount"]).ToString("C");
                            row.Cells["colStatus"].Value = reader["status"]?.ToString();
                            row.Cells["colPayment"].Value = reader["payment_method"]?.ToString();

                            // Always show action icons
                            row.Cells["colDetail"].Value = Properties.Resources.detail;
                            row.Cells["colDelete"].Value = Properties.Resources.delete;

                            // Store order ID
                            // Store order ID as an int (avoid boxed or DBNull)
                            object rawId = reader["order_id"];
                            int savedOrderId = rawId == DBNull.Value ? -1 : Convert.ToInt32(rawId);
                            row.Tag = savedOrderId;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading records: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRecord_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = dgvRecord.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = dgvRecord.Rows[e.RowIndex];
            // Safely obtain orderId from row.Tag
            int orderId;
            try
            {
                if (row.Tag is int idFromTag)
                {
                    orderId = idFromTag;
                }
                else if (row.Tag != null && row.Tag != DBNull.Value)
                {
                    orderId = Convert.ToInt32(row.Tag);
                }
                else
                {
                    MessageBox.Show("Invalid order id. Cannot open details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading order id: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (colName == "colDetail")
            {
                try
                {
                    FormDetailRecord detailForm = new FormDetailRecord(orderId);
                    detailForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening detail: " + ex.Message);
                }
            }
            else if (colName == "colDelete")
            {
                if (currentRole.ToLower() != "admin")
                {
                    MessageBox.Show("Only admins can delete records.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to delete order #{orderId}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    DeleteOrder(orderId);
                    LoadRecords();
                }
            }
        }

        private void DeleteOrder(int orderId)
        {
            string query = "DELETE FROM orders WHERE order_id = @order_id";
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@order_id", orderId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Order deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting order: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
