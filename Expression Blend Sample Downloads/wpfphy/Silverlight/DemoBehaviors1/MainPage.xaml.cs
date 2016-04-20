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
using System.Windows.Media.Imaging;
using System.Windows.Interactivity;
using Spritehand.PhysicsBehaviors;

namespace DemoBehaviors1
{
    public partial class MainPage : UserControl
    {
        PhysicsControllerMain _physicsController;

        public MainPage()
        {
            InitializeComponent();

            // note that we can use the Physics Controller to manipulate objects
            // in the simulation...
            _physicsController = LayoutRoot.GetValue(PhysicsControllerMain.PhysicsControllerProperty) as PhysicsControllerMain;
            _physicsController.Initialized += new PhysicsControllerMain.InitializedHandler(_physicsController_Initialized);
            _physicsController.TimerLoop += new PhysicsControllerMain.TimerLoopHandler(_physicsController_TimerLoop);
        }

        void _physicsController_Initialized(object source)
        {
            // here you can do initialization stuff
        }

        void _physicsController_TimerLoop(object source)
        {
            // here you can handle logic per "frame"

            // NOTE that you can get a reference to the Fluid Container like so:
            FluidContainerMain fluidContainerMain = rectWater.GetValue(FluidContainerMain.FluidControllerProperty) as FluidContainerMain;

            // NOTE that you can get references to "sprite" objects in the simulation like so:
            PhysicsSprite astronaut = _physicsController.PhysicsObjects["spaceSuit"];
            float mass = astronaut.BodyObject.Mass;
            int collisionGroup = astronaut.GeometryObject.CollisionGroup;

        }

     

    }
}
