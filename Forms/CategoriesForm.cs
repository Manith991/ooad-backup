using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OOAD_Project
{
    public partial class CategoriesForm : Form
    {
        private readonly string userRole;

        public CategoriesForm(string userRole)
        {
            InitializeComponent();
            this.userRole = userRole;

            LoadCategories();
            RestrictActionsByRole();
        }

        // 🔒 Restrict Edit/Delete for non-admin users
        private void RestrictActionsByRole()
        {
            if (userRole != "(admin)")
            {
                // Hide Edit/Delete columns
                if (dgvCategory.Columns.Contains("colEdit"))
                    dgvCategory.Columns["colEdit"].Visible = false;

                if (dgvCategory.Columns.Contains("colDelete"))
                    dgvCategory.Columns["colDelete"].Visible = false;
            }
        }

        // 🟢 Load categories from the database
        private void LoadCategories()
        {
            dgvCategory.Rows.Clear();

            string query = "SELECT categoryid, categoryname, imagepath FROM categories ORDER BY categoryid ASC";
            DataTable dt = Database.GetData(query);

            int rowNo = 1;
            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["categoryid"]);
                string name = row["categoryname"].ToString();
                string imgPath = row["imagepath"]?.ToString();

                Image img = null;
                if (!string.IsNullOrEmpty(imgPath))
                {
                    string fullPath = Path.Combine(Application.StartupPath, "Resources", imgPath + ".png");
                    if (File.Exists(fullPath))
                        img = Image.FromFile(fullPath);
                }

                // 🖼️ Load default image if not found
                if (img == null)
                {
                    string defaultImg = Path.Combine(Application.StartupPath, "Resources", "no_image.png");
                    if (File.Exists(defaultImg))
                        img = Image.FromFile(defaultImg);
                }

                int rowIndex = dgvCategory.Rows.Add(rowNo++, name, img);
                dgvCategory.Rows[rowIndex].Tag = id; // store categoryid here
            }
        }

        // ✏️ Edit or ❌ Delete category (Admins only)
        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvCategory.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = dgvCategory.Rows[e.RowIndex];

            int categoryId = (int)(row.Tag ?? -1);
            string categoryName = row.Cells["colCategory"].Value?.ToString();
            Image existingImage = row.Cells["colIcon"].Value as Image;

            // 🛑 Restrict non-admin users
            if (userRole != "(admin)")
            {
                MessageBox.Show("Only admins can modify or delete categories.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (colName == "colEdit")
            {
                using (FormEditCate editForm = new FormEditCate(categoryId, categoryName, existingImage))
                {
                    if (editForm.ShowDialog(this) == DialogResult.OK)
                    {
                        try
                        {
                            Database.Execute(
                                "UPDATE categories SET categoryname = @name, imagepath = @image WHERE categoryid = @id",
                                new NpgsqlParameter("@name", editForm.CategoryName),
                                new NpgsqlParameter("@image", string.IsNullOrEmpty(editForm.ImagePath)
                                    ? DBNull.Value
                                    : Path.GetFileNameWithoutExtension(editForm.ImagePath)),
                                new NpgsqlParameter("@id", categoryId)
                            );

                            MessageBox.Show("Category updated successfully!");

                            // Reload the data without closing the form
                            LoadCategories();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating category:\n" + ex.Message, "SQL Error");
                        }
                    }
                }
            }

            // 🔴 Delete
            else if (colName == "colDelete")
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete '{categoryName}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    string query = "DELETE FROM categories WHERE categoryid = @id";
                    Database.Execute(query, new NpgsqlParameter("@id", categoryId));
                    LoadCategories();
                }
            }
        }
    }
}
