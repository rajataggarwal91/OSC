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

namespace Demo4
{
    public partial class Page : UserControl
    {
        ucRagDoll _ucRagDollNew;

        int _collisionGroup = 1;
        Random _rand = new Random();

        public Page()
        {
            InitializeComponent();

        }

        private void PhysicsController_TimerLoop(object source)
        {
            if (physicsController1.LastFPS.ToString() != txtFPS.Text)
            {
                txtFPS.Text = physicsController1.LastFPS.ToString() + " fps";
            }

        }

        private void btnAddDoll_Click(object sender, RoutedEventArgs e)
        {
            if (_ucRagDollNew == null)
            {
                _ucRagDollNew = new ucRagDoll();
                cnvGame.Children.Add(_ucRagDollNew);

                // we want to change the collision group for this new doll,
                // so that it will collide with other ragdolls of this type.
                _collisionGroup++;
                foreach (UIElement elem in (_ucRagDollNew.FindName("LayoutRoot") as Canvas).Children)
                {
                    if (elem is Spritehand.FarseerHelper.PhysicsJoint)
                    {
                        (elem as PhysicsJoint).CollisionGroup = _collisionGroup;
                    }
                }

                _ucRagDollNew.SetValue(Canvas.LeftProperty, Convert.ToDouble(_rand.Next(50, 950)));
                _ucRagDollNew.SetValue(Canvas.TopProperty, 0D);

                physicsController1.AddPhysicsBody(_ucRagDollNew);
                _ucRagDollNew = null;
            }
        }


    }
}
