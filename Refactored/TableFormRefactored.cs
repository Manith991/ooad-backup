using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OOAD_Project.Domain;
using OOAD_Project.Patterns.Repository;
using OOAD_Project.Patterns.Command;
using OOAD_Project.Patterns.TemplateMethod;

namespace OOAD_Project.Refactored
{
    /// <summary>
    /// REFACTORED TableForm demonstrating:
    /// - REPOSITORY PATTERN  : TableRepository handles all DB calls
    /// - COMMAND PATTERN     : Every mutation is wrapped in an ICommand (undo/redo)
    /// - TEMPLATE METHOD     : BaseCrudForm defines the workflow; this class fills in the steps
    /// </summary>
    public partial class TableFormRefactored : BaseCrudForm<Table>
    {
        // ✅ REPOSITORY PATTERN
        private readonly IRepository<Table> _tableRepository;

        // Value-type to store extra row metadata
        private readonly record struct TableRowInfo(int TableId);

        public TableFormRefactored(string userRole) : base(userRole)
        {
            InitializeComponent();

            // ✅ REPOSITORY PATTERN: injected via concrete class
            _tableRepository = new TableRepository();

            // Wire controls for the base class template
            dataGridView = dgvTable;
            btnAdd = this.btnAdd;

            // ✅ TEMPLATE METHOD: runs SetupEventHandlers → LoadData → RestrictActionsByRole
            InitializeForm();
        }

        // ─────────────────────────────────────────────────────────────────────
        // TEMPLATE METHOD IMPLEMENTATIONS
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Load step: pull all tables from the repository and populate grid.</summary>
        protected override void LoadData()
        {
            dgvTable.Rows.Clear();

            // ✅ REPOSITORY PATTERN
            var tables = _tableRepository.GetAll();
            int rowNo = 1;

            foreach (var table in tables)
            {
                int rowIndex = dgvTable.Rows.Add(rowNo++, table.TableName, table.Capacity, table.Status);
                dgvTable.Rows[rowIndex].Tag = new TableRowInfo(table.TableId);
            }

            Console.WriteLine($"[TableFormRefactored] Loaded {tables.Count()} tables");
        }

        /// <summary>Edit step: show dialog → wrap save in a Command.</summary>
        protected override void OnEdit(Table table)
        {
            using var editForm = new FormAddTable(
                table.TableId, table.TableName, table.Capacity, table.Status);

            editForm.StartPosition = FormStartPosition.CenterParent;

            if (editForm.ShowDialog(this) != DialogResult.OK)
                return;

            var updatedTable = new Table
            {
                TableId = table.TableId,
                TableName = editForm.TableName,
                Capacity = editForm.Capacity,
                Status = editForm.Status
            };

            // ✅ COMMAND PATTERN: mutation is undoable
            var command = new UpdateTableCommand(updatedTable, _tableRepository);
            try
            {
                commandInvoker.ExecuteCommand(command);
                MessageBox.Show("Table updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ShowCommandStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating table: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Delete step: wrap deletion in a Command for undo support.</summary>
        protected override void OnDelete(int id)
        {
            // ✅ COMMAND PATTERN
            var command = new DeleteTableCommand(id, _tableRepository);
            try
            {
                commandInvoker.ExecuteCommand(command);
                MessageBox.Show("Table deleted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ShowCommandStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting table: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Maps a DataGridView row back to a Table domain object.</summary>
        protected override Table GetEntityFromRow(DataGridViewRow row)
        {
            var info = row.Tag is TableRowInfo tri ? tri : new TableRowInfo(-1);

            if (info.TableId > 0)
            {
                var fromRepo = _tableRepository.GetById(info.TableId);
                if (fromRepo != null) return fromRepo;
            }

            // Fallback
            return new Table
            {
                TableId = info.TableId,
                TableName = row.Cells["colTable"].Value?.ToString() ?? string.Empty,
                Capacity = int.TryParse(row.Cells["colCapacity"].Value?.ToString(), out int cap) ? cap : 0,
                Status = row.Cells["colStatus"].Value?.ToString() ?? "Available"
            };
        }

        /// <summary>Returns the primary key for the entity in the given row.</summary>
        protected override int GetEntityId(DataGridViewRow row)
        {
            var info = row.Tag is TableRowInfo tri ? tri : new TableRowInfo(-1);
            return info.TableId;
        }

        // ─────────────────────────────────────────────────────────────────────
        // ADD (overrides base class button handler)
        // ─────────────────────────────────────────────────────────────────────

        protected override void OnAddClick(object sender, EventArgs e)
        {
            base.OnAddClick(sender, e); // checks admin permission
            if (userRole != "(admin)") return;

            using var addForm = new FormAddTable();
            addForm.StartPosition = FormStartPosition.CenterParent;

            if (addForm.ShowDialog(this) != DialogResult.OK)
                return;

            var newTable = new Table
            {
                TableName = addForm.TableName,
                Capacity = addForm.Capacity,
                Status = addForm.Status
            };

            // ✅ COMMAND PATTERN
            var command = new AddTableCommand(newTable, _tableRepository);
            try
            {
                commandInvoker.ExecuteCommand(command);
                MessageBox.Show("Table added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ShowCommandStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding table: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // UNDO / REDO  (Ctrl+Z / Ctrl+Y)
        // ─────────────────────────────────────────────────────────────────────

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Z)) { PerformUndo(); return true; }
            if (keyData == (Keys.Control | Keys.Y)) { PerformRedo(); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PerformUndo()
        {
            if (!commandInvoker.CanUndo)
            {
                MessageBox.Show("Nothing to undo.", "Undo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string desc = commandInvoker.GetUndoDescription();
                commandInvoker.Undo();
                LoadData();
                ShowCommandStatus();
                MessageBox.Show($"Undid: {desc}", "Undo Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Undo failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PerformRedo()
        {
            if (!commandInvoker.CanRedo)
            {
                MessageBox.Show("Nothing to redo.", "Redo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string desc = commandInvoker.GetRedoDescription();
                commandInvoker.Redo();
                LoadData();
                ShowCommandStatus();
                MessageBox.Show($"Redid: {desc}", "Redo Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Redo failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}