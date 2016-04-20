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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Spritehand.FarseerHelper;
using System.Collections.Generic;
using System.ComponentModel;

namespace Spritehand.PhysicsBehaviors
{
#if !SILVERLIGHT && BLEND3
	[Microsoft.Windows.Design.ToolboxCategory("Physics")]
#endif
	[Category("Physics")]
	[Description("Provides a reference environment for all other behaviors such as gravity")]
	public class PhysicsControllerBehavior : Behavior<Canvas>
    {
        private PhysicsControllerMain _physicsControllerMain = new PhysicsControllerMain();
 
        public PhysicsControllerMain PhysicsMain
        {
            get { return _physicsControllerMain; }
            set { _physicsControllerMain = value; }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);

            _physicsControllerMain.AutoAddCanvasObjects = false;

            PhysicsControllerMain.SetPhysicsController(this.AssociatedObject, _physicsControllerMain);

        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas parentCanvas = this.AssociatedObject;
            PhysicsControllerMain.ParentCanvas = parentCanvas;
            _physicsControllerMain.Initialize();
        }


        [Category("Physics")]
		[Description("The number of times Farseer internal physics calculates are made. Increase for better accuracy at the cost of performance.")]
        public double TimeStep
        {
            get { return _physicsControllerMain.TimeStep; }
            set
            {
                _physicsControllerMain.TimeStep = value;
            }
        }

        [Category("Physics")]
		[Description("If true, then objects can be manipulated with the mouse.")]
        public bool MousePickEnabled
        {
            get { return _physicsControllerMain.MousePickEnabled; }
            set
            {
                _physicsControllerMain.MousePickEnabled = value;
            }
        }

        [Category("Physics")]
		[Description("Gravity setting in Horizontal Direction.")]
        public int GravityHorizontal
        {
            get { return _physicsControllerMain.GravityHorizontal; }
            set
            {
                _physicsControllerMain.GravityHorizontal = value;
            }
        }

        [Category("Physics")]
		[Description("Gravity setting in Vertical Direction.")]
        public int GravityVertical
        {
            get { return _physicsControllerMain.GravityVertical; }
            set
            {
                _physicsControllerMain.GravityVertical = value;
            }
        }

        [Category("Physics")]
		[Description("The number of times Farseer internal physics calculates are made. Increase for better accuracy at the cost of performance.")]
        public int Iterations
        {
            get { return _physicsControllerMain.Iterations; }
            set
            {
                _physicsControllerMain.Iterations = value;
            }
        }

        [Category("Physics")]
		[Description("If you need to decrease jitter, try a lower value.")]
        public double BiasFactor
        {
            get { return _physicsControllerMain.BiasFactor; }
            set
            {
                _physicsControllerMain.BiasFactor = value;
            }
        }

        [Category("Physics")]
		[Description("Use this to adjust the initial friction value of all geometries. Note that you can still modify individual geometries.")]
        public double FrictionDefault
        {
            get { return _physicsControllerMain.FrictionDefault; }
            set
            {
                _physicsControllerMain.FrictionDefault = value;
            }
        }

        [Category("Physics")]
		[Description("Use this property to define the outline points of an element to speed application startup.")]
        public List<PointObject> PointListCollection
        {
            get { return _physicsControllerMain.PointListCollection; }
            set
            {
                _physicsControllerMain.PointListCollection = value;
            }
        }

        [Category("Physics")]
		[Description("If true, then collision and body boundaries are drawn.")]
        public bool DebugMode
        {
            get { return _physicsControllerMain.DebugMode; }
            set
            {
                _physicsControllerMain.DebugMode = value;
            }
        }

		[Category("Physics")]
		[Description("Specify the amount of drag applied to the linear direction of movement.  A larger value means more drag.")]
		public double LinearDragCoefficient
		{
			get { return _physicsControllerMain.LinearDragCoefficient; }
			set
			{
				_physicsControllerMain.LinearDragCoefficient = value;
			}
		}

		[Category("Physics")]
		[Description("Specify the amount of drag to be applied to the rotation rate.")]
		public double RotationalDragCoefficient
		{
			get { return _physicsControllerMain.RotationalDragCoefficient; }
			set
			{
				_physicsControllerMain.RotationalDragCoefficient = value;
			}
		}
    }
}
