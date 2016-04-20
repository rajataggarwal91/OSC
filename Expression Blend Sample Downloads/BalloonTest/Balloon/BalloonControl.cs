using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Balloon
{
    namespace BalloonControl
    {
        public enum BalloonBoxTypeEnum
        {
            ClickToShow,
            MouseOverToShow,
            Manual
        }
        [TemplatePart(Name = BalloonBox.HeaderElement, Type = typeof(ContentPresenter))]
        [TemplatePart(Name = BalloonBox.ContentElement, Type = typeof(ContentPresenter))]
        [TemplatePart(Name = BalloonBox.PopUpElement, Type = typeof(Popup))]
        [TemplatePart(Name = BalloonBox.TextBlockElement, Type = typeof(TextBlock))]
        [TemplatePart(Name = BalloonBox.CloseRectangleElement, Type = typeof(Grid))]
        [TemplatePart(Name = BalloonBox.ScrollViewerElement, Type = typeof(ScrollViewer))]
        public class BalloonBox : HeaderedContentControl
        {
            DispatcherTimer PopupTimer = new DispatcherTimer();

            private const string HeaderElement = "PART_HeaderContentControl";
            private const string ContentElement = "PART_ContenContentControl";
            private const string PopUpElement = "PART_PopUp";
            private const string TextBlockElement = "PART_HeaderTextBlock";
            private const string CloseRectangleElement = "PART_CloseGrid";
            private const string ScrollViewerElement = "PART_ScrollViewerControl";


            private ContentPresenter headerContentControl;
            private ContentPresenter contentContentControl;
            private Popup popUp;
            private TextBlock headerTextBlock;
            private Grid closeGrid;
            private ScrollViewer scrollViewerControl;
            private bool textGotMouse = false;
            private bool headerGotMouse = false;
            private bool contentGotMouse = false;
            private bool scrollViewerGotMouse = false;
            public BalloonBox()
            {
                DefaultStyleKey = typeof(BalloonBox);
            }
            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();

                headerContentControl = GetTemplateChild(HeaderElement) as ContentPresenter;
                contentContentControl = GetTemplateChild(ContentElement) as ContentPresenter;
                popUp = GetTemplateChild(PopUpElement) as Popup;
                headerTextBlock = GetTemplateChild(TextBlockElement) as TextBlock;
                scrollViewerControl = GetTemplateChild(ScrollViewerElement) as ScrollViewer;

                if (contentContentControl != null)
                {
                    contentContentControl.MouseEnter += (s, e) => { contentGotMouse = true; };
                    contentContentControl.MouseLeave += (s, e) => { contentGotMouse = false; };
                }
                if (headerContentControl != null)
                {
                    headerContentControl.MouseLeftButtonDown += new MouseButtonEventHandler(headerContentControl_MouseDown);
                    headerContentControl.MouseMove += new MouseEventHandler(headerContentControl_MouseMove);
                    headerContentControl.MouseEnter += (s, e) => headerGotMouse = true;
                    headerContentControl.MouseLeave += (s, e) => headerGotMouse = false;
                }
                if (headerTextBlock != null)
                {
                    headerTextBlock.MouseLeftButtonDown += new MouseButtonEventHandler(headerContentControl_MouseDown);
                    headerTextBlock.MouseMove += new MouseEventHandler(headerContentControl_MouseMove);
                    headerTextBlock.MouseEnter += (s, e) => textGotMouse = true;
                    headerTextBlock.MouseLeave += (s, e) => textGotMouse = false;
                }
                if (scrollViewerControl != null)
                {
                    scrollViewerControl.MouseEnter += (s, e) => scrollViewerGotMouse = true;
                    scrollViewerControl.MouseLeave += (s, e) => scrollViewerGotMouse = false;
                }
                PopupTimer = new DispatcherTimer();
                PopupTimer.Tick += new EventHandler(PopupTimer_Tick);
                if (string.IsNullOrEmpty(LabelText))
                {
                    if (headerTextBlock != null) headerTextBlock.Foreground = new SolidColorBrush(Colors.Gray);
                    if (headerTextBlock != null) headerTextBlock.Text = WaterMark;
                }
                else
                {
                    if (headerTextBlock != null) headerTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                    if (headerTextBlock != null) headerTextBlock.Text = LabelText;
                }
                closeGrid = GetTemplateChild(CloseRectangleElement) as Grid;
                if (closeGrid != null) closeGrid.MouseLeftButtonDown += new MouseButtonEventHandler(closeRectangle_MouseDown);
            }


            void closeRectangle_MouseDown(object sender, MouseButtonEventArgs e)
            {
                PopupTimer.Stop();
                Storyboard storyboard = (Storyboard)this.Resources["ShrinkFast"]; //TryFindResource("ShrinkFast") as Storyboard;}
                if (storyboard != null)
                {
                    storyboard.Begin();
                    popUp.IsOpen = false;
                    IsOpen = false;
                }

                else
                {
                    popUp.IsOpen = false;
                    IsOpen = false;
                }


                e.Handled = true;
            }

            void headerContentControl_MouseMove(object sender, MouseEventArgs e)
            {
                if (Type == BalloonBoxTypeEnum.MouseOverToShow)
                {
                    if (popUp != null)
                    {
                        Storyboard storyboard = (Storyboard)this.Resources["Show"];
                        if (storyboard != null)
                            storyboard.Begin();
                        else
                            popUp.IsOpen = true;
                    }
                    PopupTimer.Interval = new TimeSpan(0, 0, 0, 2);
                    PopupTimer.Start();
                }
            }
            void headerContentControl_MouseDown(object sender, MouseButtonEventArgs e)
            {
                if (Type == BalloonBoxTypeEnum.ClickToShow)
                {
                    if (popUp != null)
                    {

                        Storyboard storyboard = (Storyboard)this.Resources["Show"];
                        if (storyboard != null)
                            storyboard.Begin();
                        else
                            popUp.IsOpen = true;
                    }
                    PopupTimer.Interval = new TimeSpan(0, 0, 0, 3);
                    PopupTimer.Start();
                }

            }
            void PopupTimer_Tick(object sender, EventArgs e)
            {

                if (!contentGotMouse && !(headerGotMouse || textGotMouse) && !scrollViewerGotMouse)
                {
                    PopupTimer.Stop();
                    Storyboard storyboard = (Storyboard)this.Resources["Shrink"];
                    if (storyboard != null)
                        storyboard.Begin();
                    else
                        popUp.IsOpen = false;
                }

            }



            public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen",
                                                                                                   typeof(bool),
                                                                                                   typeof(BalloonBox),
                                                                                                   new PropertyMetadata
                                                                                                       (new PropertyChangedCallback
                                                                                                            (OnIsOpenChanged)));
            public bool IsOpen
            {
                get { return (bool)GetValue(IsOpenProperty); }
                set { SetValue(IsOpenProperty, value); }
            }

            private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                BalloonBox balloonBox = (BalloonBox)d;
                balloonBox.popUp.IsOpen = (bool)e.NewValue;
            }

            public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark",
                                                                                                      typeof(string),
                                                                                                      typeof(BalloonBox),
                                                                                                      new PropertyMetadata
                                                                                                          (new PropertyChangedCallback
                                                                                                               (OnWatermarkChanged)));


            public string WaterMark
            {
                get { return (string)GetValue(WaterMarkProperty); }
                set { SetValue(WaterMarkProperty, value); }
            }

            private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {

            }

            public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText",
                                                                                                      typeof(string),
                                                                                                      typeof(BalloonBox)
                                                                                                      , new PropertyMetadata(new PropertyChangedCallback(OnLabelTextChanged)));

            public string LabelText
            {
                get { return (string)GetValue(LabelTextProperty); }
                set
                {
                    SetValue(LabelTextProperty, value);
                    headerTextBlock.Text = value.Replace("\n", " ").Replace("\r", " ");
                }
            }
            private static void OnLabelTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                BalloonBox balloonBox = (BalloonBox)d;
                if (e.NewValue == null || string.IsNullOrEmpty(e.NewValue.ToString()))
                {
                    if (balloonBox.headerTextBlock != null) balloonBox.headerTextBlock.Foreground = new SolidColorBrush(Colors.Gray);
                    if (balloonBox.headerTextBlock != null) balloonBox.headerTextBlock.Text = balloonBox.WaterMark;
                }
                else
                {
                    if (balloonBox.headerTextBlock != null)
                        balloonBox.headerTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                    if (balloonBox.headerTextBlock != null) balloonBox.headerTextBlock.Text = e.NewValue.ToString().Replace("\n", " ").Replace("\r", " ");
                }


            }
            public static readonly DependencyProperty BalloonBoxTypeProperty = DependencyProperty.Register(
                "BalloonBoxType", typeof(BalloonBoxTypeEnum), typeof(BalloonBox), new PropertyMetadata(new PropertyChangedCallback(OnBalloonBoxTypeChanged)));

            public BalloonBoxTypeEnum Type
            {
                get { return (BalloonBoxTypeEnum)GetValue(BalloonBoxTypeProperty); }
                set { SetValue(BalloonBoxTypeProperty, value); }
            }

            private static void OnBalloonBoxTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                //who cares? 
            }
        }
    }
}
