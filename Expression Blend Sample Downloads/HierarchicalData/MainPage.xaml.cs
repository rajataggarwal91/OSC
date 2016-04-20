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
using System.Collections.ObjectModel;

namespace HierarchicalData
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            DataSource dataSource = new DataSource();
        }
    }

    public class DataSource
    {
        public static ObservableCollection<HierarchicalSource> Source { get; set; }

        public DataSource()
        {
            //the source here is hard coded, you may get source from wcf method for example
            Source = new ObservableCollection<HierarchicalSource>();
            HierarchicalSource hSource = new HierarchicalSource { Name = "Root", Children = new List<HierarchicalSource>() };
        
            HierarchicalSource child1 = new HierarchicalSource
                {
                    Name = "Child1",
                    Parent = hSource
                };
            child1.Children = (new HierarchicalSource[]
                    {
                        new HierarchicalSource {Name = "Child1-1", Parent = child1},
                        new HierarchicalSource {Name = "Child1-2", Parent = child1},
                        new HierarchicalSource {Name = "Child1-3", Parent = child1}
                    }).ToList();

            hSource.Children.Add(child1);

            HierarchicalSource child2 = new HierarchicalSource
            {
                Name = "Child2",
                Parent = hSource
            };

            child2.Children = (new HierarchicalSource[]
                    {
                        new HierarchicalSource {Name = "Child2-1", Parent = child2},
                        new HierarchicalSource {Name = "Child2-2", Parent = child2}                        
                    }).ToList();

            hSource.Children.Add(child2);

            HierarchicalSource child3 = new HierarchicalSource
            {
                Name = "Child3",
                Parent = hSource
            };

            child3.Children = (new HierarchicalSource[]
                    {
                        new HierarchicalSource {Name = "Child3-1", Parent = child3}
                    }).ToList();

            hSource.Children.Add(child3);

            Source.Add(hSource);
        }
    }
}
