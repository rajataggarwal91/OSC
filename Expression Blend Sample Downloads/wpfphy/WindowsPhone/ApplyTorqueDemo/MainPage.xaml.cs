using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Spritehand.Devices.Sensors;
using Spritehand.FarseerHelper;

namespace ApplyTorqueDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        AccelerometerWrapper _accelerometer;
        PhysicsControllerMain _physicsController;
        AccelerometerState _lastReading = new AccelerometerState();

        public MainPage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            Application.Current.Host.Settings.EnableFrameRateCounter = true;
            Application.Current.Host.Settings.EnableCacheVisualization = false;
            Application.Current.Host.Settings.MaxFrameRate = 30;


        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _physicsController = cnvGame.GetValue(PhysicsControllerMain.PhysicsControllerProperty) as PhysicsControllerMain;
            _physicsController.TimerLoop += new PhysicsControllerMain.TimerLoopHandler(_physicsController_TimerLoop);
            
            _accelerometer = new AccelerometerWrapper(cnvGame);
            _accelerometer.ReadingChanged += new AccelerometerWrapper.ReadingChangedHandler(_accelerometer_ReadingChanged);
        }

        void _accelerometer_ReadingChanged(AccelerometerState state)
        {
            _lastReading = state;
        }       
        
        void _physicsController_TimerLoop(object source)
        {
            _physicsController.PhysicsObjects["ball"].BodyObject.ApplyTorque(-(float)(_lastReading.X * 100000));
        }

        

 
    }
}
