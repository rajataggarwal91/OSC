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

namespace Demo3
{
    public partial class Page : UserControl
    {
        bool _keyLeftDown, _keyRightDown;

        public Page()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            this.KeyDown += new KeyEventHandler(UserControl_KeyDown);
            this.KeyUp += new KeyEventHandler(UserControl_KeyUp);
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
            }
        }

        private void physicsController1_TimerLoop(object source)
        {
            if (_keyLeftDown)
                physicsController1.PhysicsObjects["head1"].BodyObject.ApplyTorque(-6000);

            if (_keyRightDown)
                physicsController1.PhysicsObjects["head1"].BodyObject.ApplyTorque(6000);

			
        }

        private void physicsController1_Collision(PhysicsController source, string sprite1, string sprite2)
        {
            // check for collisions between the foobar and the star.
            if (sprite1 == "head1" && sprite2 == "star")
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
