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
using FarseerGames.FarseerPhysics.Dynamics.Joints;

namespace Spritehand.FarseerHelper
{

    public partial class PhysicsJoint : UserControl
    {
        private PhysicsJointMain _physicsJointMain = new PhysicsJointMain();

        public PhysicsJoint()
        {
            InitializeComponent();
            _physicsJointMain.VisualElement = this;
        }

        public PhysicsJointMain JointMain
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

        public static readonly DependencyProperty BodyOneProperty =
          DependencyProperty.Register(
          "BodyOne", typeof(string),
          typeof(PhysicsJoint), new PropertyMetadata("", new PropertyChangedCallback(BodyOneChanged))
          );

        private static void BodyOneChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PhysicsJoint joint = obj as PhysicsJoint;
            joint.JointMain.BodyOne = Convert.ToString(args.NewValue);
        }

        [Category("Physics")]
		[Description("The first Physics Body attached to the joint.")]
        public string BodyOne
        {
            get { return (string)GetValue(BodyOneProperty); }
            set
            {
                SetValue(BodyOneProperty, value);
            }
        }

        public static readonly DependencyProperty BodyTwoProperty =
          DependencyProperty.Register(
          "BodyTwo", typeof(string),
          typeof(PhysicsJoint), new PropertyMetadata("", new PropertyChangedCallback(BodyTwoChanged))
          );

        private static void BodyTwoChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PhysicsJoint joint = obj as PhysicsJoint;
            joint.JointMain.BodyTwo = Convert.ToString(args.NewValue);
        }

        [Category("Physics")]
		[Description("The second Physics Body attached to the joint.")]
        public string BodyTwo
        {
            get { return (string)GetValue(BodyTwoProperty); }
            set
            {
                SetValue(BodyTwoProperty, value);
            }
        }


        public static readonly DependencyProperty AngleSpringEnabledProperty =
          DependencyProperty.Register(
          "AngleSpringEnabled", typeof(Boolean),
          typeof(PhysicsJoint), new PropertyMetadata(false, new PropertyChangedCallback(AngleSpringEnabledChanged))
          );

        private static void AngleSpringEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PhysicsJoint joint = obj as PhysicsJoint;
            joint.JointMain.AngleSpringEnabled = Convert.ToBoolean(args.NewValue);
        }

        [Category("Physics")]
		[Description("If true, then enable an angle spring on this joint.")]
        public bool AngleSpringEnabled
        {
            get { return (bool)GetValue(AngleSpringEnabledProperty); }
            set
            {
                SetValue(AngleSpringEnabledProperty, value);
            }
        }

        public static readonly DependencyProperty AngleSpringConstantProperty =
          DependencyProperty.Register(
          "AngleSpringConstant", typeof(int),
          typeof(PhysicsJoint), new PropertyMetadata(5000, new PropertyChangedCallback(AngleSpringConstantChanged))
          );

        private static void AngleSpringConstantChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PhysicsJoint joint = obj as PhysicsJoint;
            joint.JointMain.AngleSpringConstant = Convert.ToInt32(args.NewValue);
        }

        [Category("Physics")]
		[Description("The strength of the Angle Spring, if enabled.")]
        public int AngleSpringConstant
        {
            get { return (int)GetValue(AngleSpringConstantProperty); }
            set
            {
                SetValue(AngleSpringConstantProperty, value);
            }
        }

        public static readonly DependencyProperty AngleSpringDampningConstantProperty =
          DependencyProperty.Register(
          "AngleSpringDampningConstant", typeof(int),
          typeof(PhysicsJoint), new PropertyMetadata(1000, new PropertyChangedCallback(AngleSpringDampningConstantChanged))
          );

        private static void AngleSpringDampningConstantChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PhysicsJoint joint = obj as PhysicsJoint;
            joint.JointMain.AngleSpringDampningConstant = Convert.ToInt32(args.NewValue);
        }

        [Category("Physics")]
		[Description("The Dampning of the Angle Spring, if enabled.")]
        public int AngleSpringDampningConstant
        {
            get { return (int)GetValue(AngleSpringDampningConstantProperty); }
            set
            {
                SetValue(AngleSpringDampningConstantProperty, value);
            }
        }


        public static readonly DependencyProperty CollisionGroupProperty =
          DependencyProperty.Register(
          "CollisionGroup", typeof(int),
          typeof(PhysicsJoint), new PropertyMetadata(0, new PropertyChangedCallback(CollisionGroupChanged))
          );

        private static void CollisionGroupChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PhysicsJoint joint = obj as PhysicsJoint;
            joint.JointMain.CollisionGroup = Convert.ToInt32(args.NewValue);
        }

        [Category("Physics")]
		[Description("The Collision Group to assign both bodies on the Joint.")]
        public int CollisionGroup
        {
            get { return (int)GetValue(CollisionGroupProperty); }
            set
            {
                SetValue(CollisionGroupProperty, value);
            }
        }

        private RevoluteJoint _revoluteJointObject;
        public RevoluteJoint RevoluteJointObject
        {
            get { return _revoluteJointObject; }
            set
            {
                _revoluteJointObject = value;
                _physicsJointMain.RevoluteJointObject = value;
            }
        }

        public Point GetCenter()
        {

            return _physicsJointMain.GetCenter();
        }
    }
}
