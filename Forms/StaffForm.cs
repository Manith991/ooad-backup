using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OOAD_Project
{
    public partial class StaffForm : Form
    {
        private readonly string userRole;

        public StaffForm(string userRole)
        {
            InitializeComponent();
            this.userRole = userRole;

            LoadStaffData();
            RestrictActionsByRole();
        }

        // 🔒 Restrict access based on role
        private void RestrictActionsByRole()
        {
            if (userRole != "(admin)")
            {
                btnAdd.Enabled = false;
                btnAdd.Visible = true;

                if (dgvStaff.Columns.Contains("colEdit"))
                    dgvStaff.Columns["colEdit"].Visible = false;

                if (dgvStaff.Columns.Contains("colDelete"))
                    dgvStaff.Columns["colDelete"].Visible = false;
            }
        }

        // 🟢 Load all staff from database
        private void LoadStaffData()
        {
            try
            {
                dgvStaff.Rows.Clear();

                string query = "SELECT id, name, role, status, image FROM users ORDER BY id";
                DataTable dt = Database.GetData(query);

                int no = 1;
                foreach (DataRow row in dt.Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    string name = row["name"]?.ToString() ?? "";
                    string role = row["role"]?.ToString() ?? "";
                    string status = row["status"]?.ToString() ?? "";
                    string? imagePath = row["image"] == DBNull.Value ? null : row["image"].ToString();

                    Image? img = LoadStaffImage(imagePath);
                    int rowIndex = dgvStaff.Rows.Add(no++, name, role, status, img, null, null);

                    dgvStaff.Rows[rowIndex].Tag = Tuple.Create(id, imagePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff: {ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🖼️ Load image safely from Resources
        private Image? LoadStaffImage(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            try
            {
                string[] possiblePaths =
                {
                    imagePath,
                    Path.Combine(Application.StartupPath, "Resources", imagePath),
                    Path.Combine(Application.StartupPath, "Resources", imagePath + ".png"),
                    Path.Combine(Application.StartupPath, "Resources", imagePath + ".jpg"),
                    Path.Combine(Application.StartupPath, "Resources", imagePath + ".jpeg")
                };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                        return Image.FromFile(path);
                }
            }
            catch { }

            return null;
        }

        // ✏️ Handle edit / delete / image click
        private void dgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dgvStaff.Columns[e.ColumnIndex].Name;
            var row = dgvStaff.Rows[e.RowIndex];
            var tag = row.Tag as Tuple<int, string?>;
            if (tag == null) return;

            int userId = tag.Item1;
            string? imagePath = tag.Item2;
            string staffName = row.Cells["colStaff"].Value?.ToString() ?? "";
            string role = row.Cells["colRole"].Value?.ToString() ?? "";
            string status = row.Cells["colStatus"].Value?.ToString() ?? "";

            // Restrict non-admins
            if (userRole != "(admin)")
            {
                MessageBox.Show("Only admins can perform this action.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (columnName == "colEdit")
            {
                using (var editForm = new FormEditStaff(userId, staffName, role, status, imagePath))
                {
                    editForm.StartPosition = FormStartPosition.CenterParent;
                    if (editForm.ShowDialog() == DialogResult.OK)
                        LoadStaffData();
                }
            }
            else if (columnName == "colDelete")
            {
                var confirm = MessageBox.Show($"Delete staff '{staffName}'?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        string deleteQuery = "DELETE FROM users WHERE id = @id";
                        Database.Execute(deleteQuery, new NpgsqlParameter("@id", userId));
                        LoadStaffData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting staff: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // 📸 Double-click image cell to change photo
        private void dgvStaff_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string columnName = dgvStaff.Columns[e.ColumnIndex].Name;
            if (columnName != "colImage") return;

            if (userRole != "(admin)")
            {
                MessageBox.Show("Only admins can change staff images.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvStaff.Rows[e.RowIndex];
            var tag = row.Tag as Tuple<int, string?>;
            if (tag == null) return;

            int userId = tag.Item1;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = ofd.FileName;
                    string fileName = Path.GetFileName(selectedPath);
                    string destDir = Path.Combine(Application.StartupPath, "Resources");
                    string destPath = Path.Combine(destDir, fileName);

                    // Ensure directory exists
                    Directory.CreateDirectory(destDir);

                    // Copy image (overwrite if same name)
                    File.Copy(selectedPath, destPath, true);

                    try
                    {
                        string updateQuery = "UPDATE users SET \"image\" = @image WHERE id = @id";
                        try
                        {
                            Database.Execute(updateQuery,
                                new NpgsqlParameter("@image", fileName),
                                new NpgsqlParameter("@id", userId));

                            LoadStaffData();

                            MessageBox.Show("Image updated successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error updating image: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                        // Reload data
                        LoadStaffData();

                        MessageBox.Show("Image updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating image: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
