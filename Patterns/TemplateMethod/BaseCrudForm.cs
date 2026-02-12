using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OOAD_Project.Patterns.Command;

namespace OOAD_Project.Patterns.TemplateMethod
{
    /// <summary>
    /// TEMPLATE METHOD PATTERN - Base CRUD form
    /// Defines the skeleton of CRUD operations with customizable steps
    /// </summary>
    public abstract partial class BaseCrudForm<T> : Form where T : class
    {
        protected readonly string userRole;
        protected DataGridView dataGridView;
        protected Button btnAdd;
        protected readonly CommandInvoker commandInvoker;

        protected BaseCrudForm(string userRole)
        {
            this.userRole = userRole;
            this.commandInvoker = new CommandInvoker(maxHistorySize: 50);
        }

        #region Template Method - Main workflow

        /// <summary>
        /// TEMPLATE METHOD - Initialize form (main algorithm)
        /// This method defines the skeleton of the initialization process
        /// </summary>
        protected void InitializeForm()
        {
            SetupEventHandlers();
            LoadData();
            RestrictActionsByRole();
            ShowCommandStatus();
        }

        #endregion

        #region Abstract Methods - Must be implemented by subclasses

        /// <summary>
        /// Load data into the grid
        /// </summary>
        protected abstract void LoadData();

        /// <summary>
        /// Handle edit operation
        /// </summary>
        protected abstract void OnEdit(T entity);

        /// <summary>
        /// Handle delete operation
        /// </summary>
        protected abstract void OnDelete(int id);

        /// <summary>
        /// Get entity from DataGridView row
        /// </summary>
        protected abstract T GetEntityFromRow(DataGridViewRow row);

        /// <summary>
        /// Get entity ID from row
        /// </summary>
        protected abstract int GetEntityId(DataGridViewRow row);

        #endregion

        #region Virtual Methods - Can be overridden

        /// <summary>
        /// Setup event handlers (can be overridden)
        /// </summary>
        protected virtual void SetupEventHandlers()
        {
            if (dataGridView != null)
            {
                dataGridView.CellContentClick -= DataGridView_CellContentClick;
                dataGridView.CellContentClick += DataGridView_CellContentClick;
            }

            if (btnAdd != null)
            {
                btnAdd.Click -= OnAddClick;
                btnAdd.Click += OnAddClick;
            }
        }

        /// <summary>
        /// Restrict UI based on user role
        /// </summary>
        protected virtual void RestrictActionsByRole()
        {
            if (userRole != "(admin)")
            {
                // Hide Add button for non-admins
                if (btnAdd != null)
                {
                    btnAdd.Enabled = false;
                    btnAdd.Visible = false;
                }

                // Hide Edit/Delete columns
                if (dataGridView != null)
                {
                    if (dataGridView.Columns.Contains("colEdit"))
                        dataGridView.Columns["colEdit"].Visible = false;

                    if (dataGridView.Columns.Contains("colDelete"))
                        dataGridView.Columns["colDelete"].Visible = false;
                }
            }
        }

        /// <summary>
        /// Handle Add button click
        /// </summary>
        protected virtual void OnAddClick(object sender, EventArgs e)
        {
            if (userRole != "(admin)")
            {
                MessageBox.Show("Only admins can add items.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handle cell click for Edit/Delete operations
        /// </summary>
        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dataGridView.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];

            // Check admin permission
            if (userRole != "(admin)")
            {
                MessageBox.Show("Only admins can modify or delete items.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            T entity = GetEntityFromRow(row);
            int id = GetEntityId(row);

            if (columnName == "colEdit")
            {
                OnEdit(entity);
            }
            else if (columnName == "colDelete")
            {
                var confirm = MessageBox.Show(
                    "Are you sure you want to delete this item?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    OnDelete(id);
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Load image from file path with fallback
        /// </summary>
        protected Image LoadImage(string imagePath, string subfolder = "")
        {
            if (string.IsNullOrEmpty(imagePath))
                return GetDefaultImage();

            string[] possiblePaths = string.IsNullOrEmpty(subfolder)
                ? new[]
                {
                    imagePath,
                    Path.Combine(Application.StartupPath, "Resources", imagePath),
                    Path.Combine(Application.StartupPath, "Resources", imagePath + ".png"),
                    Path.Combine(Application.StartupPath, "Resources", imagePath + ".jpg"),
                    Path.Combine(Application.StartupPath, "Resources", imagePath + ".jpeg")
                }
                : new[]
                {
                    imagePath,
                    Path.Combine(Application.StartupPath, "Resources", subfolder, imagePath),
                    Path.Combine(Application.StartupPath, "Resources", subfolder, imagePath + ".png"),
                    Path.Combine(Application.StartupPath, "Resources", subfolder, imagePath + ".jpg"),
                    Path.Combine(Application.StartupPath, "Resources", subfolder, imagePath + ".jpeg")
                };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        return Image.FromFile(path);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return GetDefaultImage();
        }

        /// <summary>
        /// Get default placeholder image
        /// </summary>
        protected Image GetDefaultImage()
        {
            string defaultPath = Path.Combine(Application.StartupPath, "Resources", "no_image.png");
            if (File.Exists(defaultPath))
            {
                try
                {
                    return Image.FromFile(defaultPath);
                }
                catch { }
            }

            // Return a simple placeholder bitmap if no default image exists
            Bitmap placeholder = new Bitmap(100, 100);
            using (Graphics g = Graphics.FromImage(placeholder))
            {
                g.Clear(Color.LightGray);
                using (Font font = new Font("Arial", 10))
                {
                    g.DrawString("No Image", font, Brushes.DarkGray, 10, 40);
                }
            }
            return placeholder;
        }

        /// <summary>
        /// Show command history status
        /// </summary>
        protected void ShowCommandStatus()
        {
            Console.WriteLine($"[BaseCrudForm] Undo: {commandInvoker.UndoCount}, Redo: {commandInvoker.RedoCount}");
        }

        #endregion
    }
}