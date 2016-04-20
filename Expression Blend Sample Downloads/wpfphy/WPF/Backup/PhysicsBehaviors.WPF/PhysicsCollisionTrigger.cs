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
using System.Windows.Input;
using System.Windows.Interactivity;
using Spritehand.FarseerHelper;
using System.ComponentModel;

namespace Spritehand.PhysicsBehaviors
{
    /// <summary>
    /// Trigger for specific key events such as 'space key' or 'Control-A'.
	/// </summary>
#if !SILVERLIGHT && BLEND3
	[Microsoft.Windows.Design.ToolboxCategory("Physics")]
#endif
	[Category("Physics")]
	[Description("Raises an event when two elements collide")]
	public class PhysicsCollisionTrigger : TriggerBase<FrameworkElement>
    {

        // private FrameworkElement root;

        public static readonly DependencyProperty BodyOneProperty = DependencyProperty.Register("BodyOne", typeof(string), typeof(PhysicsCollisionTrigger), new PropertyMetadata(string.Empty));

        [Category("Physics"), Description("The first Physics Body involved in a collision."), CustomPropertyValueEditor(CustomPropertyValueEditor.Element)]
        public string BodyOne
        {
            get { return (string)GetValue(BodyOneProperty); }
            set
            {
                SetValue(BodyOneProperty, value);
            }
        }

        public static readonly DependencyProperty BodyTwoProperty = DependencyProperty.Register("BodyTwo", typeof(string), typeof(PhysicsCollisionTrigger), new PropertyMetadata(string.Empty));
        [Category("Physics")]
		[Description("The second Physics Body involved in a collision."), CustomPropertyValueEditor(CustomPropertyValueEditor.Element)]
        public string BodyTwo
        {
            get { return (string)GetValue(BodyTwoProperty); }
            set
            {
                SetValue(BodyTwoProperty, value);
            }
        }

        public static readonly DependencyProperty MatchLikeBodyNamesProperty = DependencyProperty.Register("MatchLikeBodyNames", typeof(bool), typeof(PhysicsCollisionTrigger), new PropertyMetadata(false));

        [Category("Physics")]
		[Description("If true, then Body Names that are similar will trigger a collision (useful for copies of controls).")]
        public bool MatchLikeBodyNames
        {
            get { return (bool)GetValue(MatchLikeBodyNamesProperty); }
            set
            {
                SetValue(MatchLikeBodyNamesProperty, value);
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


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(PhysicsCollisionTrigger), new PropertyMetadata(ModifierKeys.None));

        /// <summary>
        /// Any modifier keys which must accompany the key press
        /// </summary>
        public ModifierKeys Modifiers
        {
            get { return (ModifierKeys)this.GetValue(PhysicsCollisionTrigger.ModifiersProperty); }
            set { this.SetValue(PhysicsCollisionTrigger.ModifiersProperty, value); }
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
            controller.Initialized += new PhysicsControllerMain.InitializedHandler(controller_Initialized);
            controller.Collision += new PhysicsControllerMain.CollisionHandler(controller_Collision);
        }

        void controller_Initialized(object source)
        {
        }

        void controller_Collision(string sprite1, string sprite2)
        {
            bool bCollision = false;
            if (MatchLikeBodyNames)
            {
                if (sprite1.StartsWith(BodyOne) && sprite2.StartsWith(BodyTwo))
                {
                    bCollision = true;
                }
            }
            else
                if (sprite1 == BodyOne && sprite2 == BodyTwo)
                {
                    bCollision = true;
                }

            if (bCollision)
            {
                this.InvokeActions(null);
            }
        }

        /// <summary>
        /// Remove the appropriate events.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

    }
}
