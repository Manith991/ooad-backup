using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OOAD_Project
{
    public partial class MenuForm : Form
    {
        private string currentUser;
        private string currentRole;

        public MenuForm(string username, string role)
        {
            InitializeComponent();
            currentUser = username;
            currentRole = role;
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            lblUser.Text = currentUser;
            lblRole.Text = currentRole;
        }

        public void AddControls(Form f)
        {
            panelCenter.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            panelCenter.Controls.Add(f);
            f.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            AddControls(new HomeForm());
        }

        private void btnAway_Click(object sender, EventArgs e)
        {
            POSForm posForm = new POSForm(this, currentUser, "Takeaway");
            posForm.Show();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            AddControls(new StaffForm(currentRole));
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            AddControls(new RecordForm(currentRole));
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            AddControls(new TableForm(currentRole));
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            AddControls(new ProductForm(currentRole));
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            AddControls(new CategoriesForm(currentRole));
        }

        private void btnHere_Click(object sender, EventArgs e)
        {
            AddControls(new DiningForm(currentUser));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
