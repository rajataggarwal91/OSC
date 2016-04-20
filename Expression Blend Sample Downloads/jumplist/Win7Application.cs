using System.Windows;

namespace SingleInstance
{
    public class Win7Application : Application
    {
        public MainWindow AppWindow { get; private set; }
        public Win7Application()
            : base()
        { }

        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            AppWindow = new MainWindow();
            ProcessArgs(e.Args, true);
            AppWindow.Show();
        }

        public void ProcessArgs(string[] args, bool isFirstInstance)
        {
            if (args.Length > 0)
            {
                if (isFirstInstance)
                    AppWindow.initState = args[0];
                else
                    AppWindow.updateState(args[0]);
            }
            
        }
    }
}
