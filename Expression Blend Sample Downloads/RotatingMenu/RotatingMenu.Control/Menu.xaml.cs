using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Menu : UserControl
    {
        private double Angle { get; set; } // contains the angle which separates the items

        // collection which contains all the items in the menu, this is a DP 
        [Category("Menu Properties")]
        public ObservableCollection<MenuItem> MenuItems
        {
            get { return (ObservableCollection<MenuItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }
        public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register("MenuItems", typeof(ObservableCollection<MenuItem>), typeof(Menu), new PropertyMetadata(new ObservableCollection<MenuItem>()));

        // contains the radius of the circle, this is a DP
        [Category("Menu Properties")]
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(Menu), new PropertyMetadata(1.0, new PropertyChangedCallback(OnRadiusChanged)));

        public Menu()
        {
            InitializeComponent();
            // listen if the collection of items changes
            this.MenuItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(MenuItems_CollectionChanged);
        }

        // executes when an item is added or removed
        void MenuItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            timerBoard.Stop();
            mainCanvas.Children.Clear();
            // set the angle and radius
            this.Angle = 360 / this.MenuItems.Count;
            MenuItem.Radius = this.Radius;

            createMenuItems();
            timerBoard.Begin();
        }

        // set the new radius when it is changed
        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuItem.Radius = Convert.ToDouble(e.NewValue);
        }

        // create instances of MenuItem
        private void createMenuItems()
        {
            int i = 0;
            foreach (MenuItem mi in this.MenuItems)
            {
                mi.ItemAngle = this.Angle * i; // set the angle for each item
                MenuItem.Radius = this.Radius; // set the radius (the same for all the items)
                mi.Click += new RoutedEventHandler(linePosition); // listen to the click event
                mainCanvas.Children.Add(mi);
                i++;
            }
        }

        // sets the items in a circle
        void cirlePosition(object sender, RoutedEventArgs e)
        {
            foreach (MenuItem mi in this.MenuItems)
            {
                mi.startAnimation(mi.X, mi.Y);
                mi.Click -= new RoutedEventHandler(cirlePosition);
                mi.Click += new RoutedEventHandler(linePosition);
            }
            timerBoard.Begin();
        }

        // sets the items on one line
        void linePosition(object sender, RoutedEventArgs e)
        {
            timerBoard.Stop();
            double xPos = -calculateHorizontalWidth() / 2;
            foreach (MenuItem mi in this.MenuItems)
            {
                mi.Click -= new RoutedEventHandler(linePosition);
                mi.Click += new RoutedEventHandler(cirlePosition);

                mi.startAnimation(xPos, 0);
                xPos += mi.ActualWidth + mi.Margin.Left + mi.Margin.Right;
            }
        }

        // calculates the total width of all the items
        private double calculateHorizontalWidth()
        {
            if (this.MenuItems.Count == 0)
                return 0;

            double width = 0;
            foreach (MenuItem mi in this.MenuItems)
            {
                width += mi.ActualWidth + mi.Margin.Left + mi.Margin.Right;
            }

            return width;
        }

        // start the storyboard every time it is completed to rotate the items
        private void timerBoard_Completed(object sender, EventArgs e)
        {
            foreach (MenuItem mi in this.MenuItems)
            {
                mi.FrameCounter++;
            }
            timerBoard.Begin();
        }
    }
}
