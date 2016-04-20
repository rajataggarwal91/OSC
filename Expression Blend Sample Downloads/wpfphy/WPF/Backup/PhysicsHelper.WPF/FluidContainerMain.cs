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
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics.Springs;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Dynamics.Joints;
using System.Text;
using FarseerGames.FarseerPhysics.Controllers;


namespace Spritehand.FarseerHelper
{


    public class FluidContainerMain
    {
        public static readonly DependencyProperty FluidControllerProperty = DependencyProperty.RegisterAttached("FluidController", typeof(FluidContainerMain), typeof(FrameworkElement), new PropertyMetadata(null));

        private Polygon _wavePolygon;
        private PointCollection _points;
        private Point _bottomRightPoint;
        private Point _bottomLeftPoint;

        public WaveController WaveControllerObject { get; set; }

        public static FluidContainerMain GetFluidController(DependencyObject target)
        {
            return (FluidContainerMain)target.GetValue(FluidContainerMain.FluidControllerProperty);
        }

        public static void SetFluidContainer(DependencyObject target, FluidContainerMain value)
        {
            target.SetValue(FluidContainerMain.FluidControllerProperty, value);
        }

        public FluidContainerMain()
        {
            WaveControllerObject = new WaveController();
        }

        private Shape _visualElement;
        public Shape VisualElement
        {
            get { return _visualElement; }
            set
            {
                _visualElement = value;
            }
        }

        private double _waveGeneratorMin = 5;
        public double WaveGeneratorMin
        {
            get { return _waveGeneratorMin; }
            set
            {
                _waveGeneratorMin = value;
                if (WaveControllerObject != null)
                    WaveControllerObject.WaveGeneratorMin = (float)value;
            }
        }

        private double _waveGeneratorMax = 25;
        public double WaveGeneratorMax
        {
            get { return _waveGeneratorMax; }
            set { _waveGeneratorMax = value;
            if (WaveControllerObject != null)
                WaveControllerObject.WaveGeneratorMax = (float)value;
            }
        }

        private double _waveGeneratorStep = 3;
        public double WaveGeneratorStep
        {
            get { return _waveGeneratorStep; }
            set { _waveGeneratorStep = value; }
        }

        private double _fluidDensity = 0.00025;
        public double FluidDensity
        {
            get { return _fluidDensity; }
            set { _fluidDensity = value; }
        }

        private double _linearDragCoefficient = 0.02;
        public double LinearDragCoefficient
        {
            get { return _linearDragCoefficient; }
            set { _linearDragCoefficient = value; }
        }

        private double _rotationalDragCoefficient = 0.01;
        public double RotationalDragCoefficient
        {
            get { return _rotationalDragCoefficient; }
            set { _rotationalDragCoefficient = value; }
        }

        private int _gravityHorizontal = 0;
        public int GravityHorizontal
        {
            get { return _gravityHorizontal; }
            set { _gravityHorizontal = value; }
        }

        private int _gravityVertical = 500;
        public int GravityVertical
        {
            get { return _gravityVertical; }
            set { _gravityVertical = value; }
        }

        private int _nodeCount = 20;
        public int NodeCount
        {
            get { return _nodeCount; }
            set { _nodeCount = value; }
        }

        private double _dampingCoefficient = .95;
        public double DampingCoefficient
        {
            get { return _dampingCoefficient; }
            set { _dampingCoefficient = value; }
        }

        private double _frequency = .16;
        public double Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        public void Initialize(PhysicsSimulator physicsSimulator)
        {
            int count = WaveControllerObject.NodeCount;
            _points = new PointCollection();
            for (int i = 0; i < count; i++)
            {
                _points.Add(new Point((WaveControllerObject.XPosition[i]), (WaveControllerObject.CurrentWave[i])));
            }

            _bottomRightPoint = new Point((WaveControllerObject.XPosition[count - 1]), (WaveControllerObject.Position.Y) + (WaveControllerObject.Height));
            _bottomLeftPoint = new Point((WaveControllerObject.XPosition[0]), (WaveControllerObject.Position.Y) + (WaveControllerObject.Height));
            _points.Add(_bottomRightPoint);
            _points.Add(_bottomLeftPoint);

            _wavePolygon = new Polygon();
            _wavePolygon.Points = _points;
            _wavePolygon.IsHitTestVisible = false;
            _wavePolygon.Opacity = VisualElement.Opacity;

            int insertAt = PhysicsControllerMain.ParentCanvas.Children.IndexOf(VisualElement);
            string name = VisualElement.GetValue(Canvas.NameProperty).ToString();
            PhysicsControllerMain.ParentCanvas.Children.Add(_wavePolygon);
            //PhysicsControllerMain.ParentCanvas.Children.Insert(insertAt, _wavePolygon);
            PhysicsControllerMain.ParentCanvas.Children.Remove(VisualElement);
            if (name.Length > 0)
                _wavePolygon.SetValue(Canvas.NameProperty, name);

            _wavePolygon.Fill = VisualElement.Fill;
            _wavePolygon.Stroke = VisualElement.Stroke;
            _wavePolygon.StrokeThickness = VisualElement.StrokeThickness;
            _wavePolygon.SetValue(Canvas.ZIndexProperty, Convert.ToInt32(VisualElement.GetValue(Canvas.ZIndexProperty)));

        }


        public void Draw()
        {
            _points.Clear();
            for (int i = 0; i < WaveControllerObject.NodeCount; i++)
            {
                Point p = new Point((WaveControllerObject.XPosition[i]), (WaveControllerObject.CurrentWave[i]) + (WaveControllerObject.Position.Y));
                _points.Add(p);
            }

            _points.Add(_bottomRightPoint);
            _points.Add(_bottomLeftPoint);
            _wavePolygon.Points = _points;
        }



    }
}
