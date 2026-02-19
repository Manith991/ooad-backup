using System.Data;

namespace OOAD_Project
{
    public partial class FormAddProduct : Form
    {
        public string ProductName => txtProduct.Text.Trim();
        public decimal Price
        {
            get
            {
                decimal.TryParse(txtPrice.Text, out decimal value);
                return value;
            }
        }
        public int CategoryID
        {
            get
            {
                if (cbCategory.SelectedValue != null)
                    return Convert.ToInt32(cbCategory.SelectedValue);
                return -1;
            }
        }
        public string ImagePath => imagePath;

        private bool isEditMode = false;
        private int productId = -1;
        private string imagePath = string.Empty;

        public FormAddProduct()
        {
            InitializeComponent();
            this.Text = "Add Product";
            btnSave.Text = "Add";
            LoadCategories();
            lblProduct.Text = "Add Product";
        }

        public FormAddProduct(int id, string productName, decimal price, int categoryId, string imagePath = "")
        {
            InitializeComponent();
            this.Text = "Edit Product";
            btnSave.Text = "Save";
            isEditMode = true;
            productId = id;
            this.imagePath = imagePath;

            txtProduct.Text = productName;
            txtPrice.Text = price.ToString();
            LoadCategories();
            cbCategory.SelectedValue = categoryId;

            // Load image preview if exists
            if (!string.IsNullOrEmpty(imagePath))
            {
                string fullPath = imagePath;

                if (!File.Exists(fullPath))
                {
                    string tryResourcePath = Path.Combine(Application.StartupPath, "Resources", "Foods", imagePath + ".png");
                    if (File.Exists(tryResourcePath))
                        fullPath = tryResourcePath;
                }

                if (File.Exists(fullPath))
                    pbIcon.Image = Image.FromFile(fullPath);
            }

            lblProduct.Text = "Edit Product";
        }

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT categoryid, categoryname FROM categories ORDER BY categoryname ASC";
                DataTable dt = Database.GetData(query);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No categories found. Please add a category first.");
                    return;
                }

                cbCategory.DataSource = dt;
                cbCategory.DisplayMember = "categoryname";
                cbCategory.ValueMember = "categoryid";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories:\n" + ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Product Image";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = openFileDialog.FileName;
                    pbIcon.Image = Image.FromFile(imagePath);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProduct.Text))
            {
                MessageBox.Show("Enter a product name.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal _))
            {
                MessageBox.Show("Invalid price format.");
                return;
            }

            if (cbCategory.SelectedValue == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            // ✅ No database call here — parent form will handle insert/update
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
