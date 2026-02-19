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
    /// REFACTORED StaffForm demonstrating:
    /// - REPOSITORY PATTERN  : UserRepository handles all DB calls
    /// - COMMAND PATTERN     : Every mutation is wrapped in an ICommand (undo/redo)
    /// - TEMPLATE METHOD     : BaseCrudForm defines the workflow; this class fills in the steps
    /// </summary>
    public partial class StaffFormRefactored : BaseCrudForm<User>
    {
        // ✅ REPOSITORY PATTERN
        private readonly IRepository<User> _userRepository;

        // Helper value-type to store extra row metadata
        private readonly record struct StaffRowInfo(int UserId, string? ImagePath);

        public StaffFormRefactored(string userRole) : base(userRole)
        {
            InitializeComponent();

            // ✅ REPOSITORY PATTERN: injected via concrete class (could use DI container)
            _userRepository = new UserRepository();

            // Wire controls for the base class template
            dataGridView = dgvStaff;
            btnAdd = this.btnAdd;

            // ✅ TEMPLATE METHOD: runs SetupEventHandlers → LoadData → RestrictActionsByRole
            InitializeForm();
        }

        // ─────────────────────────────────────────────────────────────────────
        // TEMPLATE METHOD IMPLEMENTATIONS
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Load step: pull from repository and populate the grid.</summary>
        protected override void LoadData()
        {
            dgvStaff.Rows.Clear();

            // ✅ REPOSITORY PATTERN
            var users = _userRepository.GetAll();
            int rowNo = 1;

            foreach (var user in users)
            {
                Image? img = LoadImage(user.ImagePath);
                int rowIndex = dgvStaff.Rows.Add(rowNo++, user.Name, user.Role, user.Status, img);
                dgvStaff.Rows[rowIndex].Tag = new StaffRowInfo(user.Id, user.ImagePath);
            }

            Console.WriteLine($"[StaffFormRefactored] Loaded {users.Count()} staff members");
        }

        /// <summary>Edit step: show dialog → wrap save in a Command.</summary>
        protected override void OnEdit(User user)
        {
            // ✅ FIXED: FormEditStaff now exposes StaffNameValue / RoleValue /
            //           StatusValue / CurrentImagePath instead of raw controls.
            using var editForm = new FormEditStaff(
                user.Id, user.Name, user.Role, user.Status, user.ImagePath);

            editForm.StartPosition = FormStartPosition.CenterParent;

            if (editForm.ShowDialog(this) != DialogResult.OK)
                return;

            var updatedUser = new User
            {
                Id = user.Id,
                Username = user.Username,   // keep existing username
                Name = editForm.StaffNameValue,
                Role = editForm.RoleValue,
                Status = editForm.StatusValue,
                ImagePath = editForm.CurrentImagePath
            };

            // ✅ COMMAND PATTERN: mutation is undoable
            var command = new UpdateStaffCommand(updatedUser, _userRepository);
            try
            {
                commandInvoker.ExecuteCommand(command);
                MessageBox.Show("Staff updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ShowCommandStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating staff: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Delete step: wrap deletion in a Command for undo support.</summary>
        protected override void OnDelete(int id)
        {
            // ✅ COMMAND PATTERN
            var command = new DeleteStaffCommand(id, _userRepository);
            try
            {
                commandInvoker.ExecuteCommand(command);
                MessageBox.Show("Staff deleted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ShowCommandStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting staff: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Maps a DataGridView row back to a User domain object.</summary>
        protected override User GetEntityFromRow(DataGridViewRow row)
        {
            var info = row.Tag is StaffRowInfo sri ? sri : new StaffRowInfo(-1, null);

            // Prefer a fresh read from the repository so data is always up-to-date
            if (info.UserId > 0)
            {
                var fromRepo = _userRepository.GetById(info.UserId);
                if (fromRepo != null) return fromRepo;
            }

            // Fallback if repository read fails
            return new User
            {
                Id = info.UserId,
                Name = row.Cells["colStaff"].Value?.ToString() ?? string.Empty,
                Role = row.Cells["colRole"].Value?.ToString() ?? string.Empty,
                Status = row.Cells["colStatus"].Value?.ToString() ?? "Active",
                ImagePath = info.ImagePath
            };
        }

        /// <summary>Returns the primary key for the entity in the given row.</summary>
        protected override int GetEntityId(DataGridViewRow row)
        {
            var info = row.Tag is StaffRowInfo sri ? sri : new StaffRowInfo(-1, null);
            return info.UserId;
        }

        // ─────────────────────────────────────────────────────────────────────
        // ADD (overrides base class button handler)
        // ─────────────────────────────────────────────────────────────────────

        protected override void OnAddClick(object sender, EventArgs e)
        {
            base.OnAddClick(sender, e); // checks admin permission
            if (userRole != "(admin)") return;

            // TODO: Replace MessageBox with a proper AddStaffForm when created.
            // The pattern is identical to ProductFormRefactored.OnAddClick:
            //
            //   using var addForm = new FormAddStaff();
            //   if (addForm.ShowDialog(this) == DialogResult.OK)
            //   {
            //       var newUser = new User { Name = addForm.StaffName, Role = addForm.Role, ... };
            //       var command = new AddStaffCommand(newUser, _userRepository);
            //       commandInvoker.ExecuteCommand(command);
            //       LoadData();
            //   }

            MessageBox.Show(
                "Add Staff requires a dedicated AddStaffForm (not yet created).\n" +
                "Hint: Follow the same pattern used in ProductFormRefactored.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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