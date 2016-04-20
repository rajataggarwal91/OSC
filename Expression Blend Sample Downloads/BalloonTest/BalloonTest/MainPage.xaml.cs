using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BalloonTest
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += (s, e) => Note.DataContext = new NoteClass();
        }
        public class NoteClass : INotifyPropertyChanged
        {
            private string _note;
            public string Note
            {
                get { return _note; }
                set
                {
                    _note = value;
                    RaisePropertyChanged("Note");
                }
            }
            protected void RaisePropertyChanged(string propertyName)
            {

                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    var e = new PropertyChangedEventArgs(propertyName);
                    handler(this, e);

                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
