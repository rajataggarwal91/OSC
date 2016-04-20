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
using System.ComponentModel;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Dynamics;

namespace Spritehand.PhysicsBehaviors
{
    public class PhysicsSoundBehavior : TriggerAction<FrameworkElement>
    {
        bool _isControllerInitialized = false;
        SoundMain _soundMain;
        bool _playOnInitialize = false;

        public PhysicsSoundBehavior()
        {
            Buffers = 3;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

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

        }

        void controller_Initialized(object source)
        {
            _isControllerInitialized = true;

#if SILVERLIGHT_PHONE
            _soundMain = new SoundMain(PhysicsControllerMain.ParentCanvas, SourceWave, Buffers, LoopTime);
#else
            _soundMain = new SoundMain(PhysicsControllerMain.ParentCanvas, Source, Buffers, LoopTime);
#endif

            if (_playOnInitialize)
                _soundMain.Play();
        }

        [Category("Physics"), Description("The number of sounds buffers to use. Set this to the Max # of simultaneous times this sound is played.")]
        public int Buffers { get; set; }

        [Category("Physics"), Description("The path to the sound file.")]
        public string Source { get; set; }

        [Category("Physics"), Description("The path to an alternate WAV file for Windows Phone.")]
        public string SourceWave { get; set; }

        [Category("Physics"), Description("If you want to loop the sound, then set this to the # of milliseconds where a looped sound should repeat.")]
        public double LoopTime { get; set; }

        protected override void Invoke(object args)
        {
            if (_isControllerInitialized)
            {
                _soundMain.Play();
            }
            else
                _playOnInitialize = true;

        }

    }
}
