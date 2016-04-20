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
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Collisions;
using Spritehand.FarseerHelper;

namespace Demo5
{
    public partial class Page : UserControl
    {
        List<PhysicsSprite> _obstacles = new List<PhysicsSprite>();
        const int _numObstacles = 30;
        Body _body;
        DateTime _dtLastCameraChange;
        Random _rand = new Random();


        public Page()
        {
            InitializeComponent();
        }

        private void physicsController1_Initialized(object source)
        {
            _body = physicsController1.PhysicsObjects["body"].BodyObject;

            // create a bunch of obstacles
            for (int i = 0; i < _numObstacles; i++)
            {
                ucObstacle obstacle = new ucObstacle();
                LayoutRoot.Children.Add(obstacle);
                physicsController1.AddPhysicsBody(obstacle);

                string newObstacleName = "ball_" + (i + 1).ToString();
                PhysicsSprite newObstacle = physicsController1.PhysicsObjects[newObstacleName];

                _obstacles.Add(newObstacle);


            }
        }

        private void physicsController1_TimerLoop(object source)
        {
            // let's adjust the position of obstacles so that we always have some ahead of the dummy
            double ragDollY = Convert.ToDouble(_body.Position.Y);
            foreach (PhysicsSprite obstacle in _obstacles)
            {
                double obstacleY = Convert.ToDouble(obstacle.BodyObject.Position.Y);

                if (obstacleY < ragDollY - 400)
                {

                    PositionObstacle(obstacle);
                }
            }

            // check for a camera zoom change
            if ((DateTime.Now - _dtLastCameraChange).Seconds > 1)
            {
                if (_body.LinearVelocity.Y < 100)
                {
                    cameraController1.ZoomPercentage = 200;
                }
                else
                {
                    cameraController1.ZoomPercentage = 100;
                }

            }
        }

        private void PositionObstacle(PhysicsSprite obstacle)
        {
            float newX, newY;


            int minX = Convert.ToInt32(_body.Position.X - 350);
            int maxX = Convert.ToInt32(_body.Position.X + 400);
            int minY = Convert.ToInt32(_body.Position.Y + 200);
            int maxY = Convert.ToInt32(_body.Position.Y + 2000);

            newX = _rand.Next(minX, maxX);
            newY = _rand.Next(minY, maxY);

            Vector2 newPos = new Vector2(newX, newY);

            // make sure we're not too close to another obstacle
            bool bTooClose = false;
            foreach (PhysicsSprite obstacleCheck in _obstacles)
            {
                if (obstacleCheck != obstacle && DistanceBetweenPoints(newPos, obstacleCheck.BodyObject.Position) < 200)
                {
                    bTooClose = true;
                    break;
                }
            }

            if (!bTooClose) 
                obstacle.BodyObject.Position = newPos;




        }

        public static double DistanceBetweenPoints(Vector2 pt1, Vector2 pt2)
        {

            double result = Math.Sqrt(Math.Pow((pt2.X - pt1.X), 2) + Math.Pow((pt2.Y - pt1.Y), 2));

            return result;
        }
    }
}
