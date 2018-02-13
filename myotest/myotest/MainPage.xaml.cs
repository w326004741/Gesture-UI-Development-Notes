using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Myo;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace myotest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : INotifyPropertyChanged
    {
        private readonly global::Myo.Myo _myo;
        private readonly global::Myo.Myo _myo1;
      
            public MainPage()
        {
            this.InitializeComponent();

            _myo = new global::Myo.Myo(); // :: is Namespace,别名限定符,用于查找标识符,始终位于两个标识符之间。
                                                                        // 命名空间可以为global,这将调用全局命名空间(而不是别名命名空间)中的查找
            try
            {
                _myo.Connect();
                _myo.OnPoseDetected += _myo_OnPoseDetected; //Detected 检测到的
                // for accelerometer data
                _myo.DataAvailable += _myo_DataAvailable;
                _myo.OnError += _myo_OnError;
                tblStatus.Text = "Successfully connected to Myo";
                _myo.Vibrate(Myo.Myo.VibrationType.Medium);
               // _myo.Unlock(Myo.Myo.UnlockType);
                //_myo.OnEMGAvailable += _myo_OnEMGAvailable; //EMG 肌电图 
            }
            catch (Exception err)
            {
                tblStatus.Text = "Problem connecting to Myo." + System.Environment.NewLine + err.Message;
            }
        }

        private void _myo_OnError(object sender, MyoErrorEventArgs e)
        {

            tblStatus.Text = "Problem connecting to Myo." + Environment.NewLine + e.Message; ;

        }

        private async void _myo_DataAvailable(object sender, MyoDataEventArgs e)
        {
            await Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    
                    updateUIAccelerometer(e.Acceletometer.X,
                                                                e.Acceletometer.Y,
                                                                e.Acceletometer.Z);
                });

        }

        private void updateUIAccelerometer(double xRoll, double yPitch, double zYaw)
        {
            var pitchDegree = (yPitch * 180.0) / System.Math.PI;
            var yawDegree = (zYaw * 180.0) / System.Math.PI;
            var rollDegree = (xRoll * 180.0) / System.Math.PI;
            // from UI
            tblXValue.Text = "Pitch:  " + (pitchDegree).ToString("0.00"); // pitch : 投
            tblYValue.Text = "Yaw:  " + (yawDegree).ToString("0.00"); // yaw: 偏离
            tblZValue.Text = "Roll:  " + (rollDegree).ToString("0.00"); // raw : 卷，翻滚
            // from UI
            pitchLine.X2 = pitchLine.X1 + pitchDegree;
            yawLine.Y2 = yawLine.Y1 - yawDegree;
            rollLine.X2 = rollLine.X1 - rollDegree;
            //show the values on an axis

        }

        private async void _myo_OnPoseDetected(object sender, MyoPoseEventArgs e)
        {
            string pose;
            switch (e.Pose)           // swich -> two type -> change to the e.Pose --> Enter
            {
                case MyoPoseEventArgs.PoseType.Rest:
                    pose = e.Pose.ToString();
                    break;
                case MyoPoseEventArgs.PoseType.Fist:
                    break;
                case MyoPoseEventArgs.PoseType.WaveIn:
                    break;
                case MyoPoseEventArgs.PoseType.WaveOut:
                    break;
                case MyoPoseEventArgs.PoseType.DoubleTap:
                    break;
                case MyoPoseEventArgs.PoseType.FingersSpread:
                    break;
                default:
                    break;
            }
            pose = e.Pose.ToString();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    updateUI(pose);

                });
            //            if (e.Pose != MyoPoseEventArgs.PoseType.Fist) return; //Fist拳头 Args参数
            //           Debug.WriteLine("Fist");
        }

        private void updateUI(string pose)
        {
            tblStatus.Text = "Current Pose: " + pose;
            
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
