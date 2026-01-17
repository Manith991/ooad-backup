namespace OOAD_Project.Components
{
    partial class TableCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableCard));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            lblCapacity = new Label();
            label2 = new Label();
            guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            lblStatus = new Label();
            lblTable = new Label();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            guna2Panel2.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.FromArgb(64, 64, 64);
            guna2Panel1.BorderRadius = 20;
            guna2Panel1.Controls.Add(guna2PictureBox1);
            guna2Panel1.Controls.Add(lblCapacity);
            guna2Panel1.Controls.Add(label2);
            guna2Panel1.Controls.Add(guna2Panel2);
            guna2Panel1.Controls.Add(lblTable);
            guna2Panel1.CustomizableEdges = customizableEdges5;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Panel1.Size = new Size(405, 260);
            guna2Panel1.TabIndex = 0;
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.CustomizableEdges = customizableEdges1;
            guna2PictureBox1.FillColor = Color.Black;
            guna2PictureBox1.Image = (Image)resources.GetObject("guna2PictureBox1.Image");
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(3, 57);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2PictureBox1.Size = new Size(245, 200);
            guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            guna2PictureBox1.TabIndex = 5;
            guna2PictureBox1.TabStop = false;
            // 
            // lblCapacity
            // 
            lblCapacity.AutoSize = true;
            lblCapacity.BackColor = Color.Transparent;
            lblCapacity.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCapacity.ForeColor = Color.White;
            lblCapacity.Location = new Point(347, 222);
            lblCapacity.Name = "lblCapacity";
            lblCapacity.Size = new Size(46, 28);
            lblCapacity.TabIndex = 3;
            lblCapacity.Text = "400";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(288, 222);
            label2.Name = "label2";
            label2.Size = new Size(67, 28);
            label2.TabIndex = 2;
            label2.Text = "Seats: ";
            // 
            // guna2Panel2
            // 
            guna2Panel2.BackColor = Color.FromArgb(192, 255, 192);
            guna2Panel2.BorderRadius = 10;
            guna2Panel2.Controls.Add(lblStatus);
            guna2Panel2.CustomizableEdges = customizableEdges3;
            guna2Panel2.Location = new Point(246, 12);
            guna2Panel2.Name = "guna2Panel2";
            guna2Panel2.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Panel2.Size = new Size(147, 39);
            guna2Panel2.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Green;
            lblStatus.Location = new Point(0, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(147, 39);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Available";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTable
            // 
            lblTable.AutoSize = true;
            lblTable.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTable.ForeColor = Color.White;
            lblTable.Location = new Point(12, 12);
            lblTable.Name = "lblTable";
            lblTable.Size = new Size(71, 32);
            lblTable.TabIndex = 0;
            lblTable.Text = "Table";
            // 
            // TableCard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(guna2Panel1);
            Name = "TableCard";
            Size = new Size(405, 260);
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            guna2Panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Label lblTable;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Label lblStatus;
        private Label label2;
        private Label lblCapacity;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
    }
}
