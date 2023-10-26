using System.Threading;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using SetupBA.MVVM.View;
using SetupBA.MVVM.ViewModel;

namespace SetupBA
{
    public class SetupBA : BootstrapperApplication
    {
        // global dispatcher
        static public Dispatcher BootstrapperDispatcher { get; private set; }

        // entry point for our custom UI
        protected override void Run()
        {
            BootstrapperDispatcher = Dispatcher.CurrentDispatcher;

            MainView view = new MainView();
            view.DataContext = new MainViewModel(this);
            Engine.Detect();
            if (Command.Display != Display.None && Command.Display != Display.Embedded)
            {
                view.Closed += (sender, e) => BootstrapperDispatcher.InvokeShutdown();
                view.Show();
            }

            Dispatcher.Run();

            Engine.Quit(0);
        }
    }
}