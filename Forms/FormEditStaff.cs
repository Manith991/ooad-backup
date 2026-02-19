using Npgsql;

namespace OOAD_Project
{
    public partial class FormEditStaff : Form
    {
        private readonly int userId;
        private string? _currentImagePath;

        // ✅ FIXED: Expose controls and image path as public properties
        //           so StaffFormRefactored can read them after ShowDialog().
        public string CurrentImagePath => _currentImagePath ?? string.Empty;
        public string StaffNameValue => txtStaff.Text.Trim();
        public string RoleValue => cbRole.Text;
        public string StatusValue => cbStatus.Text;

        public FormEditStaff(int userId, string name, string role, string status, string? imagePath)
        {
            InitializeComponent();

            this.userId = userId;
            txtStaff.Text = name;
            cbRole.Text = role;
            cbStatus.Text = status;
            _currentImagePath = imagePath;

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

                    using (var fs = new FileStream(selectedPath, FileMode.Open, FileAccess.Read))
                        pbImage.Image = Image.FromStream(fs);

                    string resourcesFolder = Path.Combine(Application.StartupPath, "Resources");
                    Directory.CreateDirectory(resourcesFolder);

                    string fileNameOnly = Path.GetFileNameWithoutExtension(selectedPath);
                    string extension = Path.GetExtension(selectedPath);
                    string destPath = Path.Combine(resourcesFolder, Path.GetFileName(selectedPath));

                    int counter = 1;
                    while (File.Exists(destPath))
                    {
                        destPath = Path.Combine(resourcesFolder, $"{fileNameOnly}_{counter}{extension}");
                        counter++;
                    }

                    File.Copy(selectedPath, destPath, false);
                    _currentImagePath = Path.GetFileName(destPath);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        // 💾 Save
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

            string query = @"UPDATE users 
                             SET name   = @name, 
                                 role   = @role, 
                                 status = @status, 
                                 image  = @image 
                             WHERE id = @id";
            try
            {
                Database.Execute(query,
                    new NpgsqlParameter("@name", newName),
                    new NpgsqlParameter("@role", newRole),
                    new NpgsqlParameter("@status", newStatus),
                    new NpgsqlParameter("@image", (object?)_currentImagePath ?? DBNull.Value),
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