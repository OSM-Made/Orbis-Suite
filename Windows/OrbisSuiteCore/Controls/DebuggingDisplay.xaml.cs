using OrbisLib2.Dialog;
using OrbisLib2.Targets;
using SimpleUI.Controls;
using System.Windows;
using System.Windows.Controls;
using OrbisLib2.General;
using OrbisLib2.Common.Database.Types;

namespace OrbisSuiteCore.Controls
{
    public class SprxLoadedEventArgs : EventArgs
    {
        public int Handle { get; }

        public SprxLoadedEventArgs(int handle)
        {
            Handle = handle;
        }
    }

    /// <summary>
    /// Interaction logic for DebuggingDisplay.xaml
    /// </summary>
    public partial class DebuggingDisplay : UserControl
    {
        private TargetStatusType _currentState = TargetStatusType.None;

        public event EventHandler<SprxLoadedEventArgs> SprxLoadedEvent;
        public event EventHandler PayloadLoadedEvent;
        public event EventHandler ElfLoadedEvent;

        public DebuggingDisplay()
        {
            InitializeComponent();

            Events.ProcAttach += Events_ProcAttach;
            Events.ProcDetach += Events_ProcDetach;
            Events.ProcDie += Events_ProcDie;
            Events.TargetStateChanged += Events_TargetStateChanged;
            Events.DBTouched += Events_DBTouched;
            Events.SelectedTargetChanged += Events_SelectedTargetChanged;

            Task.Run(RefreshView);
        }

        #region Control Management

        private async Task RefreshView()
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Abort if there is no saved target.
            if (currentTarget == null)
                return;

            // If the current program stat matches the target state we do nothing here.
            var newTargetState = currentTarget.MutableInfo.Status;
            if (_currentState == newTargetState)
                return;

            // Update the targets state to reflect the new changes.
            _currentState = newTargetState;

            // Change the program for the new state of the target.
            switch (newTargetState)
            {
                default:
                    break;

                case TargetStatusType.Offline:
                case TargetStatusType.Online:

                    Dispatcher.Invoke(() =>
                    {
                        AttachProcess.IsEnabled = false;
                        LoadSomething.IsEnabled = true;
                        RestartTarget.IsEnabled = false;
                        ShutdownTarget.IsEnabled = false;

                        DetachProcess.IsEnabled = false;
                        KillProcess.IsEnabled = false;

                        CurrentDebuggingProccess.FieldText = "N/A";
                    });

                    break;

                case TargetStatusType.APIAvailable:

                    Dispatcher.Invoke(() =>
                    {
                        AttachProcess.IsEnabled = true;
                        LoadSomething.IsEnabled = true;
                        RestartTarget.IsEnabled = true;
                        ShutdownTarget.IsEnabled = true;

                        DetachProcess.IsEnabled = false;
                        KillProcess.IsEnabled = false;

                        CurrentDebuggingProccess.FieldText = "N/A";
                    });

                    break;

                case TargetStatusType.DebuggingActive:

                    Dispatcher.Invoke(() =>
                    {
                        AttachProcess.IsEnabled = true;
                        LoadSomething.IsEnabled = true;
                        RestartTarget.IsEnabled = true;
                        ShutdownTarget.IsEnabled = true;

                        DetachProcess.IsEnabled = true;
                        KillProcess.IsEnabled = true;
                    });
                    

                    (var result, var procInfo) = await currentTarget.Debug.GetCurrentProcess();

                    if (result.Succeeded && procInfo.ProcessId != -1)
                    {
                        // Set the current debugging process name and PID.
                        Dispatcher.Invoke(() => CurrentDebuggingProccess.FieldText = $"{procInfo.Name}({procInfo.ProcessId})");
                    }

                    break;
            }
        }

        #endregion

        #region Events

        private async void Events_SelectedTargetChanged(object? sender, SelectedTargetChangedEvent e)
        {
            await RefreshView();
        }

        private async void Events_DBTouched(object? sender, DBTouchedEvent e)
        {
            await RefreshView();
        }

        private async void Events_TargetStateChanged(object? sender, TargetStateChangedEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Make sure a target is set.
            if (currentTarget== null)
                return;

            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != currentTarget.IPAddress)
                return;

            await RefreshView();
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
            await RefreshView();
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
            await RefreshView();
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
            await RefreshView();
        }

        #endregion

        #region Buttons

        private async void AttachProcess_Click(object sender, RoutedEventArgs e)
        {
            await SelectProcess.ShowDialog(Window.GetWindow(this));
        }

        private async void DetachProcess_Click(object sender, RoutedEventArgs e)
        {
            await TargetManager.SelectedTarget.Debug.Detach();
        }

        private void LoadSomething_Click(object sender, RoutedEventArgs e)
        {
            OrbisLib2.Dialog.LoadSomething.ShowDialog(Window.GetWindow(this));
        }

        private async void KillProcess_Click(object sender, RoutedEventArgs e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Get the process list.
            (var result, var processList) = await TargetManager.SelectedTarget.GetProcList();

            // If we failed abort here and display why.
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), $"Could not kill process: {result.ErrorMessage}", "Failed to kill proceess");
                return;
            }

            // Get the current debugging pid.
            (result, var processId) = await currentTarget.Debug.GetCurrentProcessId();

            // If we failed abort here and display why.
            if (!result.Succeeded || processId == -1)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), $"Could not kill process: {result.ErrorMessage}", "Failed to kill proceess");
                return;
            }

            // Find the process in the list if not abort.
            var process = processList.Find(x => x.ProcessId == processId);
            if (process == null)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), $"Could not kill process: Process {processId} does not exist. Is it already dead?", "Failed to kill proceess");
                return;
            }

            // We currently only can kill using the app Id so alert the user we cant kill this.
            if (process.AppId <= 0)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), $"Could not kill process \"{process.Name}\" because Orbis Suite doesnt currently support killing processes with out an appId.", "Error: Could not kill this process.");
                return;
            }

            // Kill the target process.
            await currentTarget.Application.Stop(process.TitleId);
        }

        private async void RestartTarget_Click(object sender, RoutedEventArgs e)
        {
            var currentTarget = TargetManager.SelectedTarget;
            var dialogResult = SimpleMessageBox.ShowInformation(
                Window.GetWindow(this),
                $"Are you sure you want to reboot {currentTarget.Name}?",
                "Are you sure?",
                SimpleUI.SimpleMessageBoxButton.YesNo);

            if (dialogResult == SimpleUI.SimpleMessageBoxResult.Yes)
            {
                var result = await currentTarget.Reboot();
                if (!result.Succeeded)
                {
                    SimpleMessageBox.ShowError(Window.GetWindow(this), $"Could not restart target {result.ErrorMessage}.", "Failed to restart target.");
                }
            }
        }

        private async void ShutdownTarget_Click(object sender, RoutedEventArgs e)
        {
            var currentTarget = TargetManager.SelectedTarget;
            var dialogResult = SimpleMessageBox.ShowInformation(
                Window.GetWindow(this),
                $"Are you sure you want to shutdown {currentTarget.Name}?",
                "Are you sure?",
                SimpleUI.SimpleMessageBoxButton.YesNo);

            if (dialogResult == SimpleUI.SimpleMessageBoxResult.Yes)
            {
                var result = await TargetManager.SelectedTarget.Shutdown();
                if (!result.Succeeded)
                {
                    SimpleMessageBox.ShowError(Window.GetWindow(this), $"Could not shutdown target {result.ErrorMessage}.", "Failed to shutdown target.");
                }
            }
        }

        #endregion
    }
}
