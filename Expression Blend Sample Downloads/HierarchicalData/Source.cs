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
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace HierarchicalData
{
    /// <summary>
    /// Hierarchical source
    /// </summary>
    public class HierarchicalSource : INotifyPropertyChanged
    {
        private bool? isChecked;
        private static HierarchicalSource firstInstance = null;

        /// <summary>
        /// AfterChecked event to do some extra task after check
        /// </summary>
        public event EventHandler AfterChecked;

        /// <summary>
        /// Gets or sets parent
        /// </summary>
        public HierarchicalSource Parent { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }        

        /// <summary>
        /// Gets or sets byPass field
        /// </summary>
        public static bool byPass = false;

        /// <summary>
        /// Gets or sets IsChecked
        /// </summary>
        public bool? IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                    }

                    if (Children != null && IsChecked.HasValue)
                    {
                        byPass = true;
                        if (firstInstance == null)
                        {
                            firstInstance = this;
                        }
                        Children.ForEach(x => x.IsChecked = IsChecked.Value);
                        if (ReferenceEquals(this, firstInstance))
                        {
                            byPass = false;
                            firstInstance = null;
                        }
                    }                  
                    if (byPass)
                    {
                        return;
                    }
                    if (Parent != null)
                    {
                        if (Parent.Children.All(x => x.IsChecked == true))
                        {
                            Parent.IsChecked = true;
                        }
                        else if (Parent.Children.All(x => x.IsChecked == false))
                        {
                            Parent.IsChecked = false;
                        }
                        else
                        {
                            Parent.IsChecked = null;
                        }
                    }
                    if (AfterChecked != null)
                    {
                        AfterChecked(this, EventArgs.Empty);
                    }
                }
            }
        }
        public List<HierarchicalSource> Children { get; set; }

        public HierarchicalSource()
        {
            isChecked = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
