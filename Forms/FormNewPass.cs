using Npgsql;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace OOAD_Project
{
    public partial class FormNewPass : Form
    {
        private readonly string userEmail;

        private const string newPassPlaceholder = "Enter New Password...";
        private const string confirmPassPlaceholder = "Confirm Password...";

        private PictureBox eyeNew;
        private PictureBox eyeConfirm;
        private bool newPassVisible = false;
        private bool confirmPassVisible = false;

        public FormNewPass(string email)
        {
            InitializeComponent();
            this.userEmail = email;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.AcceptButton = btnSubmit;
        }

        private void FormNewPass_Load(object sender, EventArgs e)
        {
            // Initialize placeholders
            txtNewPass.Text = newPassPlaceholder;
            txtNewPass.ForeColor = Color.Gray;
            txtNewPass.PasswordChar = '\0';

            txtConfirmPass.Text = confirmPassPlaceholder;
            txtConfirmPass.ForeColor = Color.Gray;
            txtConfirmPass.PasswordChar = '\0';

            // Add bottom borders
            AddBottomBorder(txtNewPass);
            AddBottomBorder(txtConfirmPass);

            // Add eye icons
            AddEyeIcons();
        }

        private void AddBottomBorder(TextBox textBox)
        {
            Panel border = new Panel
            {
                Height = 2,
                Width = textBox.Width,
                BackColor = Color.Gray,
                Location = new Point(textBox.Location.X, textBox.Location.Y + textBox.Height)
            };
            textBox.Parent.Controls.Add(border);
            border.BringToFront();
        }

        #region Placeholder Events

        private void TxtNewPass_Enter(object sender, EventArgs e)
        {
            if (txtNewPass.Text == newPassPlaceholder)
            {
                txtNewPass.Text = "";
                txtNewPass.ForeColor = Color.Black;
                txtNewPass.PasswordChar = '●';
            }
        }

        private void TxtNewPass_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPass.Text))
            {
                txtNewPass.Text = newPassPlaceholder;
                txtNewPass.ForeColor = Color.Gray;
                txtNewPass.PasswordChar = '\0';
            }
        }

        private void TxtConfirmPass_Enter(object sender, EventArgs e)
        {
            if (txtConfirmPass.Text == confirmPassPlaceholder)
            {
                txtConfirmPass.Text = "";
                txtConfirmPass.ForeColor = Color.Black;
                txtConfirmPass.PasswordChar = '●';
            }
        }

        private void TxtConfirmPass_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfirmPass.Text))
            {
                txtConfirmPass.Text = confirmPassPlaceholder;
                txtConfirmPass.ForeColor = Color.Gray;
                txtConfirmPass.PasswordChar = '\0';
            }
        }

        #endregion

        #region Eye Icons

        private void AddEyeIcons()
        {
            eyeNew = CreateEyeIcon(txtNewPass);
            eyeNew.Click += (s, e) =>
            {
                if (txtNewPass.Text == newPassPlaceholder) return;
                newPassVisible = !newPassVisible;
                txtNewPass.PasswordChar = newPassVisible ? '\0' : '●';
                DrawEyeIcon(eyeNew, newPassVisible);
            };
            DrawEyeIcon(eyeNew, false);

            eyeConfirm = CreateEyeIcon(txtConfirmPass);
            eyeConfirm.Click += (s, e) =>
            {
                if (txtConfirmPass.Text == confirmPassPlaceholder) return;
                confirmPassVisible = !confirmPassVisible;
                txtConfirmPass.PasswordChar = confirmPassVisible ? '\0' : '●';
                DrawEyeIcon(eyeConfirm, confirmPassVisible);
            };
            DrawEyeIcon(eyeConfirm, false);
        }

        private PictureBox CreateEyeIcon(TextBox textBox)
        {
            PictureBox eye = new PictureBox
            {
                Size = new Size(24, 24),
                Location = new Point(textBox.Location.X + textBox.Width - 30,
                                     textBox.Location.Y + (textBox.Height - 24) / 2),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Cursor = Cursors.Hand,
                BackColor = Color.White
            };
            textBox.Parent.Controls.Add(eye);
            eye.BringToFront();
            return eye;
        }

        private void DrawEyeIcon(PictureBox pic, bool open)
        {
            Bitmap bmp = new Bitmap(24, 24);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                Pen pen = new Pen(Color.Gray, 2);

                if (open)
                {
                    g.DrawEllipse(pen, 2, 8, 20, 10);
                    g.FillEllipse(Brushes.Gray, 9, 10, 6, 6);
                }
                else
                {
                    g.DrawEllipse(pen, 2, 8, 20, 10);
                    g.DrawLine(pen, 3, 20, 21, 6);
                }
            }
            pic.Image = bmp;
        }

        #endregion

        #region Submit Button

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string newPass = txtNewPass.Text.Trim();
            string confirmPass = txtConfirmPass.Text.Trim();

            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass) ||
                newPass == newPassPlaceholder || confirmPass == confirmPassPlaceholder)
            {
                MessageBox.Show("Please fill in all password fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string hashedPassword = HashPassword(newPass);

            try
            {
                Database.Execute("UPDATE users SET password=@Password WHERE email=@Email",
                    new NpgsqlParameter("@Password", hashedPassword),
                    new NpgsqlParameter("@Email", userEmail));

                MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
