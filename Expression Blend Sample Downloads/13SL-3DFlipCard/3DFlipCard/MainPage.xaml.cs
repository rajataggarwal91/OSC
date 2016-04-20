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

namespace _3DFlipCard
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            ReturnAnimation.Completed += new EventHandler(ReturnAnimation_Completed);
        }

        void ReturnAnimation_Completed(object sender, EventArgs e)
        {
            ExecutivesListBox.SelectedIndex = -1;
        }

        private void ExecutivesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ExecutivesListBox.SelectedIndex != -1) FlipAnimation.Begin();
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnAnimation.Begin();
        }
    }
}
