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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace WPFCustomControls.Presentations
{

    [TemplatePart(Name = "PART_Content", Type = typeof(ContentControl))]
    [TemplatePart(Name = "PART_ShatteringContent", Type = typeof(ContentControl))]
    public sealed partial class ShatteringControl : ContentControl
    {
        static ShatteringControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShatteringControl), new FrameworkPropertyMetadata(typeof(ShatteringControl)));

            StartedEvent = EventManager.RegisterRoutedEvent("Started", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ShatteringControl));
            FinishedEvent = EventManager.RegisterRoutedEvent("Finished", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ShatteringControl));

        }

        public ShatteringControl()
        {

        }


        private RelayCommand _StartShatterCommand;
        public RelayCommand StartShatterCommand
        {
            get
            {
                if (_StartShatterCommand == null)
                {
                    _StartShatterCommand = new RelayCommand(
                        param => this.ShatterMethod(),
                        param => this.CanShatter
                        );
                }
                return _StartShatterCommand;
            }
        }

        public void ShatterMethod()
        {
            BuildPieces();
            //MessageBox.Show("relay command fired");
        }
        bool CanShatter
        {
            get
            {
                return true;
            }
        }



        ContentControl _shatteringContent;
        ContentControl _content;

        public static readonly RoutedEvent StartedEvent;
        public event RoutedEventHandler Started
        {
            add
            {
                this.AddHandler(StartedEvent, value);
            }
            remove
            {
                this.RemoveHandler(StartedEvent, value);
            }
        }

        public static readonly RoutedEvent FinishedEvent;// = EventManager.RegisterRoutedEvent("Finished", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ShatteringControl));

        public event RoutedEventHandler Finished
        {
            add
            {
                this.AddHandler(FinishedEvent, value);
            }
            remove
            {
                this.RemoveHandler(FinishedEvent, value);
            }
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _shatteringContent = base.Template.FindName("PART_ShatteringContent", this) as ContentControl;
            _content = base.Template.FindName("PART_Content", this) as ContentControl;

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.Loaded -= new RoutedEventHandler(ShatteringControl_Loaded);
            this.Loaded += new RoutedEventHandler(ShatteringControl_Loaded);
        }

        void ShatteringControl_Loaded(object sender, RoutedEventArgs e)
        {
            //  BuildPieces();


        }



        delegate void ReplaceContentDelegate(ShatteringControl sc);
        private void ReplaceContentMethod(ShatteringControl sc)
        {

            DispatcherTimer t = new DispatcherTimer() { };
            t.Interval = new TimeSpan(0, 0, 2);
            t.Tick += delegate
            {
                sc.Content = null;
                //  sc.Content = sc.ShatteringContent;
                t.Stop();
            };
            t.Start();
        }





        public ShartteringEffect EffectToUse
        {
            get { return (ShartteringEffect)GetValue(EffectToUseProperty); }
            set { SetValue(EffectToUseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EffectToUse.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EffectToUseProperty =
            DependencyProperty.Register("EffectToUse", typeof(ShartteringEffect), typeof(ShatteringControl), new UIPropertyMetadata((ShartteringEffect)ShartteringEffect.SlideOff));



        public bool IsShattering
        {
            get { return (bool)GetValue(IsShatteringProperty); }
            set { SetValue(IsShatteringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShattering.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShatteringProperty =
            DependencyProperty.Register("IsShattering", typeof(bool), typeof(ShatteringControl), new UIPropertyMetadata((bool)false,
              delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
              {

              }));


        public int BuildingPeiceIndex
        {
            get { return (int)GetValue(BuildingPeiceIndexProperty); }
            set { SetValue(BuildingPeiceIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BuildingPeiceIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BuildingPeiceIndexProperty =
            DependencyProperty.Register("BuildingPeiceIndex", typeof(int), typeof(ShatteringControl), new UIPropertyMetadata(0,
                delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    ShatteringControl sc = (ShatteringControl)sender;
                    //sc.Dispatcher.Invoke(new ReplaceContentDelegate(sc.ReplaceContentMethod), sc);                  

                }));


        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            // Content = new Button() { Background = Brushes.Blue, Content="New" };
            // base.ArrangeOverride(arrangeBounds);
            //BuildPieces(arrangeBounds);


            return base.ArrangeOverride(arrangeBounds);
            // return arrangeBounds;

        }

        Canvas _canvasPeices = new Canvas();
        double PeiceSizeHeight = .1;
        double PeiceSizeWidth = .1;
        TimeSpan AnimationLength = new TimeSpan(0, 0, 5);
        Storyboard sb = new Storyboard();

        public void DetermineAction(ShatteringControl sc)
        {
            switch (sc.EffectToUse)
            {
                case ShartteringEffect.SlideOn:
                    MovePeicesToPostion();
                    break;
                case ShartteringEffect.SlideOff:
                    MovePeicesFromPosition();
                    break;
                //case ShartteringEffect.FadeOn:
                //    break;
                //case ShartteringEffect.FadeOff:
                //    break;
                default:
                    break;
            }

        }

        public void DetermineContentOnDone()
        {

        }

        public void BuildPieces()
        {
            _content.Visibility = Visibility.Hidden;
            _shatteringContent.Visibility = Visibility.Visible;
           

            double PeiceWidth = (_content.ActualWidth * PeiceSizeWidth);
            double PeiceHeight = (_content.ActualHeight * PeiceSizeHeight);

            double remainingPeiceWidth = Math.IEEERemainder(_content.ActualWidth, PeiceWidth);
            double remainingPeiceHeight = Math.IEEERemainder(_content.ActualHeight, PeiceHeight);

            int peicesAcross;
            int peicesDown;

            if (_content.ActualWidth != 0 & _content.ActualHeight != 0)
            {
                peicesAcross = Convert.ToInt32(Math.Floor(_content.ActualWidth / PeiceWidth));
                peicesDown = Convert.ToInt32(Math.Floor(_content.ActualHeight / PeiceHeight));
            }
            else { return; }

            for (int i = 0; i < peicesAcross; i++)
            {
                VisualBrush vb = new VisualBrush(_content.Content as FrameworkElement);
                vb.Stretch = Stretch.None;
                vb.ViewboxUnits = BrushMappingMode.Absolute;
                vb.Viewbox = new Rect(PeiceWidth * i, 0, PeiceWidth, PeiceHeight);
                Grid cont = new Grid() { Background = vb, Height = PeiceHeight, Width = PeiceWidth };
                cont.Name = "horz" + i.ToString();
                //if (i != 0)
                //{
                cont.SetValue(Canvas.TopProperty, 0.0);
                cont.SetValue(Canvas.LeftProperty, ((i) * PeiceWidth));
                //}

                _canvasPeices.Children.Add(cont);

                for (int ii = 1; ii < peicesDown; ii++)
                {
                    VisualBrush vb2 = new VisualBrush(_content.Content as FrameworkElement);
                    vb2.Stretch = Stretch.None;
                    vb2.ViewboxUnits = BrushMappingMode.Absolute;
                    vb2.Viewbox = new Rect(PeiceWidth * i, PeiceHeight * ii, PeiceWidth, PeiceHeight);
                    Grid cont2 = new Grid() { Background = vb2, Height = PeiceHeight, Width = PeiceWidth };
                    cont2.Name = "vert" + ii.ToString();
                    //if (ii != 0)
                    //{
                    cont2.SetValue(Canvas.LeftProperty, ((i) * PeiceWidth));
                    cont2.SetValue(Canvas.TopProperty, ((ii) * PeiceHeight));
                    //}

                    _canvasPeices.Children.Add(cont2);
                }
            }
            _shatteringContent.Content = _canvasPeices;

            DetermineAction(this);


        }

        public void MovePeicesToPostion()
        {
          //  _canvasPeices.Children.Clear();
            sb.Completed -= new EventHandler(sb_Completed);
            sb.Completed += new EventHandler(sb_Completed);
            double LengthPerPeice = AnimationLength.TotalMilliseconds / _canvasPeices.Children.Count;
            double currentPeiceTime = 0;

            foreach (Grid item in _canvasPeices.Children)
            {
                DoubleAnimationUsingKeyFrames frame = new DoubleAnimationUsingKeyFrames();
                frame.SetValue(Storyboard.TargetProperty, item);
                frame.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Canvas.LeftProperty));
                DoubleAnimationUsingKeyFrames frameY = new DoubleAnimationUsingKeyFrames();
                frameY.SetValue(Storyboard.TargetProperty, item);
                frameY.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Canvas.TopProperty));


                SplineDoubleKeyFrame startFrame = new SplineDoubleKeyFrame();
                startFrame.KeyTime = TimeSpan.FromMilliseconds(currentPeiceTime);
                startFrame.Value = -item.Width;
                SplineDoubleKeyFrame startFrameY = new SplineDoubleKeyFrame();
                startFrameY.KeyTime = TimeSpan.FromMilliseconds(currentPeiceTime);
                startFrameY.Value = -item.Height;

                currentPeiceTime = currentPeiceTime + LengthPerPeice;

                SplineDoubleKeyFrame endFrame = new SplineDoubleKeyFrame();
                endFrame.KeyTime = TimeSpan.FromMilliseconds(currentPeiceTime);
                endFrame.Value = (double)item.GetValue(Canvas.LeftProperty);
                SplineDoubleKeyFrame endFrameY = new SplineDoubleKeyFrame();
                endFrameY.KeyTime = TimeSpan.FromMilliseconds(currentPeiceTime);
                endFrameY.Value = (double)item.GetValue(Canvas.TopProperty);


                frame.KeyFrames.Add(startFrame);
                frame.KeyFrames.Add(endFrame);
                sb.Children.Add(frame);

                frameY.KeyFrames.Add(startFrameY);
                frameY.KeyFrames.Add(endFrameY);
                sb.Children.Add(frameY);

            }

            foreach (Grid item in _canvasPeices.Children)
            {
                item.SetValue(Canvas.LeftProperty, -item.Width);
                item.SetValue(Canvas.TopProperty, -item.Height);

            }

            _canvasPeices.BeginStoryboard(sb);
        }

        public void MovePeicesFromPosition()
        {
            sb.Completed -= new EventHandler(sb_Completed);
            sb.Completed += new EventHandler(sb_Completed);
            double LengthPerPeice = AnimationLength.TotalMilliseconds / _canvasPeices.Children.Count;
            double currentPeiceTime = 0;


            for (int i = _canvasPeices.Children.Count; i > 1; i--)
            {

                Grid item = _canvasPeices.Children[i-1] as Grid;

                DoubleAnimationUsingKeyFrames frame = new DoubleAnimationUsingKeyFrames();
                frame.SetValue(Storyboard.TargetProperty, item);
                frame.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Canvas.LeftProperty));
                DoubleAnimationUsingKeyFrames frameY = new DoubleAnimationUsingKeyFrames();
                frameY.SetValue(Storyboard.TargetProperty, item);
                frameY.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(Canvas.TopProperty));


                SplineDoubleKeyFrame startFrame = new SplineDoubleKeyFrame();
                startFrame.KeyTime = TimeSpan.FromMilliseconds(currentPeiceTime);
                startFrame.Value = (double)item.GetValue(Canvas.LeftProperty);// -item.Width;
                SplineDoubleKeyFrame startFrameY = new SplineDoubleKeyFrame();
                startFrameY.KeyTime = TimeSpan.FromMilliseconds(currentPeiceTime);
                startFrameY.Value = (double)item.GetValue(Canvas.TopProperty);// -item.Height;

                currentPeiceTime = currentPeiceTime + LengthPerPeice;

                SplineDoubleKeyFrame endFrame = new SplineDoubleKeyFrame();
                endFrame.KeyTime = TimeSpan.FromMilliseconds(currentPeiceTime);
                endFrame.Value = -item.Width;// (double)item.GetValue(Canvas.LeftProperty);
                SplineDoubleKeyFrame endFrameY = new SplineDoubleKeyFrame();
                endFrameY.KeyTime = TimeSpan.FromMilliseconds(currentPeiceTime);
                endFrameY.Value = -item.Height;// (double)item.GetValue(Canvas.TopProperty);


                frame.KeyFrames.Add(startFrame);
                frame.KeyFrames.Add(endFrame);
                sb.Children.Add(frame);

                frameY.KeyFrames.Add(startFrameY);
                frameY.KeyFrames.Add(endFrameY);
                sb.Children.Add(frameY);

            }
            //foreach (Grid item in _canvasPeices.Children)
            //{
            //    item.SetValue(Canvas.LeftProperty, -item.Width);
            //    item.SetValue(Canvas.TopProperty, -item.Height);

            //}

            _canvasPeices.BeginStoryboard(sb);
        }

        public void FadePeicesOn()
        {

        }

        public void FadePeicesOff()
        {

        }

        void sb_Completed(object sender, EventArgs e)
        {
            _content.Visibility = Visibility.Visible;
            _shatteringContent.Visibility = Visibility.Collapsed;
            _canvasPeices.Children.Clear();

            RoutedEventArgs args = new RoutedEventArgs(FinishedEvent, this);
            if (args != null)
            {
                RaiseEvent(args);
            }
        }


        //protected override Size MeasureOverride(Size constraint)
        //{

        //    Size resultSize = new Size(0, 0);

        //    resultSize.Width = double.IsPositiveInfinity(constraint.Width) ?
        //        resultSize.Width : constraint.Width;

        //    resultSize.Height = double.IsPositiveInfinity(constraint.Height) ?
        //        resultSize.Height : constraint.Height;

        //    return resultSize;
        //  //  return base.MeasureOverride(constraint);
        //}
    }

    public enum ShartteringEffect
    {
        SlideOn, SlideOff //, FadeOn, FadeOff
    }

    public enum CompletionAction
    {

    }
}
