namespace OOAD_Project.Components
{
    partial class FoodCard
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FoodCard));
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            pbFood = new PictureBox();
            lblPrice = new Label();
            lblTitle = new Label();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbFood).BeginInit();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            guna2Panel1.BackColor = Color.White;
            guna2Panel1.BorderColor = Color.FromArgb(64, 64, 64);
            guna2Panel1.BorderRadius = 1;
            guna2Panel1.BorderThickness = 1;
            guna2Panel1.Controls.Add(pbFood);
            guna2Panel1.Controls.Add(lblPrice);
            guna2Panel1.Controls.Add(lblTitle);
            guna2Panel1.CustomizableEdges = customizableEdges1;
            guna2Panel1.FillColor = Color.White;
            guna2Panel1.Location = new Point(7, 5);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel1.Size = new Size(395, 221);
            guna2Panel1.TabIndex = 0;
            // 
            // pbFood
            // 
            pbFood.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pbFood.BorderStyle = BorderStyle.FixedSingle;
            pbFood.Image = (Image)resources.GetObject("pbFood.Image");
            pbFood.Location = new Point(232, 37);
            pbFood.Name = "pbFood";
            pbFood.Size = new Size(150, 150);
            pbFood.SizeMode = PictureBoxSizeMode.StretchImage;
            pbFood.TabIndex = 2;
            pbFood.TabStop = false;
            // 
            // lblPrice
            // 
            lblPrice.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 14F);
            lblPrice.ForeColor = Color.FromArgb(0, 192, 0);
            lblPrice.Location = new Point(17, 166);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(98, 38);
            lblPrice.TabIndex = 1;
            lblPrice.Text = "$12.99";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold | FontStyle.Italic);
            lblTitle.Location = new Point(8, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(193, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Seafood Pizza";
            // 
            // FoodCard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Blue;
            Controls.Add(guna2Panel1);
            Name = "FoodCard";
            Size = new Size(410, 235);
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbFood).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        public Label lblTitle;
        public Label lblPrice;
        public PictureBox pbFood;
    }
}
