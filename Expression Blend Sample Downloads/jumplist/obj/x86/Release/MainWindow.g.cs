﻿#pragma checksum "..\..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E647BA8D2D40C72DC59547474BC36FA5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.21006.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Expression.Interactivity.Core;
using Microsoft.Windows.Themes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SingleInstance {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 603 "..\..\..\MainWindow.xaml"
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 608 "..\..\..\MainWindow.xaml"
        internal System.Windows.VisualStateGroup ScreenStates;
        
        #line default
        #line hidden
        
        
        #line 612 "..\..\..\MainWindow.xaml"
        internal System.Windows.VisualState HomeActive;
        
        #line default
        #line hidden
        
        
        #line 622 "..\..\..\MainWindow.xaml"
        internal System.Windows.VisualState ShoppingActive;
        
        #line default
        #line hidden
        
        
        #line 630 "..\..\..\MainWindow.xaml"
        internal System.Windows.VisualStateGroup NavStates;
        
        #line default
        #line hidden
        
        
        #line 631 "..\..\..\MainWindow.xaml"
        internal System.Windows.VisualState NavActive;
        
        #line default
        #line hidden
        
        
        #line 651 "..\..\..\MainWindow.xaml"
        internal System.Windows.VisualState NavInactive;
        
        #line default
        #line hidden
        
        
        #line 673 "..\..\..\MainWindow.xaml"
        internal System.Windows.Controls.Grid HomeScreen;
        
        #line default
        #line hidden
        
        
        #line 684 "..\..\..\MainWindow.xaml"
        internal System.Windows.Controls.Grid ShoppingScreen;
        
        #line default
        #line hidden
        
        
        #line 695 "..\..\..\MainWindow.xaml"
        internal System.Windows.Controls.Canvas Navigation;
        
        #line default
        #line hidden
        
        
        #line 704 "..\..\..\MainWindow.xaml"
        internal System.Windows.Controls.Viewbox Pallette;
        
        #line default
        #line hidden
        
        
        #line 723 "..\..\..\MainWindow.xaml"
        internal System.Windows.Controls.Button HomeButton;
        
        #line default
        #line hidden
        
        
        #line 730 "..\..\..\MainWindow.xaml"
        internal System.Windows.Controls.Button ShoppingButton;
        
        #line default
        #line hidden
        
        
        #line 737 "..\..\..\MainWindow.xaml"
        internal System.Windows.Controls.Button GalleryButton;
        
        #line default
        #line hidden
        
        
        #line 738 "..\..\..\MainWindow.xaml"
        internal System.Windows.Controls.Button ExitButton;
        
        #line default
        #line hidden
        
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
            System.Uri resourceLocater = new System.Uri("/SingleInstance;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\MainWindow.xaml"
            ((SingleInstance.MainWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.ScreenStates = ((System.Windows.VisualStateGroup)(target));
            return;
            case 4:
            this.HomeActive = ((System.Windows.VisualState)(target));
            return;
            case 5:
            this.ShoppingActive = ((System.Windows.VisualState)(target));
            return;
            case 6:
            this.NavStates = ((System.Windows.VisualStateGroup)(target));
            return;
            case 7:
            this.NavActive = ((System.Windows.VisualState)(target));
            return;
            case 8:
            this.NavInactive = ((System.Windows.VisualState)(target));
            return;
            case 9:
            this.HomeScreen = ((System.Windows.Controls.Grid)(target));
            return;
            case 10:
            this.ShoppingScreen = ((System.Windows.Controls.Grid)(target));
            return;
            case 11:
            this.Navigation = ((System.Windows.Controls.Canvas)(target));
            return;
            case 12:
            this.Pallette = ((System.Windows.Controls.Viewbox)(target));
            
            #line 704 "..\..\..\MainWindow.xaml"
            this.Pallette.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Pallette_MouseDown);
            
            #line default
            #line hidden
            return;
            case 13:
            this.HomeButton = ((System.Windows.Controls.Button)(target));
            return;
            case 14:
            this.ShoppingButton = ((System.Windows.Controls.Button)(target));
            return;
            case 15:
            this.GalleryButton = ((System.Windows.Controls.Button)(target));
            return;
            case 16:
            this.ExitButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
