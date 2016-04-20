using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace SplitButton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyObject d = VisualTreeHelper.GetChild(b, 0);
            Button dropDownButton = LogicalTreeHelper.FindLogicalNode(d, "DropDownButton") as Button;
            dropDownButton.Click += new RoutedEventHandler(dropDownButton_Click);
        }

        void dropDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (b.ContextMenu != null)
            {
                b.ContextMenu.PlacementTarget = b;
                b.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                b.ContextMenu.IsOpen = true;
            }
            e.Handled = true;   
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("'" + (b.Content.ToString() + "'" + " Clicked"));
        }

        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("'" + ((MenuItem)e.Source).Header.ToString() + "'" + " Clicked");
        }
    }
}