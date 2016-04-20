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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interactivity;
using Spritehand.FarseerHelper;
using FarseerGames.FarseerPhysics.Dynamics.Joints;
using System.ComponentModel;

namespace Spritehand.PhysicsBehaviors
{
    public class PhysicsJointBehavior : Behavior<FrameworkElement>
    {
        private PhysicsJointMain _physicsJointMain = new PhysicsJointMain();

        protected override void OnAttached()
        {
            base.OnAttached();

            _physicsJointMain.VisualElement = this.AssociatedObject;

            this.AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);

            PhysicsJointMain.SetPhysicsJoint(this.AssociatedObject, _physicsJointMain);

        }

        public PhysicsJointMain PhysicsJointMain
        {
            get
            {
                return _physicsJointMain;
            }
            set
            {
                _physicsJointMain = value;
            }
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

        [Category("Physics"), Description("The first Physics Body attached to the joint."), CustomPropertyValueEditor(CustomPropertyValueEditor.Element)]
        public string BodyOne
        {
            get { return _physicsJointMain.BodyOne; }
            set
            {
                _physicsJointMain.BodyOne = value;
            }
        }

        [Category("Physics"), Description("The second Physics Body attached to the joint."), CustomPropertyValueEditor(CustomPropertyValueEditor.Element)]
        public string BodyTwo
        {
            get { return _physicsJointMain.BodyTwo; }
            set
            {
                _physicsJointMain.BodyTwo = value;
            }
        }


        [Category("Physics"), Description("If true, then enable an angle spring on this joint.")]
        public bool AngleSpringEnabled
        {
            get { return _physicsJointMain.AngleSpringEnabled; }
            set
            {
                _physicsJointMain.AngleSpringEnabled = value;
            }
        }

        [Category("Physics"), Description("The strength of the Angle Spring, if enabled.")]
        public int AngleSpringConstant
        {
            get { return _physicsJointMain.AngleSpringConstant; }
            set
            {
                _physicsJointMain.AngleSpringConstant = value;
            }
        }

        [Category("Physics"), Description("The Dampning of the Angle Spring, if enabled.")]
        public int AngleSpringDampningConstant
        {
            get { return _physicsJointMain.AngleSpringDampningConstant; }
            set
            {
                _physicsJointMain.AngleSpringDampningConstant = value;
            }
        }

        [Category("Physics"), Description("(Optional) Specifies the minimum angle the joint can rotate.")]
        public double AngleLimitLower
        {
            get { return _physicsJointMain.AngleLowerLimit; }
            set
            {
                _physicsJointMain.AngleLowerLimit = value;
            }
        }

        [Category("Physics"), Description("(Optional) Specifies the maximum angle the joint can rotate.")]
        public double AngleLimitUpper
        {
            get { return _physicsJointMain.AngleUpperLimit; }
            set
            {
                _physicsJointMain.AngleUpperLimit = value;
            }
        }


        [Category("Physics"), Description("The Collision Group to assign both bodies on the Joint.")]
        public int CollisionGroup
        {
            get { return _physicsJointMain.CollisionGroup; }
            set
            {
                _physicsJointMain.CollisionGroup = value;
            }
        }


        void controller_Initialized(object source)
        {
            string name = this.AssociatedObject.Name;
            Controller.AddJoint(_physicsJointMain);
        }

    }
}
