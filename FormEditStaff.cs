using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Npgsql;

namespace OOAD_Project
{
    public partial class FormEditStaff : Form
    {
        private readonly int userId;
        private string? currentImagePath;

        public FormEditStaff(int userId, string name, string role, string status, string? imagePath)
        {
            InitializeComponent();

            this.userId = userId;
            txtStaff.Text = name;
            cbRole.Text = role;
            cbStatus.Text = status;
            currentImagePath = imagePath;

            LoadImage(imagePath);
        }

        // 🟢 Load image safely
        private void LoadImage(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

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
                {
                    pbImage.Image = Image.FromFile(path);
                    return;
                }
            }
        }

        // 📂 Browse for new image
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = ofd.FileName;

                    // Load image without locking file
                    using (var fs = new FileStream(selectedPath, FileMode.Open, FileAccess.Read))
                    {
                        pbImage.Image = Image.FromStream(fs);
                    }

                    string resourcesFolder = Path.Combine(Application.StartupPath, "Resources");
                    if (!Directory.Exists(resourcesFolder))
                        Directory.CreateDirectory(resourcesFolder);

                    string originalFileName = Path.GetFileName(selectedPath);
                    string destPath = Path.Combine(resourcesFolder, originalFileName);

                    // 🟢 Create a unique filename if file exists
                    string fileNameOnly = Path.GetFileNameWithoutExtension(originalFileName);
                    string extension = Path.GetExtension(originalFileName);

                    int counter = 1;
                    while (File.Exists(destPath))
                    {
                        string newFileName = $"{fileNameOnly}_{counter}{extension}";
                        destPath = Path.Combine(resourcesFolder, newFileName);
                        counter++;
                    }

                    // 🟢 Copy safely (no overwrite, no freeze)
                    File.Copy(selectedPath, destPath, false);

                    // Save the final filename to DB
                    currentImagePath = Path.GetFileName(destPath);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 💾 Save Staff Info (update)
        private void btnSave_Click(object sender, EventArgs e)
        {
            string newName = txtStaff.Text.Trim();
            string newRole = cbRole.Text;
            string newStatus = cbStatus.Text;

            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newRole))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Correct SQL column names
            string query = @"UPDATE users 
                             SET name = @name, 
                                 role = @role, 
                                 status = @status, 
                                 image = @image 
                             WHERE id = @id";

            try
            {
                Database.Execute(query,
                    new NpgsqlParameter("@name", newName),
                    new NpgsqlParameter("@role", newRole),
                    new NpgsqlParameter("@status", newStatus),
                    new NpgsqlParameter("@image", (object?)currentImagePath ?? DBNull.Value),
                    new NpgsqlParameter("@id", userId));

                MessageBox.Show("Staff information updated successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating staff: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
