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
using System.Windows.Shapes;

namespace TestClient
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class ShatteringControlWindow : Window
    {
        public ShatteringControlWindow()
        {
            InitializeComponent();
        }

        private void shatterControl_Finished(object sender, RoutedEventArgs e)
        {
           // shatterControl.BuildPieces();
        }
    }
}
