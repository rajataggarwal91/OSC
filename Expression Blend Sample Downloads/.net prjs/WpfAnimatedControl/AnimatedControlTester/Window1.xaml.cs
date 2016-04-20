using System;
using System.Windows;
using System.Drawing;
using System.Diagnostics;
using WpfAnimatedControl;
using System.Windows.Documents;
using System.Collections.Generic;

namespace AnimatedControlTester
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        List<AnimatedImage> imgs = new List<AnimatedImage>();

        public Window1()
        {
            InitializeComponent();
            rich.Document = new System.Windows.Documents.FlowDocument();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            foreach (AnimatedImage i in imgs)
                i.StopAnimate();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            foreach (AnimatedImage i in imgs)
                i.StartAnimate();
        }
        
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                Paragraph p = new Paragraph();
                AnimatedImage aimg = new AnimatedImage();
                aimg.Stretch = System.Windows.Media.Stretch.None;
                System.Drawing.Image img;
                img = AnimatedControlTester.Resources.Resources.wrong;
                aimg.LoadSmile((System.Drawing.Bitmap)img);
                p.Inlines.Add(aimg);
                rich.Document.Blocks.Add(p);
                imgs.Add(aimg);
            }
        }

        private void aimg_AnimatedBitmapChanged(object sender, RoutedPropertyChangedEventArgs<Bitmap> e)
        {
            Debug.WriteLine("AnimatedBitmapChanged event occured, add extra code here if necessary.");
        }

        
    }
}
