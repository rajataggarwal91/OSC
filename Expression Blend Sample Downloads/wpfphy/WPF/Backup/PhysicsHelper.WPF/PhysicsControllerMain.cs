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
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics.Springs;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Dynamics.Joints;
using System.Text;
using FarseerGames.FarseerPhysics.Controllers;

namespace Spritehand.FarseerHelper
{
    public partial class PhysicsControllerMain
    {
        public PhysicsSimulator Simulator { get; set; }
        public Dictionary<string, PhysicsSprite> PhysicsObjects { get; set; }
        Storyboard _timerInitialize;
        int _timerInitializeAttempts = 0;
        static bool _pageLoaded;
        bool _initialized;

        FixedLinearSpring _mousePickSpring;
        Line _lineShowSpring;
        PhysicsSprite _pickedSprite;
        private int _uniqueSpriteIndex;

        private static Canvas _parentCanvas;

        // event for Physics initialized
        public delegate void InitializedHandler(object source);
        public event InitializedHandler Initialized;

        // event for a Timer Tick
        public delegate void TimerLoopHandler(object source);
        public event TimerLoopHandler TimerLoop;

        // event for Collision between geometries
        public delegate void CollisionHandler(string sprite1, string sprite2);
        public event CollisionHandler Collision;

        // Mouse Events for geometries
        public delegate void MouseLeftButtonDownHandler(string sprite);
        public event MouseLeftButtonDownHandler MouseLeftButtonDown;
        public delegate void MouseLeftButtonUpHandler(string sprite);
        public event MouseLeftButtonUpHandler MouseLeftButtonUp;
        public delegate void MouseMoveHandler(string sprite);
        public event MouseMoveHandler MouseMove;

        // track the frames per second
        public int LastFPS { get; set; }
        private int _lastTick;
        private int _frameCounter;

        public bool PauseSimulation { get; set; }

        public static readonly DependencyProperty PhysicsControllerProperty = 
			DependencyProperty.RegisterAttached("PhysicsController", typeof(PhysicsControllerMain), typeof(Canvas), new PropertyMetadata(null));

        public static PhysicsControllerMain GetPhysicsController(DependencyObject target)
        {
            return (PhysicsControllerMain)target.GetValue(PhysicsControllerMain.PhysicsControllerProperty);
        }

        public static void SetPhysicsController(DependencyObject target, PhysicsControllerMain value)
        {
            target.SetValue(PhysicsControllerMain.PhysicsControllerProperty, value);
        }

        public static PhysicsControllerMain FindController(FrameworkElement element)
        {
            while (element != null)
            {
                PhysicsControllerMain context = PhysicsControllerMain.GetPhysicsController(element);
                if (context != null)
                    return context;
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;
            }

            return null;
        }

        public PhysicsControllerMain()
        {
            PauseSimulation = true;

            PointListCollection = new List<PointObject>();

        }

        public bool IsInitialized
        {
            get
            {
                return _initialized;
            }
        }

        public void Initialize()
        {
            Simulator = new PhysicsSimulator(new Vector2(GravityHorizontal, GravityVertical));
            Simulator.Iterations = Iterations;
            Simulator.BiasFactor = (float)BiasFactor;

            if (_parentCanvas != null)
            {
                PhysicsObjects = new Dictionary<string, PhysicsSprite>();
#if !SILVERLIGHT
			_pageLoaded = _parentCanvas.IsLoaded;
#endif
                _timerInitialize = new Storyboard();
                _timerInitialize.Duration = TimeSpan.FromMilliseconds(50);
                _timerInitialize.Completed += new EventHandler(_timerInitialize_Completed);
                _timerInitialize.Begin();
            }
            else
                throw new Exception("The ParentCanvas property was not set on the PhysicsController.");

            // create our "game loop" timer
			CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);

        }

        /// <summary>
        /// The container canvas
        /// </summary>
        public static Canvas ParentCanvas
        {
            get
            {
                return _parentCanvas;
            }
            set
            {
                _parentCanvas = value;
#if SILVERLIGHT
                _parentCanvas.LayoutUpdated += new EventHandler(parentCanvas_LayoutUpdated);
#endif
            }
        }


        private bool _autoAddCanvasObjects = true;
        public bool AutoAddCanvasObjects
        {
            get { return _autoAddCanvasObjects; }
            set { _autoAddCanvasObjects = value; }
        }

        private double _timeStep = 0.02D;
        public double TimeStep
        {
            get { return _timeStep; }
            set { _timeStep = value; }
        }

        private bool _mousePickEnabled = false;
        [Category("Physics")]
		[Description("If true, then objects can be manipulated with the mouse.")]
        public bool MousePickEnabled
        {
            get { return _mousePickEnabled; }
            set { _mousePickEnabled = value; }
        }

        private int _gravityHorizontal = 0;
        public int GravityHorizontal
        {
            get { return _gravityHorizontal; }
            set { _gravityHorizontal = value; }
        }

        private int _gravityVertical = 500;
        public int GravityVertical
        {
            get { return _gravityVertical; }
            set { _gravityVertical = value; }
        }


        private int _iterations = 10;
        public int Iterations
        {
            get { return _iterations; }
            set { _iterations = value; }
        }

        private double _biasFactor = 0.1D;
        public double BiasFactor
        {
            get { return _biasFactor; }
            set { _biasFactor = value; }
        }

        private double _frictionDefault = 5.0D;
        public double FrictionDefault
        {
            get { return _frictionDefault; }
            set { _frictionDefault = value; }
        }

		public double _linearDragCoefficient = 0.001d;
		public double LinearDragCoefficient
		{
			get { return _linearDragCoefficient; }
			set { _linearDragCoefficient = value; }
		}

		public double _rotationalDragCoefficient = 0.001d;
		public double RotationalDragCoefficient
		{
			get { return _rotationalDragCoefficient; }
			set { _rotationalDragCoefficient = value; }
		}
		
		private List<PointObject> _pointListCollection;
        public List<PointObject> PointListCollection
        {
            get { return _pointListCollection; }
            set { _pointListCollection = value; }
        }

        public bool _debugMode = false;
        public bool DebugMode
        {
            get { return _debugMode; }
            set { _debugMode = value; }
        }

        void _timerInitialize_Completed(object sender, EventArgs e)
        {
            // HACK: We cannot do HitTest's until a UIElement is drawn to the screen!
            // So we need to make sure the page is laid out and give it some time to display
            // before we can do HitTests!
            bool restartTimer = true;

            // Windows Phone has issue with Layout.Rendering event being called earlier than SL3.
            // So we will cap the initialize attempts to 5.
            if (_timerInitializeAttempts > 5)
                _pageLoaded = true;

            if (_pageLoaded)
            {
                if (!_initialized)
                {
                    InitializeObjects();
                    restartTimer = false;
                }
               
            }

            if (restartTimer)
            {
                _timerInitialize.Begin();
                _timerInitializeAttempts++;
            }
        }

        public void InitializeObjects()
        {
            _initialized = true;

            if (AutoAddCanvasObjects == true)
            {
                // Add _ALL_ named elements in the canvas as Physics objects.
                AddPhysicsBodyForCanvas(_parentCanvas);
            }

            if (Initialized != null)
            {
                Initialized(this);
            }

            // add a pick spring if selected by user
            _lineShowSpring = new Line();
            _lineShowSpring.Stroke = new SolidColorBrush(Colors.Black);
            _lineShowSpring.Visibility = Visibility.Collapsed;
            _parentCanvas.Children.Add(_lineShowSpring);

#if (DEBUG)
            // echo out the collection of points. This can be useful to set the PointCollection property
            // on the PhysicsController to save startup time.
            StringBuilder sb = new StringBuilder();

            sb.Append("public static void ReadBoundaryCache(PhysicsController physicsController)\n");
            sb.Append("{\n");
            sb.Append("    physicsController.PointListCollection = new System.Collections.Generic.List<PointObject>();\n");


            foreach (KeyValuePair<string, PhysicsSprite> item in PhysicsObjects)
            {
                PhysicsSprite physObject = item.Value;
                sb.Append("    physicsController.PointListCollection.Add(new PointObject(\"");


                sb.Append(physObject.OriginalElementName);
                sb.Append("\", new List<Point> { ");
                foreach (Point pt in physObject.OutlinePoints)
                {

                    sb.Append("new Point(");
                    sb.Append(pt.X);
                    sb.Append(",");
                    sb.Append(pt.Y);
                    sb.Append(")");

                    if (physObject.OutlinePoints.IndexOf(pt) < physObject.OutlinePoints.Count() - 1)
                        sb.Append(", ");
                }
                sb.Append("}));\n");
            }

            sb.Append("}\n");
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            StartGameLoopTimer();
        }

        void StartGameLoopTimer()
        {
            PauseSimulation = false;
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (PauseSimulation)
                return;

            // run the physics sim update
            Update();

            // fire the timer tick event 
            if (TimerLoop != null)
                TimerLoop(this);

            // show the pick spring
            if (_lineShowSpring != null && _lineShowSpring.Visibility != Visibility.Collapsed)
            {
                _lineShowSpring.X1 = _mousePickSpring.Body.Position.X;
                _lineShowSpring.Y1 = _mousePickSpring.Body.Position.Y;
            }

            // track the Frames Per Second
            _frameCounter++;
            if (System.Environment.TickCount - _lastTick > 1000)
            {
                LastFPS = _frameCounter;
                _frameCounter = 0;
                _lastTick = System.Environment.TickCount;
            }
        }

        static void parentCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            //_pageLoaded = true;
        }

        /// <summary>
        /// Update the Simulation for a "frame"
        /// </summary>
        public void Update()
        {
            Simulator.Update((float)TimeStep);

            foreach (KeyValuePair<string, PhysicsSprite> item in PhysicsObjects)
            {
                item.Value.Update();
            }
        }

        /// <summary>
        /// Adds ALL named XAML elements into the Physics simulation.
        /// </summary>
        /// <param name="cnvContainer">The container to add items from.</param>
        public void AddPhysicsBodyForCanvas(Canvas cnvContainer)
        {
            List<string> elements = new List<string>();
            for (int i = 0; i < cnvContainer.Children.Count; i++)
            {
                if (cnvContainer.Children[i].GetValue(Canvas.NameProperty).ToString() != string.Empty
                    && !(IsParallaxLayer(cnvContainer.Children[i]))
                    && !(cnvContainer.Children[i] is PhysicsJoint)
                    && !(cnvContainer.Children[i] is PhysicsStaticHolder)
                    && !(cnvContainer.Children[i] is PhysicsController)
                    && !(cnvContainer.Children[i] is CameraController)
                    && !(cnvContainer.Children[i] is CameraLayer)
                    && !(cnvContainer.Children[i].GetValue(Canvas.NameProperty).ToString().StartsWith("__UniqueId")))
                    elements.Add(cnvContainer.Children[i].GetValue(Canvas.NameProperty).ToString());
            }

            foreach (string element in elements)
                AddPhysicsBody(cnvContainer.FindName(element) as UIElement);

            // process the static bodies
            AddStaticHoldersForCanvas(cnvContainer);

            // process the joints
            AddJointsForCanvas(cnvContainer);


        }

        /// <summary>
        /// Adds ALL named XAML elements into the Physics simulation.
        /// </summary>
        /// <param name="cnvContainer">The container to add items from.</param>
        public void AddPhysicsBodyForCanvasWithBehaviors(Canvas cnvContainer)
        {
#if SILVERLIGHT
            for (int i = 0; i < cnvContainer.Children.Count(); i++)
#else
           for (int i = 0; i < cnvContainer.Children.Count; i++)
#endif
            {
                FrameworkElement element = cnvContainer.Children[i] as FrameworkElement;
                string elemName = element.GetValue(Canvas.NameProperty).ToString();
                element.SetValue(Canvas.TagProperty, elemName);
            }

            PhysicsControllerMain.EnsureUniqueNames(cnvContainer, false);

            List<PhysicsObjectMain> objectsToAdd = new List<PhysicsObjectMain>();
#if SILVERLIGHT
            for (int i = 0; i < cnvContainer.Children.Count() - 1; i++)
#else
            for (int i = 0; i < cnvContainer.Children.Count - 1; i++)
#endif
            {
                FrameworkElement element = cnvContainer.Children[i] as FrameworkElement;

                PhysicsObjectMain physObject = element.GetValue(PhysicsObjectMain.PhysicsObjectProperty) as PhysicsObjectMain;
                if (physObject != null)
                   objectsToAdd.Add(physObject);

            }

            foreach (PhysicsObjectMain physObject in objectsToAdd)
                AddPhysicsBody(physObject);



#if SILVERLIGHT
            for (int i = cnvContainer.Children.Count() - 1; i >= 0; i--)
#else
            for (int i = cnvContainer.Children.Count - 1; i >= 0; i--)
#endif
            {
                FrameworkElement element = cnvContainer.Children[i] as FrameworkElement;
                PhysicsJointMain joint = element.GetValue(PhysicsJointMain.PhysicsJointProperty) as PhysicsJointMain;
                if (joint != null)
                    AddJoint(joint);
            }

        }

        private bool IsParallaxLayer(UIElement ctl)
        {
            bool retval = false;
            if (ctl is Canvas)
            {
                Canvas cnvCtl = (ctl as Canvas);
                Canvas cnvParent = cnvCtl.Parent as Canvas;

                foreach (UIElement ctlCheck in cnvParent.Children)
                {
                    if (ctlCheck is CameraLayer)
                    {
                        CameraLayer ctlLayer = ctlCheck as CameraLayer;
                        if (ctlLayer.CanvasLayer == cnvCtl.Name)
                        {
                            retval = true;
                            break;
                        }
                    }
                }
            }
            return retval;
        }

        public PhysicsSprite AddPhysicsBody(PhysicsObjectMain physObject)
        {
            FrameworkElement element = physObject.VisualElement;

            string thisName = element.GetValue(Canvas.NameProperty).ToString();
            element.SetValue(Canvas.TagProperty, thisName);
            
            // special case: if we are adding a UserControl _AND_ the UserControl contains one or more
            // Physics Controllers, then we handle that UserControl as a "nested" canvas!
            if (BoundaryHelper.IsInsideNestedUsercontrol(element))  //element is UserControl)
            {
                UserControl ucParent = BoundaryHelper.GetParentUserControl(element);
                Canvas cnvNestedPhysics = FindPhysicsCanvas(ucParent);
                if (cnvNestedPhysics != null)
                {
                    EnsureUniqueNames(cnvNestedPhysics);
                    if (element is UserControl)
                    {
                        AddPhysicsBodyForCanvasWithBehaviors(cnvNestedPhysics);
                        return null;
                    }
                }
            }            

            // look to see if we have processed this object before, and if so,
            // use its point collection instead of recalculating
            List<Point> points = FindMatchingUIElement(element);

            string nameKey = (string)element.GetValue(Canvas.NameProperty);
            FrameworkElement boundaryElement = null;
            if (physObject.BoundaryElement != null && physObject.BoundaryElement != string.Empty)
                boundaryElement = element.FindName(physObject.BoundaryElement) as FrameworkElement;

            if (nameKey == string.Empty)
            {
                nameKey = GetUniqueSpriteName();
                element.Name = nameKey;
            }

            PhysicsSprite sprite = new PhysicsSprite(Simulator, _parentCanvas, element, points, DebugMode, (float)FrictionDefault, boundaryElement);

            sprite.Name = nameKey;
            if (sprite.OutlinePoints != null)
            {
                sprite.Collision += new PhysicsSprite.CollisionHandler(polygon_Collision);
                _parentCanvas.Children.Add(sprite);
                PhysicsObjects.Add(sprite.Name, sprite);
                sprite.Update();
            }

            sprite.MouseLeftButtonDown += new MouseButtonEventHandler(polygon_MouseLeftButtonDown);
            sprite.MouseLeftButtonUp += new MouseButtonEventHandler(polygon_MouseLeftButtonUp);
            sprite.MouseMove += new MouseEventHandler(polygon_MouseMove);

            sprite.GeometryObject.CollisionGroup = physObject.CollisionGroup;
            sprite.BodyObject.IsStatic = physObject.IsStatic;

            if (physObject.RestitutionCoefficient != 0)
                sprite.GeometryObject.RestitutionCoefficient = (float)physObject.RestitutionCoefficient;
            if (physObject.FrictionCoefficient != 0)
                sprite.GeometryObject.FrictionCoefficient = (float)physObject.FrictionCoefficient;
            if (physObject.MomentOfIntertia != 0)
                sprite.BodyObject.MomentOfInertia = (float)physObject.MomentOfIntertia;
			if (physObject.Mass != 0)
				sprite.BodyObject.Mass = (float)physObject.Mass;

            return sprite;
        }

        /// <summary>
        /// Gets a unique name for a sprite
        /// </summary>
        /// <returns></returns>
        private string GetUniqueSpriteName()
        {
            string spriteName = "sprite_" + _uniqueSpriteIndex;
            while (ElementExists(spriteName))
            {
                _uniqueSpriteIndex++;
                spriteName = "sprite_" + _uniqueSpriteIndex;
            }
            return spriteName;
        }

        private bool ElementExists(string name)
        {
            // this crappy wrapper for FindName is required to work around 
            // a difference in FindName between WPF and Silverlight.
            bool exists = false;

            foreach (UIElement element in _parentCanvas.Children)
            {
                if (element.GetValue(Canvas.NameProperty).ToString() == name)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        public PhysicsSprite AddPhysicsBody(UIElement element)
        {
            return AddPhysicsBody(element, null);
        }

        /// <summary>
        /// Adds a single XAML element into the Physics simulation.
        /// </summary>
        /// <param name="cnvContainer">The container to add items from.</param>
        public PhysicsSprite AddPhysicsBody(UIElement element, UIElement boundaryElement)
        {

            string thisName = element.GetValue(Canvas.NameProperty).ToString();
            element.SetValue(Canvas.TagProperty, thisName);

            // special case: if we are adding a UserControl _AND_ the UserControl contains one or more
            // Physics Controllers, then we handle that UserControl as a "nested" canvas!
            if (element is UserControl)
            {
                Canvas cnvNestedPhysics = FindPhysicsCanvas(element as UserControl);
                if (cnvNestedPhysics != null)
                {
                    EnsureUniqueNames(cnvNestedPhysics);
                    AddPhysicsBodyForCanvas(cnvNestedPhysics);
                    return null;
                }
            }

            // look to see if we have processed this object before, and if so,
            // use its point collection instead of recalculating
            List<Point> points = FindMatchingUIElement(element);

            string nameKey = (string)element.GetValue(Canvas.NameProperty);
            PhysicsSprite polygon = new PhysicsSprite(Simulator, _parentCanvas, element, points, DebugMode, (float)FrictionDefault, boundaryElement);
            polygon.Name = nameKey;
            if (polygon.OutlinePoints != null)
            {
                polygon.Collision += new PhysicsSprite.CollisionHandler(polygon_Collision);
                _parentCanvas.Children.Add(polygon);
                PhysicsObjects.Add(polygon.Name, polygon);
                polygon.Update();
            }

            //if (MousePickEnabled)
            //{
            polygon.MouseLeftButtonDown += new MouseButtonEventHandler(polygon_MouseLeftButtonDown);
            polygon.MouseLeftButtonUp += new MouseButtonEventHandler(polygon_MouseLeftButtonUp);
            polygon.MouseMove += new MouseEventHandler(polygon_MouseMove);
            //}

            return polygon;

        }

        /// <summary>
        /// Tries to find a matching element that has already had its points processed,
        /// and if so, returns that list of points.
        /// </summary>
        /// <returns>The list of matching points, or null</returns>
        private List<Point> FindMatchingUIElement(UIElement element)
        {
            List<Point> retval = null;
            string sName = element.GetValue(Canvas.NameProperty).ToString();

            foreach (PhysicsSprite sprite in PhysicsObjects.Values)
            {
                if (element.GetValue(Canvas.TagProperty) != null)
                {
                    if (sprite.OriginalElementName == element.GetValue(Canvas.TagProperty).ToString() || 
						ElementIsACopy(sprite.OriginalElementName, element.GetValue(Canvas.TagProperty).ToString()))
                    {
                        retval = sprite.OutlinePoints;
                        System.Diagnostics.Debug.WriteLine(sName + " - Outline was retrieved from previously added element " + sprite.OriginalElementName);
                        break;
                    }
                }
            }

            if (retval != null)
            {
                return retval;
            }

            // now check the point cache for a match
            if (PointListCollection != null)
            {
                foreach (PointObject item in PointListCollection)
                {
                    if (sName != null)
                    {
                        if (item.ElementName == sName || ElementIsACopy(item.ElementName, sName))
                        {

                            List<Point> points = new List<Point>();
                            try
                            {
                                foreach (Point pt in item.ListPoints)
                                {
                                    points.Add(pt);
                                }
                            }
                            catch (Exception)
                            {
                                throw new Exception("There was a format problem in the PointListCollection property of the Physics Controller.");
                            }

                            System.Diagnostics.Debug.WriteLine(sName + " - Outline was retrieved from cached element " + item.ElementName);


                            retval = points;
                        }
                    }
                }

            }

            if (retval == null)
                System.Diagnostics.Debug.WriteLine("Could not find " + sName + " in PointCache.");

            return retval;

        }

        /// <summary>
        /// Checks if an element is two elements can be considered the same (a copy)
        /// </summary>
        public bool ElementIsACopy(string elem1, string elem2)
        {
            if (elem1.Contains("_"))
            {
                string suffix = elem1.Substring(elem1.LastIndexOf("_") + 1);
                int index;
                if (Int32.TryParse(suffix, out index))
                {
                    elem1 = elem1.Substring(0, elem1.LastIndexOf("_"));
                }
            }
            if (elem2.Contains("_"))
            {
                string suffix = elem2.Substring(elem2.LastIndexOf("_") + 1);
                int index;
                if (Int32.TryParse(suffix, out index))
                {
                    elem2 = elem2.Substring(0, elem2.LastIndexOf("_"));
                }
            }

            return (elem1.ToString() == elem2.ToString());
        }

        public static void EnsureUniqueNames(Canvas cnv)
        {
            EnsureUniqueNames(cnv, true);
        }
        
        /// <summary>
        /// Returns a guaranteed unique name for an element in the canvas
        /// </summary>
        public static void EnsureUniqueNames(Canvas cnv, bool setTag)
        {
            foreach (UIElement element in cnv.Children)
            {
                string thisName = element.GetValue(Canvas.NameProperty).ToString();
                string newName = thisName;

                if (thisName == string.Empty)
                    continue;

                newName = EnsureUniqueNameSingle(_parentCanvas, newName);

                element.SetValue(Canvas.NameProperty, newName);
                if (setTag) 
                    element.SetValue(Canvas.TagProperty, thisName);

                // we also need to update contoller body names for joins, static bodies
                foreach (UIElement elementSub in cnv.Children)
                {
                    if (elementSub is PhysicsJoint)
                    {
                        PhysicsJoint thisJoint = elementSub as PhysicsJoint;
                        if (thisJoint.BodyOne == thisName)
                            thisJoint.BodyOne = newName;
                        if (thisJoint.BodyTwo == thisName)
                            thisJoint.BodyTwo = newName;
                    }
                    if (elementSub is PhysicsStaticHolder)
                    {
                        PhysicsStaticHolder thisHolder = elementSub as PhysicsStaticHolder;
                        if (thisHolder.Body == thisName)
                            thisHolder.Body = newName;
                    }
                    PhysicsJointMain physJoint = elementSub.GetValue(PhysicsJointMain.PhysicsJointProperty) as PhysicsJointMain;
                    if (physJoint != null)
                    {
                        if (physJoint.BodyOne == thisName)
                            physJoint.BodyOne = newName;
                        if (physJoint.BodyTwo == thisName)
                            physJoint.BodyTwo = newName;
                    }

                }
            }

        }

        static string EnsureUniqueNameSingle(Canvas cnv, string name)
        {
            int suffix = 1;
            string thisName = name;
            while (cnv.FindName(name) != null)
            {
                name = thisName + "_" + suffix.ToString();
                suffix++;
            }
            return name;
        }

        void polygon_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mousePickSpring != null)
            {
                Vector2 point = new Vector2((float)(e.GetPosition(_parentCanvas).X), (float)(e.GetPosition(_parentCanvas).Y));
                _mousePickSpring.WorldAttachPoint = point;

                _lineShowSpring.X2 = point.X;
                _lineShowSpring.Y2 = point.Y;
                _lineShowSpring.Visibility = Visibility.Visible;
            }

            if (MouseMove != null && _pickedSprite != null)
                MouseMove(_pickedSprite.Name);

        }

        void polygon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_pickedSprite != null)
                _pickedSprite.ReleaseMouseCapture();
            if (_mousePickSpring != null && _mousePickSpring.IsDisposed == false)
            {
                _mousePickSpring.Dispose();
                _mousePickSpring = null;

                _lineShowSpring.Visibility = Visibility.Collapsed;

                e.Handled = true;
            }

            if (MouseLeftButtonUp != null && _pickedSprite != null)
                MouseLeftButtonUp(_pickedSprite.Name);

            _pickedSprite = null;
        }

        void polygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Vector2 point = new Vector2((float)(e.GetPosition(_parentCanvas).X), (float)(e.GetPosition(_parentCanvas).Y));
            _pickedSprite = (sender as PhysicsSprite);
            _pickedSprite.CaptureMouse();

            if (MousePickEnabled)
            {
                if (_pickedSprite != null)
                {
                    _mousePickSpring = ControllerFactory.Instance.CreateFixedLinearSpring(Simulator, _pickedSprite.BodyObject, _pickedSprite.BodyObject.GetLocalPosition(point), point, 10, 5);
                    _lineShowSpring.X1 = point.X;
                    _lineShowSpring.Y1 = point.Y;
                    _lineShowSpring.X2 = point.X;
                    _lineShowSpring.Y2 = point.Y;
                    _lineShowSpring.Visibility = Visibility.Visible;
                    e.Handled = true;
                }
            }

            if (MouseLeftButtonDown != null)
                MouseLeftButtonDown(_pickedSprite.Name);

        }

        private Canvas FindPhysicsCanvas(UserControl uc)
        {
            Canvas cnvRetVal = null;
            Canvas cnvLayoutRoout = uc.FindName("LayoutRoot") as Canvas;

            if (cnvLayoutRoout != null)
            {

                foreach (UIElement child in cnvLayoutRoout.Children)
                {
                    if (child is PhysicsJoint || child is PhysicsSprite || child is PhysicsStaticHolder 
                        || child.GetValue(PhysicsJointMain.PhysicsJointProperty) != null
                        || child.GetValue(PhysicsObjectMain.PhysicsObjectProperty) != null)
                        cnvRetVal = cnvLayoutRoout;
                }
            }
            return cnvRetVal;
        }

        void polygon_Collision(string sourceSprite, string collisionSprite)
        {
            if (Collision != null)
            {
                Collision(sourceSprite, collisionSprite);
            }
        }

        /// <summary>
        /// Adds ALL Joint objects from a Canvas into the simulation.
        /// </summary>
        /// <param name="cnvContainer"></param>
        public void AddJointsForCanvas(Canvas cnvContainer)
        {
            List<string> elements = new List<string>();
            for (int i = 0; i < cnvContainer.Children.Count; i++)
            {
                if (cnvContainer.Children[i] is PhysicsJoint)
                    AddJoint(cnvContainer.Children[i] as PhysicsJoint);
            }

        }

        public void AddJoint(PhysicsJointMain joint)
        {
            string bodyOne = joint.BodyOne;
            string bodyTwo = joint.BodyTwo;
            int collisionGroup = joint.CollisionGroup;
            bool isAngleSpringEnabled = joint.AngleSpringEnabled;
            float springConstant = joint.AngleSpringConstant;
            float dampeningConstant = joint.AngleSpringDampningConstant;
            float angleLowerLimit = (float)joint.AngleLowerLimit;
            float angleUpperLimit = (float)joint.AngleUpperLimit;


            Point center = joint.GetCenter();
            Vector2 ptCollisionCenter = new Vector2((float)center.X, (float)center.Y);


            if (!PhysicsObjects.ContainsKey(bodyOne))
                throw new Exception("Cannot add joint for an invalid BodyOne value of '" + bodyOne + "'. If using Behaviors, did you forgot to add a PhysicsObjectBehavior?");
            if (!PhysicsObjects.ContainsKey(bodyTwo))
                throw new Exception("Cannot add joint for an invalid BodyTwo value of '" + bodyTwo + "'. If using Behaviors, did you forgot to add a PhysicsObjectBehavior?");

            Body body1 = PhysicsObjects[bodyOne].BodyObject;
            Body body2 = PhysicsObjects[bodyTwo].BodyObject;

            Geom geom1 = PhysicsObjects[bodyOne].GeometryObject;
            Geom geom2 = PhysicsObjects[bodyTwo].GeometryObject;
            RevoluteJoint revoluteJoint = JointFactory.Instance.CreateRevoluteJoint(Simulator, body1, body2, ptCollisionCenter);

            if (isAngleSpringEnabled)
            {
                AngleSpring angleSpring = new AngleSpring(body1, body2, springConstant, dampeningConstant);
                Simulator.Add(angleSpring);
            }

            if (angleUpperLimit != -1 && angleLowerLimit != -1)
            {
                float upperAngle = (float)DegreesToRadians(angleUpperLimit);
                float lowerAngle = (float)DegreesToRadians(angleLowerLimit);
                AngleLimitJoint angleLimitJoint = new AngleLimitJoint(body1, body2, lowerAngle, upperAngle);
                Simulator.Add(angleLimitJoint);
            }

            if (collisionGroup > 0)
            {
                geom1.CollisionGroup = collisionGroup;
                geom2.CollisionGroup = collisionGroup;
            }

            // get rid of the UI representation of the joint
            joint.VisualElement.Visibility = Visibility.Collapsed;


        }

        public void AddFluidContainer(FluidContainerMain fluidContainerMain)
        {
            double x = Convert.ToDouble(fluidContainerMain.VisualElement.GetValue(Canvas.LeftProperty));
            double y = Convert.ToDouble(fluidContainerMain.VisualElement.GetValue(Canvas.TopProperty));
            float width = (float)fluidContainerMain.VisualElement.ActualWidth;
            float height = (float)fluidContainerMain.VisualElement.ActualHeight;

            WaveController waveController = fluidContainerMain.WaveControllerObject;
            waveController.Position = new Vector2((float)x, (float)y); 
            waveController.Width = width; 
            waveController.Height = height; 
            waveController.NodeCount = fluidContainerMain.NodeCount; 
            waveController.DampingCoefficient = (float)fluidContainerMain.DampingCoefficient; 
            waveController.Frequency = (float)fluidContainerMain.Frequency; 

            waveController.WaveGeneratorMax = (float)fluidContainerMain.WaveGeneratorMax;
            waveController.WaveGeneratorMin = (float) fluidContainerMain.WaveGeneratorMin;
            waveController.WaveGeneratorStep = (float)fluidContainerMain.WaveGeneratorStep;

            waveController.Initialize();

            Vector2 vecTopLeft = new Vector2((float)x, (float)y);
            Vector2 vecBottomRight = new Vector2((float)x + width, (float)y + height);
            
            AABB waterAABB = new AABB(vecTopLeft, vecBottomRight);
            AABBFluidContainer waterContainer = new AABBFluidContainer(waterAABB);
            FluidDragController fluidDragController = new FluidDragController();

            foreach (KeyValuePair<string, PhysicsSprite> item in PhysicsObjects)
            {
                PhysicsSprite sprite = item.Value;
                fluidDragController.AddGeom(sprite.GeometryObject);
            }

            fluidDragController.Initialize(waterContainer, (float)fluidContainerMain.FluidDensity, (float)fluidContainerMain.LinearDragCoefficient, (float)fluidContainerMain.RotationalDragCoefficient, new Vector2((float)fluidContainerMain.GravityHorizontal, (float)fluidContainerMain.GravityVertical));
            Simulator.Add(fluidDragController);
        }

        /// <summary>
        /// Adds a single joint objectt from a Canvas into the simulation.
        /// </summary>
        /// <param name="cnvContainer">The Canvas to add joints from</param>
        public void AddJoint(PhysicsJoint joint)
        {
            if (!PhysicsObjects.ContainsKey(joint.BodyOne))
                throw new Exception("A PhysicsJoint exists with an invalid BodyOne value of '" + joint.BodyOne + "'.");
            if (!PhysicsObjects.ContainsKey(joint.BodyTwo))
                throw new Exception("A PhysicsJoint exists with an invalid BodyTwo value of '" + joint.BodyTwo + "'.");


            Body body1 = PhysicsObjects[joint.BodyOne].BodyObject;
            Body body2 = PhysicsObjects[joint.BodyTwo].BodyObject;

            Geom geom1 = PhysicsObjects[joint.BodyOne].GeometryObject;
            Geom geom2 = PhysicsObjects[joint.BodyTwo].GeometryObject;

 
            Vector2 ptCollisionCenter = new Vector2((float)(joint.GetCenter().X), (float)(joint.GetCenter().Y));

            RevoluteJoint revoluteJoint = JointFactory.Instance.CreateRevoluteJoint(Simulator, body1, body2, ptCollisionCenter);
            joint.RevoluteJointObject = revoluteJoint;

            if (joint.AngleSpringEnabled)
            {
                float springConstant = joint.AngleSpringConstant;
                float dampeningConstant = joint.AngleSpringDampningConstant;
                AngleSpring angleSpring = new AngleSpring(body1, body2, springConstant, dampeningConstant);
                Simulator.Add(angleSpring);
            }

            if (joint.CollisionGroup > 0)
            {
                geom1.CollisionGroup = joint.CollisionGroup;
                geom2.CollisionGroup = joint.CollisionGroup;
            }

            joint.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// Adds ALL Static holder objects from a canvas into the simulation.
        /// </summary>
        /// <param name="cnvContainer">The Canvas to add static holders from</param>
        public void AddStaticHoldersForCanvas(Canvas cnvContainer)
        {
            List<string> elements = new List<string>();
            for (int i = 0; i < cnvContainer.Children.Count; i++)
            {
                if (cnvContainer.Children[i] is PhysicsStaticHolder)
                    AddStaticHolder(cnvContainer.Children[i] as PhysicsStaticHolder);
            }

        }

        /// <summary>
        /// Adds a single Static holder object from a canvas into the simulation.
        /// </summary>
        /// <param name="cnvContainer">The static holder to add</param>
        public void AddStaticHolder(PhysicsStaticHolder staticHolder)
        {
            if (!PhysicsObjects.ContainsKey(staticHolder.Body))
                throw new Exception("A PhysicsStaticHolder exists with an invalid Body value of '" + staticHolder.Body + "'.");

            Body body1 = PhysicsObjects[staticHolder.Body].BodyObject;
            body1.IsStatic = true;

            staticHolder.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// Adds a single Static holder object from a canvas into the simulation.
        /// </summary>
        /// <param name="cnvContainer">The static holder to add</param>
        public void AddStaticHolder(string body)
        {
            if (!PhysicsObjects.ContainsKey(body))
                throw new Exception("A PhysicsStaticHolder exists with an invalid Body value of '" + body + "'.");

            Body body1 = PhysicsObjects[body].BodyObject;
            body1.IsStatic = true;

        }


        /// <summary>
        /// Given a list of points, get the overall width and height to cover them all, as well as the mininum X and Y values occurring.
        /// </summary>
        /// <param name="points">The list of points</param>
        public static void GetPointDimensions(List<Point> points, out double width, out double height, out double minX, out double minY)
        {
            minX = int.MaxValue;
            minY = int.MaxValue;

            double maxX = int.MinValue, maxY = int.MinValue; //
            foreach (Point point in points)
            {
                if (point.X < minX) minX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.X > maxX) maxX = point.X;
                if (point.Y > maxY) maxY = point.Y;
            }

            width = (maxX - minX);
            height = (maxY - minY);
        }

        /// <summary>
        /// Create a path object based on a list of vertices - used when creating geometry
        /// </summary>
        public static System.Windows.Shapes.Path CreatePathFromVertices(Vertices vertices, double width, double height)
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Stroke = new SolidColorBrush(Color.FromArgb(255, 76, 108, 76));
            path.StrokeThickness = 4;
            path.StrokeStartLineCap = PenLineCap.Round;
            path.Fill = new SolidColorBrush(Color.FromArgb(255, 106, 147, 106));


            PathGeometry pathGeom = new PathGeometry();
            PathFigureCollection figures = new PathFigureCollection();
            pathGeom.Figures = figures;
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point((double)vertices[0].X + width / 2, (double)vertices[0].Y + height / 2);
            figure.Segments = new PathSegmentCollection();
            pathGeom.Figures.Add(figure);

            foreach (Vector2 vector in vertices)
            {
                LineSegment line = new LineSegment() { Point = new Point((double)vector.X + width / 2, (double)vector.Y + height / 2) };
                figure.Segments.Add(line);
            }

            path.Data = pathGeom;
            return path;
        }

        /// <summary>
        /// Get the distance, in pixels between two points.
        /// </summary>
        public static double DistanceBetweenTwoPoints(Point pt1, Point pt2)
        {

            double a = pt2.X - pt1.X;
            double b = pt2.Y - pt1.Y;
            return Math.Sqrt(a * a + b * b);
        }

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="angle">The degree value to convert</param>
        /// <returns>Radian value</returns>
        public double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public double RadiansToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
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
            double theta = angle * 0.0174532925;
            Point p = new Point();
            p.X = ptStart.X + distance * Math.Cos(theta);
            p.Y = ptStart.Y + distance * Math.Sin(theta);
            return p;
        }

        /// <summary>
        /// Completely remove a sprite
        /// </summary>
        /// <param name="sprite"></param>
        public void DeletePhysicsObject(string sprite)
        {
            bool found = PhysicsObjects.Where(e => e.Key == sprite).Count() > 0;
            if (found)
            {
                PhysicsObjects[sprite].IsActive = false;
                PhysicsObjects[sprite].Update();
                Simulator.Remove(PhysicsObjects[sprite].BodyObject);
                Simulator.Remove(PhysicsObjects[sprite].GeometryObject);
                _parentCanvas.Children.Remove(_parentCanvas.FindName(sprite) as UIElement);
                PhysicsObjects.Remove(sprite);
            }

        }


    }



}
