using Npgsql;

namespace OOAD_Project
{
    public partial class FormResetPass : Form
    {
        private const string emailPlaceholder = "Enter Email...";
        public FormResetPass()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            txtEmail.Text = emailPlaceholder;
            txtEmail.ForeColor = Color.Gray;
            txtEmail.BorderStyle = BorderStyle.None;

            // Add bottom border (if you still want it)
            AddBottomBorder(txtEmail);

            this.AcceptButton = btnReset;
        }

        private void FormResetPass_Load(object sender, EventArgs e)
        {
            txtEmail.Text = emailPlaceholder;
            txtEmail.ForeColor = Color.Gray;
            txtEmail.BorderStyle = BorderStyle.None;
            AddBottomBorder(txtEmail);
        }

        private void AddBottomBorder(TextBox textBox)
        {
            Panel borderPanel = new Panel
            {
                Height = 2,
                Width = textBox.Width,
                BackColor = Color.Gray,
                Location = new Point(textBox.Location.X, textBox.Location.Y + textBox.Height)
            };
            textBox.Parent.Controls.Add(borderPanel);
            borderPanel.BringToFront();
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == emailPlaceholder)
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = Color.Black;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = emailPlaceholder;
                txtEmail.ForeColor = Color.Gray;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (email == emailPlaceholder || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM users WHERE email = @Email AND LOWER(status) = 'active';";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 1)
                        {
                            MessageBox.Show("Email verified! Proceed to set your new password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FormNewPass newPass = new FormNewPass(email);
                            newPass.StartPosition = FormStartPosition.CenterScreen;
                            newPass.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Email not found or inactive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
