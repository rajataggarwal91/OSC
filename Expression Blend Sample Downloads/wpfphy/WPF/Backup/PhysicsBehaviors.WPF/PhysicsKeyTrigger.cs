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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using Spritehand.FarseerHelper;

namespace Spritehand.PhysicsBehaviors
{
    /// <summary>
    /// Trigger for specific key events such as 'space key' or 'Control-A'.
    /// </summary>
#if !SILVERLIGHT && BLEND3
	[Microsoft.Windows.Design.ToolboxCategory("Physics")]
#endif
    [Category("Physics")]
    [Description("Trigger for specific key events such as 'space key' or 'Control-A'")]
    public class PhysicsKeyTrigger : TriggerBase<FrameworkElement>
    {
        public enum TriggerOn
        {
            WhileKeyPressed,
            OnKeyDown,
            OnKeyUp
        }

        // private FrameworkElement root;
        public event EventHandler KeyDown;
        public event EventHandler KeyUp;

        /// <summary>
        /// The key press which this is to be triggered on.
        /// </summary>
        public Key Key
        {
            get { return (Key)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
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

        /// <summary>
        /// The key press which this is to be triggered on.
        /// </summary>
        public TriggerOn TriggeredOn
        {
            get { return (TriggerOn)GetValue(TriggeredOnProperty); }
            set { SetValue(TriggeredOnProperty, value); }
        }

        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(PhysicsKeyTrigger), new PropertyMetadata(Key.None));
        public static readonly DependencyProperty TriggeredOnProperty = DependencyProperty.Register("TriggeredOn", typeof(TriggerOn), typeof(PhysicsKeyTrigger), new PropertyMetadata(TriggerOn.WhileKeyPressed));


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(PhysicsKeyTrigger), new PropertyMetadata(ModifierKeys.None));

        /// <summary>
        /// Any modifier keys which must accompany the key press
        /// </summary>
        public ModifierKeys Modifiers
        {
            get { return (ModifierKeys)this.GetValue(PhysicsKeyTrigger.ModifiersProperty); }
            set { this.SetValue(PhysicsKeyTrigger.ModifiersProperty, value); }
        }

        /// <summary>
        /// Attach the appropriate events.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            PhysicsControllerMain controller = PhysicsControllerMain.FindController(this.AssociatedObject);
            if (controller == null)
            {
                throw new Exception("You must add a PhysicsController Behavior to the Canvas representing the main Container.");
            }

            this.Controller = controller;
            controller.TimerLoop += new PhysicsControllerMain.TimerLoopHandler(controller_TimerLoop);
            controller.Initialized += new PhysicsControllerMain.InitializedHandler(controller_Initialized);
        }

        void controller_Initialized(object source)
        {
#if SILVERLIGHT
            Application.Current.RootVisual.KeyDown += new KeyEventHandler(ParentCanvas_KeyDown);
            Application.Current.RootVisual.KeyUp += new KeyEventHandler(ParentCanvas_KeyUp);

            UserControl uc = Application.Current.RootVisual as UserControl;

            // NOTE: Following removed for Windows Phone Support.
            //if (System.Windows.Browser.HtmlPage.Plugin != null)
            //    System.Windows.Browser.HtmlPage.Plugin.Focus();
#else
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(ParentCanvas_KeyDown);
            Application.Current.MainWindow.KeyUp += new KeyEventHandler(ParentCanvas_KeyUp);

            Window uc = Application.Current.MainWindow as Window;
#endif
            // this ensures we get key stroke events. 
            if (uc != null)
                uc.Focus();

        }

        bool _isKeyDown = false;
        void ParentCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == this.Key && Keyboard.Modifiers == this.Modifiers)
            {
                if (!_isKeyDown)
                {
                    _isKeyDown = true;

                    if (TriggeredOn == TriggerOn.OnKeyDown)
                        this.InvokeActions(null);
                }

                if (KeyDown != null)
                    KeyDown(this, new EventArgs());

            }

        }

        void ParentCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == this.Key && Keyboard.Modifiers == this.Modifiers)
            {
                if (_isKeyDown)
                {
                    _isKeyDown = false;

                    if (TriggeredOn == TriggerOn.OnKeyUp)
                        this.InvokeActions(null);
                }

                if (KeyUp != null)
                    KeyUp(this, new EventArgs());
            }

        }

        void controller_TimerLoop(object source)
        {
            if (_isKeyDown && TriggeredOn == TriggerOn.WhileKeyPressed)
                this.InvokeActions(null);
        }

        /// <summary>
        /// Remove the appropriate events.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
#if SILVERLIGHT
            Application.Current.RootVisual.KeyDown -= ParentCanvas_KeyDown;
            Application.Current.RootVisual.KeyUp -= ParentCanvas_KeyUp;
#else
            Application.Current.MainWindow.KeyDown -= ParentCanvas_KeyDown;
            Application.Current.MainWindow.KeyUp -= ParentCanvas_KeyUp;
#endif
        }

    }
}
