using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace OOAD_Project.Components
{
    public partial class TableCard : UserControl
    {
        private int tableId;
        private string tableStatus;

        public TableCard()
        {
            InitializeComponent();
            AttachClickEvent(this);
        }

        // Recursively attach click event to all children
        private void AttachClickEvent(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                ctl.Click += (s, e) => this.OnClick(e); // bubble up to TableCard
                if (ctl.HasChildren)
                    AttachClickEvent(ctl); // recursive for nested controls
            }
        }


        // Properly set all table data at once
        public void SetTableData(int id, string tableName, int capacity, string status)
        {
            tableId = id;
            lblTable.Text = tableName;
            lblCapacity.Text = capacity.ToString();
            lblStatus.Text = status;
            tableStatus = status;

            ApplyStatusStyle(status);
        }

        private void ApplyStatusStyle(string status)
        {
            status = status?.Trim().ToLower();
            switch (status)
            {
                case "available":
                    guna2Panel2.BackColor = Color.FromArgb(192, 255, 192);
                    lblStatus.ForeColor = Color.Green;
                    guna2Panel1.BackColor = Color.FromArgb(64, 64, 64);
                    break;
                case "taken":
                case "occupied":
                    guna2Panel2.BackColor = Color.FromArgb(192, 192, 255);
                    lblStatus.ForeColor = Color.Navy;
                    guna2Panel1.BackColor = Color.FromArgb(40, 40, 64);
                    break;
                case "unavailable":
                    guna2Panel2.BackColor = Color.FromArgb(255, 192, 192);
                    lblStatus.ForeColor = Color.Maroon;
                    guna2Panel1.BackColor = Color.FromArgb(80, 0, 0);
                    break;
                default:
                    guna2Panel2.BackColor = Color.Gray;
                    lblStatus.ForeColor = Color.White;
                    guna2Panel1.BackColor = Color.FromArgb(64, 64, 64);
                    break;
            }
        }

        // Read-only properties for outside access
        public int TableId => tableId;
        public string TableStatus => tableStatus;
        public string TableName => lblTable.Text;
        public int TableCapacity => int.Parse(lblCapacity.Text);

    }
}
