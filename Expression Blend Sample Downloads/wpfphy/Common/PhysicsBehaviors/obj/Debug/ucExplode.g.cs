﻿#pragma checksum "C:\dev\Silverlight\PhysicsHelper\Common\PhysicsBehaviors\ucExplode.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "38EB82F4D6F5D415A6E8984108F62E44"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30128.1
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


namespace Spritehand.PhysicsBehaviors {
    
    
    public partial class ucExplode : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard sbExplode;
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Media.RotateTransform rotateExplode;
        
        internal System.Windows.Shapes.Path path;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Spritehand.PhysicsBehaviors;component/ucExplode.xaml", System.UriKind.Relative));
            this.sbExplode = ((System.Windows.Media.Animation.Storyboard)(this.FindName("sbExplode")));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.rotateExplode = ((System.Windows.Media.RotateTransform)(this.FindName("rotateExplode")));
            this.path = ((System.Windows.Shapes.Path)(this.FindName("path")));
        }
    }
}

