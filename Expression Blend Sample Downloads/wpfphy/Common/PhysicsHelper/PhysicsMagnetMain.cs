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
using System.Windows;
using System.Windows.Controls;
using FarseerGames.FarseerPhysics.Dynamics.Joints;

namespace Spritehand.FarseerHelper
{
	public class PhysicsMagnetMain
	{

		public PhysicsMagnetMain()
		{ }

		public static readonly DependencyProperty PhysicsMagnetProperty =
			DependencyProperty.RegisterAttached("PhysicsMagnet", typeof(PhysicsMagnetMain), typeof(Canvas), new PropertyMetadata(null));

		public static PhysicsMagnetMain GetPhysicsMagnet(DependencyObject target)
		{
			return (PhysicsMagnetMain)target.GetValue(PhysicsMagnetMain.PhysicsMagnetProperty);
		}

		public static void SetPhysicsMagnet(DependencyObject target, PhysicsMagnetMain value)
		{
			target.SetValue(PhysicsMagnetMain.PhysicsMagnetProperty, value);
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

		private string _bodyName = string.Empty;
		public string BodyName
		{
			get { return _bodyName; }
			set { _bodyName = value; }
		}

		private double _magnetism = 0;
		public double Magnetism
		{
			get { return _magnetism; }
			set { _magnetism = value; }
		}

		private double _fallOff = 0;
		public double FallOff
		{
			get { return _fallOff; }
			set { _fallOff = value; }
		}

		private double _maxDistance = 0;
		public double MaxDistance
		{
			get { return _maxDistance; }
			set { _maxDistance = value; }
		}

	}
}
