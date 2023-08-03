using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Vlc.DotNet.Forms;

namespace RtspStreamApp {
    public partial class Form1 : Form {
       
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            InitializeVlcControl();
        }

        private VlcControl vlcControl;
        private VlcControl vlcControl1;

        private void InitializeVlcControl() {
            vlcControl = new VlcControl();
            vlcControl1 = new VlcControl();

          var vlcArgs = new List<string>
    {
        "--no-plugins-cache",
        "--no-video-title-show",
        "--no-snapshot-preview",
        "--file-caching=0",
        "--clock-jitter=0",
        "--network-caching=80",
        "--live-caching=0",
        "--no-overlay",
        "--clock-jitter=0"
    };


            vlcControl.BeginInit();
            vlcControl.VlcMediaplayerOptions = vlcArgs.ToArray();
            vlcControl.VlcLibDirectory = new DirectoryInfo(@"C:\Program Files (x86)\VideoLAN\VLC"); // VLC kütüphane dizini
            vlcControl.EndInit();
            vlcControl.Dock = DockStyle.Fill;
            panel1.Controls.Add(vlcControl); // RTSP yayınını göstermek için bir Panel kontrolü ekleyin


            vlcControl1.BeginInit();
            vlcControl1.VlcMediaplayerOptions = vlcArgs.ToArray();
            vlcControl1.VlcLibDirectory = new DirectoryInfo(@"C:\Program Files (x86)\VideoLAN\VLC"); // VLC kütüphane dizini
            vlcControl1.EndInit();
            vlcControl1.Dock = DockStyle.Fill;
            panel2.Controls.Add(vlcControl1); // RTSP yayınını göstermek için bir Panel kontrolü ekleyin
        }

        private void StartCapture(string rtspUrl) {
            if (vlcControl != null) {
                vlcControl.Stop(); // Önceki medya durduruluyor
                vlcControl.Play(new Uri(rtspUrl));
            }

     
        }
         
        private void StartRightVideo(string rtspUrl) {
            if (vlcControl1 != null) {
                vlcControl1.Stop(); // Önceki medya durduruluyor
                vlcControl1.Play(new Uri(rtspUrl));
            }
        }

        private void StopCapture() {
            if (vlcControl != null) {
                vlcControl.Stop();
            }
        }


        private void startButton_Click(object sender, EventArgs e) {
            string rtspUrl = "rtsp://192.168.1.10:554/user=admin&password=&channel=1&stream=0.sdp?";
            string rtspUrl1 = "rtsp://192.168.1.10:554/user=admin&password=&channel=3&stream=0.sdp?";
            StartCapture(rtspUrl);
            StartRightVideo(rtspUrl1);


        }

        private void stopButton_Click(object sender, EventArgs e) {
            StopCapture();


        }
    }
}
