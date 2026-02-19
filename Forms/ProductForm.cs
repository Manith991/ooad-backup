using Npgsql;
using System.Data;

namespace OOAD_Project
{
    public partial class ProductForm : Form
    {
        // Small helper to store additional row info (productId, categoryId, imagePath)
        private readonly record struct ProductRowInfo(int ProductId, int CategoryId, string? ImagePath);

        private readonly string userRole; // 🟢 Store logged-in user role

        public ProductForm(string userRole)
        {
            InitializeComponent();
            this.userRole = userRole;
            LoadProducts();
            RestrictActionsByRole();
        }

        // 🔒 Restrict Add/Edit/Delete for non-admins
        private void RestrictActionsByRole()
        {
            if (userRole != "(admin)")
            {
                // Hide Add button
                btnAdd.Enabled = false;
                btnAdd.Visible = true;

                // Hide edit and delete columns if they exist
                if (dgvProduct.Columns.Contains("colEdit"))
                    dgvProduct.Columns["colEdit"].Visible = false;

                if (dgvProduct.Columns.Contains("colDelete"))
                    dgvProduct.Columns["colDelete"].Visible = false;
            }
        }

        // 🟢 Load products with category names and category IDs
        private void LoadProducts()
        {
            dgvProduct.Rows.Clear();

            string query = @"
                SELECT p.productid, p.productname, p.price, p.categoryid, c.categoryname, p.imagepath
                FROM products p
                LEFT JOIN categories c ON p.categoryid = c.categoryid
                ORDER BY p.productid ASC";

            DataTable dt = Database.GetData(query);

            int rowNo = 1;
            foreach (DataRow row in dt.Rows)
            {
                int id = row["productid"] == DBNull.Value ? -1 : Convert.ToInt32(row["productid"]);
                string productName = row["productname"]?.ToString() ?? string.Empty;
                decimal price = row["price"] == DBNull.Value ? 0m : Convert.ToDecimal(row["price"]);
                int categoryId = row.Table.Columns.Contains("categoryid") && row["categoryid"] != DBNull.Value
                    ? Convert.ToInt32(row["categoryid"])
                    : -1;
                string categoryName = row["categoryname"]?.ToString() ?? "Unknown";
                string? imgPath = row["imagepath"]?.ToString();

                Image? img = null;

                if (!string.IsNullOrEmpty(imgPath))
                {
                    string[] possiblePaths = new string[]
                    {
                        imgPath,
                        Path.Combine(Application.StartupPath, "Resources", "Foods", imgPath),
                        Path.Combine(Application.StartupPath, "Resources", "Foods", imgPath + ".png"),
                        Path.Combine(Application.StartupPath, "Resources", "Foods", imgPath + ".jpg"),
                        Path.Combine(Application.StartupPath, "Resources", "Foods", imgPath + ".jpeg")
                    };

                    foreach (string path in possiblePaths)
                    {
                        if (File.Exists(path))
                        {
                            img = Image.FromFile(path);
                            break;
                        }
                    }
                }

                if (img == null)
                {
                    string defaultImg = Path.Combine(Application.StartupPath, "Resources", "no_image.png");
                    if (File.Exists(defaultImg))
                        img = Image.FromFile(defaultImg);
                }

                int rowIndex = dgvProduct.Rows.Add(rowNo++, productName, price.ToString("0.00"), categoryName, img);
                dgvProduct.Rows[rowIndex].Tag = new ProductRowInfo(id, categoryId, imgPath);
            }
        }

        // ➕ Add new product (Admins only)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (userRole != "(admin)")
            {
                MessageBox.Show("Only admins can add products.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (FormAddProduct addForm = new FormAddProduct())
            {
                addForm.StartPosition = FormStartPosition.CenterParent;

                if (addForm.ShowDialog(this) == DialogResult.OK)
                {
                    string query = "INSERT INTO products (productname, price, categoryid, imagepath) VALUES (@name, @price, @catid, @image)";
                    Database.Execute(query,
                        new NpgsqlParameter("@name", addForm.ProductName),
                        new NpgsqlParameter("@price", addForm.Price),
                        new NpgsqlParameter("@catid", addForm.CategoryID),
                        new NpgsqlParameter("@image", Path.GetFileNameWithoutExtension(addForm.ImagePath)));

                    LoadProducts();
                }
            }
        }

        // ✏️ Edit or ❌ Delete (Admins only)
        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dgvProduct.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
            ProductRowInfo info = row.Tag is ProductRowInfo pr ? pr : new ProductRowInfo(-1, -1, null);

            int productId = info.ProductId;
            int categoryId = info.CategoryId;
            string imagePathFromTag = info.ImagePath ?? string.Empty;
            string productName = row.Cells["colProduct"].Value?.ToString() ?? string.Empty;
            decimal.TryParse(row.Cells["colPrice"].Value?.ToString() ?? "0", out decimal price);
            string categoryName = row.Cells["colCategory"].Value?.ToString() ?? string.Empty;

            // 🛑 Restrict non-admins
            if (userRole != "(admin)")
            {
                MessageBox.Show("Only admins can modify or delete products.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🟢 Edit
            if (columnName == "colEdit")
            {
                using (FormAddProduct editForm = new FormAddProduct(productId, productName, price, categoryId, imagePathFromTag))
                {
                    editForm.StartPosition = FormStartPosition.CenterParent;

                    if (editForm.ShowDialog(this) == DialogResult.OK)
                    {
                        string query = @"UPDATE products 
                                         SET productname = @name, price = @price, categoryid = @catid, imagepath = @image 
                                         WHERE productid = @id";
                        Database.Execute(query,
                            new NpgsqlParameter("@name", editForm.ProductName),
                            new NpgsqlParameter("@price", editForm.Price),
                            new NpgsqlParameter("@catid", editForm.CategoryID),
                            new NpgsqlParameter("@image", Path.GetFileNameWithoutExtension(editForm.ImagePath)),
                            new NpgsqlParameter("@id", productId));

                        LoadProducts();
                    }
                }
            }
            // 🔴 Delete
            else if (columnName == "colDelete")
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete '{productName}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    string query = "DELETE FROM products WHERE productid = @id";
                    Database.Execute(query, new NpgsqlParameter("@id", productId));
                    LoadProducts();
                }
            }
        }
    }
}
