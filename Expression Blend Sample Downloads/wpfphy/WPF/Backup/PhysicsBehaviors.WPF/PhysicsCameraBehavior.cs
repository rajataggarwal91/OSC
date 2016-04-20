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
using System.ComponentModel;
using System.Windows;
using Spritehand.FarseerHelper;
using System.Windows.Interactivity;

namespace Spritehand.PhysicsBehaviors
{
#if !SILVERLIGHT && BLEND3
	[Microsoft.Windows.Design.ToolboxCategory("Physics")]
#endif
	[Category("Physics")]
	[Description("Applying this behavior to an element ensures it is always center stage")]
	public class PhysicsCameraBehavior : Behavior<FrameworkElement>
    {
        private PhysicsControllerMain _physicsController;
        private CameraControllerMain _cameraControllerMain = new CameraControllerMain();


        protected override void OnAttached()
        {
            base.OnAttached();


            this.AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);

        }

        void physicsController_Initialized(object source)
        {
            CameraControllerMain.SetCameraController(this.AssociatedObject, _cameraControllerMain);
            _cameraControllerMain.Initialize(PhysicsControllerMain.ParentCanvas, _physicsController);
            _cameraControllerMain.Target = this.AssociatedObject.Name;

        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            // also check for a physics controller behavior
            _physicsController = PhysicsControllerMain.FindController(this.AssociatedObject);
			if (_physicsController == null) return;

			_physicsController.Initialized += new PhysicsControllerMain.InitializedHandler(physicsController_Initialized);
        
           
        }
    }
}
