using System;
using System.Windows.Forms;

namespace OOAD_Project
{
    public partial class FormAddTable : Form
    {
        public string TableName => txtTable.Text.Trim();

        public int Capacity
        {
            get
            {
                int.TryParse(txtCapacity.Text, out int value);
                return value;
            }
        }

        public string Status => cbStatus.Text;

        private bool isEditMode = false;
        private int tableId = -1;

        public FormAddTable()
        {
            InitializeComponent();
            this.Text = "Add Table";
            btnSave.Text = "Add";
            LoadStatusOptions();
            lblTable.Text = "Add Table";
        }

        public FormAddTable(int id, string tableName, int capacity, string status)
        {
            InitializeComponent();
            this.Text = "Edit Table";
            btnSave.Text = "Save";
            isEditMode = true;
            tableId = id;

            txtTable.Text = tableName;
            txtCapacity.Text = capacity.ToString();

            LoadStatusOptions();

            // set status only if the items exist
            if (cbStatus.Items.Count > 0)
            {
                cbStatus.SelectedItem = status;
                // if the incoming status isn't in items, fallback
                if (cbStatus.SelectedItem == null)
                    cbStatus.SelectedIndex = 0;
            }

            lblTable.Text = "Edit Table";
        }

        private void LoadStatusOptions()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.AddRange(new string[] { "Available", "Unavailable" });
            if (cbStatus.Items.Count > 0)
                cbStatus.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTable.Text))
            {
                MessageBox.Show("Please enter a table name.");
                return;
            }

            if (!int.TryParse(txtCapacity.Text, out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Please enter a valid capacity (number > 0).");
                return;
            }

            if (string.IsNullOrWhiteSpace(cbStatus.Text))
            {
                MessageBox.Show("Please select a status.");
                return;
            }

            // IMPORTANT: no database calls here.
            // Parent form will perform insert/update when ShowDialog() returns OK.
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
