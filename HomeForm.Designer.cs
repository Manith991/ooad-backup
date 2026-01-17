namespace OOAD_Project
{
    partial class HomeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeForm));
            mediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)mediaPlayer).BeginInit();
            SuspendLayout();
            // 
            // mediaPlayer
            // 
            mediaPlayer.Dock = DockStyle.Fill;
            mediaPlayer.Enabled = true;
            mediaPlayer.Location = new Point(0, 0);
            mediaPlayer.Name = "mediaPlayer";
            mediaPlayer.OcxState = (AxHost.State)resources.GetObject("mediaPlayer.OcxState");
            mediaPlayer.Size = new Size(1430, 970);
            mediaPlayer.TabIndex = 0;
            mediaPlayer.PlayStateChange += MediaPlayer_PlayStateChange;
            mediaPlayer.KeyDownEvent += MediaPlayer_KeyDown;
            mediaPlayer.MouseDownEvent += MediaPlayer_MouseDownEvent;
            // 
            // HomeForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1430, 970);
            Controls.Add(mediaPlayer);
            FormBorderStyle = FormBorderStyle.None;
            Name = "HomeForm";
            Text = "FormHome";
            Load += HomeForm_Load;
            ((System.ComponentModel.ISupportInitialize)mediaPlayer).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer mediaPlayer;
    }
}