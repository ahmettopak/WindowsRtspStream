using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Vlc.DotNet.Forms;
using SharpDX.DirectInput;
using SharpDX.XInput;


namespace RtspStreamApp {
    public partial class Form1 : Form {
       
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            InitializeVlcControl();
            //InitializeJoystick();
            //SetupJoystickTimer();

            InitializeController();
            SetupControllerTimer();
        }

        #region VlcStream


        private VlcControl vlcControl;
        private VlcControl vlcControl1;

        private void InitializeVlcControl()
        {
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

        private void StartCapture(string rtspUrl)
        {
            if (vlcControl != null)
            {
                vlcControl.Stop(); // Önceki medya durduruluyor
                vlcControl.Play(new Uri(rtspUrl));
            }


        }

        private void StartRightVideo(string rtspUrl)
        {
            if (vlcControl1 != null)
            {
                vlcControl1.Stop(); // Önceki medya durduruluyor
                vlcControl1.Play(new Uri(rtspUrl));
            }
        }

        private void StopCapture()
        {
            if (vlcControl != null)
            {
                vlcControl.Stop();
            }
        }
        #endregion

        //private DirectInput directInput;
        //private Joystick joystick;

        //private void InitializeJoystick()
        //{
        //    directInput = new DirectInput();

        //    // Joystick cihazlarını taramak için DirectInput'i kullanın
        //    var joystickGuid = Guid.Empty;
        //    var availableDevices = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);
        //    foreach (var deviceInstance in availableDevices)
        //    {
        //        joystickGuid = deviceInstance.InstanceGuid;
        //        break; // İlk bulunan joystick'i kullanıyoruz
        //    }

        //    if (joystickGuid == Guid.Empty)
        //    {
        //        MessageBox.Show("Joystick bulunamadı!  " + availableDevices.Count);
        //        return;
        //    }

        //    joystick = new Joystick(directInput, joystickGuid);
        //    joystick.Properties.BufferSize = 128; // Giriş verilerini depolamak için önbellek boyutu

        //    // Joystick'i başlat
        //    try
        //    {
        //        joystick.Acquire();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Joystick başlatılırken bir hata oluştu: " + ex.Message);
        //    }
        //}

        //private void ReadJoystickInput()
        //{
        //    if (joystick == null)
        //        return;

        //    joystick.Poll();
        //    var joystickState = joystick.GetCurrentState();

        //    // Joystick verilerini işleyin
        //    int xAxis = joystickState.X;
        //    int yAxis = joystickState.Y;
        //    int zAxis = joystickState.Z;
        //   // int[] buttons = joystickState.get;

        //    // Burada yapmak istediğiniz işlemi gerçekleştirin veya verileri görselleştirin
        //    // Örneğin: Eksen verilerini label'larda veya ProgressBar'larda göstermek, buton durumlarını değerlendirmek, vb.
        //}

        //private Timer joystickTimer;

        //private void SetupJoystickTimer()
        //{
        //    joystickTimer = new Timer();
        //    joystickTimer.Interval = 100; // 100 ms aralıklarla veri alacak
        //    joystickTimer.Tick += (sender, e) => ReadJoystickInput();
        //    joystickTimer.Start();
        //}

        private Controller controller;
        private UserIndex playerIndex = UserIndex.One; // Eğer birden fazla controller varsa, uygun playerIndex'i seçin.

        private void InitializeController()
        {
            try
            {
                controller = new Controller(playerIndex);
                if (!controller.IsConnected)
                {
                    MessageBox.Show("Controller bulunamadı!");
                    return;
                }
                else
                {
                    MessageBox.Show("Bulundu");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Controller başlatılırken bir hata oluştu: " + ex.Message);
            }
        }

        private void ReadControllerInput()
        {
            if (controller == null || !controller.IsConnected)
                return;

            Gamepad gamepadState = controller.GetState().Gamepad;

            // Gamepad verilerini işleyin
            short leftThumbX = gamepadState.LeftThumbX;
            short leftThumbY = gamepadState.LeftThumbY;
            short rightThumbX = gamepadState.RightThumbX;
            short rightThumbY = gamepadState.RightThumbY;
            byte leftTrigger = gamepadState.LeftTrigger;
            byte rightTrigger = gamepadState.RightTrigger;
            bool buttonA = (gamepadState.Buttons & GamepadButtonFlags.A) != 0;
            bool buttonB = (gamepadState.Buttons & GamepadButtonFlags.B) != 0;
            bool buttonX = (gamepadState.Buttons & GamepadButtonFlags.X) != 0;
            bool buttonY = (gamepadState.Buttons & GamepadButtonFlags.Y) != 0;
          
            label1.Text = NormalizeAxisValue(leftThumbX)  + " " + NormalizeAxisValue(leftThumbY);
        }

        private Timer controllerTimer;

        private void SetupControllerTimer()
        {
            controllerTimer = new Timer();
            controllerTimer.Interval = 100; // 100 ms aralıklarla veri alacak
            controllerTimer.Tick += (sender, e) => ReadControllerInput();
            controllerTimer.Start();
        }

        private float NormalizeAxisValue(short axisValue)
        {
            const float minAxisValue = -32768f;
            const float maxAxisValue = 32767f;

            return (axisValue - minAxisValue) / ((maxAxisValue - minAxisValue) / 2f) - 1f;
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
