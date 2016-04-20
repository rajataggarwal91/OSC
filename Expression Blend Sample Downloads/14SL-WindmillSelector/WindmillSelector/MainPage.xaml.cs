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

namespace WindmillSelector
{
    public partial class MainPage : UserControl
    {

        public MainPage()
        {
            InitializeComponent();

            //start the animation
            RotationStoryboard.Begin();
            
            //set the details view to the first item in the collection
            DetailsGrid.DataContext = ((Executives)this.Resources["Executives"])[0];
        }

        private void WindmillListBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //pause the animation
            RotationStoryboard.Pause();

            //set the details view to the selected item
            DetailsGrid.DataContext = ((FrameworkElement)sender).DataContext;
        }

        private void WindmillListBoxItem_MouseLeave(object sender, MouseEventArgs e)
        {
            //resume the animation
            RotationStoryboard.Resume();
        }

        
    }
}
