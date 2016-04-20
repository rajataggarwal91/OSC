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
using FarseerGames.FarseerPhysics.Mathematics;

namespace Spritehand.FarseerHelper
{
    public class PhysicsObjectMain
    {
		private FrameworkElement _visualElement = null;
		private string _boundaryElement = null;

        public static readonly DependencyProperty PhysicsObjectProperty = 
			DependencyProperty.RegisterAttached("PhysicsObject", typeof(PhysicsObjectMain), typeof(Canvas), new PropertyMetadata(null));

        public static PhysicsObjectMain GetPhysicsObject(DependencyObject target)
        {
            return (PhysicsObjectMain)target.GetValue(PhysicsObjectMain.PhysicsObjectProperty);
        }

        public static void SetPhysicsObject(DependencyObject target, PhysicsObjectMain value)
        {
            target.SetValue(PhysicsObjectMain.PhysicsObjectProperty, value);
        }

		public FrameworkElement VisualElement
        {
            get { return _visualElement; }
            set { _visualElement = value; }
        }

        public string BoundaryElement
        {
            get { return _boundaryElement; }
            set { _boundaryElement = value; }
        }
        
        private int _collisionGroup = 0;
        public int CollisionGroup
        {
            get { return _collisionGroup; }
            set { _collisionGroup = value; }
        }

        private bool _isStatic = false;
        public bool IsStatic
        {
            get { return _isStatic; }
            set { _isStatic = value; }
        }

        private double _restitutionCoefficient;
        public double RestitutionCoefficient
        {
            get { return _restitutionCoefficient; }
            set { _restitutionCoefficient = value; }
        }

        private double _frictionCoefficient;
        public double FrictionCoefficient
        {
            get { return _frictionCoefficient; }
            set { _frictionCoefficient = value; }
        }

        private double _momentOfIntertia;
        public double MomentOfIntertia
        {
            get { return _momentOfIntertia; }
            set { _momentOfIntertia = value; }
        }

        private double _mass;
        public double Mass
        {
            get { return _mass; }
            set { _mass = value; }
        }

	}
}
