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
    public partial class CameraControllerMain
    {
        Canvas _parentCanvas;
        PhysicsSprite _targetElement;
        TranslateTransform _translateTransform;
        double _relativeTranslateX;
        double _relativeTranslateY;
        ScaleTransform _scaleTransform;

        public static readonly DependencyProperty CameraControllerProperty = DependencyProperty.RegisterAttached("CameraController", typeof(CameraControllerMain), typeof(Canvas), new PropertyMetadata(null));

        public static CameraControllerMain GetCameraController(DependencyObject target)
        {
            return (CameraControllerMain)target.GetValue(CameraControllerMain.CameraControllerProperty);
        }

        public static void SetCameraController(DependencyObject target, CameraControllerMain value)
        {
            target.SetValue(CameraControllerMain.CameraControllerProperty, value);
        }

        private PhysicsControllerMain Controller { get; set; }


        public CameraControllerMain()
        {

        }

        
        /// <summary>
        /// The list of layers to render in simulated 3D space.
        /// </summary>
        public List<ParallaxLayer> ParallaxLayers { get; set; }

        private string _target;
        public string Target
        {
            get { return _target; }
            set
            {
                _target = value;
                SetupTarget();
            }
        }

        private double _zoomSpeed = 0.05D;
        public double ZoomSpeed
        {
            get { return _zoomSpeed; }
            set { _zoomSpeed = value; }
        }

        private double _scrollSpeed;
        public double ScrollSpeed
        {
            get { return _scrollSpeed; }
            set { _scrollSpeed = value; }
        }

        private double _targetOverrideX;
        public double TargetOverrideX
        {
            get { return _targetOverrideX; }
            set { _targetOverrideX = value; }
        }

        private double _targetOverrideY;
        public double TargetOverrideY
        {
            get { return _targetOverrideY; }
            set { _targetOverrideY = value; }
        }

        private double _zoomPercentage = 100D;
        public double ZoomPercentage
        {
            get { return _zoomPercentage; }
            set { _zoomPercentage = value; }
        }

        private int _centeringThreshold = 5;
        public int CenteringThreshold
        {
            get { return _centeringThreshold; }
            set { _centeringThreshold = value; }
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        private bool _zoomEnabled = true;
        public bool ZoomEnabled
        {
            get { return _zoomEnabled; }
            set { _zoomEnabled = value; }
        }

        private bool _scrollEnabled = true;
        public bool ScrollEnabled
        {
            get { return _scrollEnabled; }
            set { _scrollEnabled = value; }
        }

        private bool _scrollX = true;
        public bool ScrollX
        {
            get { return _scrollX; }
            set { _scrollX = value; }
        }

        private bool _scrollY = true;
        public bool ScrollY
        {
            get { return _scrollY; }
            set { _scrollY = value; }
        }

        public void Initialize(Canvas parentCanvas)
        {
            Initialize(parentCanvas, null);
        }

        public void Initialize(Canvas parentCanvas, PhysicsControllerMain controller)
        {
            Controller = controller;

            _parentCanvas = parentCanvas;
            if (_parentCanvas == null)
                throw new Exception(String.Format("The Camera Controller must exist in a Canvas with a PhysicsController."));


            // if there is a PhysicsController in the parent canvas, then we will wait
            // for it's Initialize event.
            foreach (UIElement element in _parentCanvas.Children)
            {
                if (element is PhysicsController)
                {
                    Enabled = false;
                    Controller = (element as PhysicsController).PhysicsMain;
                    Controller.Initialized += new PhysicsControllerMain.InitializedHandler(CameraController_Initialized);
                    break;
                }
            }


            ParallaxLayers = new List<ParallaxLayer>();

            // _userControlLoaded = true;

            // create our "game loop" timer
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);


        }

        private void SetupTarget()
        {
            if (Controller == null || !Controller.PhysicsObjects.Keys.Contains(Target))
                return;

            _targetElement = Controller.PhysicsObjects[Target];

            // try to find a scale transform
            if (_parentCanvas.RenderTransform != null && _parentCanvas.RenderTransform is TransformGroup)
            {
                foreach (Transform trn in (_parentCanvas.RenderTransform as TransformGroup).Children)
                {
                    if (trn is ScaleTransform)
                    {
                        _scaleTransform = trn as ScaleTransform;
                    }
                    if (trn is TranslateTransform)
                    {
                        _translateTransform = trn as TranslateTransform;
                    }
                }
            }
            if (_parentCanvas.RenderTransform is ScaleTransform)
            {
                _scaleTransform = _parentCanvas.RenderTransform as ScaleTransform;
            }
            if (_parentCanvas.RenderTransform is TranslateTransform)
            {
                _translateTransform = _parentCanvas.RenderTransform as TranslateTransform;
            }

            // last ditch effort: if we did not find the transforms we need, then add them in.
            if (_translateTransform == null)
            {
                _translateTransform = new TranslateTransform();
                if (_parentCanvas.RenderTransform == null || !(_parentCanvas.RenderTransform is TransformGroup))
                {
                    _parentCanvas.RenderTransform = new TransformGroup();
                }
                (_parentCanvas.RenderTransform as TransformGroup).Children.Add(_translateTransform);
            }
            if (_scaleTransform == null)
            {
                _scaleTransform = new ScaleTransform();
                (_parentCanvas.RenderTransform as TransformGroup).Children.Add(_scaleTransform);
            }


        }

        void CameraController_Initialized(object source)
        {
            // if we are here, then we waited for the physics controller to initialize.
            SetupTarget();
            Enabled = true;
        }

        /// <summary>
        /// Scroll directly to center on a location
        /// </summary>
        public void ScrollToXY(double x, double y)
        {
            // just scroll directly to target
            double currentScreenCenterX, currentScreenCenterY;

            currentScreenCenterX = (_parentCanvas.ActualWidth / 2) / _scaleTransform.ScaleX;
            currentScreenCenterY = (_parentCanvas.ActualHeight / 2) / _scaleTransform.ScaleY;

            _translateTransform.X = currentScreenCenterX - (x);
            _translateTransform.Y = currentScreenCenterY - (y);

        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!Enabled)
                return;

            double currentTargetXOffset = 0, currentTargetYOffset = 0;
            double currentScreenCenterX, currentScreenCenterY;

            currentScreenCenterX = (_parentCanvas.ActualWidth / 2) / _scaleTransform.ScaleX;
            currentScreenCenterY = (_parentCanvas.ActualHeight / 2) / _scaleTransform.ScaleY;

            if (Target != string.Empty && ScrollEnabled)
            {
                if (!Controller.PhysicsObjects.Keys.Contains(Target))
                    return;

                _targetElement = Controller.PhysicsObjects[Target];

                currentTargetXOffset = _targetElement.BodyObject.Position.X;
                currentTargetYOffset = _targetElement.BodyObject.Position.Y;

                if (TargetOverrideX != 0)
                    currentTargetXOffset = TargetOverrideX;     // use the override X instead of the Target X
                else
                    currentTargetXOffset += _targetElement.Width / 2;  // use the center position of the Target 

                if (TargetOverrideY != 0)
                    currentTargetYOffset = TargetOverrideY;     // use the override Y instead of the Target Y
                else
                    currentTargetYOffset += _targetElement.Height / 2; // use the center position of the Target

                if (ScrollSpeed == 0)
                {
                    // just scroll directly to target
                    _translateTransform.X = currentScreenCenterX - (_relativeTranslateX + currentTargetXOffset);
                    _translateTransform.Y = currentScreenCenterY - (_relativeTranslateY + currentTargetYOffset);
                }
                else
                {
                    // do a gradual scroll
                    double diffX = (_relativeTranslateX + currentTargetXOffset) - currentScreenCenterX;
                    double diffY = (_relativeTranslateY + currentTargetYOffset) - currentScreenCenterY;
                    if (Math.Abs(diffX) > CenteringThreshold)
                    {
                        if (diffX > 0)
                            _relativeTranslateX -= ScrollSpeed;
                        else
                            _relativeTranslateX += ScrollSpeed;
                    }
                    if (Math.Abs(diffY) > CenteringThreshold)
                    {
                        if (diffY > 0)
                            _relativeTranslateY -= ScrollSpeed;
                        else
                            _relativeTranslateY += ScrollSpeed;
                    }

                    if (ScrollX)
                        _translateTransform.X = (_relativeTranslateX);
                    if (ScrollY)
                        _translateTransform.Y = (_relativeTranslateY);
                }

                // Now do the Zoom
                if (ZoomEnabled)
                {
                    double targetZoom = ZoomPercentage / 100;
                    double currentZoom = _scaleTransform.ScaleX;

                    // allow a threshold of zoomspeed
                    if (ZoomSpeed == 0)
                    {
                        // just zoom directly to target
                        _scaleTransform.ScaleX = targetZoom;
                        _scaleTransform.ScaleY = targetZoom;
                    }
                    else
                        if (Math.Abs(currentZoom - targetZoom) > ZoomSpeed)
                        {
                            // zooming out
                            if (currentZoom > targetZoom)
                            {
                                _scaleTransform.ScaleX -= ZoomSpeed;
                                _scaleTransform.ScaleY -= ZoomSpeed;

                                if (_scaleTransform.ScaleX < -1)
                                {
                                    _scaleTransform.ScaleX = -1;
                                    _scaleTransform.ScaleY = -1;
                                }
                            }
                            else
                            {
                                _scaleTransform.ScaleX += ZoomSpeed;
                                _scaleTransform.ScaleY += ZoomSpeed;
                            }
                        }
                }
            }


            // update any parallax layers
            foreach (ParallaxLayer layer in ParallaxLayers)
            {

                layer.Z = layer.Z;

                double x = Convert.ToDouble(layer.CanvasContainer.GetValue(Canvas.LeftProperty));
                double y = Convert.ToDouble(layer.CanvasContainer.GetValue(Canvas.TopProperty));
                double zChange = 1 / Math.Abs(layer.Z);

                double newX, newY;

                newX = -_translateTransform.X;
                newY = -_translateTransform.Y;

                // compensate for camera
                newX = newX + (zChange * _translateTransform.X);
                newY = newY + (zChange * _translateTransform.Y);

                layer.TransformPosition.X = newX;
                layer.TransformPosition.Y = newY;
            }


        }

    }


    public class ParallaxLayer
    {
        public Canvas CanvasContainer { get; set; }
        public double Z { get; set; }
        public ScaleTransform TransformScale { get; set; }
        public TranslateTransform TransformPosition { get; set; }

        public ParallaxLayer(Canvas cnv)
        {
            CanvasContainer = cnv;

            // try to find a scale transform
            if (CanvasContainer.RenderTransform != null && CanvasContainer.RenderTransform is TransformGroup)
            {
                foreach (Transform trn in (CanvasContainer.RenderTransform as TransformGroup).Children)
                {
                    if (trn is ScaleTransform)
                    {
                        TransformScale = trn as ScaleTransform;
                    }
                    if (trn is TranslateTransform)
                    {
                        TransformPosition = trn as TranslateTransform;
                    }
                }
            }
            if (CanvasContainer.RenderTransform is ScaleTransform)
            {
                TransformScale = CanvasContainer.RenderTransform as ScaleTransform;
            }
            if (CanvasContainer.RenderTransform is TranslateTransform)
            {
                TransformPosition = CanvasContainer.RenderTransform as TranslateTransform;
            }

            // last ditch effort: if we did not find the transforms we need, then add them in.
            if (TransformPosition == null)
            {
                TransformPosition = new TranslateTransform();
                if (CanvasContainer.RenderTransform == null || !(CanvasContainer.RenderTransform is TransformGroup))
                {
                    CanvasContainer.RenderTransform = new TransformGroup();
                }
                (CanvasContainer.RenderTransform as TransformGroup).Children.Add(TransformPosition);
            }
            if (TransformScale == null)
            {
                TransformScale = new ScaleTransform();
                (CanvasContainer.RenderTransform as TransformGroup).Children.Add(TransformScale);
            }
        }
    }
}

