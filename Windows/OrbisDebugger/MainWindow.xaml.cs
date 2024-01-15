using Microsoft.Extensions.Logging;
using OrbisLib2.Common.Database.Types;
using OrbisLib2.Common.Dispatcher;
using OrbisLib2.General;
using OrbisLib2.Targets;
using SimpleUI.Controls;
using System.Threading.Tasks;

namespace OrbisDebugger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SimpleWindow
    {
        public MainWindow()
        {
            // System.Windows.Forms.Application.EnableVisualStyles();

            InitializeComponent();
        }

        public void Show(ILogger logger)
        {
            base.Show();

            DispatcherClient.Subscribe(logger);

            Events.ProcAttach += Events_ProcAttach;
            Events.ProcDetach += Events_ProcDetach;
            Events.ProcDie += Events_ProcDie;
            Events.TargetStateChanged += Events_TargetStateChanged;
            Events.DBTouched += Events_DBTouched;
            Events.SelectedTargetChanged += Events_SelectedTargetChanged;

            // Update State
            Task.Run(async () =>
            {
                if (TargetManager.SelectedTarget != null)
                {
                    await EnableProgram(await TargetManager.SelectedTarget.Debug.IsDebugging());
                }
            });
        }

        #region Events

        private async Task EnableProgram(bool attached)
        {
            var currentTarget = TargetManager.SelectedTarget;
            if (currentTarget.MutableInfo.Status != TargetStatusType.APIAvailable)
                attached = false;

            Dispatcher.Invoke(() =>
            {
                
            });

            if (attached)
            {
                
            }
            else
            {
                
            }
        }

        private async void Events_TargetStateChanged(object? sender, TargetStateChangedEvent e)
        {
            if (e.Name != TargetManager.SelectedTarget.Name)
                return;

            switch (e.State)
            {
                case TargetStateChangedEvent.TargetState.APIAvailable:
                    await EnableProgram(await TargetManager.SelectedTarget.Debug.IsDebugging());
                    break;
                case TargetStateChangedEvent.TargetState.APIUnAvailable:
                    await EnableProgram(false);
                    break;
            }
        }

        private async void Events_ProcDie(object? sender, ProcDieEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Make sure a target is set.
            if (currentTarget == null)
                return;

            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != currentTarget.IPAddress)
                return;

            // Disable the attached options.
            await EnableProgram(false);
        }

        private async void Events_ProcDetach(object? sender, ProcDetachEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Make sure a target is set.
            if (currentTarget == null)
                return;

            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != currentTarget.IPAddress)
                return;

            // Disable the attached options.
            await EnableProgram(false);
        }

        private async void Events_ProcAttach(object? sender, ProcAttachEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Make sure a target is set.
            if (currentTarget == null)
                return;

            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != currentTarget.IPAddress)
                return;

            // Enable the attached options.
            await EnableProgram(true);
        }

        private async void Events_DBTouched(object? sender, DBTouchedEvent e)
        {
            await EnableProgram(await TargetManager.SelectedTarget.Debug.IsDebugging());
        }

        private async void Events_SelectedTargetChanged(object? sender, SelectedTargetChangedEvent e)
        {
            await EnableProgram(await TargetManager.SelectedTarget.Debug.IsDebugging());
        }

        #endregion

        #region Buttons

        

        #endregion
    }
}
