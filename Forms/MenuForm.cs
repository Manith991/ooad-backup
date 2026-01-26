using System;
using System.Windows.Forms;
using OOAD_Project.Patterns;

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

        // ============================================
        // FACTORY PATTERN IMPLEMENTATION
        // All button clicks now use FormFactoryManager
        // ============================================
        private void btnHome_Click(object sender, EventArgs e)
        {
            // ✅ Using Factory Pattern
            IFormFactory factory = new HomeFormFactory();
            Form form = factory.CreateForm();
            AddControls(form);
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            // ✅ Using Factory Pattern with role parameter
            IFormFactory factory = new StaffFormFactory(currentRole);
            Form form = factory.CreateForm();
            AddControls(form);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            // ✅ Using Factory Pattern
            IFormFactory factory = new ProductFormFactory(currentRole);
            Form form = factory.CreateForm();
            AddControls(form);
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            // ✅ Using Factory Pattern
            IFormFactory factory = new CategoriesFormFactory(currentRole);
            Form form = factory.CreateForm();
            AddControls(form);
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            // ✅ Using Factory Pattern
            IFormFactory factory = new TableFormFactory(currentRole);
            Form form = factory.CreateForm();
            AddControls(form);
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            // ✅ Using Factory Pattern
            IFormFactory factory = new RecordFormFactory(currentRole);
            Form form = factory.CreateForm();
            AddControls(form);
        }

        private void btnHere_Click(object sender, EventArgs e)
        {
            // ✅ Using Factory Pattern for Dining Form (opens in panel)
            IFormFactory factory = new DiningFormFactory(currentUser);
            Form form = factory.CreateForm();
            AddControls(form);
        }

        private void btnAway_Click(object sender, EventArgs e)
        {
            // ✅ Using Factory Pattern for POS Form (opens as new window)
            IFormFactory factory = new POSFormFactory(this, currentUser, "Takeaway");
            Form posForm = factory.CreateForm();
            posForm.Show();
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