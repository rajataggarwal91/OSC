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
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;

namespace Spritehand.FarseerHelper
{
    public partial class PhysicsController : UserControl
    {
        private static PhysicsControllerMain _physicsControllerMain = new PhysicsControllerMain();

        // Mouse Events for geometries
        public delegate void MouseLeftButtonDownHandler(string sprite);
        public new event MouseLeftButtonDownHandler MouseLeftButtonDown;
        public delegate void MouseLeftButtonUpHandler(string sprite);
        public new event MouseLeftButtonUpHandler MouseLeftButtonUp;
        public delegate void MouseMoveHandler(string sprite);
        public new event MouseMoveHandler MouseMove;

        public PhysicsController()
        {
            InitializeComponent();
            _physicsControllerMain.MouseLeftButtonDown += new PhysicsControllerMain.MouseLeftButtonDownHandler(_physicsControllerMain_MouseLeftButtonDown);
            _physicsControllerMain.MouseLeftButtonUp += new PhysicsControllerMain.MouseLeftButtonUpHandler(_physicsControllerMain_MouseLeftButtonUp);
            _physicsControllerMain.MouseMove += new PhysicsControllerMain.MouseMoveHandler(_physicsControllerMain_MouseMove);
        }

        void _physicsControllerMain_MouseMove(string sprite)
        {
            if (MouseMove != null)
                MouseMove(sprite);
        }

        void _physicsControllerMain_MouseLeftButtonUp(string sprite)
        {
            if (MouseLeftButtonUp != null)
                MouseLeftButtonUp(sprite);
        }

        void _physicsControllerMain_MouseLeftButtonDown(string sprite)
        {
            if (MouseLeftButtonDown != null)
                MouseLeftButtonDown(sprite);
        }

        public PhysicsControllerMain PhysicsMain
        {
            get
            {
                return _physicsControllerMain;
            }
            set
            {
                _physicsControllerMain = value;
            }
        }

        // event for Collision between geometries
        public delegate void CollisionHandler(PhysicsController source, string sprite1, string sprite2);
        public event CollisionHandler Collision;

        // event for a Timer Tick
        public delegate void TimerLoopHandler(object source);
        public event TimerLoopHandler TimerLoop;

        // event for Physics initialized
        public delegate void InitializedHandler(object source);
#if SILVERLIGHT
        public event InitializedHandler Initialized;
#else
        public new event InitializedHandler Initialized;
#endif
        public PhysicsSimulator Simulator
        {
            get
            {
                return _physicsControllerMain.Simulator;
            }
            set
            {
                _physicsControllerMain.Simulator = value;
            }
        }

        // track the frames per second
        public int LastFPS
        {
            get
            {
                return _physicsControllerMain.LastFPS;
            }
            set
            {
                _physicsControllerMain.LastFPS = value;
            }
        }


        public Dictionary<string, PhysicsSprite> PhysicsObjects
        {
            get
            {
                return _physicsControllerMain.PhysicsObjects;
            }
            set
            {
                _physicsControllerMain.PhysicsObjects = value;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            bool isDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);

            if (isDesignMode)
                return;


            Canvas parentCanvas = this.Parent as Canvas;
            _physicsControllerMain.Collision += new PhysicsControllerMain.CollisionHandler(_physicsControllerMain_Collision);
            _physicsControllerMain.Initialized += new PhysicsControllerMain.InitializedHandler(_physicsControllerMain_Initialized);
            _physicsControllerMain.TimerLoop += new PhysicsControllerMain.TimerLoopHandler(_physicsControllerMain_TimerLoop);
            PhysicsControllerMain.ParentCanvas = parentCanvas;
            _physicsControllerMain.Initialize();
            this.Visibility = Visibility.Collapsed;
        }

        void _physicsControllerMain_TimerLoop(object source)
        {
            if (TimerLoop != null)
                TimerLoop(this);
        }

        void _physicsControllerMain_Initialized(object source)
        {
            if (Initialized != null)
                Initialized(this);
        }

        void _physicsControllerMain_Collision(string sprite1, string sprite2)
        {
            if (Collision != null)
                Collision(this, sprite1, sprite2);
        }

        public static readonly DependencyProperty AutoAddCanvasObjectsProperty =
          DependencyProperty.Register(
          "AutoAddCanvasObjects", typeof(bool),
          typeof(PhysicsController), new PropertyMetadata(true, new PropertyChangedCallback(AutoAddCanvasObjectsChanged))
          );

        private static void AutoAddCanvasObjectsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.AutoAddCanvasObjects = Convert.ToBoolean(args.NewValue);
        }

        [Category("Physics")]
		[Description("If true, then all named canvas objects are translated into Physics objects.")]
        public bool AutoAddCanvasObjects
        {
            get { return (bool)GetValue(AutoAddCanvasObjectsProperty); }
            set
            {
                SetValue(AutoAddCanvasObjectsProperty, value);
            }
        }

        public static readonly DependencyProperty TimeStepProperty =
          DependencyProperty.Register(
          "TimeStep", typeof(double),
          typeof(PhysicsController), new PropertyMetadata(0.02D, new PropertyChangedCallback(TimeStepChanged))
          );

        private static void TimeStepChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.TimeStep = Convert.ToDouble(args.NewValue);
        }


        [Category("Physics")]
		[Description("The number of times Farseer internal physics calculates are made. Increase for better accuracy at the cost of performance.")]
        public double TimeStep
        {
            get { return (double)GetValue(TimeStepProperty); }
            set
            {
                SetValue(TimeStepProperty, value);
            }
        }


        public static readonly DependencyProperty MousePickEnabledProperty =
          DependencyProperty.Register(
          "MousePickEnabled", typeof(bool),
          typeof(PhysicsController), new PropertyMetadata(false, new PropertyChangedCallback(MousePickEnabledChanged))
          );

        private static void MousePickEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.MousePickEnabled = Convert.ToBoolean(args.NewValue);
        }


        [Category("Physics")]
		[Description("If true, then objects can be manipulated with the mouse.")]
        public bool MousePickEnabled
        {
            get { return (bool)GetValue(MousePickEnabledProperty); }
            set
            {
                SetValue(MousePickEnabledProperty, value);
            }
        }

        public static readonly DependencyProperty GravityHorizontalProperty =
          DependencyProperty.Register(
          "GravityHorizontal", typeof(int),
          typeof(PhysicsController), new PropertyMetadata(0, new PropertyChangedCallback(GravityHorizontalChanged))
          );

        private static void GravityHorizontalChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.GravityHorizontal = Convert.ToInt32(args.NewValue);
        }

        [Category("Physics")]
		[Description("Gravity setting in Horizontal Direction.")]
        public int GravityHorizontal
        {
            get { return (int)GetValue(GravityHorizontalProperty); }
            set
            {
                SetValue(GravityHorizontalProperty, value);
            }
        }

        public static readonly DependencyProperty GravityVerticalProperty =
          DependencyProperty.Register(
          "GravityVertical", typeof(int),
          typeof(PhysicsController), new PropertyMetadata(0, new PropertyChangedCallback(GravityVerticalChanged))
          );

        private static void GravityVerticalChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.GravityVertical = Convert.ToInt32(args.NewValue);
        }

        [Category("Physics")]
		[Description("Gravity setting in Vertical Direction.")]
        public int GravityVertical
        {
            get { return (int)GetValue(GravityVerticalProperty); }
            set
            {
                SetValue(GravityVerticalProperty, value);
            }
        }

        public static readonly DependencyProperty IterationsProperty =
          DependencyProperty.Register(
          "Iterations", typeof(int),
          typeof(PhysicsController), new PropertyMetadata(10, new PropertyChangedCallback(IterationsChanged))
          );

        private static void IterationsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.Iterations = Convert.ToInt32(args.NewValue);
        }

        [Category("Physics")]
		[Description("The number of times Farseer internal physics calculates are made. Increase for better accuracy at the cost of performance.")]
        public int Iterations
        {
            get { return (int)GetValue(IterationsProperty); }
            set
            {
                SetValue(IterationsProperty, value);
            }
        }

        public static readonly DependencyProperty BiasFactorProperty =
          DependencyProperty.Register(
          "BiasFactor", typeof(double),
          typeof(PhysicsController), new PropertyMetadata(0.1D, new PropertyChangedCallback(BiasFactorChanged))
          );

        private static void BiasFactorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.BiasFactor = Convert.ToDouble(args.NewValue);
        }
        
        [Category("Physics")]
		[Description("If you need to decrease jitter, try a lower value.")]
        public double BiasFactor
        {
            get { return (double)GetValue(BiasFactorProperty); }
            set
            {
                SetValue(BiasFactorProperty, value);
            }
        }

        public static readonly DependencyProperty FrictionDefaultProperty =
          DependencyProperty.Register(
          "FrictionDefault", typeof(double),
          typeof(PhysicsController), new PropertyMetadata(5.0D, new PropertyChangedCallback(FrictionDefaultChanged))
          );

        private static void FrictionDefaultChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.FrictionDefault = Convert.ToDouble(args.NewValue);
        }

        [Category("Physics")]
		[Description("Use this to adjust the initial friction value of all geometries. Note that you can still modify individual geometries.")]
        public double FrictionDefault
        {
            get { return (double)GetValue(FrictionDefaultProperty); }
            set
            {
                SetValue(FrictionDefaultProperty, value);
            }
        }




        public static readonly DependencyProperty PointListCollectionProperty =
          DependencyProperty.Register(
          "PointListCollection", typeof(List<PointObject>),
          typeof(PhysicsController), new PropertyMetadata(null, new PropertyChangedCallback(PointListCollectionChanged))
          );

        private static void PointListCollectionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.PointListCollection = args.NewValue as List<PointObject>;
        }

        [Category("Physics")]
		[Description("Use this property to define the outline points of an element to speed application startup.")]
        public List<PointObject> PointListCollection
        {
            get { return (List<PointObject>)GetValue(PointListCollectionProperty); }
            set
            {
                SetValue(PointListCollectionProperty, value);
            }
        }



        public static readonly DependencyProperty DebugModeProperty =
          DependencyProperty.Register(
          "DebugMode", typeof(bool),
          typeof(PhysicsController), new PropertyMetadata(false, new PropertyChangedCallback(DebugModeChanged))
          );

        private static void DebugModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _physicsControllerMain.DebugMode = Convert.ToBoolean(args.NewValue);
        }


        [Category("Physics")]
		[Description("If true, then collision and body boundaries are drawn.")]
        public bool DebugMode
        {
            get { return (bool)GetValue(DebugModeProperty); }
            set
            {
                SetValue(DebugModeProperty, value);
            }
        }


        /// <summary>
        /// Update the Simulation for a "frame"
        /// </summary>
        public void Update()
        {
            _physicsControllerMain.Update();
        }

        /// <summary>
        /// Adds ALL named XAML elements into the Physics simulation.
        /// </summary>
        /// <param name="cnvContainer">The container to add items from.</param>
        public void AddPhysicsBodyForCanvas(Canvas cnvContainer)
        {
            _physicsControllerMain.AddPhysicsBodyForCanvas(cnvContainer);
        }

        /// <summary>
        /// Adds a single XAML element into the Physics simulation.
        /// </summary>
        /// <param name="cnvContainer">The container to add items from.</param>
        public PhysicsSprite AddPhysicsBody(UIElement element)
        {
            return _physicsControllerMain.AddPhysicsBody(element);
        }

        /// <summary>
        /// Adds ALL Joint objects from a Canvas into the simulation.
        /// </summary>
        /// <param name="cnvContainer"></param>
        public void AddJointsForCanvas(Canvas cnvContainer)
        {
            _physicsControllerMain.AddJointsForCanvas(cnvContainer);
        }

        /// <summary>
        /// Adds a single joint objectt from a Canvas into the simulation.
        /// </summary>
        /// <param name="cnvContainer">The Canvas to add joints from</param>
        public void AddJoint(PhysicsJoint joint)
        {
            _physicsControllerMain.AddJoint(joint);
        }

        /// <summary>
        /// Adds ALL Static holder objects from a canvas into the simulation.
        /// </summary>
        /// <param name="cnvContainer">The Canvas to add static holders from</param>
        public void AddStaticHoldersForCanvas(Canvas cnvContainer)
        {
            _physicsControllerMain.AddStaticHoldersForCanvas(cnvContainer);
        }

        /// <summary>
        /// Adds a single Static holder object from a canvas into the simulation.
        /// </summary>
        /// <param name="cnvContainer">The static holder to add</param>
        public void AddStaticHolder(PhysicsStaticHolder staticHolder)
        {
            _physicsControllerMain.AddStaticHolder(staticHolder);
        }
        
        /// <summary>
        /// Given a list of points, get the overall width and height to cover them all, as well as the mininum X and Y values occurring.
        /// </summary>
        /// <param name="points">The list of points</param>
        public static void GetPointDimensions(List<Point> points, out double width, out double height, out double minX, out double minY)
        {
            PhysicsControllerMain.GetPointDimensions(points, out width, out height, out minX, out minY);
        }

        /// <summary>
        /// Create a path object based on a list of vertices - used when creating geometry
        /// </summary>
        public static System.Windows.Shapes.Path CreatePathFromVertices(Vertices vertices, double width, double height)
        {
            return PhysicsControllerMain.CreatePathFromVertices(vertices, width, height);
        }

        /// <summary>
        /// Get the distance, in pixels between two points.
        /// </summary>
        public static double DistanceBetweenTwoPoints(Point pt1, Point pt2)
        {
            return PhysicsControllerMain.DistanceBetweenTwoPoints(pt1, pt2);
        }

        /// <summary>
        /// A utility function to get a point that is a given distance and angle from another point.
        /// </summary>
        /// <param name="angle">The angle to travel</param>
        /// <param name="distance">The distance (in pixels) to go</param>
        /// <param name="ptStart">The start point</param>
        /// <returns></returns>
        public static Point GetPointFromDistanceAngle(double angle, double distance, Point ptStart)
        {
            return PhysicsControllerMain.GetPointFromDistanceAngle(angle, distance, ptStart);
        }





    }



}
