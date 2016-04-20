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
using System.Windows.Shapes;

namespace Spritehand.PhysicsBehaviors
{
#if !SILVERLIGHT && BLEND3
	[Microsoft.Windows.Design.ToolboxCategory("Physics")]
#endif
    [Category("Physics")]
    [Description("Adds fluid behavior to an element")]
    public class PhysicsFluidContainerBehavior : Behavior<Shape>
    {
        private FluidContainerMain _fluidContainerMain = new FluidContainerMain();

        protected override void OnAttached()
        {
            base.OnAttached();

            _fluidContainerMain.VisualElement = this.AssociatedObject;
            FluidContainerMain.SetFluidContainer(this.AssociatedObject, _fluidContainerMain);

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
            controller.TimerLoop += new PhysicsControllerMain.TimerLoopHandler(controller_TimerLoop);
        }

        void controller_TimerLoop(object source)
        {
            _fluidContainerMain.WaveControllerObject.Update((float)Controller.TimeStep, (float)Controller.TimeStep);
            _fluidContainerMain.Draw();
        }

        void controller_Initialized(object source)
        {
            Controller.AddFluidContainer(_fluidContainerMain);
            _fluidContainerMain.Initialize(Controller.Simulator);

        }

        [Category("Physics")]
        [Description("The minimum wave generator size.")]
        public double WaveGeneratorMin
        {
            get { return _fluidContainerMain.WaveGeneratorMin; }
            set
            {
                _fluidContainerMain.WaveGeneratorMin = value;
            }
        }

        [Category("Physics")]
        [Description("The maximum wave generator size.")]
        public double WaveGeneratorMax
        {
            get { return _fluidContainerMain.WaveGeneratorMax; }
            set
            {
                _fluidContainerMain.WaveGeneratorMax = value;
            }
        }

        [Category("Physics")]
        [Description("How quickly the wave rises and falls.")]
        public double WaveGeneratorStep
        {
            get { return _fluidContainerMain.WaveGeneratorStep; }
            set
            {
                _fluidContainerMain.WaveGeneratorStep = value;
            }
        }

        [Category("Physics")]
        [Description("How many vertices make up the surface of the wave.")]
        public int NodeCount
        {
            get { return _fluidContainerMain.NodeCount; }
            set
            {
                _fluidContainerMain.NodeCount = value;
            }
        }

        [Category("Physics")]
        [Description("How quickly the wave will dissipate.")]
        public double DampingCoefficient
        {
            get { return _fluidContainerMain.DampingCoefficient; }
            set
            {
                _fluidContainerMain.DampingCoefficient = value;
            }
        }


        [Category("Physics")]
        [Description("How fast the wave algorithm runs, in seconds.")]
        public double Frequency
        {
            get { return _fluidContainerMain.Frequency; }
            set
            {
                _fluidContainerMain.Frequency = value;
            }
        }

        [Category("Physics")]
        [Description("The Horizontal Gravity within the Container.")]
        public int GravityHorizontal
        {
            get { return _fluidContainerMain.GravityHorizontal; }
            set
            {
                _fluidContainerMain.GravityHorizontal = value;
            }
        }

        [Category("Physics")]
        [Description("The Vertical Gravity within the Container.")]
        public int GravityVertical
        {
            get { return _fluidContainerMain.GravityVertical; }
            set
            {
                _fluidContainerMain.GravityVertical = value;
            }
        }

        [Category("Physics")]
        [Description("The density of the fluid.")]
        public double FluidDensity
        {
            get { return _fluidContainerMain.FluidDensity; }
            set
            {
                _fluidContainerMain.FluidDensity = value;
            }
        }

        [Category("Physics")]
        [Description("Linear Drag Coefficient within the fluid.")]
        public double LinearDragCoefficient
        {
            get { return _fluidContainerMain.LinearDragCoefficient; }
            set
            {
                _fluidContainerMain.LinearDragCoefficient = value;
            }
        }

        [Category("Physics")]
        [Description("Rotational Drag Coefficient within the fluid.")]
        public double RotationalDragCoefficient
        {
            get { return _fluidContainerMain.RotationalDragCoefficient; }
            set
            {
                _fluidContainerMain.RotationalDragCoefficient = value;
            }
        }
    }
}
