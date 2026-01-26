namespace OOAD_Project
{
    partial class FormResetPass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResetPass));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panel1 = new Panel();
            txtEmail = new TextBox();
            btnClose = new Guna.UI2.WinForms.Guna2ImageButton();
            btnReset = new Button();
            label2 = new Label();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(txtEmail);
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(btnReset);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(502, 278);
            panel1.TabIndex = 0;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.ForeColor = Color.Gray;
            txtEmail.Location = new Point(32, 120);
            txtEmail.Multiline = true;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(433, 40);
            txtEmail.TabIndex = 22;
            txtEmail.Text = "Enter email...";
            txtEmail.Enter += txtEmail_Enter;
            txtEmail.Leave += txtEmail_Leave;
            // 
            // btnClose
            // 
            btnClose.CheckedState.ImageSize = new Size(64, 64);
            btnClose.HoverState.ImageSize = new Size(64, 64);
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageOffset = new Point(0, 0);
            btnClose.ImageRotate = 0F;
            btnClose.ImageSize = new Size(50, 50);
            btnClose.Location = new Point(433, 2);
            btnClose.Name = "btnClose";
            btnClose.PressedState.ImageSize = new Size(64, 64);
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges1;
            btnClose.Size = new Size(66, 66);
            btnClose.TabIndex = 21;
            btnClose.Click += btnClose_Click;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.Blue;
            btnReset.Font = new Font("Segoe UI", 10F);
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(32, 185);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(433, 56);
            btnReset.TabIndex = 20;
            btnReset.Text = "Continue";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(93, 64);
            label2.Name = "label2";
            label2.Size = new Size(310, 25);
            label2.TabIndex = 19;
            label2.Text = "Fill up the form to reset the password";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            label1.Location = new Point(102, 10);
            label1.Name = "label1";
            label1.Size = new Size(301, 54);
            label1.TabIndex = 18;
            label1.Text = "Reset Password";
            // 
            // FormResetPass
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(502, 278);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormResetPass";
            Text = "ForgotPasswordForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2ImageButton btnClose;
        private Button btnReset;
        private Label label2;
        private Label label1;
    }
}