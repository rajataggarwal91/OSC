using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace RotatingMenu.Control
{
    public class MenuItem : Button
    {
        public static double Radius { get; set; } // radius of the circle
        [Category("Item Properties")]
        public double X { get; set; } // holds the current Y position
        [Category("Item Properties")]
        public double Y { get; set; } // holds the current X position

        // angle off each item
        private double _itemAngle;
        public double ItemAngle
        {
            get { return _itemAngle; }
            set { _itemAngle = value; positionElements(); }
        }

        // counter to move the items
        private int _frameCounter;
        public int FrameCounter
        {
            get { return _frameCounter; }
            set { _frameCounter = value; if (_frameCounter > 360) _frameCounter += -360; positionElements(); }
        }

        // constructor
        public MenuItem()
        {
        }

        // calculates the new position for each element and saves it in the properties X and Y
        private void positionElements()
        {
            this.X = Math.Cos(degreesToRadians(this.FrameCounter + this.ItemAngle)) * Radius;
            this.Y = Math.Sin(degreesToRadians(this.FrameCounter + this.ItemAngle)) * Radius;

            this.SetValue(Canvas.LeftProperty, this.X);
            this.SetValue(Canvas.TopProperty, this.Y);
        }

        // converts degrees to radians
        private static double degreesToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }

        // animates the item to a new top and left position in 0.5 seconds using a storyboard
        public void startAnimation(double leftPosition, double topPosition)
        {
            Duration duration = new Duration(TimeSpan.FromSeconds(0.5));
            DoubleAnimation xAnimation = new DoubleAnimation();
            DoubleAnimation yAnimation = new DoubleAnimation();
            xAnimation.Duration = duration;
            yAnimation.Duration = duration;

            Storyboard itemAnimation = new Storyboard();
            itemAnimation.Duration = duration;
            itemAnimation.Children.Add(xAnimation);
            itemAnimation.Children.Add(yAnimation);

            Storyboard.SetTarget(xAnimation, this);
            Storyboard.SetTarget(yAnimation, this);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTargetProperty(yAnimation, new PropertyPath("(Canvas.Top)"));
            xAnimation.To = leftPosition;
            yAnimation.To = topPosition;

            itemAnimation.Begin();
        }
    }
}