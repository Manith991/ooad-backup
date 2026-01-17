namespace OOAD_Project
{
    partial class FormNewPass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewPass));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panel1 = new Panel();
            btnClose = new Guna.UI2.WinForms.Guna2ImageButton();
            txtConfirmPass = new TextBox();
            txtNewPass = new TextBox();
            btnSubmit = new Button();
            label1 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(txtConfirmPass);
            panel1.Controls.Add(txtNewPass);
            panel1.Controls.Add(btnSubmit);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(565, 413);
            panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.CheckedState.ImageSize = new Size(64, 64);
            btnClose.HoverState.ImageSize = new Size(64, 64);
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageOffset = new Point(0, 0);
            btnClose.ImageRotate = 0F;
            btnClose.ImageSize = new Size(50, 50);
            btnClose.Location = new Point(496, 2);
            btnClose.Name = "btnClose";
            btnClose.PressedState.ImageSize = new Size(64, 64);
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges1;
            btnClose.Size = new Size(66, 66);
            btnClose.TabIndex = 20;
            btnClose.Click += btnClose_Click;
            // 
            // txtConfirmPass
            // 
            txtConfirmPass.Font = new Font("Segoe UI", 10F);
            txtConfirmPass.ForeColor = Color.Gray;
            txtConfirmPass.Location = new Point(43, 225);
            txtConfirmPass.Multiline = true;
            txtConfirmPass.Name = "txtConfirmPass";
            txtConfirmPass.Size = new Size(479, 40);
            txtConfirmPass.TabIndex = 19;
            txtConfirmPass.Text = "Confirm your password...";
            txtConfirmPass.Enter += TxtConfirmPass_Enter;
            txtConfirmPass.Leave += TxtNewPass_Leave;
            // 
            // txtNewPass
            // 
            txtNewPass.Font = new Font("Segoe UI", 10F);
            txtNewPass.ForeColor = Color.Gray;
            txtNewPass.Location = new Point(43, 153);
            txtNewPass.Multiline = true;
            txtNewPass.Name = "txtNewPass";
            txtNewPass.Size = new Size(479, 40);
            txtNewPass.TabIndex = 18;
            txtNewPass.Text = "Enter new password...";
            txtNewPass.Enter += TxtNewPass_Enter;
            txtNewPass.Leave += TxtNewPass_Leave;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.Blue;
            btnSubmit.Font = new Font("Segoe UI", 10F);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(44, 294);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(479, 70);
            btnSubmit.TabIndex = 12;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            label1.Location = new Point(133, 38);
            label1.Name = "label1";
            label1.Size = new Size(286, 54);
            label1.TabIndex = 10;
            label1.Text = "New Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(154, 92);
            label2.Name = "label2";
            label2.Size = new Size(244, 25);
            label2.TabIndex = 11;
            label2.Text = "Please enter a new password ";
            // 
            // FormNewPass
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(565, 413);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormNewPass";
            Text = "FormNewPass";
            Load += FormNewPass_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnSubmit;
        private Label label1;
        private Label label2;
        private TextBox txtConfirmPass;
        private TextBox txtNewPass;
        private Guna.UI2.WinForms.Guna2ImageButton btnClose;
    }
}