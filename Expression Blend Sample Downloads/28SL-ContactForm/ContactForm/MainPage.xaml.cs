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

namespace ContactForm
{
    public partial class MainPage : UserControl
    {
        private bool _flag;

        public MainPage()
        {
            InitializeComponent();
        }
        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_flag) FormAppear.Begin();
        }

        private void SubmitButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FormDisappear.Begin();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            FormDisappear.Begin();
        }

        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            MyWatermarkedTextBox textBox = (MyWatermarkedTextBox)sender;
            textBox.ShowWatermark = Visibility.Collapsed;
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            MyWatermarkedTextBox textBox = (MyWatermarkedTextBox)sender;
            textBox.ShowWatermark = textBox.Text == string.Empty ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Form_MouseLeave(object sender, MouseEventArgs e)
        {
            _flag = true;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            _flag = false;
        }
    }
}