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

namespace LoginLogoutTransition
{
    public partial class LoginLogoutControl : UserControl
    {
        public LoginLogoutControl()
        {
            InitializeComponent();
            LogIn_Button.Click += new RoutedEventHandler(LogIn_Button_Click);
            LogOut_Button.Click += new RoutedEventHandler(LogOut_Button_Click);
        }

        void LogOut_Button_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "LoggedOut", true);
            PwdTextBox.Password = string.Empty;
        }

        void LogIn_Button_Click(object sender, RoutedEventArgs e)
        {
            if (UserTextBox.Text != string.Empty && PwdTextBox.Password != string.Empty)
            {
                VisualStateManager.GoToState(this, "LoggedIn", true);
            }
        }
    }
}
