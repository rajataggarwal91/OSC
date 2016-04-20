/////////////////////////////////////////////////////////////////
//
// this class contains utility functions to "outline" a UIElement with a series of points.
// this is used for boundary detection.
// Portions of this Boundary Helper class are taken from Physics2D.NET by Jonathan Porter. (http://physics2d.googlepages.com/)
// this code has a very unrestrictive MIT license, which is copied below.
//
/////////////////////////////////////////////////////////////////


#region MIT License
/*
 * Portions of this file, BoundaryHelper.cs are Copyright (c) 2005-2008 Jonathan Mark Porter. http://physics2d.googlepages.com/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of 
 * the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be 
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */
#endregion


using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Spritehand.FarseerHelper
{
    public class BoundaryHelper
    {


        /// <summary>
        /// Get a list of points that represents the outline of a UI element
        /// </summary>
        public static List<Point> GetPointsForElement(UIElement element, UIElement containerElement, bool positionOnly)
        {

            if (element is Rectangle)
                return GetPointsForRectangle(element, positionOnly);

            if (element is Image)
                return GetPointsForImage(element as Image, containerElement, positionOnly);

            return GetPointsForPath(element, containerElement, positionOnly);
        }

        public static List<Point> GetPointsForPath(UIElement element, UIElement containerElement, bool positionOnly)
        {
            List<Point> outline = new List<Point>();
#if SILVERLIGHT
            double left = Convert.ToInt32(element.GetValue(Canvas.LeftProperty));
            double top = Convert.ToInt32(element.GetValue(Canvas.TopProperty));
#else
            double left = (double)element.GetValue(Canvas.LeftProperty);
            double top = (double)element.GetValue(Canvas.TopProperty);

            // get the list of child UI elements for the main element
            List<UIElement> childElements = new List<UIElement>();
            GetChildElements(element, childElements);
#endif
            double width = (double)element.GetValue(Canvas.WidthProperty);
            double height = (double)element.GetValue(Canvas.HeightProperty);

            if (Double.IsNaN(width) && element is FrameworkElement)
                width = (element as FrameworkElement).ActualWidth;

            if (Double.IsNaN(height) && element is FrameworkElement)
                height = (element as FrameworkElement).ActualHeight;

            // if this is a "nested" (child) element containing physics, and there is a parent physics controller,
            // then we need to offset the top/left accordingly in the parent.
#if SILVERLIGHT
			// WPF doesn't need because of the change to the hit test
            UserControl ucParent = null;
            Canvas cnvParent = null;

            if (IsInsideNestedUsercontrol(element))
            {
                cnvParent = (element as FrameworkElement).Parent as Canvas;
                ucParent = (cnvParent.Parent as UserControl);
            }


            double offSetParentLeft = 0;
            double offSetParentTop = 0;
            if (ucParent != null)
            {
                // take this element out of its parent canvas
                //cnvParent.Children.Remove(containerElement);

                offSetParentLeft = Convert.ToInt32(ucParent.GetValue(Canvas.LeftProperty));
                offSetParentTop = Convert.ToInt32(ucParent.GetValue(Canvas.TopProperty));

                left = left + offSetParentLeft;
                top = top + offSetParentTop;

            }
#endif

            if (Double.IsNaN(width) || Double.IsNaN(height))
                return null;

			if (Double.IsNaN(left))
				left = 0;

			if (Double.IsNaN(top))
				top = 0;

			if (positionOnly)
                return null;

            Point ptFirstHit = new Point();
            bool bFoundPoint = false;

            // get the first point
            for (int x = Convert.ToInt32(left + width - 1); x > left - 1; --x)
            {
                for (int y = Convert.ToInt32(top); y < top + height; ++y)
                {
                    Point ptTest = new Point(x, y);
#if SILVERLIGHT
                    List<UIElement> hits = System.Windows.Media.VisualTreeHelper.FindElementsInHostCoordinates(ptTest, element) as List<UIElement>;
                    if (hits.Contains(element))
                    {
                        ptFirstHit = new Point(x, y);
                        bFoundPoint = true;
                        break;
                    }
#else
					if (element.IsHitTestVisible)
					{
						// Look for the points in the parent coordinates
						var hit = System.Windows.Media.VisualTreeHelper.HitTest((UIElement)System.Windows.Media.VisualTreeHelper.GetParent(element), ptTest);
						if (hit != null)
						{
                            if (hit.VisualHit != null && childElements.Contains( hit.VisualHit as UIElement ))
							{
								ptFirstHit = new Point(x, y);
								bFoundPoint = true;
								break;
							}
						}
					}
#endif
				}

				if (bFoundPoint) break;
            }

            if (!bFoundPoint)
            {
                throw new ArgumentException("Could not determine the outline of UIElement " + element.GetValue(Canvas.NameProperty) + ". Could not find a point within its boundaries.");
            }

            Point current = ptFirstHit;
            Point last = new Point(ptFirstHit.X, ptFirstHit.Y - 1);
            List<Point> result = new List<Point>();
            do
            {
                result.Add(current);
#if SILVERLIGHT
                current = GetNextVertex(element, current, last, left, top, width, height, null);
#else
                current = GetNextVertex(element, current, last, left, top, width, height, null, childElements);
#endif
                last = result[result.Count - 1];
            } while (!(current.X == ptFirstHit.X && current.Y == ptFirstHit.Y));

            if (result.Count < 3)
            {
                throw new ArgumentException("The UIElement " + element.GetValue(Canvas.NameProperty) + " is too small to determine outline (must be at least 3x3 pixels)");
            }

            // TODO: Move this into upper "do" loop for optimization
#if SILVERLIGHT
			// not needed for WPF because of the change to the hit test
            if (ucParent != null)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    result[i] = new Point(result[i].X - offSetParentLeft, result[i].Y - offSetParentTop);
                }
            }
#endif
            return result;

        }

        public static List<Point> GetPointsForImage(Image element, UIElement containerElement, bool positionOnly)
        {
            List<Point> outline = new List<Point>();

            double baseLeft = Convert.ToInt32(element.GetValue(Canvas.LeftProperty));
            double baseTop = Convert.ToInt32(element.GetValue(Canvas.TopProperty));
            double left = baseLeft;
            double top = baseTop;
            double width = (double)element.GetValue(Canvas.WidthProperty);
            double height = (double)element.GetValue(Canvas.HeightProperty);

            if (Double.IsNaN(width) && element is FrameworkElement)
                width = (element as FrameworkElement).ActualWidth;

            if (Double.IsNaN(height) && element is FrameworkElement)
                height = (element as FrameworkElement).ActualHeight;

#if SILVERLIGHT
            WriteableBitmap bitmap = new WriteableBitmap((int)width, (int)height); ;
            bitmap.Render(element, new TranslateTransform());
            bitmap.Invalidate();
#else
            ImageSource ims = element.Source;
            BitmapSource bitmapImage = (BitmapSource)ims;
            int stride = (bitmapImage.PixelWidth * bitmapImage.Format.BitsPerPixel + 7) / 8;
            Int32[] pixels = new Int32[bitmapImage.PixelWidth * bitmapImage.PixelHeight];
            bitmapImage.CopyPixels(Int32Rect.Empty, pixels, stride, 0);
#endif

            // if this is a "nested" (child) element containing physics, and there is a parent physics controller,
            // then we need to offset the top/left accordingly in the parent.
            UserControl ucParent = null;
            Canvas cnvParent = null;

            if (IsInsideNestedUsercontrol(containerElement))
            {
                cnvParent = (containerElement as FrameworkElement).Parent as Canvas;
                ucParent = (cnvParent.Parent as UserControl);
            }


            double offSetParentLeft = 0;
            double offSetParentTop = 0;
            if (ucParent != null)
            {
                // take this element out of its parent canvas
                //cnvParent.Children.Remove(containerElement);

                offSetParentLeft = Convert.ToInt32(ucParent.GetValue(Canvas.LeftProperty));
                offSetParentTop = Convert.ToInt32(ucParent.GetValue(Canvas.TopProperty));

            }


            if (Double.IsNaN(width) || Double.IsNaN(height))
                return null;

            if (positionOnly)
                return null;

            Point ptFirstHit = new Point();
            bool bFoundPoint = false;

            // get the first point
            for (int x = Convert.ToInt32(left + width - 1); x > left - 1; --x)
            {
                for (int y = Convert.ToInt32(top); y < top + height; ++y)
                {
                    int offSetX = Convert.ToInt32(x - baseLeft);
                    int offsetY = Convert.ToInt32(y - baseTop);

#if SILVERLIGHT
                    int pixel = bitmap.Pixels[bitmap.PixelWidth * offsetY + offSetX];
#else
                    int pixel = pixels[bitmapImage.PixelWidth * offsetY + offSetX];
#endif
                    if (pixel != 0)
                    {
                        ptFirstHit = new Point(x, y);
                        bFoundPoint = true;
                        break;
                    }
                }
                if (bFoundPoint) break;
            }

            Point current = ptFirstHit;
            Point last = new Point(ptFirstHit.X, ptFirstHit.Y - 1);
            List<Point> result = new List<Point>();
            do
            {
                result.Add(current);
#if SILVERLIGHT
                current = GetNextVertex(element, current, last, left, top, width, height, bitmap);
#else
                current = GetNextVertex(element, current, last, left, top, width, height, bitmapImage, null);
#endif
                last = result[result.Count - 1];
            } while (!(current.X == ptFirstHit.X && current.Y == ptFirstHit.Y));
            if (result.Count < 3)
            {
                throw new ArgumentException("The UIElement " + element.GetValue(Canvas.NameProperty) + " is too small to determine outline (must be at least 3x3 pixels)");
            }

            return result;

        }

        public static List<Point> GetPointsForRectangle(UIElement element, bool positionOnly)
        {
            List<Point> outline = new List<Point>();

            double left = Convert.ToInt32(element.GetValue(Canvas.LeftProperty));
            double top = Convert.ToInt32(element.GetValue(Canvas.TopProperty));
            double width = (double)element.GetValue(Canvas.WidthProperty);
            double height = (double)element.GetValue(Canvas.HeightProperty);

            // if this is a "nested" (child) element containing physics, and there is a parent physics controller,
            // then we need to offset the top/left accordingly in the parent.
            UserControl ucParent = null;
            Canvas cnvParent = null;

            if (IsInsideNestedUsercontrol(element))
            {
                cnvParent = (element as FrameworkElement).Parent as Canvas;
                ucParent = (cnvParent.Parent as UserControl);
            }

            if (ucParent != null)
            {
                // take this element out of its parent canvas
                cnvParent.Children.Remove(element);

            }

            if (Double.IsNaN(width) || Double.IsNaN(height))
                return null;

            if (positionOnly)
                return null;

            List<Point> result = new List<Point>();

            result.Add(new Point(left, top));
            result.Add(new Point(left + width, top));
            result.Add(new Point(left + width, top + height));
            result.Add(new Point(left, top + height));

            return result;

        }

        /// <summary>
        /// Returns true if the given element is inside a user control with its own physics controllers.
        /// </summary>
        public static bool IsInsideNestedUsercontrol(UIElement element)
        {
            bool bRetVal = false;

            UserControl ucParentUserControl = GetParentUserControl(element);
            if (ucParentUserControl != null)
            {
                if (ucParentUserControl.Parent is Canvas)
                {
                    Canvas cnvParentContainer = (ucParentUserControl.Parent as Canvas);
                    // first check for a PhysicsController property on the canvas
                    if (cnvParentContainer.GetValue(PhysicsControllerMain.PhysicsControllerProperty) != null)
                    {
                        bRetVal = true;
                    }
                    else
                    {
                        // fallback to UserControl model for physics
                        foreach (UIElement child in cnvParentContainer.Children)
                        {
                            if (child is PhysicsController)
                            {
                                bRetVal = true;
                                break;
                            }
                        }
                    }
                }
            }


            return bRetVal;

        }

        public static UserControl GetParentUserControl(UIElement element)
        {
            UserControl ucParentUserControl = null;

            if ((element as FrameworkElement).Parent is Canvas)
            {
                Canvas cnvParent = (element as FrameworkElement).Parent as Canvas;
                if (cnvParent.Parent is UserControl)
                {
                    ucParentUserControl = (cnvParent.Parent as UserControl);
                }
            }

            return ucParentUserControl;
        }

#if !SILVERLIGHT
        public static void GetChildElements(UIElement element, List<UIElement> childElements)
        {
            if (childElements == null)
                childElements = new List<UIElement>();

            if (childElements.Count == 0)
                childElements.Add(element);

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                // Retrieve child visual at specified index value.
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(element, i);
                childElements.Add(childVisual);

                // Do processing of the child visual object.

                // Enumerate children of the child visual object.
                GetChildElements(childVisual, childElements);
            }

        }
#endif


        static readonly Point[] uiElementPoints = new Point[]{
            new Point (1,1),
            new Point (0,1),
            new Point (-1,1),
            new Point (-1,0),
            new Point (-1,-1),
            new Point (0,-1),
            new Point (1,-1),
            new Point (1,0),
        };

        public static void PointAdd(ref Point left, ref  Point right, out Point result)
        {
            result = new Point();
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
        }

        public static bool PointEquals(Point left, Point right)
        {
            return
                left.X == right.X &&
                left.Y == right.Y;
        }


        /// <summary>
        /// This function gets the next point along the "traced outline" of a UIElement.
        /// </summary>
		/// <remarks>
		/// Changed from WriteableBitmap to RenderTargetBitmap for WPF
		/// </remarks>
#if SILVERLIGHT
        private static Point GetNextVertex(UIElement element, Point current, Point last, double left, double top, double width, double height, WriteableBitmap bitmap)
#else
        private static Point GetNextVertex(UIElement element, Point current, Point last, double left, double top, double width, double height, BitmapSource bitmap, List<UIElement> childElements)
#endif
        {
            int offset = 0;
            Point point;
            for (int index = 0; index < uiElementPoints.Length; ++index)
            {
                PointAdd(ref current, ref uiElementPoints[index], out point);
                if (PointEquals(point, last))
                {
                    offset = index + 1;
                    break;
                }
            }
#if !SILVERLIGHT
			// Added to handle the change from WritableBitmap to RenderTargetBitmap
			Int32[] pixels = null;
			if (bitmap != null)
			{
				pixels = new Int32[bitmap.PixelWidth * bitmap.PixelHeight];
				int stride = bitmap.PixelWidth * bitmap.Format.BitsPerPixel/8;
				bitmap.CopyPixels(Int32Rect.Empty, pixels, stride, 0);
			}
#endif
			for (int index = 0; index < uiElementPoints.Length; ++index)
            {
                PointAdd(
                    ref current,
                    ref uiElementPoints[(index + offset) % uiElementPoints.Length],
                    out point);

                bool bHit = false;
                if (bitmap == null)
                {
                    // this is a XAML element
#if SILVERLIGHT
                    List<UIElement> hits = System.Windows.Media.VisualTreeHelper.FindElementsInHostCoordinates(point, element) as List<UIElement>;
                    bHit = hits.Contains(element);
#else
					// Look for the points in the parent coordinates
					var o = System.Windows.Media.VisualTreeHelper.HitTest((UIElement)System.Windows.Media.VisualTreeHelper.GetParent(element), point);
                    bHit = o != null && o.VisualHit != null && childElements.Contains(o.VisualHit as UIElement);
#endif
                }
                else
                {
                    // this is an Image element
                    int offSetX = Convert.ToInt32(point.X - left);
                    int offsetY = Convert.ToInt32(point.Y - top);

#if SILVERLIGHT
                    int pixel = bitmap.Pixels[bitmap.PixelWidth * offsetY + offSetX];
#else
                    int pixel = pixels[bitmap.PixelWidth * offsetY + offSetX];
#endif
                    bHit = (pixel != 0);
                }
                if (point.X >= left && point.X < left + width &&
                    point.Y >= top && point.Y < top + height &&
                    bHit)
                {
                    return point;
                }
            }
            throw new ArgumentException("The UIElement " + element.GetValue(Canvas.NameProperty) + " is too small to determine outline (must be at least 3x3 pixels)");
        }

    }
}
