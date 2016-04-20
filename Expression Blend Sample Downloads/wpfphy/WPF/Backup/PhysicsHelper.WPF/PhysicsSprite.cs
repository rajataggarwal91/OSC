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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Factories;
using System.Text;
using System.Collections;

namespace Spritehand.FarseerHelper
{
	public class PhysicsSprite : UserControl
	{
		// This Xaml is used by the constructor.  The class is creating the sprite in cide
		// so it can be used by both WPF and Silverlight.  Ultimately this class could be
		// generated from code but it both easier to see the structure of the sprite and 
		// to change it if the class is generated from Xaml.
#if SILVERLIGHT
		static string defaultNamespace = "http://schemas.microsoft.com/client/2007";
#else
		static string defaultNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
#endif
		string xaml = string.Format( @"
				<Canvas xmlns=""{0}""
					xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
					Width=""640"" Height=""480"" x:Name=""RootCanvas"">
					<Canvas.RenderTransform>
						<TransformGroup>
							<!-- This transform conrols the rotation-->
							<RotateTransform x:Name=""rotateSprite"" Angle=""0"" CenterX=""320"" CenterY=""240""/>
							<TranslateTransform x:Name=""translateTransform"" X=""0"" Y=""0""/>
						</TransformGroup>
					</Canvas.RenderTransform>
					<Ellipse Width=""10"" Height=""10"" Fill=""#FFFFFFFF"" Stroke=""#FF000000"" x:Name=""ellipseCenter""/>
					<Rectangle Width=""249"" Height=""135"" Canvas.Left=""10"" Canvas.Top=""25"" StrokeThickness=""1"" x:Name=""rectoutline"" Opacity=""0.1"">
						<Rectangle.Fill>
							<LinearGradientBrush EndPoint=""0.5,1"" StartPoint=""0.5,0"">
								<GradientStop Color=""#FFB13232"" Offset=""0""/>
								<GradientStop Color=""#FFA55E5E"" Offset=""1""/>
							</LinearGradientBrush>
						</Rectangle.Fill>
						<Rectangle.Stroke>
							<LinearGradientBrush EndPoint=""0.5,1"" StartPoint=""0.5,0"">
								<GradientStop Color=""#FFD63939"" Offset=""0""/>
								<GradientStop Color=""#FFD08787"" Offset=""1""/>
							</LinearGradientBrush>
						</Rectangle.Stroke>
					</Rectangle>
					<Canvas x:Name=""InsideCanvas"">
			            
					</Canvas>

				</Canvas>
				", defaultNamespace);

		Canvas RootCanvas = null;
		Canvas InsideCanvas = null;

		private bool _isActive = true;
		private Body _bodyObject;
		private float _X;
		private float _Y;
		private float _rotation;
		private Vector2 _offsetCentroid;
		private System.Windows.Shapes.Path _pathDebug { get; set; }

		public PhysicsSimulator Simulator { get; set; }
		public RotateTransform RotationTransform { get; set; }
		public TranslateTransform TranslationTransform { get; set; }
		public List<Point> OutlinePoints { get; set; }
		public UIElement uiElement { get; set; }
		public string OriginalElementName { get; set; }
		public string OriginalCanvasName { get; set; }
		public string OriginalUserControlName { get; set; }

		// event for Collision between geometries
		public delegate void CollisionHandler(string sourceSprite, string collisionSprite);
		public event CollisionHandler Collision;

		public PhysicsSprite(PhysicsSimulator physicsSim, Canvas parentCanvas, UIElement element, List<Point> points, bool showDebug, float defaultFriction, UIElement boundaryElement)
		{
#if SILVERLIGHT
			RootCanvas = (Canvas)System.Windows.Markup.XamlReader.Load(xaml);
#else
			using(System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(new System.IO.StringReader(xaml)))
				RootCanvas = (Canvas)System.Windows.Markup.XamlReader.Load(xmlReader);
#endif
			this.Content = RootCanvas;
			this.InsideCanvas = (Canvas)RootCanvas.FindName("InsideCanvas");

			Simulator = physicsSim;
			RotationTransform = RootCanvas.FindName("rotateSprite") as RotateTransform;
			TranslationTransform = RootCanvas.FindName("translateTransform") as TranslateTransform;

			IsNestedUsercontrol = BoundaryHelper.IsInsideNestedUsercontrol(element);
			OriginalNestedLeft = 0; OriginalNestedTop = 0;
			OriginalElementName = element.GetValue(Canvas.NameProperty).ToString();
			OriginalCanvasName = parentCanvas.GetValue(Canvas.NameProperty).ToString();

			if (IsNestedUsercontrol)
			{
				// this is a user control with nested physics elements.
				// store it's offset for later positioning
				Canvas cnvParent = (element as FrameworkElement).Parent as Canvas;
				UserControl ucParentUserControl = (cnvParent.Parent as UserControl);
				OriginalUserControlName = ucParentUserControl.Name;
				OriginalNestedLeft = Convert.ToInt32(ucParentUserControl.GetValue(Canvas.LeftProperty));
				OriginalNestedTop = Convert.ToInt32(ucParentUserControl.GetValue(Canvas.TopProperty));

				// we'll process the storyboards so that they point to element references
				foreach (DictionaryEntry item in ucParentUserControl.Resources)
				{
					if (item.Value is Storyboard)
					{
						Storyboard sb = item.Value as Storyboard;
						foreach (Timeline tl in sb.Children)
						{
							//if (tl.GetValue(Storyboard.TargetNameProperty) == element.GetValue(Canvas.NameProperty))
							string target = tl.GetValue(Storyboard.TargetNameProperty).ToString();
							UIElement targetElement = ucParentUserControl.FindName(target) as UIElement;
							if (targetElement != null)
								Storyboard.SetTarget(sb, targetElement);

						}
					}
				}
			}

			// do we want to use a different element to determine boundary outline?
			UIElement elementBoundary = element;
			if (boundaryElement != null)
				elementBoundary = boundaryElement;


			if (points == null)
				points = BoundaryHelper.GetPointsForElement(elementBoundary, element, false);
			else
				BoundaryHelper.GetPointsForElement(elementBoundary, element, true);

			if (points == null || points.Count == 0)
				return;

			OutlinePoints = new List<Point>();
			OutlinePoints.AddRange(points);


			// let's clean up the number of points, for a little performance boost..
			int distanceThreshold = 5;
			Point ptLast = points[points.Count - 1];
			for (int i = points.Count - 2; i >= 0; i--)
			{
				if (points.Count < 10) break;

				if (PhysicsController.DistanceBetweenTwoPoints(ptLast, points[i]) < distanceThreshold)
				{
					points.Remove(points[i]);
				}
				else
					ptLast = points[i];
			}

			// get the overall dimensions
			double width, height, minX, minY;
			PhysicsController.GetPointDimensions(points, out width, out height, out minX, out minY);


			// make the canvas size match the path it contains
			this.Width = width;
			this.Height = height;
			RootCanvas.SetValue(Canvas.WidthProperty, width);
			RootCanvas.SetValue(Canvas.HeightProperty, height);

			// nested user control elements are removed from their user control
			// and copied into the main game canvas
			if (BoundaryHelper.IsInsideNestedUsercontrol(element))
			{
				UserControl ucParent = BoundaryHelper.GetParentUserControl(element);

				Canvas cnvParent = (element as FrameworkElement).Parent as Canvas;
				cnvParent.Children.Remove(element);

			}

			// draw the polygon
			if (uiElement != null)
			{
				RootCanvas.Children.Remove(uiElement);
				uiElement = null;
			}

			// create the physics body
			Vertices vertices = new Vertices();

			double centerX = (this.Width / 2);
			double centerY = (this.Height / 2);


			foreach (Point point in points)
			{
				// note vertices are based on 0,0 at center of body
				double x, y;

				// first we need to remove the offset position of the drawn shape on the main canvas
				x = Convert.ToDouble(point.X) - Convert.ToDouble(element.GetValue(Canvas.LeftProperty)); //minX;
				y = Convert.ToDouble(point.Y) - Convert.ToDouble(element.GetValue(Canvas.TopProperty)); //minY;

				// we need to make points relative to origin (center) of object
				x = x - centerX;
				y = y - centerY;

				vertices.Add(new Vector2((float)x, (float)y));
			}

			// if there are too few points, then subdivide the edges
			if (vertices.Count <= 4)
				vertices.SubDivideEdges(6);

			_offsetCentroid = vertices.GetCentroid();


			// initialize the polygon sprite
			BodyObject = BodyFactory.Instance.CreateBody(physicsSim, 0.5f, vertices.GetMomentOfInertia());
			GeometryObject = GeomFactory.Instance.CreatePolygonGeom(physicsSim, BodyObject, vertices, 0);

			GeometryObject.FrictionCoefficient = defaultFriction;
			GeometryObject.OnCollision += new FarseerGames.FarseerPhysics.Collisions.CollisionEventHandler(HandleCollision);

			BodyObject.Enabled = true;
			BodyObject.ClearForce();
			BodyObject.ClearTorque();

			// adjust position to take into consideration origin at middle 
			Vector2 position = new Vector2((float)minX, (float)minY);
			position.X = (position.X + ((float)getWidth / 2));
			position.Y = (position.Y + ((float)getHeight / 2));

			// adjust for centroid
			bool bAdjustCentroid = true;
			if ((element is Image) && (element as Image).Clip != null) bAdjustCentroid = false;

			if (bAdjustCentroid)
			{
				position.X = position.X + _offsetCentroid.X;
				position.Y = position.Y + _offsetCentroid.Y;
			}

			BodyObject.Position = position;

			Update();

			uiElement = element;
#if SILVERLIGHT
            parentCanvas.Children.Remove(uiElement);
#else
			if (parentCanvas == System.Windows.Media.VisualTreeHelper.GetParent(element))
				parentCanvas.Children.Remove(uiElement);
			else
			{
				Canvas cnv = (Canvas)System.Windows.Media.VisualTreeHelper.GetParent(element);
				if (cnv == null)
					parentCanvas.Children.Remove(uiElement);
				else
					cnv.Children.Remove(element);
			}
#endif

			uiElement.SetValue(Canvas.LeftProperty, -(width / 2));
			uiElement.SetValue(Canvas.TopProperty, -(height / 2));

			TranslateTransform trans = new TranslateTransform();
			trans.X = width / 2;
			trans.Y = height / 2;
			uiElement.RenderTransform = trans;

			InsideCanvas.Children.Add(uiElement);

			uiElement.SetValue(Canvas.LeftProperty, (double)(-((width / 2) + _offsetCentroid.X)));
			uiElement.SetValue(Canvas.TopProperty, (double)(-((height / 2) + _offsetCentroid.Y)));

			if (IsNestedUsercontrol)
			{
				BodyObject.Position = new Vector2((float)(BodyObject.Position.X + OriginalNestedLeft), (float)(BodyObject.Position.Y + OriginalNestedTop));
				Update();
			}

			if (showDebug)
			{
				_pathDebug = PhysicsController.CreatePathFromVertices(vertices, getWidth, getHeight);
				_pathDebug.Opacity = 0.7;
				RootCanvas.Children.Add(_pathDebug);
			}


			SetDebug(showDebug);


		}

		private bool HandleCollision(Geom g1, Geom g2, ContactList contactList)
		{
			if (Collision != null)
				Collision(g1.Tag.ToString(), g2.Tag.ToString());
			return true;

		}

		/// <summary>
		/// If true, then this sprite started life as a nested object inside a separate usercontrol
		/// </summary>
		public bool IsNestedUsercontrol { get; set; }

		/// <summary>
		/// If this sprite was a nested object, then this is its parent's original left position
		/// </summary>
		public double OriginalNestedLeft { get; set; }

		/// <summary>
		/// If this sprite was a nested object, then this is its parent's original top position
		/// </summary>
		public double OriginalNestedTop { get; set; }

		public new string Name
		{
			get
			{
				return this.GetValue(Canvas.NameProperty).ToString();
			}
			set
			{
				if (GeometryObject != null)
					GeometryObject.Tag = value;
				this.SetValue(Canvas.NameProperty, value);
			}
		}

		/// <summary>
		/// The Body element for Physics simulations
		/// </summary>
		public Body BodyObject
		{

			get
			{
				return _bodyObject;
			}
			set
			{
				_bodyObject = value;

				// set up our translation transform - this is needed because Farseer uses a centered origin instead of top-left coordinates
				TranslationTransform.X = -getWidth / 2;
				TranslationTransform.Y = -getHeight / 2;

				// set up our rotation transform - this is so Farseer can rotate the object as necessary
				RotationTransform.CenterX = getWidth / 2;
				RotationTransform.CenterY = getHeight / 2;
			}
		}

		/// <summary>
		/// The Geometry object (shape used for collision detection)
		/// </summary>
		public Geom GeometryObject { get; set; }

		/// <summary>
		/// This method must be called to keep the sprite position and rotation in sync
		/// with the Farseer Physics object body.
		/// </summary>
		public virtual void Update()
		{

			_X = BodyObject.Position.X;
			this.SetValue(Canvas.LeftProperty, Convert.ToDouble(_X));

			_Y = BodyObject.Position.Y;
			this.SetValue(Canvas.TopProperty, Convert.ToDouble(_Y));

			_rotation = BodyObject.Rotation;
			RotationTransform.Angle = (_rotation * 360) / (2 * Math.PI);
		}

		/// <summary>
		/// Whether or not the sprite is active (participates in collisions, etc.)
		/// </summary>
		public virtual bool IsActive
		{
			get { return _isActive; }
			set
			{
				_isActive = value;

				BodyObject.Enabled = value;
				GeometryObject.CollisionEnabled = value;

				if (value == true)
				{
					if (Simulator.BodyList.IndexOf(BodyObject) < 0)
						Simulator.Add(BodyObject);
					if (Simulator.GeomList.IndexOf(GeometryObject) < 0)
						Simulator.Add(GeometryObject);

					if (_pathDebug != null)
						_pathDebug.Visibility = Visibility.Visible;
				}
				if (value == false)
				{
					Simulator.Remove(BodyObject);
					Simulator.Remove(GeometryObject);

					if (_pathDebug != null)
						_pathDebug.Visibility = Visibility.Collapsed;
				}
			}
		}

		/// <summary>
		/// Whether or not the sprite is visible (Drawn to the screen)
		/// </summary>
		public virtual bool Visible
		{
			get { return (this.Visibility == Visibility.Visible); }
			set
			{
				if (value)
					this.Visibility = Visibility.Visible;
				else
					this.Visibility = Visibility.Collapsed;
			}
		}

		/// <summary>
		/// The width, in pixels, of the sprite
		/// </summary>
		public virtual double getWidth
		{
			get { return this.Width; }
			set { this.Width = value; }
		}

		/// <summary>
		/// The height, in pixels, of the sprite
		/// </summary>
		public virtual double getHeight
		{
			get { return this.Height; }
			set { this.Height = value; }
		}


		private void SetDebug(bool debug)
		{
			Ellipse ellipseCenter = RootCanvas.FindName("ellipseCenter") as Ellipse;
			Rectangle rectOutline = RootCanvas.FindName("rectoutline") as Rectangle;

			if (debug)
			{
				double centerX = (this.Width / 2);
				double centerY = (this.Height / 2);

				// add some debug stuff
				ellipseCenter.SetValue(Canvas.LeftProperty, centerX - 5);
				ellipseCenter.SetValue(Canvas.TopProperty, centerY - 5);
				ellipseCenter.SetValue(Canvas.ZIndexProperty, 1000);

				rectOutline.SetValue(Canvas.LeftProperty, 0D);
				rectOutline.SetValue(Canvas.TopProperty, 0D);
				rectOutline.Width = this.Width;
				rectOutline.Height = this.Height;

				ellipseCenter.Visibility = Visibility.Visible;
				rectOutline.Visibility = Visibility.Visible;
			}
			else
			{
				ellipseCenter.Visibility = Visibility.Collapsed;
				rectOutline.Visibility = Visibility.Collapsed;
			}
		}
	}
}