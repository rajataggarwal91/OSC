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

namespace Spritehand.PhysicsBehaviors
{
    public partial class ucExplode : UserControl
    {
        public ucExplode()
        {
            InitializeComponent();
            sbExplode.Completed += new EventHandler(sbExplode_Completed);
        }

        void sbExplode_Completed(object sender, EventArgs e)
        {
            sbExplode.Stop();
            this.Visibility = Visibility.Collapsed;
        }
    }
}
