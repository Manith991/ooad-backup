using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace OOAD_Project
{
    public partial class TableForm : Form
    {
        private readonly record struct TableRowInfo(int TableId);
        private readonly string userRole;

        public TableForm(string userRole)
        {
            InitializeComponent();
            this.userRole = userRole;
            LoadTables();
            RestrictActionsByRole();
        }

        // 🟢 Load tables from the database
        private void LoadTables()
        {
            dgvTable.Rows.Clear();

            string query = @"SELECT table_id, table_name, capacity, status
                             FROM tables
                             ORDER BY table_id ASC";

            DataTable dt = Database.GetData(query);

            int rowNo = 1;
            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["table_id"]);
                string name = row["table_name"].ToString() ?? string.Empty;
                int capacity = Convert.ToInt32(row["capacity"]);
                string status = row["status"].ToString() ?? "Available";

                int rowIndex = dgvTable.Rows.Add(rowNo++, name, capacity, status);
                dgvTable.Rows[rowIndex].Tag = new TableRowInfo(id);
            }

            // Disable edit/delete buttons if not admin
            if (userRole != "(admin)")
            {
                foreach (DataGridViewRow r in dgvTable.Rows)
                {
                    if (r.Cells["colEdit"] is DataGridViewButtonCell editCell)
                        editCell.ReadOnly = true;

                    if (r.Cells["colDelete"] is DataGridViewButtonCell deleteCell)
                        deleteCell.ReadOnly = true;
                }
            }
        }

        // 🔒 Apply role permissions and change button image
        private void RestrictActionsByRole()
        {
            if (userRole != "(admin)")
            {
                // Hide Add button
                btnAdd.Enabled = false;
                btnAdd.Visible = true;

                // Hide Edit/Delete columns
                if (dgvTable.Columns.Contains("colEdit"))
                    dgvTable.Columns["colEdit"].Visible = false;

                if (dgvTable.Columns.Contains("colDelete"))
                    dgvTable.Columns["colDelete"].Visible = false;
            }
        }

        // ➕ Add new table
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (userRole != "(admin)")
            {
                MessageBox.Show("Only admin users can add tables.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (FormAddTable addForm = new FormAddTable())
            {
                addForm.StartPosition = FormStartPosition.CenterParent;

                if (addForm.ShowDialog(this) == DialogResult.OK)
                {
                    string query = @"INSERT INTO tables (table_name, capacity, status)
                                     VALUES (@name, @cap, @status)";
                    Database.Execute(query,
                        new NpgsqlParameter("@name", addForm.TableName),
                        new NpgsqlParameter("@cap", addForm.Capacity),
                        new NpgsqlParameter("@status", addForm.Status));

                    LoadTables();
                }
            }
        }

        // ✏️ Edit or ❌ Delete
        private void dgvTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string colName = dgvTable.Columns[e.ColumnIndex].Name;

            if (userRole != "(admin)" && (colName == "colEdit" || colName == "colDelete"))
            {
                MessageBox.Show("Only admin users can modify or delete tables.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvTable.Rows[e.RowIndex];
            TableRowInfo info = row.Tag is TableRowInfo t ? t : new TableRowInfo(-1);
            int tableId = info.TableId;

            string tableName = row.Cells["colTable"].Value?.ToString() ?? string.Empty;
            int capacity = Convert.ToInt32(row.Cells["colCapacity"].Value ?? 0);
            string status = row.Cells["colStatus"].Value?.ToString() ?? "Available";

            if (colName == "colEdit")
            {
                using (FormAddTable editForm = new FormAddTable(tableId, tableName, capacity, status))
                {
                    editForm.StartPosition = FormStartPosition.CenterParent;

                    if (editForm.ShowDialog(this) == DialogResult.OK)
                    {
                        string query = @"UPDATE tables
                                         SET table_name=@name, capacity=@cap, status=@status
                                         WHERE table_id=@id";
                        Database.Execute(query,
                            new NpgsqlParameter("@name", editForm.TableName),
                            new NpgsqlParameter("@cap", editForm.Capacity),
                            new NpgsqlParameter("@status", editForm.Status),
                            new NpgsqlParameter("@id", tableId));

                        LoadTables();
                    }
                }
            }
            else if (colName == "colDelete")
            {
                var confirm = MessageBox.Show(
                    $"Are you sure you want to delete '{tableName}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    string query = "DELETE FROM tables WHERE table_id = @id";
                    Database.Execute(query, new NpgsqlParameter("@id", tableId));
                    LoadTables();
                }
            }
        }
    }
}
