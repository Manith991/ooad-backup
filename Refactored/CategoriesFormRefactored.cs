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
    /// REFACTORED CategoriesForm demonstrating all three patterns:
    /// - REPOSITORY PATTERN for data access
    /// - COMMAND PATTERN for undo/redo operations
    /// - TEMPLATE METHOD PATTERN for common CRUD workflow
    /// </summary>
    public partial class CategoriesFormRefactored : BaseCrudForm<Category>
    {
        // ✅ REPOSITORY PATTERN
        private readonly IRepository<Category> _categoryRepository;

        public CategoriesFormRefactored(string userRole) : base(userRole)
        {
            InitializeComponent();

            _categoryRepository = new CategoryRepository();

            // Assign controls for base class
            dataGridView = dgvCategory;
            btnAdd = this.btnAdd;

            // ✅ TEMPLATE METHOD - Initialize using base class workflow
            InitializeForm();
        }

        #region Template Method Implementation

        /// <summary>
        /// TEMPLATE METHOD - Load data step
        /// </summary>
        protected override void LoadData()
        {
            dgvCategory.Rows.Clear();

            // ✅ REPOSITORY PATTERN - Get from repository
            var categories = _categoryRepository.GetAll();

            int rowNo = 1;
            foreach (var category in categories)
            {
                Image img = LoadImage(category.ImagePath);

                int rowIndex = dgvCategory.Rows.Add(
                    rowNo++,
                    category.CategoryName,
                    img
                );

                dgvCategory.Rows[rowIndex].Tag = category.CategoryId;
            }

            Console.WriteLine($"[CategoriesFormRefactored] Loaded {categories.Count()} categories");
        }

        /// <summary>
        /// TEMPLATE METHOD - Edit operation
        /// </summary>
        protected override void OnEdit(Category category)
        {
            using (FormEditCate editForm = new FormEditCate(
                category.CategoryId,
                category.CategoryName,
                LoadImage(category.ImagePath)))
            {
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    var updatedCategory = new Category
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = editForm.CategoryName,
                        ImagePath = !string.IsNullOrEmpty(editForm.ImagePath)
                            ? Path.GetFileNameWithoutExtension(editForm.ImagePath)
                            : category.ImagePath
                    };

                    // ✅ COMMAND PATTERN - Use command for undo/redo support
                    var command = new UpdateCategoryCommand(updatedCategory, _categoryRepository);

                    try
                    {
                        commandInvoker.ExecuteCommand(command);

                        MessageBox.Show("Category updated successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LoadData();
                        ShowCommandStatus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating category: {ex.Message}",
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
            // ✅ COMMAND PATTERN - Use command for undo/redo support
            var command = new DeleteCategoryCommand(id, _categoryRepository);

            try
            {
                commandInvoker.ExecuteCommand(command);

                MessageBox.Show("Category deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadData();
                ShowCommandStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// TEMPLATE METHOD - Get entity from row
        /// </summary>
        protected override Category GetEntityFromRow(DataGridViewRow row)
        {
            int categoryId = row.Tag is int id ? id : -1;

            // Get the actual category from repository to have complete data
            if (categoryId > 0)
            {
                return _categoryRepository.GetById(categoryId);
            }

            return new Category
            {
                CategoryId = categoryId,
                CategoryName = row.Cells["colCategory"].Value?.ToString() ?? string.Empty,
                ImagePath = null
            };
        }

        /// <summary>
        /// TEMPLATE METHOD - Get entity ID from row
        /// </summary>
        protected override int GetEntityId(DataGridViewRow row)
        {
            return row.Tag is int id ? id : -1;
        }

        #endregion

        #region Add Category (Override base class method)

        /// <summary>
        /// Handle add button click with Command Pattern
        /// </summary>
        protected override void OnAddClick(object sender, EventArgs e)
        {
            base.OnAddClick(sender, e); // Check admin permission

            if (userRole != "(admin)") return;

            // Show add category dialog
            using (FormEditCate addForm = new FormEditCate(0, "New Category", null))
            {
                addForm.Text = "Add Category";

                if (addForm.ShowDialog(this) == DialogResult.OK)
                {
                    var newCategory = new Category
                    {
                        CategoryName = addForm.CategoryName,
                        ImagePath = !string.IsNullOrEmpty(addForm.ImagePath)
                            ? Path.GetFileNameWithoutExtension(addForm.ImagePath)
                            : null
                    };

                    // ✅ COMMAND PATTERN - Execute add via command
                    var command = new AddCategoryCommand(newCategory, _categoryRepository);

                    try
                    {
                        commandInvoker.ExecuteCommand(command);

                        MessageBox.Show("Category added successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LoadData();
                        ShowCommandStatus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding category: {ex.Message}",
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