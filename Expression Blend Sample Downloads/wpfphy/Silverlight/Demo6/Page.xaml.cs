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
using Spritehand.FarseerHelper;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Collisions;

namespace Demo6
{
    public partial class Page : UserControl
    {
        bool _keyLeftDown, _keyRightDown, _keyUpDown, _keyDownDown;
        DateTime _dtLastCameraChange;
        Body _playerBody;
        List<PhysicsSprite> _obstacles = new List<PhysicsSprite>();
        const int _numObstacles = 10;

        public Page()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            this.KeyDown += new KeyEventHandler(UserControl_KeyDown);
            this.KeyUp += new KeyEventHandler(UserControl_KeyUp);
            this.UseLayoutRounding = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // this ensures we get key stroke events.
            System.Windows.Browser.HtmlPage.Plugin.Focus();
            this.Focus();
        }

        private void PhysicsController_Initialized(object source)
        {
            // we can add custom physics objects such as springs, etc. here...
            _playerBody = physicsController1.PhysicsObjects["cnvWheelBack"].BodyObject;
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _keyLeftDown = true;
                    break;
                case Key.Right:
                    _keyRightDown = true;
                    break;
				case Key.Up:
					_keyUpDown = true;
					break;
				case Key.Down:
					_keyDownDown = true;
					break;
            }
        }
        
        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _keyLeftDown = false;
                    break;
                case Key.Right:
                    _keyRightDown = false;
                    break;
				case Key.Up:
					_keyUpDown = false;
					break;
				case Key.Down:
					_keyDownDown = false;
					break;
			}
        }

        private void physicsController1_TimerLoop(object source)
        {
            if (_keyLeftDown && _playerBody.Torque > -30000)
                _playerBody.ApplyTorque(-12000);

            if (_keyRightDown && _playerBody.Torque < 30000)
                _playerBody.ApplyTorque(12000);

            // check for a camera zoom change
            if ((DateTime.Now - _dtLastCameraChange).Milliseconds > 500)
            {
                if (_playerBody.LinearVelocity.Length() < 200)
                {
                    cameraController1.ZoomPercentage = 200;
                }
                else
                {
                    cameraController1.ZoomPercentage = 50;
                }
                _dtLastCameraChange = DateTime.Now;
            }

            if (physicsController1.LastFPS.ToString() != txtFPS.Text)
            {
                txtFPS.Text = physicsController1.LastFPS.ToString() + " fps";
            }

        }

        private void physicsController1_Collision(PhysicsController source, string sprite1, string sprite2)
        {
            // check for collisions between the foobar and the star.
            if (sprite1 == "cnvBody" && sprite2 == "star")
            {
                if (physicsController1.PhysicsObjects["star"].BodyObject.Enabled)
                {
                    // deactivate the star in the physics simulator
                    physicsController1.PhysicsObjects["star"].IsActive = false;
                    physicsController1.PhysicsObjects["star"].Update();

                    (physicsController1.PhysicsObjects["star"].uiElement as starGoal).sbDisolve.Begin();

                    sndScore.Stop();
                    sndScore.Play();
                }
            }
        }

        
    }
}
