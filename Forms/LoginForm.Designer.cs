namespace OOAD_Project
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            panel1 = new Panel();
            lblForgotPassword = new Label();
            label6 = new Label();
            lblRegister = new Label();
            label5 = new Label();
            label4 = new Label();
            btnLogin = new Button();
            txtPassword = new TextBox();
            label3 = new Label();
            label2 = new Label();
            txtUsername = new TextBox();
            label1 = new Label();
            pbProfile = new PictureBox();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbProfile).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BackgroundImageLayout = ImageLayout.None;
            panel1.Controls.Add(lblForgotPassword);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(lblRegister);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(btnLogin);
            panel1.Controls.Add(txtPassword);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtUsername);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pbProfile);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(0, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1088, 744);
            panel1.TabIndex = 5;
            // 
            // lblForgotPassword
            // 
            lblForgotPassword.AutoSize = true;
            lblForgotPassword.BackColor = Color.White;
            lblForgotPassword.ForeColor = Color.Gray;
            lblForgotPassword.Location = new Point(747, 673);
            lblForgotPassword.Name = "lblForgotPassword";
            lblForgotPassword.Size = new Size(154, 25);
            lblForgotPassword.TabIndex = 14;
            lblForgotPassword.Text = "Forgot Password?";
            lblForgotPassword.Click += lblForgotPassword_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(649, 185);
            label6.Name = "label6";
            label6.Size = new Size(367, 25);
            label6.TabIndex = 12;
            label6.Text = "Welcome to Restaurant Management System";
            // 
            // lblRegister
            // 
            lblRegister.AutoSize = true;
            lblRegister.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            lblRegister.ForeColor = Color.Blue;
            lblRegister.Location = new Point(758, 629);
            lblRegister.Name = "lblRegister";
            lblRegister.Size = new Size(133, 28);
            lblRegister.TabIndex = 11;
            lblRegister.Text = "Register Now";
            lblRegister.Click += LblRegister_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Gray;
            label5.Location = new Point(768, 595);
            label5.Name = "label5";
            label5.Size = new Size(114, 25);
            label5.TabIndex = 10;
            label5.Text = "No Account?";
            // 
            // label4
            // 
            label4.BackColor = Color.Black;
            label4.Location = new Point(596, 577);
            label4.Name = "label4";
            label4.Size = new Size(462, 1);
            label4.TabIndex = 9;
            label4.Text = "label4";
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.Black;
            btnLogin.Font = new Font("Segoe UI", 14F);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(596, 479);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(462, 74);
            btnLogin.TabIndex = 8;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.ForeColor = Color.Gray;
            txtPassword.Location = new Point(596, 408);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(462, 40);
            txtPassword.TabIndex = 7;
            txtPassword.Text = "Enter Password...";
            txtPassword.Enter += TxtPassword_Enter;
            txtPassword.Leave += TxtPassword_Leave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(596, 358);
            label3.Name = "label3";
            label3.Size = new Size(116, 32);
            label3.TabIndex = 6;
            label3.Text = "Password:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(596, 243);
            label2.Name = "label2";
            label2.Size = new Size(126, 32);
            label2.TabIndex = 5;
            label2.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 10F);
            txtUsername.ForeColor = Color.Gray;
            txtUsername.Location = new Point(596, 296);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(462, 40);
            txtUsername.TabIndex = 3;
            txtUsername.Text = "Enter Username...";
            txtUsername.Enter += TxtUsername_Enter;
            txtUsername.Leave += TxtUsername_Leave;
            // 
            // label1
            // 
            label1.Font = new Font("Script MT Bold", 20F, FontStyle.Bold);
            label1.Location = new Point(770, 128);
            label1.Name = "label1";
            label1.Size = new Size(118, 50);
            label1.TabIndex = 2;
            label1.Text = "Login ";
            // 
            // pbProfile
            // 
            pbProfile.BackColor = Color.White;
            pbProfile.BorderStyle = BorderStyle.FixedSingle;
            pbProfile.Image = (Image)resources.GetObject("pbProfile.Image");
            pbProfile.Location = new Point(779, 25);
            pbProfile.Name = "pbProfile";
            pbProfile.Padding = new Padding(2);
            pbProfile.Size = new Size(100, 100);
            pbProfile.SizeMode = PictureBoxSizeMode.StretchImage;
            pbProfile.TabIndex = 1;
            pbProfile.TabStop = false;
            pbProfile.Paint += PbProfile_Paint;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.loginPic;
            pictureBox1.Location = new Point(1, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(570, 740);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1088, 744);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Restaurant Management System";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbProfile).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label6;
        private Label lblRegister;
        private Label label5;
        private Label label4;
        private Button btnLogin;
        private TextBox txtPassword;
        private Label label3;
        private Label label2;
        private TextBox txtUsername;
        private Label label1;
        private PictureBox pbProfile;
        private PictureBox pictureBox1;
        private Label lblForgotPassword;
    }
}
