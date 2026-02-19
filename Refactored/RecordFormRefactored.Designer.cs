namespace OOAD_Project.Refactored
{
    partial class RecordFormRefactored
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
            dgvRecord = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvRecord).BeginInit();
            SuspendLayout();
            // 
            // dgvRecord
            // 
            dgvRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecord.Location = new Point(257, 120);
            dgvRecord.Name = "dgvRecord";
            dgvRecord.RowHeadersWidth = 62;
            dgvRecord.Size = new Size(360, 225);
            dgvRecord.TabIndex = 0;
            // 
            // RecordFormRefactored
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvRecord);
            Name = "RecordFormRefactored";
            Text = "RecordFormRefactored";
            ((System.ComponentModel.ISupportInitialize)dgvRecord).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvRecord;
    }
}