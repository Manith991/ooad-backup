using AxWMPLib;
using WMPLib;
using System;
using System.Windows.Forms;

namespace OOAD_Project
{
    public partial class HomeForm : Form
    {
        private bool isPaused = false;
        private bool isMuted = true;

        public HomeForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            string videoPath = @"D:\RUPP\OOAD\OOAD_Project\OOAD_Project\Video\Restaurant Ad Video Template.mp4"; 

            mediaPlayer.settings.autoStart = true;
            mediaPlayer.settings.setMode("loop", false); // We'll handle looping manually
            mediaPlayer.settings.mute = isMuted;
            mediaPlayer.URL = videoPath;
            mediaPlayer.uiMode = "none";
            mediaPlayer.Ctlcontrols.play();
        }

        private void MediaPlayer_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == (int)WMPPlayState.wmppsMediaEnded)
            {
                // Manual loop
                mediaPlayer.Ctlcontrols.currentPosition = 0;
                mediaPlayer.Ctlcontrols.play();
            }
        }

        private void MediaPlayer_MouseDownEvent(object sender, _WMPOCXEvents_MouseDownEvent e)
        {
            if (e.nButton == 1)
            {
                TogglePlayPause();
            }
            else if (e.nButton == 2) // Right-click
            {
                mediaPlayer.Ctlcontrols.stop();
                isPaused = true;
            }
        }

        private void MediaPlayer_KeyDown(object sender, _WMPOCXEvents_KeyDownEvent e)
        {
            switch (e.nKeyCode)
            {
                case (int)Keys.Enter:
                    TogglePlayPause();
                    break;
                case (int)Keys.F:
                    mediaPlayer.fullScreen = !mediaPlayer.fullScreen;
                    break;
                case (int)Keys.M:
                    isMuted = !isMuted;
                    mediaPlayer.settings.mute = isMuted;
                    break;
            }
        }

        private void TogglePlayPause()
        {
            if (isPaused)
            {
                mediaPlayer.Ctlcontrols.play();
                isPaused = false;
            }
            else
            {
                mediaPlayer.Ctlcontrols.pause();
                isPaused = true;
            }
        }
    }
}