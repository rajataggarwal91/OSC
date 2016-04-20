using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ChangeThemes
{
    class LogicSkin : DependencyObject
    {
        public static readonly DependencyProperty SelectSkinProperty = DependencyProperty.RegisterAttached("SelectSkin", typeof(Uri), typeof(LogicSkin), new UIPropertyMetadata(null, SelectSkinChanged));

        public static void SelectSkinChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is FrameworkElement)
            {
                ApplySkin(obj as FrameworkElement, GetCurrentSkin(obj));
            }
        }
        public static Uri GetCurrentSkin(DependencyObject obj)
        {
            return (Uri)obj.GetValue(SelectSkinProperty);
        }
        public static void SetCurrentSkin(DependencyObject obj,Uri value)
        {
            obj.SetValue(SelectSkinProperty,value);
        }
        public static void ApplySkin(FrameworkElement targetElement, Uri dictionaryUri)
        {
            if (targetElement == null)
                return;
            try
            {
                ResourceDictionary rd = null;
                if (dictionaryUri != null)
                {
                    rd = new ResourceDictionary();
                    rd.Source = dictionaryUri;

                    targetElement.Resources.MergedDictionaries.Insert(0, rd);
                }

                List<ResourceDictionary> ed = (from dictionary in targetElement.Resources.MergedDictionaries.OfType<ResourceDictionary>() select dictionary).ToList();

                foreach (ResourceDictionary thDictionary in ed)
                {
                    if (rd == thDictionary) continue;
                    targetElement.Resources.MergedDictionaries.Remove(thDictionary);
                }
            }
            finally { }
        }
    }
}