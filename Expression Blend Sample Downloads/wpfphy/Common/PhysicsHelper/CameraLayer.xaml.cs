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

namespace Spritehand.FarseerHelper
{
	public partial class CameraLayer : UserControl
	{
		public CameraLayer()
		{
			InitializeComponent();
		}


		bool _userControlLoaded;
		Canvas _parentCanvas;

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			bool isDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);

			if (_userControlLoaded || isDesignMode)
				return;

			this.Visibility = Visibility.Collapsed;

			_parentCanvas = this.Parent as Canvas;
			if (_parentCanvas == null)
				throw new Exception("The Camera Layer Controller must be the child of a Canvas element.");

			SetupCameraController();
		
			_userControlLoaded = true;

		}

		public static readonly DependencyProperty CanvasLayerProperty =
			DependencyProperty.Register(
			"CanvasLayer", typeof(string),
			typeof(CameraLayer), new PropertyMetadata("")
			);

		[Category("Physics")]
		[Description("The name of the Canvas to treat as the Parallax Layer.")]
		public string CanvasLayer
		{
			get { return (string)GetValue(CanvasLayerProperty); }
			set
			{
				SetValue(CanvasLayerProperty, value);
			}
		}


		public static readonly DependencyProperty ZOrderProperty =
			DependencyProperty.Register(
			"ZOrder", typeof(int),
			typeof(CameraLayer), new PropertyMetadata(-4)
			);

		[Category("Physics")]
		[Description("This is the Z index for the layer.")]
		public int ZOrder
		{
			get { return (int)GetValue(ZOrderProperty); }
			set { SetValue(ZOrderProperty, value); }
		}



		public static readonly DependencyProperty CameraControllerNameProperty =
	  DependencyProperty.Register(
	  "CameraControllerName", typeof(string),
	  typeof(CameraLayer), new PropertyMetadata("")
	  );

		[Category("Physics")]
		[Description("The name of the Camera Controller that the layer belongs to.")]
		public string CameraControllerName
		{
			get { return (string)GetValue(CameraControllerNameProperty); }
			set
			{
				SetValue(CameraControllerNameProperty, value);
			}
		}

		private void SetupCameraController()
		{
			CameraController controller = _parentCanvas.FindName(CameraControllerName) as CameraController;

			if (controller == null)
				throw new Exception("Could not find a Camera Controller named " + CameraControllerName + ". Make sure the camera controller exists in the same Canvas as the CameraLayer control.");

			Canvas targetCanvas = _parentCanvas.FindName(CanvasLayer) as Canvas;

			if (targetCanvas == null)
				throw new Exception("Could not find a Canvas named " + CanvasLayer + " to use as a Parallax Layer.");


			ParallaxLayer layer = new ParallaxLayer(targetCanvas);
			layer.Z = ZOrder;

			controller.ParallaxLayers.Add(layer);

		}

	}
}
