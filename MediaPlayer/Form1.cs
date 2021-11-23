﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;
using Microsoft.DirectX.AudioVideoPlayback;
using WMPLib;

namespace MediaPlayer
{
    public partial class Form1 : Form
    {
        SoundPlayer player = new SoundPlayer();
        private int selectedIndex = 0;
        string[] vPath;
        string filename;
        double SkipReverseSpeed = 30; //default value for skip/reverse button 
        

        public Form1()
        {
            InitializeComponent();
            menuStrip1.BackColor = Color.Pink;
            btnHideShowPlaylistCLICKED.Hide();
            listBoxPlayList.Hide();
            axWindowsMediaPlayer1.Hide();
            btnPause.Hide();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
            Time.Start();
           
        }

        private void PrevBtn_Click(object sender, EventArgs e) // this is the prev
        {
            int index = listBoxPlayList.SelectedIndex;
            index--;
            if (index == -1)
            {
                index = vPath[listBoxPlayList.SelectedIndex].Count() - 1;
            }
            selectedIndex = index;
            listBoxPlayList.SelectedIndex = index;
           
        }

        private void NextBtn_Click(object sender, EventArgs e) // this is the next button
        {

            int index = listBoxPlayList.SelectedIndex;
            index++;
            if (index > vPath[listBoxPlayList.SelectedIndex].Count() - 1)
            {
                index = 0;
            }
            selectedIndex = index;
            listBoxPlayList.SelectedIndex = index;
          
        }

        private void PlayBtn_Click(object sender, EventArgs e) // this is the play button
        {
            axWindowsMediaPlayer1.Show();
            axWindowsMediaPlayer1.Ctlcontrols.play();

            PlayBtn.Hide();
            btnPause.Show();
            picBoxMediaPlayIcon.Hide();
        }


        private void RptBtn_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
        }

        private void btnFastforward_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition += SkipReverseSpeed;
        }

        private void btnRewind_Click(object sender, EventArgs e)
        {
              axWindowsMediaPlayer1.Ctlcontrols.currentPosition -= SkipReverseSpeed;
        }

        private void btnFullScreen_Click(object sender, EventArgs e) // Based on lyndons fullscreen method
        { 
            if(WindowState ==  FormWindowState.Normal )  // doesnt resize video player and buttons properly 
            {
                this.WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
                menuStrip1.Hide();
            }
            else if (WindowState == FormWindowState.Maximized )
            {
                this.WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.Sizable;
                menuStrip1.Show();
            } 

           // axWindowsMediaPlayer1.fullScreen = true; thiis makes the video player fullscreen but no controls or exit fullscreen
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            // FolderBrowserDialog folder = new FolderBrowserDialog();

            // folder.ShowNewFolderButton = true;

            // DialogResult result = folder.ShowDialog();
            //// d.Filter = "Video | *.mp4";
            // if (result == DialogResult.OK)
            // {
            //    //textBox1.Text = folderDlg.SelectedPath;
            //     Environment.SpecialFolder root = folder.RootFolder;
            //     vPath = folder.SelectedPath;
            //     axWindowsMediaPlayer1.URL = vPath;
            // }


            OpenFileDialog ofd2 = new OpenFileDialog();
            ofd2.Title = "Open Media File";
            ofd2.Filter = "WMV |*.wmv|WAV|*.wav|MP3|*.mp3|MP4|*.mp4|MKV|*.mkv"; // editied to to play video files previously couldnt show video file types                                                                                // ofd2.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ofd2.Multiselect = true;
            
            if (ofd2.ShowDialog() == DialogResult.OK)
            {
                filename = ofd2.SafeFileName;
                vPath = ofd2.FileNames;
                listBoxPlayList.Items.Clear();

                /* for (int i = 0; i < filename.Length; i++)
                 {
                     listBoxPlayList.Items.Add(filename[i]) ;                  
                 }*/
                foreach(string filename  in ofd2.SafeFileNames)
                {
                    listBoxPlayList.Items.Add(filename);
                }
                listBoxPlayList.SelectedIndex = 0;
             
                axWindowsMediaPlayer1.URL = axWindowsMediaPlayer1.URL = vPath[listBoxPlayList.SelectedIndex];
                axWindowsMediaPlayer1.Show();
                picBoxMediaPlayIcon.Hide();
                PlayBtn.Hide();
                btnPause.Show();

            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            PlayBtn.Show();
            btnPause.Hide();
        }

        private void btnHideShowPlaylist_Click(object sender, EventArgs e)
        {
            listBoxPlayList.Show();
            btnHideShowPlaylistCLICKED.Show();
            btnHideShowPlaylist.Hide();
        }

        private void btnHideShowPlaylistCLICKED_Click(object sender, EventArgs e)
        {
            listBoxPlayList.Hide();
            btnHideShowPlaylistCLICKED.Hide();
            btnHideShowPlaylist.Show();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer1.Hide();
            picBoxMediaPlayIcon.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void VolumeSlider_Scroll(object sender, EventArgs e)
        {
            Volumelbl.Text = VolumeSlider.Value.ToString() + '%';
        }

        private void VolumeSlider_MouseMove(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = VolumeSlider.Value;
            Volumelbl.Text = VolumeSlider.Value.ToString() + '%';
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            LengthSlider.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
        }
        private void LengthSlider_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = LengthSlider.Value;
            Duration.Text = axWindowsMediaPlayer1.currentMedia.duration.ToString();
        }

        private void listBoxPlayList_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = vPath[listBoxPlayList.SelectedIndex];
        }

        private void x05ToolStripMenuItem_Click(object sender, EventArgs e) // toolstrip - controls -media speed - 0.5
        {
            
            axWindowsMediaPlayer1.settings.rate = 0.5;
           
        }

        private void x10ToolStripMenuItem_Click(object sender, EventArgs e) // toolstrip - controls -media speed - 1
        {
            axWindowsMediaPlayer1.settings.rate = 1;
            
        }

        private void x2ToolStripMenuItem_Click(object sender, EventArgs e) // toolstrip - controls -media speed - 2
        {
            axWindowsMediaPlayer1.settings.rate = 2;
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)// toolstrip - controls -media speed - 3
        {
            axWindowsMediaPlayer1.settings.rate = 3;
        }

        private void secondsToolStripMenuItem_Click(object sender, EventArgs e) // toolstrip Skip/Reverse - toggle - 10secs
        {
            SkipReverseSpeed = 10;
        }

        private void secondsToolStripMenuItem1_Click(object sender, EventArgs e) // toolstrip Skip/Reverse - toggle - 30secs
        {
            SkipReverseSpeed = 30;
        }

        private void minuteToolStripMenuItem_Click(object sender, EventArgs e) // toolstrip Skip/Reverse - toggle - 1min
        {
            
            SkipReverseSpeed = 60;
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e) // show hide credits 
        {
           
           
           
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }
    }
}
