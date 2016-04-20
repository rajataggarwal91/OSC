using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Spritehand.FarseerHelper;
using FarseerGames.FarseerPhysics.Mathematics;

namespace PinballGame
{
	public partial class MainPage : UserControl
	{
        PhysicsControllerMain _physicsController;

        public int Score
        {
            get
            {
                return Convert.ToInt32(txtScore.Text);
            }
            set
            {
                txtScore.Text = value.ToString();
            }
        }

        public MainPage()
		{
			// Required to initialize variables
			InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
		}

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _physicsController = LayoutRoot.GetValue(PhysicsControllerMain.PhysicsControllerProperty) as PhysicsControllerMain;
            BoundaryCache.ReadBoundaryCache(_physicsController);

            _physicsController.Collision += new PhysicsControllerMain.CollisionHandler(_physicsController_Collision);
        }

        LostTheBall _lostTheBall;

        void _physicsController_Collision(string sprite1, string sprite2)
        {
            if (sprite1 == "ellBall" && sprite2.StartsWith("cnvKicker"))
            {
                Score += 10;
            }
            if (_lostTheBall == null &&sprite1 == "ellBall" && sprite2 == "rectPlatform")
            {
                _lostTheBall = new LostTheBall();
                _lostTheBall.Closed += new EventHandler(dialog_Closed);
                _lostTheBall.Show();
            }
        }

        void dialog_Closed(object sender, EventArgs e)
        {
            _lostTheBall = null;
            PhysicsSprite ball = _physicsController.PhysicsObjects["ellBall"];
            ball.BodyObject.Position = new Vector2(460, 430);
        }
	}
}