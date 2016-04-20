using System;
using System.Windows;
using System.Windows.Controls;
using Spritehand.FarseerHelper;

namespace DemoBehaviors4
{
    public partial class ucAstronaut : UserControl
    {
        PhysicsControllerMain _physicsController;
        public ucAstronaut()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ucAstronaut_Loaded);
        }

        void ucAstronaut_Loaded(object sender, RoutedEventArgs e)
        {
            _physicsController = LayoutRoot.GetValue(PhysicsControllerMain.PhysicsControllerProperty) as PhysicsControllerMain;

        }


        private void PhysicsKeyTrigger_KeyDown(object sender, EventArgs e)
        {
            ucAstronaut ucAstronaut1 = PhysicsControllerMain.ParentCanvas.FindName("ucAstronaut1") as ucAstronaut;
            ucAstronaut1.pathThrust.Visibility = Visibility.Visible;
            ucAstronaut1.sbThrust.Begin();
        }

        private void PhysicsKeyTrigger_KeyUp(object sender, EventArgs e)
        {
            ucAstronaut ucAstronaut1 = PhysicsControllerMain.ParentCanvas.FindName("ucAstronaut1") as ucAstronaut;
            ucAstronaut1.pathThrust.Visibility = Visibility.Collapsed;
            ucAstronaut1.sbThrust.Stop();
        }
    }
}
