namespace OOAD_Project
{
    partial class DiningForm
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
            panelMain = new Panel();
            flpTable = new FlowLayoutPanel();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.White;
            panelMain.Controls.Add(flpTable);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1430, 970);
            panelMain.TabIndex = 3;
            // 
            // flpTable
            // 
            flpTable.AutoScroll = true;
            flpTable.Dock = DockStyle.Fill;
            flpTable.Location = new Point(0, 0);
            flpTable.Name = "flpTable";
            flpTable.Size = new Size(1430, 970);
            flpTable.TabIndex = 0;
            // 
            // DiningForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1430, 970);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "DiningForm";
            Text = "                             ";
            WindowState = FormWindowState.Maximized;
            panelMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel panelMain;
        private FlowLayoutPanel flpTable;
    }
}