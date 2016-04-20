using System;
using System.Linq;
using Microsoft.VisualBasic.ApplicationServices;

namespace SingleInstance
{
    public sealed class SingleInstanceManager : WindowsFormsApplicationBase
    {
        [STAThread]
        public static void Main(string[] args)
        {
            (new SingleInstanceManager()).Run(args);
        }
        public SingleInstanceManager()
        {
            IsSingleInstance = true;
        }

        public Win7Application App { get; private set; }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            App = new Win7Application();
            App.Run();
            return false;
        }
        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            App.MainWindow.Activate();
            App.ProcessArgs(eventArgs.CommandLine.ToArray(), false);
        }
    }
}
