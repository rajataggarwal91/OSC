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
using System.Windows;
using System.Windows.Interactivity;
using Spritehand.FarseerHelper;
using System.ComponentModel;


namespace Spritehand.PhysicsBehaviors
{
#if !SILVERLIGHT && BLEND3
	[Microsoft.Windows.Design.ToolboxCategory("Physics")]
#endif
	[Category("Physics")]
	[Description("Elements to which this behavior is attached respond like physical objects")]
	public class PhysicsObjectBehavior : Behavior<FrameworkElement>
    {
        private PhysicsObjectMain _physicsObjectMain = new PhysicsObjectMain();

        protected override void OnAttached()
        {
            base.OnAttached();

            _physicsObjectMain.VisualElement = this.AssociatedObject;
            PhysicsObjectMain.SetPhysicsObject(this.AssociatedObject, _physicsObjectMain);

            this.AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
        }

        private PhysicsControllerMain _controller = null;
        private PhysicsControllerMain Controller
        {
            get { return _controller; }
            set
            {
                _controller = value;
            }
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            PhysicsControllerMain controller = PhysicsControllerMain.FindController(this.AssociatedObject);
            if (controller == null)
            {
                throw new Exception("You must add a PhysicsController Behavior to the Canvas representing the main Container.");
            }

            this.Controller = controller;

            controller.Initialized += new PhysicsControllerMain.InitializedHandler(controller_Initialized);
        }

        void controller_Initialized(object source)
        {
            Controller.AddPhysicsBody(_physicsObjectMain);
        }

        [Category("Physics")]
		[Description("If true, then the element will stay in place.")]
        public bool IsStatic
        {
            get { return _physicsObjectMain.IsStatic; }
            set
            {
                _physicsObjectMain.IsStatic = value;
            }
        }

        [Category("Physics")]
		[Description("Objects collide with other objects that have different Collision Groups.")]
        public int CollisionGroup
        {
            get { return _physicsObjectMain.CollisionGroup; }
            set
            {
                _physicsObjectMain.CollisionGroup = value;
            }
        }

        [Category("Physics")]
		[Description("The ratio of velocities before and after an impact (bounciness).")]
        public double RestitutionCoefficient
        {
            get { return _physicsObjectMain.RestitutionCoefficient; }
            set
            {
                _physicsObjectMain.RestitutionCoefficient = value;
            }
        }

        [Category("Physics")]
		[Description("The amount of friction for the object.")]
        public double FrictionCoefficient
        {
            get { return _physicsObjectMain.FrictionCoefficient; }
            set
            {
                _physicsObjectMain.FrictionCoefficient = value;
            }
        }


        [Category("Physics")]
		[Description("The ease of rotation for the object. Higher is harder to rotate.")]
        public double MomentOfIntertia
        {
            get { return _physicsObjectMain.MomentOfIntertia; }
            set
            {
                _physicsObjectMain.MomentOfIntertia = value;
            }
        }

        [Category("Physics")]
		[Description("An (Optional) element to use to determine boundary outline, if you don't want to use the default shape."), CustomPropertyValueEditor(CustomPropertyValueEditor.Element)]
        public string BoundaryElement
        {
            get { return _physicsObjectMain.BoundaryElement; }
            set
            {
                _physicsObjectMain.BoundaryElement = value;
            }
        }

		[Category("Physics")]
		[Description("Specify the mass of the object (default: 1)")]
		public double Mass
		{
			get { return _physicsObjectMain.Mass; }
			set
			{
				_physicsObjectMain.Mass = value;
			}
		}
	}
}
