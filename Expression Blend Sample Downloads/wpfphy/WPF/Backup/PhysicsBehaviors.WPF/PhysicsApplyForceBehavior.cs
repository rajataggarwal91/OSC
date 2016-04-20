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
using FarseerGames.FarseerPhysics.Mathematics;

namespace Spritehand.PhysicsBehaviors
{
#if !SILVERLIGHT && BLEND3
	[Microsoft.Windows.Design.ToolboxCategory("Physics")]
#endif
	[Category("Physics")]
	[Description("Simulates applying a force to an element")]
	public class PhysicsApplyForceBehavior : TriggerAction<FrameworkElement>
    {
        bool _isControllerInitialized = false;

        public event EventHandler AppliedForce; 

        protected override void OnAttached()
        {
            base.OnAttached();

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
            _isControllerInitialized = true;
        }

        [Category("Physics")]
		[Description("The amount of force to apply in the X direction.")]
        public double ForceX { get; set; }

        [Category("Physics")]
		[Description("The amount of force to apply in the Y direction.")]
        public double ForceY { get; set; }

        [Category("Physics")]
		[Description("If true, then force is applied in direction of the current rotation.")]
        public bool ApplyForceEqualToRotation { get; set; }

        protected override void Invoke(object args)
        {
            if (_isControllerInitialized)
            {
                string body = this.AssociatedObject.Name;
                PhysicsSprite sprite;
                if (!Controller.PhysicsObjects.TryGetValue(body, out sprite))
                    return;
                Vector2 force = new Vector2((float)ForceX, (float)ForceY);

                if (ApplyForceEqualToRotation)
                {
                    force = new Vector2((float)Math.Cos(sprite.GeometryObject.Rotation), (float)Math.Sin(sprite.GeometryObject.Rotation)) * (float)ForceX ;
                }

                Controller.PhysicsObjects[body].BodyObject.ApplyForce(force);
            }

            if (AppliedForce != null)
                AppliedForce(this, new EventArgs());
        }

    }
}
