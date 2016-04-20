using System.Windows;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Reflection;
using Microsoft.Expression.Interactivity.Core;

namespace SingleInstance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarManager windowsTaskbar = TaskbarManager.Instance;
        private JumpList WindowJumpList;
        private string AppID;
        public string initState {get; set;}
        string executablePath;
        string executableFolder;
        

        public MainWindow()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close,
               new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs args) { this.Close(); })));

            // Insert code required on object creation below this point.
            AppID = "Win7Demo";
            executablePath = Assembly.GetEntryAssembly().Location;
            executableFolder = System.IO.Path.GetDirectoryName(executablePath);
        }
      
        private void initializeJumplist()
        {
            string strAppDir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string app = Assembly.GetEntryAssembly().GetName().Name + ".exe";

            WindowJumpList.AddUserTasks(new JumpListLink(
                System.IO.Path.Combine(executableFolder, app),
                "Home")
            {
                Arguments = "HomeActive"
            });
            WindowJumpList.AddUserTasks(new JumpListLink(
                System.IO.Path.Combine(executableFolder, app),
                "Shopping")
            {
                Arguments = "ShoppingActive"
            });

            WindowJumpList.Refresh();

        }

        public  void updateState(string state)
        {
            ExtendedVisualStateManager.GoToElementState(LayoutRoot, state, true);
            
        }
        private void Pallette_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            WindowJumpList = JumpList.CreateJumpList();
            initializeJumplist();
            if (initState != null)
                updateState(initState);
          
        }

    }
}
