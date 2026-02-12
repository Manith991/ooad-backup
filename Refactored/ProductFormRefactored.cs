using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OOAD_Project.Domain;
using OOAD_Project.Patterns.Repository;
using OOAD_Project.Patterns.Command;
using OOAD_Project.Patterns.TemplateMethod;

namespace OOAD_Project.Refactored
{
    /// <summary>
    /// REFACTORED ProductForm using:
    /// - REPOSITORY PATTERN for data access
    /// - COMMAND PATTERN for undo/redo operations
    /// - TEMPLATE METHOD PATTERN for common CRUD workflow
    /// </summary>
    public partial class ProductFormRefactored : BaseCrudForm<Product>
    {
        // ✅ REPOSITORY PATTERN - Injected dependency
        private readonly IRepository<Product> _productRepository;

        // Helper struct to store row metadata
        private readonly record struct ProductRowInfo(int ProductId, int CategoryId, string ImagePath);

        public ProductFormRefactored(string userRole) : base(userRole)
        {
            InitializeComponent();

            // ✅ Initialize repositories
            _productRepository = new ProductRepository();

            // Assign DataGridView reference for base class
            dataGridView = dgvProduct;  // ✅ FIXED: Use dgvProduct, not dgvCategory
            btnAdd = this.btnAdd;

            // ✅ TEMPLATE METHOD - Use base class initialization workflow
            InitializeForm();
        }

        #region Template Method Implementation

        /// <summary>
        /// TEMPLATE METHOD - Load data step
        /// </summary>
        protected override void LoadData()
        {
            dgvProduct.Rows.Clear();

            // ✅ REPOSITORY PATTERN - Get data from repository instead of raw SQL
            var products = _productRepository.GetAll();

            int rowNo = 1;
            foreach (var product in products)
            {
                Image img = LoadImage(product.ImagePath, "Foods");

                int rowIndex = dgvProduct.Rows.Add(
                    rowNo++,
                    product.ProductName,
                    product.Price.ToString("0.00"),
                    product.CategoryName,
                    img
                );

                // Store metadata in row tag
                dgvProduct.Rows[rowIndex].Tag = new ProductRowInfo(
                    product.ProductId,
                    product.CategoryId ?? -1,
                    product.ImagePath
                );
            }

            Console.WriteLine($"[ProductFormRefactored] Loaded {products.Count()} products");
        }

        /// <summary>
        /// TEMPLATE METHOD - Edit operation
        /// </summary>
        protected override void OnEdit(Product product)
        {
            using (FormAddProduct editForm = new FormAddProduct(
                product.ProductId,
                product.ProductName,
                product.Price,
                product.CategoryId ?? -1,
                product.ImagePath))
            {
                editForm.StartPosition = FormStartPosition.CenterParent;

                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    // Create updated product
                    var updatedProduct = new Product
                    {
                        ProductId = product.ProductId,
                        ProductName = editForm.ProductName,
                        Price = editForm.Price,
                        CategoryId = editForm.CategoryID,
                        ImagePath = !string.IsNullOrEmpty(editForm.ImagePath)
                            ? Path.GetFileNameWithoutExtension(editForm.ImagePath)
                            : product.ImagePath
                    };

                    // ✅ COMMAND PATTERN - Execute update via command
                    var command = new UpdateProductCommand(updatedProduct, _productRepository);

                    try
                    {
                        commandInvoker.ExecuteCommand(command);

                        MessageBox.Show("Product updated successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LoadData();
                        ShowCommandStatus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating product: {ex.Message}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// TEMPLATE METHOD - Delete operation
        /// </summary>
        protected override void OnDelete(int id)
        {
            // ✅ COMMAND PATTERN - Execute delete via command
            var command = new DeleteProductCommand(id, _productRepository);

            try
            {
                commandInvoker.ExecuteCommand(command);

                MessageBox.Show("Product deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadData();
                ShowCommandStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// TEMPLATE METHOD - Get entity from row
        /// </summary>
        protected override Product GetEntityFromRow(DataGridViewRow row)
        {
            ProductRowInfo info = row.Tag is ProductRowInfo pr
                ? pr
                : new ProductRowInfo(-1, -1, null);

            // Get complete product data from repository
            if (info.ProductId > 0)
            {
                return _productRepository.GetById(info.ProductId);
            }

            // Fallback if repository fails
            return new Product
            {
                ProductId = info.ProductId,
                ProductName = row.Cells["colProduct"].Value?.ToString() ?? string.Empty,
                Price = decimal.TryParse(row.Cells["colPrice"].Value?.ToString(), out decimal price)
                    ? price
                    : 0m,
                CategoryId = info.CategoryId,
                CategoryName = row.Cells["colCategory"].Value?.ToString() ?? string.Empty,
                ImagePath = info.ImagePath
            };
        }

        /// <summary>
        /// TEMPLATE METHOD - Get entity ID from row
        /// </summary>
        protected override int GetEntityId(DataGridViewRow row)
        {
            ProductRowInfo info = row.Tag is ProductRowInfo pr
                ? pr
                : new ProductRowInfo(-1, -1, null);

            return info.ProductId;
        }

        #endregion

        #region Add Product (Override base class method)

        /// <summary>
        /// Handle add button click with Command Pattern
        /// </summary>
        protected override void OnAddClick(object sender, EventArgs e)
        {
            base.OnAddClick(sender, e); // Check admin permission

            if (userRole != "(admin)") return;

            using (FormAddProduct addForm = new FormAddProduct())
            {
                addForm.StartPosition = FormStartPosition.CenterParent;

                if (addForm.ShowDialog(this) == DialogResult.OK)
                {
                    // Create new product
                    var newProduct = new Product
                    {
                        ProductName = addForm.ProductName,
                        Price = addForm.Price,
                        CategoryId = addForm.CategoryID,
                        ImagePath = !string.IsNullOrEmpty(addForm.ImagePath)
                            ? Path.GetFileNameWithoutExtension(addForm.ImagePath)
                            : null
                    };

                    // ✅ COMMAND PATTERN - Execute add via command
                    var command = new ProductCommand(newProduct, _productRepository);

                    try
                    {
                        commandInvoker.ExecuteCommand(command);

                        MessageBox.Show("Product added successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LoadData();
                        ShowCommandStatus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding product: {ex.Message}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        #region Undo/Redo Support

        /// <summary>
        /// Add keyboard shortcut support for undo/redo
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Ctrl+Z = Undo
            if (keyData == (Keys.Control | Keys.Z))
            {
                PerformUndo();
                return true;
            }

            // Ctrl+Y = Redo
            if (keyData == (Keys.Control | Keys.Y))
            {
                PerformRedo();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PerformUndo()
        {
            if (!commandInvoker.CanUndo)
            {
                MessageBox.Show("Nothing to undo.", "Undo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                string description = commandInvoker.GetUndoDescription();
                commandInvoker.Undo();
                LoadData();
                ShowCommandStatus();

                MessageBox.Show($"Undid: {description}",
                    "Undo Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during undo: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void PerformRedo()
        {
            if (!commandInvoker.CanRedo)
            {
                MessageBox.Show("Nothing to redo.", "Redo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                string description = commandInvoker.GetRedoDescription();
                commandInvoker.Redo();
                LoadData();
                ShowCommandStatus();

                MessageBox.Show($"Redid: {description}",
                    "Redo Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during redo: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}