﻿#pragma checksum "C:\Users\LPowers.REDMOND\Documents\~Team\Special Projects\Patterns Library\Patterns29-38\29SL-LoginLogoutTransition\LoginLogoutTransition\LoginLogoutControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BEC0A29CC670EBE60F5DC5DA4D968EF6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace LoginLogoutTransition {
    
    
    public partial class LoginLogoutControl : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.VisualStateGroup LoginStates;
        
        internal System.Windows.VisualState LoggedIn;
        
        internal System.Windows.VisualState LoggedOut;
        
        internal System.Windows.Controls.Grid LoggedInGrid;
        
        internal System.Windows.Shapes.Rectangle rectangle_Copy;
        
        internal System.Windows.Controls.TextBlock Username;
        
        internal System.Windows.Controls.Button LogOut_Button;
        
        internal System.Windows.Controls.Grid LoggedOutGrid;
        
        internal System.Windows.Shapes.Rectangle rectangle;
        
        internal System.Windows.Controls.TextBox UserTextBox;
        
        internal System.Windows.Controls.PasswordBox PwdTextBox;
        
        internal System.Windows.Controls.Button LogIn_Button;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/LoginLogoutTransition;component/LoginLogoutControl.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.LoginStates = ((System.Windows.VisualStateGroup)(this.FindName("LoginStates")));
            this.LoggedIn = ((System.Windows.VisualState)(this.FindName("LoggedIn")));
            this.LoggedOut = ((System.Windows.VisualState)(this.FindName("LoggedOut")));
            this.LoggedInGrid = ((System.Windows.Controls.Grid)(this.FindName("LoggedInGrid")));
            this.rectangle_Copy = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle_Copy")));
            this.Username = ((System.Windows.Controls.TextBlock)(this.FindName("Username")));
            this.LogOut_Button = ((System.Windows.Controls.Button)(this.FindName("LogOut_Button")));
            this.LoggedOutGrid = ((System.Windows.Controls.Grid)(this.FindName("LoggedOutGrid")));
            this.rectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle")));
            this.UserTextBox = ((System.Windows.Controls.TextBox)(this.FindName("UserTextBox")));
            this.PwdTextBox = ((System.Windows.Controls.PasswordBox)(this.FindName("PwdTextBox")));
            this.LogIn_Button = ((System.Windows.Controls.Button)(this.FindName("LogIn_Button")));
        }
    }
}
