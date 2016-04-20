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
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace WPFCustomControls.Presentations
{

    //Note: When Setting the PeiceSizeHeight and PeiceSizeWidth fields be aware that if they do not divide 
    //to a whole number then the remainder piece will not appear. Fixing/Finishing this would not be difficult.
    //or just always use numbers like 0.2, 0.25 ,0.5 or if your crazy then use 0.1.
    //If set to 0.2 then => 1 / 0.2 = 5 ---- Looks Fine
    //If set to 0.23 then => 1 / 0.23 = 4.347 ---- 4 pieces will appear with the remaining 0.347 not appearing

    [TemplatePart(Name = "PART_Viewport", Type = typeof(Viewport3D))]
    [TemplatePart(Name = "PART_FrontContentPresenter", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "PART_BackContentPresenter", Type = typeof(ContentControl))]
    public class FlippingControl : ContentControl
    {
        static FlippingControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlippingControl), new FrameworkPropertyMetadata(typeof(FlippingControl)));

            CameraPrototypeProperty = DependencyProperty.Register(
                "CameraPrototype",
                typeof(PerspectiveCamera),
                typeof(FlippingControl),
                new UIPropertyMetadata(null, OnCameraPrototypeChanged));

            BackContentProperty =
            DependencyProperty.Register("BackContent", typeof(object), typeof(FlippingControl));

        }

        #region Fields

        //**
        //These Values control the look and functionality of the animating pieces (should be exposed as Dependency Properties)

        //The follow two values control the "Absolute" size of each rotating piece (relative to the controls size.)
        double PeiceSizeHeight = 0.2;
        double PeiceSizeWidth = 0.2;

        //The Duration of one Animating piece 
        //(probably should be Total animation Length then divide by total pieces for the animation Length of one piece
        //  combined with better control over the offset time between each piece == for nicer looking effects)
        TimeSpan RotationLength = new TimeSpan(0, 0, 0, 0, 500);

        //The Accumulating Time offset from the Begin Time of one animation to another (better if key frame animations were used)
        double animationsOffsetSeconds = 0.1; //Try 0.0
        //**

        #region Don't Touch These

        double frontAnimationsOffsetCount = 0.0;
        double backAnimationsOffsetCount = 0.0;
        double visualElementWidth;
        double visualElementHeight;
        double viewWidth;
        double viewHeight;
        Viewport3D _viewport;
        PerspectiveCamera _defaultCameraPrototype;
        ContentControl frontContent;
        ContentControl backContent;
        #endregion

        #endregion

        #region Properties

        #region IsFrontInView
        public bool IsFrontInView
        {
            get { return (bool)GetValue(IsFrontOnViewProperty); }
            set { SetValue(IsFrontOnViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFrontOnView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFrontOnViewProperty =
            DependencyProperty.Register("IsFrontInView", typeof(bool), typeof(FlippingControl), new UIPropertyMetadata((bool)true,
                delegate(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
                {
                    FlippingControl fc = depObj as FlippingControl;
                    if (!fc.IsInitialized)
                        return;

                    if (fc != null && fc.CanFlip == true)
                    {
                        fc.CanFlip = false;
                        fc.Build3DScene(fc.IsFrontInView);
                    }

                },
                ForceIsFrontOnView
                ));

        static object ForceIsFrontOnView(DependencyObject sender, object value)
        {
            FlippingControl fc = (FlippingControl)sender;

            if (fc.CanFlip == false)
            {
                if ((bool)value == true)
                {
                    return (bool)false;
                }
                else
                {
                    return (bool)true;
                }

            }
            else
            {
                return (bool)value;
            }

        }

        #endregion

        #region CameraPrototype

        public PerspectiveCamera CameraPrototype
        {
            get { return (PerspectiveCamera)GetValue(CameraPrototypeProperty); }
            set { SetValue(CameraPrototypeProperty, value); }
        }

        public static readonly DependencyProperty CameraPrototypeProperty;

        static void OnCameraPrototypeChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            FlippingControl cc3D = depObj as FlippingControl;
            if (cc3D != null && cc3D._viewport != null)
                cc3D._viewport.Camera = cc3D.CreateCamera();
        }

        #endregion // CameraPrototype

        #region BackContent
        public object BackContent
        {
            get { return (object)GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }

        public static readonly DependencyProperty BackContentProperty;
        #endregion

        #region Can Flip

        public bool CanFlip
        {
            get { return (bool)GetValue(CanFlipProperty); }
            set { SetValue(CanFlipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanFlip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanFlipProperty =
            DependencyProperty.Register("CanFlip", typeof(bool), typeof(FlippingControl), new UIPropertyMetadata((bool)true, OnCanRotateChanged));


        static void OnCanRotateChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            //CommandManager.InvalidateRequerySuggested();
        }

        #endregion

        #endregion

        #region Apply Template Override

        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();

            _viewport = base.Template.FindName("PART_Viewport", this) as Viewport3D;
            frontContent = base.Template.FindName("PART_FrontContent", this) as ContentControl;
            backContent = base.Template.FindName("PART_BackContent", this) as ContentControl;


            if (_viewport != null)
            {
                _viewport.Camera = this.CreateCamera();
            }
        }
        #endregion

        #region Build 3D Scene

        void Build3DScene(bool _isFrontInView)
        {
            //Determine ratio
            if (this.ActualHeight < this.ActualWidth)
            {
                viewHeight = 1;
                viewWidth = ActualWidth / ActualHeight;
            }
            else
            {
                viewWidth = 1;
                viewHeight = ActualHeight / ActualWidth;
            }


            //Build Front and Back
            if (_isFrontInView == true)
            {
                BuildPieces(backContent.Content as FrameworkElement, true);
                BuildPieces(frontContent.Content as FrameworkElement, false);
            }
            else
            {
                BuildPieces(frontContent.Content as FrameworkElement, true);
                BuildPieces(backContent.Content as FrameworkElement, false);
            }

            PositionCamera((viewWidth / 2) + (0.001));

        }
        #endregion

        #region Build Pieces

        void BuildPieces(FrameworkElement con, bool isFront)
        {
            double PeiceWidth = Math.Round((frontContent.ActualWidth * PeiceSizeWidth), 3); //(frontContent.ActualWidth * PeiceSizeWidth);
            double PeiceHeight = Math.Round((frontContent.ActualHeight * PeiceSizeHeight), 3); //(frontContent.ActualHeight * PeiceSizeHeight);

            double remainingPeiceWidth = Math.IEEERemainder(frontContent.ActualWidth, PeiceWidth);
            double remainingPeiceHeight = Math.IEEERemainder(frontContent.ActualHeight, PeiceHeight);

            int peicesAcross;
            int peicesDown;

            if (frontContent.ActualWidth != 0 & frontContent.ActualHeight != 0)
            {
                peicesAcross = Convert.ToInt32(Math.Floor(frontContent.ActualWidth / PeiceWidth));
                peicesDown = Convert.ToInt32(Math.Floor(frontContent.ActualHeight / PeiceHeight));
            }
            else { return; }

            visualElementWidth = viewWidth * PeiceSizeWidth;
            visualElementHeight = viewHeight * PeiceSizeHeight;

            for (int i = 0; i < peicesAcross; i++)
            {
                VisualBrush vb = new VisualBrush(con);
                vb.Stretch = Stretch.None;
                vb.ViewboxUnits = BrushMappingMode.Absolute;
                vb.Viewbox = new Rect(PeiceWidth * i, 0, PeiceWidth, PeiceHeight);
                Grid cont = new Grid() { Background = vb, Height = PeiceHeight, Width = PeiceWidth };
                cont.Name = "horzBack" + i.ToString();

                Viewport2DVisual3D visial = BuildVisualHost();
                PositionVisual(ref visial, isFront, false, ((i) * visualElementWidth), 0.0);
                visial.Visual = cont;

                _viewport.Children.Add(visial);

                for (int ii = 1; ii < peicesDown; ii++)
                {
                    VisualBrush vb2 = new VisualBrush(con);
                    vb2.Stretch = Stretch.None;
                    vb2.ViewboxUnits = BrushMappingMode.Absolute;
                    vb2.Viewbox = new Rect(PeiceWidth * i, PeiceHeight * ii, PeiceWidth, PeiceHeight);
                    Grid cont2 = new Grid() { Background = vb2, Height = PeiceHeight, Width = PeiceWidth };
                    cont2.Name = "vertBack" + ii.ToString();

                    Viewport2DVisual3D visial2 = BuildVisualHost();

                    if (i == peicesAcross - 1 && ii == peicesDown - 1 && isFront == false)
                    {
                        PositionVisual(ref visial2, isFront, true, ((i) * visualElementWidth), ((ii) * visualElementHeight));
                    }
                    else
                    {
                        PositionVisual(ref visial2, isFront, false, ((i) * visualElementWidth), ((ii) * visualElementHeight));
                    }
                    visial2.Visual = cont2;

                    _viewport.Children.Add(visial2);
                }
            }
        }
        #endregion

        #region Position Piece

        void PositionVisual(ref Viewport2DVisual3D vis, bool isFront, bool isLast, double widthOffset, double heightOffset)
        {
            double visWidthPoint = (viewWidth / 2);
            double visHeightPoint = (viewHeight / 2);
            Rect bounds = new Rect(-viewWidth / 2, viewHeight / 2, viewWidth, viewHeight);


            MeshGeometry3D mesh = vis.Geometry as MeshGeometry3D;
            Point3DCollection positions = new Point3DCollection();
            double zOffset = 0.0;

            if (isFront == false)
            {
                zOffset = 0.0;
            }
            else
            {
                zOffset = 0.001;
            }

            Point3D pt1 = new Point3D(bounds.TopLeft.X + widthOffset, bounds.TopLeft.Y - visualElementHeight - heightOffset, zOffset);
            positions.Add(pt1);
            Point3D pt2 = new Point3D(bounds.TopLeft.X + visualElementWidth + widthOffset, bounds.TopLeft.Y - visualElementHeight - heightOffset, zOffset);
            positions.Add(pt2);
            Point3D pt3 = new Point3D(bounds.TopLeft.X + visualElementWidth + widthOffset, bounds.TopLeft.Y - heightOffset, zOffset);
            positions.Add(pt3);
            Point3D pt4 = new Point3D(bounds.TopLeft.X + widthOffset, bounds.TopLeft.Y - heightOffset, zOffset);
            positions.Add(pt4);

            mesh.Positions = positions;
            Rotate(ref vis, isFront, isLast, (((bounds.TopLeft.X + (visualElementWidth / 2)) + widthOffset)), ((bounds.TopLeft.Y - (visualElementHeight / 2)) - heightOffset), zOffset);

        }
        #endregion

        #region Rotate Piece

        void Rotate(ref Viewport2DVisual3D item, bool isFront, bool isLast, double centerX, double centerY, double zOffset)
        {
            Transform3DGroup trans = new Transform3DGroup();
            RotateTransform3D rot = new RotateTransform3D();
            rot.CenterX = centerX;
            rot.CenterY = centerY;
            rot.CenterZ = zOffset;
            QuaternionRotation3D quatRot = new QuaternionRotation3D();

            QuaternionAnimation anim = new QuaternionAnimation();
            //anim.AutoReverse = true;
            //anim.RepeatBehavior = new RepeatBehavior(4);// RepeatBehavior.Forever;
            anim.Duration = RotationLength;

            if (isFront == true)
            {
                quatRot.Quaternion = new Quaternion(new Vector3D(0, 1, 0), 0);
                anim.From = new Quaternion(new Vector3D(0, 1, 0), 0);
                anim.To = new Quaternion(new Vector3D(0, 1, 0), 180);
                anim.BeginTime = TimeSpan.FromSeconds(frontAnimationsOffsetCount);
                frontAnimationsOffsetCount = frontAnimationsOffsetCount + animationsOffsetSeconds;

            }
            else
            {
                quatRot.Quaternion = new Quaternion(new Vector3D(0, 1, 0), -180);
                anim.From = new Quaternion(new Vector3D(0, 1, 0), -180);
                anim.To = new Quaternion(new Vector3D(0, 1, 0), 0);
                anim.BeginTime = TimeSpan.FromSeconds(backAnimationsOffsetCount);
                backAnimationsOffsetCount = backAnimationsOffsetCount + animationsOffsetSeconds;
            }

            if (isLast == true)
            {
                anim.Completed -= new EventHandler(anim_Completed);
                anim.Completed += new EventHandler(anim_Completed);
            }

            rot.Rotation = quatRot;
            trans.Children.Add(rot);
            item.Transform = trans;

            quatRot.BeginAnimation(QuaternionRotation3D.QuaternionProperty, anim);

        }
        #endregion

        #region Build 3D Scene Helpers

        ModelVisual3D GetLight()
        {
            ModelVisual3D mv = new ModelVisual3D();
            Model3DGroup mvGroup = new Model3DGroup();
            mvGroup.Children.Add(new DirectionalLight(Colors.White, new Vector3D(0, 0, -1)));
            mvGroup.Children.Add(new AmbientLight(Colors.White));
            mv.Content = mvGroup;
            return mv;
        }
        MeshGeometry3D GetGeomatryMesh()
        {
            MeshGeometry3D geom = new MeshGeometry3D();
            Int32Collection indices = new Int32Collection();
            indices.Add(0);
            indices.Add(1);
            indices.Add(2);
            indices.Add(2);
            indices.Add(3);
            indices.Add(0);

            PointCollection textureCoors = new PointCollection();
            textureCoors.Add(new Point(0, 1));
            textureCoors.Add(new Point(1, 1));
            textureCoors.Add(new Point(1, 0));
            textureCoors.Add(new Point(0, 0));

            geom.TriangleIndices = indices;
            geom.TextureCoordinates = textureCoors;

            return geom;
        }
        DiffuseMaterial GetMaterial()
        {
            DiffuseMaterial mat = new DiffuseMaterial(Brushes.Red);
            mat.SetValue(Viewport2DVisual3D.IsVisualHostMaterialProperty, true);
            return mat;
        }
        Viewport2DVisual3D BuildVisualHost()
        {
            Viewport2DVisual3D vis = new Viewport2DVisual3D();
            vis.Material = GetMaterial();
            vis.Geometry = GetGeomatryMesh();

            return vis;
        }

        #region CreateCamera

        PerspectiveCamera PositionCamera(double maxHeight)
        {
            PerspectiveCamera cam = _viewport.Camera as PerspectiveCamera;
            cam.Position = new Point3D(0, 0, maxHeight);
            cam.LookDirection = new Vector3D(0, 0, -1);
            return cam;
        }

        PerspectiveCamera CreateCamera()
        {
            if (this.CameraPrototype != null)
                return this.CameraPrototype.Clone();
            else
                return this.DefaultCameraPrototype.Clone();
        }

        #region DefaultCameraPrototype

        PerspectiveCamera DefaultCameraPrototype
        {
            get
            {
                if (_defaultCameraPrototype == null)
                {
                    _defaultCameraPrototype = new PerspectiveCamera
                    {
                        Position = new Point3D(0, 0, 1),
                        LookDirection = new Vector3D(0, 0, -1),
                        FieldOfView = 90
                    };
                }
                return _defaultCameraPrototype;
            }
        }

        #endregion // DefaultCameraPrototype
        #endregion // CreateCamera

        #endregion

        #region Animation Complete

        #region Finished Last Animation
        void anim_Completed(object sender, EventArgs e)
        {
            CleanUp();
        }
        #endregion

        private void CleanUp()
        {
            frontAnimationsOffsetCount = 0;
            backAnimationsOffsetCount = 0;
            _viewport.Children.Clear();
            _viewport.Children.Add(GetLight());
            CanFlip = true;
        }
        #endregion

    }
}
