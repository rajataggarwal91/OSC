/////////////////////////////////////////////////////////////////
//
// Silverlight + FarSeer Physics Helper
//
// by Andy Beaulieu - http://www.andybeaulieu.com
//
// LICENSE: This code is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. ANY 
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS
// OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
/////////////////////////////////////////////////////////////////
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
using System.ComponentModel;

namespace Spritehand.FarseerHelper
{
    public partial class CameraController : UserControl
    {
        bool _userControlLoaded;
        static CameraControllerMain _cameraControllerMain = new CameraControllerMain();

        public CameraController()
        {
            InitializeComponent();
        }

        public CameraControllerMain CameraMain
        {
            get
            {
                return _cameraControllerMain;
            }
            set
            {
                _cameraControllerMain = value;
            }
        }

        /// <summary>
        /// The list of layers to render in simulated 3D space.
        /// </summary>
        public List<ParallaxLayer> ParallaxLayers
        {
            get
            {
                return _cameraControllerMain.ParallaxLayers;
            }
            set
            {
                _cameraControllerMain.ParallaxLayers = value;
            }
        }



       public static readonly DependencyProperty TargetProperty =
          DependencyProperty.Register(
          "Target", typeof(string),
          typeof(CameraController), new PropertyMetadata("", new PropertyChangedCallback(TargetChanged))
          );

        private static void TargetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.Target = Convert.ToString(args.NewValue);
        }

        [Category("Physics")]
		[Description("The name of the FrameworkElement that the camera should follow.")]
        public string Target
        {
            get { return (string)GetValue(TargetProperty); }
            set
            {
                SetValue(TargetProperty, value);
            }
        }

        public static readonly DependencyProperty ZoomSpeedProperty =
           DependencyProperty.Register(
           "ZoomSpeed", typeof(double),
           typeof(CameraController), new PropertyMetadata(0.05D, new PropertyChangedCallback(ZoomSpeedChanged))
           );

        private static void ZoomSpeedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.ZoomSpeed = Convert.ToDouble(args.NewValue);
        }

        [Category("Physics")]
		[Description("Set this property to a value greater than Zero, and the camera will do a gradual Zoom at the given rate.")]
        public double ZoomSpeed
        {
            get { return (double)GetValue(ZoomSpeedProperty); }
            set
            {
                SetValue(ZoomSpeedProperty, value);
            }
        }

        public static readonly DependencyProperty ScrollSpeedProperty =
           DependencyProperty.Register(
           "ScrollSpeed", typeof(double),
           typeof(CameraController), new PropertyMetadata(0D, new PropertyChangedCallback(ScrollSpeedChanged))
           );

        private static void ScrollSpeedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.ScrollSpeed = Convert.ToDouble(args.NewValue);
        }
        
        [Category("Physics")]
		[Description("Set this property to a value greater than Zero, and the camera will do a gradual Zoom at the given rate.")]
        public double ScrollSpeed
        {
            get { return (double)GetValue(ScrollSpeedProperty); }
            set
            {
                SetValue(ScrollSpeedProperty, value);
            }
        }

        public static readonly DependencyProperty TargetOverrideXProperty =
           DependencyProperty.Register(
           "TargetOverrideX", typeof(double),
           typeof(CameraController), new PropertyMetadata(0D, new PropertyChangedCallback(TargetOverrideXChanged))
           );

        private static void TargetOverrideXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.TargetOverrideX = Convert.ToDouble(args.NewValue);
        }

        [Category("Physics")]
		[Description("Setting this property will give the camera a static X location target instead of the Target's X location.")]
        public double TargetOverrideX
        {
            get { return (double)GetValue(TargetOverrideXProperty); }
            set
            {
                SetValue(TargetOverrideXProperty, value);
            }
        }

        public static readonly DependencyProperty TargetOverrideYProperty =
           DependencyProperty.Register(
           "TargetOverrideY", typeof(double),
           typeof(CameraController), new PropertyMetadata(0D, new PropertyChangedCallback(TargetOverrideYChanged))
           );

        private static void TargetOverrideYChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.TargetOverrideY = Convert.ToDouble(args.NewValue);
        }

        [Category("Physics")]
		[Description("Setting this property will give the camera a static Y location target instead of the Target's Y location.")]
        public double TargetOverrideY
        {
            get { return (double)GetValue(TargetOverrideYProperty); }
            set
            {
                SetValue(TargetOverrideYProperty, value);
                _cameraControllerMain.TargetOverrideY = value;

            }
        }


        public static readonly DependencyProperty ZoomPercentageProperty =
           DependencyProperty.Register(
           "ZoomPercentage", typeof(double),
           typeof(CameraController), new PropertyMetadata(100D, new PropertyChangedCallback(ZoomPercentageChanged))
           );

        private static void ZoomPercentageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.ZoomPercentage = Convert.ToDouble(args.NewValue);
        }

        [Category("Physics")]
		[Description("The target zoom percentage.")]
        public double ZoomPercentage
        {
            get { return (double)GetValue(ZoomPercentageProperty); }
            set
            {
                SetValue(ZoomPercentageProperty, value);
            }
        }

        public static readonly DependencyProperty CenteringThresholdProperty =
          DependencyProperty.Register(
          "CenteringThreshold", typeof(int),
          typeof(CameraController), new PropertyMetadata(5, new PropertyChangedCallback(CenteringThresholdChanged))
          );

        private static void CenteringThresholdChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.CenteringThreshold = Convert.ToInt32(args.NewValue);
        }
        
        [Category("Physics")]
		[Description("The threshold, in pixels, to allow off center.")]
        public int CenteringThreshold
        {
            get { return (int)GetValue(CenteringThresholdProperty); }
            set
            {
                SetValue(CenteringThresholdProperty, value);
            }
        }

        public static readonly DependencyProperty EnabledProperty =
        DependencyProperty.Register(
        "Enabled", typeof(bool),
        typeof(CameraController), new PropertyMetadata(true, new PropertyChangedCallback(EnabledChanged))
        );

        private static void EnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.Enabled = Convert.ToBoolean(args.NewValue);
        }
                
        [Category("Physics")]
		[Description("If true, then the camera is enabled.")]
        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set
            {
                SetValue(EnabledProperty, value);
            }
        }

        public static readonly DependencyProperty ZoomEnabledProperty =
       DependencyProperty.Register(
       "ZoomEnabled", typeof(bool),
       typeof(CameraController), new PropertyMetadata(true, new PropertyChangedCallback(ZoomEnabledChanged))
       );

        private static void ZoomEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.ZoomEnabled = Convert.ToBoolean(args.NewValue);
        }
        
        [Category("Physics")]
		[Description("If true, then the camera Zooming is Enabled.")]
        public bool ZoomEnabled
        {
            get { return (bool)GetValue(ZoomEnabledProperty); }
            set
            {
                SetValue(ZoomEnabledProperty, value);
                _cameraControllerMain.ZoomEnabled = value;
            }
        }

        public static readonly DependencyProperty ScrollEnabledProperty =
      DependencyProperty.Register(
      "ScrollEnabled", typeof(bool),
      typeof(CameraController), new PropertyMetadata(true, new PropertyChangedCallback(ScrollEnabledChanged))
      );

        private static void ScrollEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.ScrollEnabled = Convert.ToBoolean(args.NewValue);
        }

        [Category("Physics")]
		[Description("If true, then the camera Scrolling is Enabled.")]
        public bool ScrollEnabled
        {
            get { return (bool)GetValue(ScrollEnabledProperty); }
            set
            {
                SetValue(ScrollEnabledProperty, value);
                
            }
        }

        public static readonly DependencyProperty ScrollXProperty =
     DependencyProperty.Register(
     "ScrollX", typeof(bool),
     typeof(CameraController), new PropertyMetadata(true, new PropertyChangedCallback(ScrollXChanged))
     );

        private static void ScrollXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.ScrollX = Convert.ToBoolean(args.NewValue);
        }
        
        [Category("Physics")]
		[Description("If true, then the camera will scroll horizontally as needed to keep Target in focus.")]
        public bool ScrollX
        {
            get { return (bool)GetValue(ScrollXProperty); }
            set
            {
                SetValue(ScrollXProperty, value);
            }
        }


        public static readonly DependencyProperty ScrollYProperty =
         DependencyProperty.Register(
         "ScrollY", typeof(bool),
         typeof(CameraController), new PropertyMetadata(true, new PropertyChangedCallback(ScrollYChanged))
         );

        private static void ScrollYChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _cameraControllerMain.ScrollY = Convert.ToBoolean(args.NewValue);
        }

        [Category("Physics")]
		[Description("If true, then the camera will scroll vertically as needed to keep Target in focus.")]
        public bool ScrollY
        {
            get { return (bool)GetValue(ScrollYProperty); }
            set { SetValue(ScrollYProperty, value); }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            bool isDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);

            if (_userControlLoaded || isDesignMode)
                return;

            this.Visibility = Visibility.Collapsed;

            Canvas parentCanvas = this.Parent as Canvas;
            if (parentCanvas == null)
                throw new Exception("The Camera Controller must be the child of a Canvas element.");

            _userControlLoaded = true;

            _cameraControllerMain.Initialize(this.Parent as Canvas);

        }


        public void ScrollToXY(double x, double y)
        {
            _cameraControllerMain.ScrollToXY(x, y);
        }
    }


}

