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
using System.Windows.Controls;
using System.Windows.Interactivity;
using Spritehand.FarseerHelper;
using System.ComponentModel;
using System.Collections.Generic;
using FarseerGames.FarseerPhysics.Dynamics;

namespace Spritehand.PhysicsBehaviors
{
#if !SILVERLIGHT
	[Microsoft.Windows.Design.ToolboxCategory("Physics")]
#endif
	[Category("Physics")]
	[Description("Initiates an 'explosion' animation in response to an event which deactivates a body")]
	public class PhysicsExplodeBehavior : TriggerAction<FrameworkElement>
    {
        bool _isControllerInitialized = false;
        static List<ucExplode> _particles = new List<ucExplode>();

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
        }

        public static readonly DependencyProperty DeactivateBodyProperty = DependencyProperty.Register("DeactivateBody", typeof(bool), typeof(PhysicsExplodeBehavior), new PropertyMetadata(false));

        [Category("Physics")]
		[Description("If true, then Body one will be deactivated in the simulation.")]
        public bool DeactivateBody
        {
            get { return (bool)GetValue(DeactivateBodyProperty); }
            set
            {
                SetValue(DeactivateBodyProperty, value);
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

        void controller_Initialized(object source)
        {
            _isControllerInitialized = true;

            // create the particles
            if (_particles.Count < NumParticles - 1)
            {
                for (int i = 0; i < NumParticles; i++)
                {
                    ucExplode particle = new ucExplode();
                    _particles.Add(particle);
                }
            }
        }



        private int _numParticles;
        [Category("Physics"), Description("The number of particles in the explosion.")]
        public int NumParticles
        {
            get { return _numParticles; }
            set
            {
                _numParticles = value;
            }
        }

        protected override void Invoke(object args)
        {
            if (_isControllerInitialized)
            {
                string body = this.AssociatedObject.Name;
                if (body == string.Empty)
                    throw new Exception("There is no Physics body for the given element. Be sure you added a PhysicsObjectBehavior to the element.");
                                // make sure sprite is still active
                PhysicsSprite sprite;
                if (!Controller.PhysicsObjects.TryGetValue(body, out sprite))
                    return;

                Body bodyObj = Controller.PhysicsObjects[body].BodyObject;

                double left = bodyObj.Position.X;
                double top = bodyObj.Position.Y;

                double angleStep = 360 / NumParticles;
                double currAngle = 0;

                for (int i = 0; i < NumParticles; i++)
                {
                    if (PhysicsControllerMain.ParentCanvas.Children.IndexOf(_particles[i]) < 0)
                        PhysicsControllerMain.ParentCanvas.Children.Add(_particles[i]);

                    _particles[i].Visibility = Visibility.Visible;
                    _particles[i].SetValue(Canvas.LeftProperty, left + (_particles[i].ActualWidth / 2));
                    _particles[i].SetValue(Canvas.TopProperty, top + (_particles[i].ActualHeight / 2));
                    _particles[i].rotateExplode.Angle = currAngle;
                    currAngle += angleStep;

#if SILVERLIGHT
					// TODO for WPF
                    _particles[i].sbExplode.Begin();
#endif
                }

                if (DeactivateBody)
                    Controller.DeletePhysicsObject(body);
            }
        }

    }
}