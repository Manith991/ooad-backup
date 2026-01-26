namespace OOAD_Project
{
    partial class RegisterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterForm));
            panel1 = new Panel();
            lblLogin = new Label();
            label8 = new Label();
            cbTerm = new CheckBox();
            btnSubmit = new Button();
            label7 = new Label();
            txtCPassword = new TextBox();
            label5 = new Label();
            txtPassword = new TextBox();
            label4 = new Label();
            txtEmail = new TextBox();
            label3 = new Label();
            txtName = new TextBox();
            label2 = new Label();
            txtUsername = new TextBox();
            label6 = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(lblLogin);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(cbTerm);
            panel1.Controls.Add(btnSubmit);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(txtCPassword);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(txtPassword);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(txtEmail);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(txtName);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtUsername);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(0, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1087, 861);
            panel1.TabIndex = 0;
            // 
            // lblLogin
            // 
            lblLogin.AutoSize = true;
            lblLogin.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            lblLogin.ForeColor = Color.Blue;
            lblLogin.Location = new Point(871, 802);
            lblLogin.Name = "lblLogin";
            lblLogin.Size = new Size(111, 28);
            lblLogin.TabIndex = 28;
            lblLogin.Text = "Back Login";
            lblLogin.Click += LblLogin_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(661, 805);
            label8.Name = "label8";
            label8.Size = new Size(213, 25);
            label8.TabIndex = 27;
            label8.Text = "Already have an account?";
            // 
            // cbTerm
            // 
            cbTerm.AutoSize = true;
            cbTerm.Location = new Point(599, 669);
            cbTerm.Name = "cbTerm";
            cbTerm.Size = new Size(392, 29);
            cbTerm.TabIndex = 26;
            cbTerm.Text = "I aceept the Terms of User and Privacy Policy.";
            cbTerm.UseVisualStyleBackColor = true;
            cbTerm.CheckedChanged += ValidateForm;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.Black;
            btnSubmit.Font = new Font("Segoe UI", 14F);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(596, 714);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(462, 74);
            btnSubmit.TabIndex = 25;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += BtnSubmit_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(596, 562);
            label7.Name = "label7";
            label7.Size = new Size(236, 32);
            label7.TabIndex = 24;
            label7.Text = "Confirmed Password:";
            // 
            // txtCPassword
            // 
            txtCPassword.Font = new Font("Segoe UI", 10F);
            txtCPassword.ForeColor = Color.Gray;
            txtCPassword.Location = new Point(596, 606);
            txtCPassword.Multiline = true;
            txtCPassword.Name = "txtCPassword";
            txtCPassword.Size = new Size(462, 40);
            txtCPassword.TabIndex = 23;
            txtCPassword.Text = "Enter Confirmed Password...";
            txtCPassword.TextChanged += ValidateForm;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(596, 455);
            label5.Name = "label5";
            label5.Size = new Size(116, 32);
            label5.TabIndex = 22;
            label5.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.ForeColor = Color.Gray;
            txtPassword.Location = new Point(596, 499);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(462, 40);
            txtPassword.TabIndex = 21;
            txtPassword.Text = "Enter Password...";
            txtPassword.TextChanged += ValidateForm;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(596, 248);
            label4.Name = "label4";
            label4.Size = new Size(76, 32);
            label4.TabIndex = 20;
            label4.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.ForeColor = Color.Gray;
            txtEmail.Location = new Point(596, 292);
            txtEmail.Multiline = true;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(462, 40);
            txtEmail.TabIndex = 19;
            txtEmail.Text = "Enter Email Address...";
            txtEmail.TextChanged += ValidateForm;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(596, 146);
            label3.Name = "label3";
            label3.Size = new Size(128, 32);
            label3.TabIndex = 18;
            label3.Text = "Full Name:";
            // 
            // txtName
            // 
            txtName.Font = new Font("Segoe UI", 10F);
            txtName.ForeColor = Color.Gray;
            txtName.Location = new Point(596, 190);
            txtName.Multiline = true;
            txtName.Name = "txtName";
            txtName.PlaceholderText = "Enter Name...";
            txtName.Size = new Size(462, 40);
            txtName.TabIndex = 17;
            txtName.TextChanged += ValidateForm;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(596, 352);
            label2.Name = "label2";
            label2.Size = new Size(126, 32);
            label2.TabIndex = 16;
            label2.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 10F);
            txtUsername.ForeColor = Color.Gray;
            txtUsername.Location = new Point(596, 396);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(462, 40);
            txtUsername.TabIndex = 15;
            txtUsername.Text = "Enter Username...";
            txtUsername.TextChanged += ValidateForm;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(711, 92);
            label6.Name = "label6";
            label6.Size = new Size(257, 25);
            label6.TabIndex = 14;
            label6.Text = "Create the account to Continue";
            // 
            // label1
            // 
            label1.Font = new Font("Script MT Bold", 20F, FontStyle.Bold);
            label1.Location = new Point(723, 32);
            label1.Name = "label1";
            label1.Size = new Size(234, 50);
            label1.TabIndex = 13;
            label1.Text = "Registeration";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(570, 860);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1088, 857);
            Controls.Add(panel1);
            Name = "RegisterForm";
            Text = "RegisterForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private Label label6;
        private Label label1;
        private Label label2;
        private TextBox txtUsername;
        private Label label5;
        private TextBox txtPassword;
        private Label label4;
        private TextBox txtEmail;
        private Label label3;
        private TextBox txtName;
        private Label label7;
        private TextBox txtCPassword;
        private Button btnSubmit;
        private Label label8;
        private CheckBox cbTerm;
        private Label lblLogin;
        private TextBox textBox1;
    }
}