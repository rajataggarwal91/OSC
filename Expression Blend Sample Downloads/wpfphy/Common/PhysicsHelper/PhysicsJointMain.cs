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

    public partial class PhysicsJointMain
    {
        public PhysicsJointMain()
        {
        }

        public static readonly DependencyProperty PhysicsJointProperty = DependencyProperty.RegisterAttached("PhysicsJoint", typeof(PhysicsJointMain), typeof(FrameworkElement), new PropertyMetadata(null));

        public static PhysicsJointMain GetPhysicsJoint(DependencyObject target)
        {
            return (PhysicsJointMain)target.GetValue(PhysicsJointMain.PhysicsJointProperty);
        }

        public static void SetPhysicsJoint(DependencyObject target, PhysicsJointMain value)
        {
            target.SetValue(PhysicsJointMain.PhysicsJointProperty, value);
        }


        private FrameworkElement _visualElement;
        public FrameworkElement VisualElement
        {
            get { return _visualElement; }
            set
            {
                _visualElement = value;
            }
        }

        private string _bodyOne = string.Empty;
        public string BodyOne
        {
            get { return _bodyOne; }
            set { _bodyOne = value; }
        }

        private string _bodyTwo = string.Empty;
        public string BodyTwo
        {
            get { return _bodyTwo; }
            set { _bodyTwo = value; }
        }

        private bool _angleSpringEnabled = false;
        public bool AngleSpringEnabled
        {
            get { return _angleSpringEnabled; }
            set { _angleSpringEnabled = value; }
        }

        private double _angleLowerLimit = -1;
        public double AngleLowerLimit
        {
            get { return _angleLowerLimit; }
            set { _angleLowerLimit = value; }
        }

        private double _angleUpperLimit = -1;
        public double AngleUpperLimit
        {
            get { return _angleUpperLimit; }
            set { _angleUpperLimit = value; }
        }

        private int _angleSpringConstant = 5000;
        public int AngleSpringConstant
        {
            get { return _angleSpringConstant; }
            set { _angleSpringConstant = value; }
        }

        private int _angleSpringDampningConstant = 1000;
        public int AngleSpringDampningConstant
        {
            get { return _angleSpringDampningConstant; }
            set { _angleSpringDampningConstant = value; }
        }

        private int _collisionGroup = 0;
        public int CollisionGroup
        {
            get { return _collisionGroup; }
            set { _collisionGroup = value; }
        }

        private RevoluteJoint _revoluteJointObject;
        public RevoluteJoint RevoluteJointObject
        {
            get { return _revoluteJointObject; }
            set { _revoluteJointObject = value; }
        }

        public Point GetCenter()
        {
            // we may need to figure in offset
            double offSetLeft = 0;
            double offSetTop = 0;
            FrameworkElement parent = (VisualElement.Parent as FrameworkElement).Parent as FrameworkElement;
            while (parent != null && parent != PhysicsControllerMain.ParentCanvas)
            {
#if !SILVERLIGHT

                if (parent is Window)
                    break;
#endif
				double incLeft = Convert.ToDouble(parent.GetValue(Canvas.LeftProperty));
				double incTop = Convert.ToDouble(parent.GetValue(Canvas.TopProperty));

                // In WPF the parent top/left can be NaN which causes problems elsewhere
                offSetLeft += double.IsNaN(incLeft) ? 0 : incLeft;
				offSetTop += double.IsNaN(incTop) ? 0 : incTop;

                parent = parent.Parent as FrameworkElement;
            }

            Point pt = new Point();

            pt.X = (double)VisualElement.GetValue(Canvas.LeftProperty) + (VisualElement.Width / 2) + offSetLeft;
            pt.Y = (double)VisualElement.GetValue(Canvas.TopProperty) + (VisualElement.Height/2) + offSetTop;

            return pt;
        }
    }
}
