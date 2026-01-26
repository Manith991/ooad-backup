namespace OOAD_Project
{
    partial class RecordForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordForm));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panel1 = new Panel();
            dgvRecord = new DataGridView();
            colNo = new DataGridViewTextBoxColumn();
            colTable = new DataGridViewTextBoxColumn();
            colStaff = new DataGridViewTextBoxColumn();
            colOrder = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colPayment = new DataGridViewTextBoxColumn();
            colDetail = new DataGridViewImageColumn();
            colDelete = new DataGridViewImageColumn();
            panel2 = new Panel();
            btnAdd = new Guna.UI2.WinForms.Guna2Button();
            lblRecord = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecord).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dgvRecord);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 124);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(35, 0, 35, 35);
            panel1.Size = new Size(1430, 846);
            panel1.TabIndex = 12;
            // 
            // dgvRecord
            // 
            dgvRecord.AllowUserToAddRows = false;
            dgvRecord.AllowUserToDeleteRows = false;
            dgvRecord.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecord.BackgroundColor = Color.White;
            dgvRecord.BorderStyle = BorderStyle.None;
            dgvRecord.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.Silver;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.Silver;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvRecord.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvRecord.ColumnHeadersHeight = 70;
            dgvRecord.Columns.AddRange(new DataGridViewColumn[] { colNo, colTable, colStaff, colOrder, colDate, colTotal, colStatus, colPayment, colDetail, colDelete });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvRecord.DefaultCellStyle = dataGridViewCellStyle2;
            dgvRecord.Dock = DockStyle.Fill;
            dgvRecord.EnableHeadersVisualStyles = false;
            dgvRecord.GridColor = Color.Black;
            dgvRecord.Location = new Point(35, 0);
            dgvRecord.MultiSelect = false;
            dgvRecord.Name = "dgvRecord";
            dgvRecord.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Silver;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvRecord.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvRecord.RowHeadersVisible = false;
            dgvRecord.RowHeadersWidth = 62;
            dgvRecord.RowTemplate.Height = 70;
            dgvRecord.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecord.Size = new Size(1360, 811);
            dgvRecord.TabIndex = 7;
            dgvRecord.CellContentClick += dgvRecord_CellContentClick;
            // 
            // colNo
            // 
            colNo.FillWeight = 41.5350647F;
            colNo.HeaderText = "No.";
            colNo.MinimumWidth = 8;
            colNo.Name = "colNo";
            colNo.ReadOnly = true;
            // 
            // colTable
            // 
            colTable.FillWeight = 191.65863F;
            colTable.HeaderText = "Table Name";
            colTable.MinimumWidth = 8;
            colTable.Name = "colTable";
            colTable.ReadOnly = true;
            // 
            // colStaff
            // 
            colStaff.FillWeight = 121.257469F;
            colStaff.HeaderText = "Staff Name";
            colStaff.MinimumWidth = 8;
            colStaff.Name = "colStaff";
            colStaff.ReadOnly = true;
            // 
            // colOrder
            // 
            colOrder.FillWeight = 111.327789F;
            colOrder.HeaderText = "Order Type";
            colOrder.MinimumWidth = 8;
            colOrder.Name = "colOrder";
            colOrder.ReadOnly = true;
            // 
            // colDate
            // 
            colDate.FillWeight = 121.072037F;
            colDate.HeaderText = "Date";
            colDate.MinimumWidth = 8;
            colDate.Name = "colDate";
            colDate.ReadOnly = true;
            // 
            // colTotal
            // 
            colTotal.FillWeight = 70.8061F;
            colTotal.HeaderText = "Total";
            colTotal.MinimumWidth = 8;
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.FillWeight = 60.5200729F;
            colStatus.HeaderText = "Status";
            colStatus.MinimumWidth = 8;
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // colPayment
            // 
            colPayment.FillWeight = 82.559906F;
            colPayment.HeaderText = "Paid by";
            colPayment.MinimumWidth = 8;
            colPayment.Name = "colPayment";
            colPayment.ReadOnly = true;
            // 
            // colDetail
            // 
            colDetail.FillWeight = 31.7648869F;
            colDetail.HeaderText = "";
            colDetail.Image = Properties.Resources.detail;
            colDetail.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colDetail.MinimumWidth = 8;
            colDetail.Name = "colDetail";
            colDetail.ReadOnly = true;
            // 
            // colDelete
            // 
            colDelete.FillWeight = 31.502964F;
            colDelete.HeaderText = "";
            colDelete.Image = (Image)resources.GetObject("colDelete.Image");
            colDelete.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colDelete.MinimumWidth = 8;
            colDelete.Name = "colDelete";
            colDelete.ReadOnly = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnAdd);
            panel2.Controls.Add(lblRecord);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1430, 124);
            panel2.TabIndex = 13;
            // 
            // btnAdd
            // 
            btnAdd.BorderRadius = 10;
            btnAdd.BorderThickness = 1;
            btnAdd.CustomizableEdges = customizableEdges1;
            btnAdd.DisabledState.BorderColor = Color.DarkGray;
            btnAdd.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAdd.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAdd.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAdd.FillColor = Color.Black;
            btnAdd.Font = new Font("Segoe UI", 9F);
            btnAdd.ForeColor = Color.White;
            btnAdd.Image = (Image)resources.GetObject("btnAdd.Image");
            btnAdd.ImageSize = new Size(60, 60);
            btnAdd.Location = new Point(34, 26);
            btnAdd.Margin = new Padding(0);
            btnAdd.Name = "btnAdd";
            btnAdd.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnAdd.Size = new Size(70, 70);
            btnAdd.TabIndex = 5;
            // 
            // lblRecord
            // 
            lblRecord.AutoSize = true;
            lblRecord.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblRecord.Location = new Point(107, 42);
            lblRecord.Name = "lblRecord";
            lblRecord.Size = new Size(117, 38);
            lblRecord.TabIndex = 6;
            lblRecord.Text = "Records";
            // 
            // RecordForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1430, 970);
            Controls.Add(panel1);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "RecordForm";
            Text = "RecordForm";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRecord).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dgvRecord;
        private Panel panel2;
        private Guna.UI2.WinForms.Guna2Button btnAdd;
        private Label lblRecord;
        private DataGridViewTextBoxColumn colNo;
        private DataGridViewTextBoxColumn colTable;
        private DataGridViewTextBoxColumn colStaff;
        private DataGridViewTextBoxColumn colOrder;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colPayment;
        private DataGridViewImageColumn colDetail;
        private DataGridViewImageColumn colDelete;
    }
}