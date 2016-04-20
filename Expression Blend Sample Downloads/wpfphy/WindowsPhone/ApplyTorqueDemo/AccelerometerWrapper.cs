using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
#if SILVERLIGHT_PHONE
    using Microsoft.Devices.Sensors;
#endif

namespace Spritehand.Devices.Sensors
{

    /// <summary>
    /// provides a cross-platform wrapper for Accelerometer input.
    /// </summary>
    public class AccelerometerWrapper
    {
        public event ReadingChangedHandler ReadingChanged;
        public delegate void ReadingChangedHandler(AccelerometerState state);


#if !SILVERLIGHT_PHONE
        // if we are NOT on a Phone, then we will tie this input to a Perspective transform
 
        public AccelerometerWrapper(Canvas cnvPhone)
        {
            GameCanvas = cnvPhone;
        }



#endif

#if SILVERLIGHT_PHONE
        // if we are on a Phone, then we will tie this input to the real accelerometer.
        public AccelerometerWrapper(Canvas cnvPhone)
        {
            // really don't care about the Canvas at this point.
            this.sensor = AccelerometerSensor.Default;
            this.sensor.ReadingChanged += this.HandleSensorReadingChanged;
            try {
                this.sensor.Start();
            }
			catch (Exception) {
                // default to using Mouse for simulating accelerometer
                if (cnvPhone != null)
                {
                    GameCanvas = cnvPhone;
                }
            };
        }

        private AccelerometerSensor sensor = null;

        public void Stop()
        {
            if (this.sensor != null)
            {
                try
                {
                    this.sensor.Stop();
                }
                catch (Exception) { };

                this.sensor.ReadingChanged -= this.HandleSensorReadingChanged;
                this.sensor = null;
            }
        }

        private void HandleSensorReadingChanged(object sender, AccelerometerReadingAsyncEventArgs e)
        {
            if (this.ReadingChanged != null)
            {
                AccelerometerState newState = new AccelerometerState();
                newState.X = e.Value.Value.X;
                newState.Y = e.Value.Value.Y;
                newState.Z = e.Value.Value.Z;

                this.ReadingChanged(newState);
            }
        }
#endif

        Point _ptStart, _ptEnd;
        bool _dragging = false;
        PlaneProjection _projection = new PlaneProjection();

        private Canvas _gameCanvas;

        public Canvas GameCanvas
        {
            get
            {
                return _gameCanvas;
            }
            set
            {
                _gameCanvas = value;
                _gameCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(_gameCanvas_MouseLeftButtonDown);
                _gameCanvas.MouseLeftButtonUp += new MouseButtonEventHandler(_gameCanvas_MouseLeftButtonUp);
                _gameCanvas.MouseMove += new MouseEventHandler(_gameCanvas_MouseMove);
                _gameCanvas.Projection = _projection;
            }
        }

        void _gameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                double percentBuffer = 0.001;
                double projectionFactor = 100;
                double governorOnRotation = .3;
                Point ptCurrent = e.GetPosition(_gameCanvas);
                double changeX = (_ptStart.X - ptCurrent.X) * percentBuffer;
                double changeY = -(_ptStart.Y - ptCurrent.Y) * percentBuffer;

                // *ACK* THIS IS A GOVERNOR ON THE MAX TRANSFORM!
                if (changeX > governorOnRotation) changeX = governorOnRotation;
                if (changeX < -governorOnRotation) changeX = -governorOnRotation;
                if (changeY > governorOnRotation) changeY = governorOnRotation;
                if (changeY < -governorOnRotation) changeY = -governorOnRotation;

                _projection.RotationY = changeX * projectionFactor;
                _projection.RotationX = changeY * projectionFactor;

                AccelerometerState newState = new AccelerometerState();
                newState.Y = changeY;
                newState.X = changeX;

                ReadingChanged(newState);



            }
        }

        void _gameCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragging = false;
            _projection.RotationX = 0;
            _projection.RotationY = 0;

            AccelerometerState newState = new AccelerometerState();
            newState.X = 0;
            newState.Y = 0;
            newState.Z = 0;
            ReadingChanged(newState);
        }

        void _gameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // TODO: Add event handler implementation here.
            _ptStart = e.GetPosition(_gameCanvas);
            _dragging = true;

        }
    }

    public class AccelerometerState
    {
        public double X;
        public double Y;
        public double Z;
    }




}
