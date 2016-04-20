using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Spritehand.FarseerHelper;
using System.Windows.Interactivity;
using FarseerGames.FarseerPhysics.Mathematics;

namespace Spritehand.PhysicsBehaviors
{
#if !SILVERLIGHT && BLEND3
	[Microsoft.Windows.Design.ToolboxCategory("Physics")]
#endif
	[Category("Physics")]
	[Description("Makes a set of objects attractive")]
	public class PhysicsMagnetBehavior : Behavior<FrameworkElement>
	{
		private static List<PhysicsMagnetBehavior> worldMagnets = new List<PhysicsMagnetBehavior>();
		private PhysicsSprite sprite = null;

		public static readonly DependencyProperty MagnetismProperty =
			DependencyProperty.Register("Magnetism", typeof(double), typeof(PhysicsMagnetBehavior), new PropertyMetadata(5.0));
		public static readonly DependencyProperty FallOffProperty =
			DependencyProperty.Register("FallOff", typeof(double), typeof(PhysicsMagnetBehavior), new PropertyMetadata(0.6));
		public static readonly DependencyProperty MaxDistanceProperty =
			DependencyProperty.Register("MaxDistance", typeof(double), typeof(PhysicsMagnetBehavior), new PropertyMetadata(150.0));

		[Category("Physics")]
		[Description("Relative strength of the magnetic field")]
		public double Magnetism
		{
			get { return (double)this.GetValue(PhysicsMagnetBehavior.MagnetismProperty); }
			set { this.SetValue(PhysicsMagnetBehavior.MagnetismProperty, value); }
		}

		[Category("Physics")]
		[Description("How quickly the field strength declines")]
		public double FallOff
		{
			get { return (double)this.GetValue(PhysicsMagnetBehavior.FallOffProperty); }
			set { this.SetValue(PhysicsMagnetBehavior.FallOffProperty, value); }
		}

		[Category("Physics")]
		[Description("The maximum distance over which the magnetic field is effective")]
		public double MaxDistance
		{
			get { return (double)this.GetValue(PhysicsMagnetBehavior.MaxDistanceProperty); }
			set { this.SetValue(PhysicsMagnetBehavior.MaxDistanceProperty, value); }
		}

		private PhysicsControllerMain _controller = null;
		private PhysicsControllerMain Controller
		{
			get { return _controller; }
			set { _controller = value; }
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			// this._visualElement = this.AssociatedObject;

			this.AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (this.sprite == null) return;

			PhysicsMagnetBehavior.worldMagnets.Remove(this);
		}

		void controller_Initialized(object source)
		{
			string body = this.AssociatedObject.Name;
			if (!Controller.PhysicsObjects.TryGetValue(body, out sprite))
				return;

			_controller.TimerLoop += new PhysicsControllerMain.TimerLoopHandler(_controller_TimerLoop);
			PhysicsMagnetBehavior.worldMagnets.Add(this);
		}

		void _controller_TimerLoop(object source)
		{
			if (sprite == null) return;

			foreach (PhysicsMagnetBehavior other in worldMagnets)
			{

				if (other == this || other.sprite.BodyObject == null) continue;

				Vector2 force = other.sprite.BodyObject.Position - this.sprite.BodyObject.Position;

				if (force.Length() < (this.MaxDistance + other.MaxDistance))
				{
					force = 5000 * force * (1 / (float)System.Math.Pow(force.LengthSquared(), this.FallOff + other.FallOff)) * (float)(this.Magnetism + other.Magnetism);

					this.sprite.BodyObject.ApplyForceAtWorldPoint(force, other.sprite.BodyObject.Position);
				}
			}
		}

		void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
		{
			_controller = PhysicsControllerMain.FindController(this.AssociatedObject);
			if (_controller == null)
			{
				throw new Exception("You must add a PhysicsController Behavior to the Canvas representing the main Container.");
			}

			_controller.Initialized += new PhysicsControllerMain.InitializedHandler(controller_Initialized);

		}

	}
}
