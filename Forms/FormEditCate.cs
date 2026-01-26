using Npgsql;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OOAD_Project
{
    public partial class FormEditCate : Form
    {
        private readonly int categoryId;

        public string CategoryName { get; private set; }
        public string ImagePath { get; private set; }
        public Image CategoryImage { get; private set; }

        public FormEditCate(int id, string categoryName, Image existingImage = null)
        {
            InitializeComponent();
          
            categoryId = id;
            txtCategory.Text = categoryName;

            if (existingImage != null)
            {
                DisplayImage(existingImage);
                CategoryImage = existingImage;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Category Image";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ImagePath = openFileDialog.FileName;
                    Image selectedImage = Image.FromFile(ImagePath);
                    DisplayImage(selectedImage);
                    CategoryImage = selectedImage;
                }
            }
        }

        private void DisplayImage(Image img)
        {
            pbIcon.Image = img;
            pbIcon.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string categoryName = txtCategory.Text.Trim();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CategoryImage == null)
            {
                MessageBox.Show("Please select an image.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                CategoryName = categoryName;

                // Copy image to Resources folder
                if (!string.IsNullOrEmpty(ImagePath))
                {
                    string destFolder = Path.Combine(Application.StartupPath, "Resources");
                    if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

                    string destPath = Path.Combine(destFolder, Path.GetFileName(ImagePath));
                    File.Copy(ImagePath, destPath, true);
                    ImagePath = destPath;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving category:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
