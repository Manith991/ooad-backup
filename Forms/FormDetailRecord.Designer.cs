namespace OOAD_Project
{
    partial class FormDetailRecord
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDetailRecord));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panel1 = new Panel();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            label1 = new Label();
            btnClose = new Guna.UI2.WinForms.Guna2ImageButton();
            panel2 = new Panel();
            lblID = new Label();
            lblTable = new Label();
            lblStaff = new Label();
            lblType = new Label();
            lblDate = new Label();
            lblTotal = new Label();
            lblStatus = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            flowPanelProducts = new FlowLayoutPanel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.Controls.Add(guna2Button1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnClose);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(578, 80);
            panel1.TabIndex = 0;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderColor = Color.White;
            guna2Button1.BorderRadius = 10;
            guna2Button1.BorderThickness = 2;
            guna2Button1.CustomizableEdges = customizableEdges1;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.Black;
            guna2Button1.Font = new Font("Segoe UI", 9F);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Image = (Image)resources.GetObject("guna2Button1.Image");
            guna2Button1.ImageSize = new Size(45, 45);
            guna2Button1.Location = new Point(16, 14);
            guna2Button1.Margin = new Padding(0);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button1.Size = new Size(50, 50);
            guna2Button1.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(68, 22);
            label1.Name = "label1";
            label1.Size = new Size(166, 32);
            label1.TabIndex = 2;
            label1.Text = "Records Detail";
            // 
            // btnClose
            // 
            btnClose.CheckedState.ImageSize = new Size(64, 64);
            btnClose.HoverState.ImageSize = new Size(64, 64);
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageOffset = new Point(0, 0);
            btnClose.ImageRotate = 0F;
            btnClose.ImageSize = new Size(50, 50);
            btnClose.Location = new Point(512, 3);
            btnClose.Name = "btnClose";
            btnClose.PressedState.ImageSize = new Size(64, 64);
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges3;
            btnClose.Size = new Size(66, 66);
            btnClose.TabIndex = 1;
            btnClose.Click += btnClose_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(lblID);
            panel2.Controls.Add(lblTable);
            panel2.Controls.Add(lblStaff);
            panel2.Controls.Add(lblType);
            panel2.Controls.Add(lblDate);
            panel2.Controls.Add(lblTotal);
            panel2.Controls.Add(lblStatus);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label8);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 80);
            panel2.Name = "panel2";
            panel2.Size = new Size(578, 319);
            panel2.TabIndex = 29;
            // 
            // lblID
            // 
            lblID.AutoSize = true;
            lblID.Font = new Font("Segoe UI", 11F);
            lblID.ForeColor = Color.Black;
            lblID.Location = new Point(150, 18);
            lblID.Name = "lblID";
            lblID.Size = new Size(71, 30);
            lblID.TabIndex = 36;
            lblID.Text = "label1";
            // 
            // lblTable
            // 
            lblTable.AutoSize = true;
            lblTable.Font = new Font("Segoe UI", 11F);
            lblTable.ForeColor = Color.Black;
            lblTable.Location = new Point(150, 58);
            lblTable.Name = "lblTable";
            lblTable.Size = new Size(71, 30);
            lblTable.TabIndex = 37;
            lblTable.Text = "label2";
            // 
            // lblStaff
            // 
            lblStaff.AutoSize = true;
            lblStaff.Font = new Font("Segoe UI", 11F);
            lblStaff.ForeColor = Color.Black;
            lblStaff.Location = new Point(150, 98);
            lblStaff.Name = "lblStaff";
            lblStaff.Size = new Size(71, 30);
            lblStaff.TabIndex = 38;
            lblStaff.Text = "label3";
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Font = new Font("Segoe UI", 11F);
            lblType.ForeColor = Color.Black;
            lblType.Location = new Point(150, 138);
            lblType.Name = "lblType";
            lblType.Size = new Size(71, 30);
            lblType.TabIndex = 39;
            lblType.Text = "label4";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 11F);
            lblDate.ForeColor = Color.Black;
            lblDate.Location = new Point(150, 178);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(71, 30);
            lblDate.TabIndex = 40;
            lblDate.Text = "label5";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 11F);
            lblTotal.ForeColor = Color.Black;
            lblTotal.Location = new Point(150, 218);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(71, 30);
            lblTotal.TabIndex = 41;
            lblTotal.Text = "label6";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11F);
            lblStatus.ForeColor = Color.Black;
            lblStatus.Location = new Point(150, 258);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(71, 30);
            lblStatus.TabIndex = 42;
            lblStatus.Text = "label7";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(16, 18);
            label2.Name = "label2";
            label2.Size = new Size(103, 30);
            label2.TabIndex = 29;
            label2.Text = "Order ID:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(16, 58);
            label3.Name = "label3";
            label3.Size = new Size(133, 30);
            label3.TabIndex = 30;
            label3.Text = "Table Name:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(16, 98);
            label4.Name = "label4";
            label4.Size = new Size(125, 30);
            label4.TabIndex = 31;
            label4.Text = "Staff Name:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11F);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(16, 138);
            label5.Name = "label5";
            label5.Size = new Size(129, 30);
            label5.TabIndex = 32;
            label5.Text = "Order Type:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 11F);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(16, 178);
            label6.Name = "label6";
            label6.Size = new Size(127, 30);
            label6.TabIndex = 33;
            label6.Text = "Order Date:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 11F);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(16, 218);
            label7.Name = "label7";
            label7.Size = new Size(64, 30);
            label7.TabIndex = 34;
            label7.Text = "Total:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 11F);
            label8.ForeColor = Color.Black;
            label8.Location = new Point(16, 258);
            label8.Name = "label8";
            label8.Size = new Size(75, 30);
            label8.TabIndex = 35;
            label8.Text = "Status:";
            // 
            // flowPanelProducts
            // 
            flowPanelProducts.AutoScroll = true;
            flowPanelProducts.BackColor = Color.Silver;
            flowPanelProducts.Dock = DockStyle.Fill;
            flowPanelProducts.Location = new Point(0, 399);
            flowPanelProducts.Name = "flowPanelProducts";
            flowPanelProducts.Size = new Size(578, 256);
            flowPanelProducts.TabIndex = 30;
            // 
            // FormDetailRecord
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(578, 655);
            Controls.Add(flowPanelProducts);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormDetailRecord";
            Text = "FormDetailRecord";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Guna.UI2.WinForms.Guna2ImageButton btnClose;
        private Label label1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Panel panel2;
        private Label lblID;
        private Label lblTable;
        private Label lblStaff;
        private Label lblType;
        private Label lblDate;
        private Label lblTotal;
        private Label lblStatus;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private FlowLayoutPanel flowPanelProducts;
    }
}