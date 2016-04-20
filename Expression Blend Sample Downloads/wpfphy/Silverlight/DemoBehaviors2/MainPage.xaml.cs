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

namespace DemoBehaviors2
{
    public partial class MainPage : UserControl
    {
        int _collisionGroup = 1;
        Random _rand = new Random();
        PhysicsControllerMain _physicsController;

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _physicsController = cnvGame.GetValue(PhysicsControllerMain.PhysicsControllerProperty) as PhysicsControllerMain;

        }

        private void btnAddDoll_Click(object sender, RoutedEventArgs e)
        {
            ucRagDoll _ucRagDollNew = new ucRagDoll();
            cnvGame.Children.Add(_ucRagDollNew);

            // we want to change the collision group for this new doll,
            // so that it will collide with other ragdolls of this type.
            _collisionGroup++;

            foreach (FrameworkElement element in _ucRagDollNew.LayoutRoot.Children)
            {
                PhysicsJointMain joint = element.GetValue(PhysicsJointMain.PhysicsJointProperty) as PhysicsJointMain;
                if (joint != null)
                    joint.CollisionGroup = _collisionGroup;
            }

            _ucRagDollNew.SetValue(Canvas.LeftProperty, Convert.ToDouble(_rand.Next(50, 950)));
            _ucRagDollNew.SetValue(Canvas.TopProperty, 0D);

            _physicsController.AddPhysicsBodyForCanvasWithBehaviors(_ucRagDollNew.LayoutRoot);
        }
    }
}
