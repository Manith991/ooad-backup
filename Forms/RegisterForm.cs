using System.Drawing.Drawing2D;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql;
using System.Security.Cryptography;

namespace OOAD_Project
{
    public partial class RegisterForm : Form
    {
        private const string namePlaceholder = "Enter Name...";
        private const string emailPlaceholder = "Enter Email Address...";
        private const string usernamePlaceholder = "Enter Username...";
        private const string passwordPlaceholder = "Enter Password...";
        private const string confirmPasswordPlaceholder = "Enter Confirmed Password...";

        private PictureBox eyeIconPassword;
        private PictureBox eyeIconConfirmPassword;
        private bool isPasswordVisible = false;
        private bool isConfirmPasswordVisible = false;

        public RegisterForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetupTextBox(txtName, namePlaceholder);
            SetupTextBox(txtEmail, emailPlaceholder);
            SetupTextBox(txtUsername, usernamePlaceholder);
            eyeIconPassword = SetupPasswordTextBox(txtPassword, passwordPlaceholder);
            eyeIconConfirmPassword = SetupPasswordTextBox(txtCPassword, confirmPasswordPlaceholder);

            btnSubmit.Enabled = false;
            btnSubmit.BackColor = Color.Gray;

            lblLogin.Cursor = Cursors.Hand;

            this.AcceptButton = btnSubmit;
        }

        private void SetupTextBox(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = Color.Gray;
            txt.BorderStyle = BorderStyle.None;

            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };

            AddBottomBorder(txt);
        }

        private PictureBox SetupPasswordTextBox(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = Color.Gray;
            txt.BorderStyle = BorderStyle.None;
            PictureBox eyeIcon = null;

            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                    txt.PasswordChar = '●';
                    if (eyeIcon != null)
                        DrawEyeIcon(eyeIcon, false);
                }
            };

            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                    txt.PasswordChar = '\0';
                    if (eyeIcon != null)
                        DrawEyeIcon(eyeIcon, false);
                }
            };

            AddBottomBorder(txt);
            eyeIcon = AddEyeIcon(txt);
            return eyeIcon;
        }

        private void AddBottomBorder(TextBox textBox)
        {
            var border = new Panel
            {
                Height = 2,
                Width = textBox.Width,
                BackColor = Color.Gray,
                Left = textBox.Left,
                Top = textBox.Bottom
            };
            textBox.Parent.Controls.Add(border);
            border.BringToFront();
        }

        private PictureBox AddEyeIcon(TextBox textBox)
        {
            PictureBox eyeIcon = new PictureBox
            {
                Size = new Size(22, 22),
                Location = new Point(textBox.Right - 26, textBox.Top + (textBox.Height - 22) / 2),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Cursor = Cursors.Hand,
                BackColor = Color.White,
                Tag = textBox
            };

            DrawEyeIcon(eyeIcon, false);

            if (textBox == txtPassword)
                eyeIcon.Click += TogglePasswordVisibility;
            else if (textBox == txtCPassword)
                eyeIcon.Click += ToggleConfirmPasswordVisibility;

            textBox.Parent.Controls.Add(eyeIcon);
            eyeIcon.BringToFront();

            return eyeIcon;
        }

        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            if (txtPassword.Text == passwordPlaceholder) return;

            isPasswordVisible = !isPasswordVisible;
            txtPassword.PasswordChar = isPasswordVisible ? '\0' : '●';
            DrawEyeIcon(eyeIconPassword, isPasswordVisible);
        }

        private void ToggleConfirmPasswordVisibility(object sender, EventArgs e)
        {
            if (txtCPassword.Text == confirmPasswordPlaceholder) return;

            isConfirmPasswordVisible = !isConfirmPasswordVisible;
            txtCPassword.PasswordChar = isConfirmPasswordVisible ? '\0' : '●';
            DrawEyeIcon(eyeIconConfirmPassword, isConfirmPasswordVisible);
        }

        private void DrawEyeIcon(PictureBox eyeIcon, bool isOpen)
        {
            Bitmap bmp = new Bitmap(22, 22);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                Pen pen = new Pen(Color.Gray, 2);

                if (isOpen)
                {
                    g.DrawEllipse(pen, 2, 7, 18, 8);
                    g.FillEllipse(Brushes.Gray, 8, 9, 6, 4);
                }
                else
                {
                    g.DrawEllipse(pen, 2, 7, 18, 8);
                    g.DrawLine(pen, 3, 18, 20, 5);
                }
            }
            eyeIcon.Image = bmp;
        }

        private void ValidateForm(object sender, EventArgs e)
        {
            btnSubmit.Enabled = cbTerm.Checked;
            btnSubmit.BackColor = cbTerm.Checked ? Color.Black : Color.Gray;
        }

        private bool IsFilled(TextBox txt, string placeholder) =>
            !string.IsNullOrWhiteSpace(txt.Text) && txt.Text != placeholder;

        private bool IsValidGmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase);
        }

        // ✅ Hash password before saving
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsFilled(txtName, namePlaceholder))
            {
                ShowWarning("Please enter your full name.", txtName);
                return;
            }

            if (!IsValidGmail(txtEmail.Text))
            {
                ShowWarning("Email must be a valid Gmail address.", txtEmail);
                return;
            }

            if (!IsFilled(txtUsername, usernamePlaceholder))
            {
                ShowWarning("Please enter your username.", txtUsername);
                return;
            }

            if (!IsStrongPassword(txtPassword.Text))
            {
                ShowWarning("Password must be at least 6 characters long and contain both letters and numbers (e.g., Abc123).", txtPassword);
                return;
            }

            if (!IsFilled(txtCPassword, confirmPasswordPlaceholder))
            {
                ShowWarning("Please confirm your password.", txtCPassword);
                return;
            }

            if (txtPassword.Text != txtCPassword.Text)
            {
                ShowWarning("Passwords do not match.", txtCPassword);
                return;
            }

            if (!cbTerm.Checked)
            {
                ShowWarning("Please accept the Terms of Use.", cbTerm);
                return;
            }

            try
            {
                string query = @"INSERT INTO users (name, email, username, password)
                                 VALUES (@name, @email, @username, @password)";

                NpgsqlParameter[] parameters =
                {
                    new NpgsqlParameter("@name", txtName.Text),
                    new NpgsqlParameter("@email", txtEmail.Text),
                    new NpgsqlParameter("@username", txtUsername.Text),
                    new NpgsqlParameter("@password", HashPassword(txtPassword.Text))
                };

                Database.Execute(query, parameters);

                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoginForm loginForm = new LoginForm();
                loginForm.FormClosed += (s, args) => Close();
                loginForm.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowWarning(string message, Control focusTarget)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusTarget.Focus();
        }

        private void LblLogin_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.FormClosed += (s, args) => Close();
            loginForm.Show();
            Hide();
        }

        private bool IsStrongPassword(string password)
        {
            // At least 6 characters, must include letters and numbers
            string pattern = @"^(?=.*[A-Za-z])(?=.*\d).{6,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
