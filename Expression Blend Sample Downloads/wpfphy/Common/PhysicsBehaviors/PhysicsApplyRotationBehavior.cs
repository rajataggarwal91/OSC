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
	[Description("Allows an element to be rotated")]
	public class PhysicsApplyRotationBehavior : TriggerAction<FrameworkElement>
    {
        bool _isControllerInitialized = false;

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

        private double _angleValue;
        [Category("Physics")]
		[Description("The amount of rotation to apply to the physics element.")]
        public double RotateValue
        {
            get { return _angleValue; }
            set
            {
                _angleValue = value;
            }
        }

        protected override void Invoke(object args)
        {
            if (_isControllerInitialized)
            {
                string body = this.AssociatedObject.Name;
                if (body == string.Empty)
                    throw new Exception("There is no Physics body for the given element. Be sure you added a PhysicsObjectBehavior to the element.");

                PhysicsSprite sprite;
                if (!Controller.PhysicsObjects.TryGetValue(body, out sprite))
                    return;

                FarseerGames.FarseerPhysics.Dynamics.Body bodyObj = sprite.BodyObject;
                bodyObj.AngularVelocity = 0f;
                bodyObj.ClearTorque();
                
                double newRotation = Controller.RadiansToDegrees(bodyObj.Rotation) + ((float)_angleValue);
                bodyObj.Rotation = (float)(Controller.DegreesToRadians(newRotation));
            }
        }

    }
}
