using Microsoft.Win32;
using OrbisLib2.Dialog;
using OrbisLib2.Targets;
using SimpleUI.Controls;
using System.IO;
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
        }

        #region Control Management

        private async Task RefreshView(bool state, bool attached)
        {
            var currentTarget = TargetManager.SelectedTarget;

            if (attached)
            {
                (var result, var procInfo) = await currentTarget.Debug.GetCurrentProcess();

                if (result.Succeeded && procInfo.ProcessId != -1)
                {
                    // Set the current debugging process name and PID.
                    Dispatcher.Invoke(() => CurrentDebuggingProccess.FieldText = $"{procInfo.Name}({procInfo.ProcessId})");
                }
            }
            else
            {
                Dispatcher.Invoke(() => CurrentDebuggingProccess.FieldText = "N/A");
            }

            Dispatcher.Invoke(() =>
            {
                AttachProcess.IsEnabled = state;
                LoadSomething.IsEnabled = state;
                RestartTarget.IsEnabled = state;
                ShutdownTarget.IsEnabled = state;

                DetachProcess.IsEnabled = attached;
                KillProcess.IsEnabled = attached;
            });
        }

        #endregion

        #region Events

        private async void Events_SelectedTargetChanged(object? sender, SelectedTargetChangedEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Disable the attached options.
            await RefreshView(currentTarget.Info.Status == TargetStatusType.APIAvailable, await currentTarget.Debug.IsDebugging());
        }

        private async void Events_DBTouched(object? sender, DBTouchedEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Disable the attached options.
            await RefreshView(currentTarget.Info.Status == TargetStatusType.APIAvailable, await currentTarget.Debug.IsDebugging());
        }

        private async void Events_TargetStateChanged(object? sender, TargetStateChangedEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;
            if (e.Name != currentTarget.Name)
                return;

            switch (e.State)
            {
                case TargetStateChangedEvent.TargetState.APIAvailable:
                    await RefreshView(true, await currentTarget.Debug.IsDebugging());
                    break;
                case TargetStateChangedEvent.TargetState.APIUnAvailable:
                    await RefreshView(false, await currentTarget.Debug.IsDebugging());
                    break;
            }
        }

        private async void Events_ProcDie(object? sender, ProcDieEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != currentTarget.IPAddress)
                return;

            // Disable the attached options.
            await RefreshView(currentTarget.Info.Status == TargetStatusType.APIAvailable, false);
        }

        private async void Events_ProcDetach(object? sender, ProcDetachEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != currentTarget.IPAddress)
                return;

            // Disable the attached options.
            await RefreshView(currentTarget.Info.Status == TargetStatusType.APIAvailable, false);
        }

        private async void Events_ProcAttach(object? sender, ProcAttachEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != currentTarget.IPAddress)
                return;

            // Enable the attached options.
            await RefreshView(currentTarget.Info.Status == TargetStatusType.APIAvailable, true);
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

            //try
            //{
            //    var binaryPath = string.Empty;
            //    var openFileDialog = new OpenFileDialog();
            //
            //    openFileDialog.Title = "Open File...";
            //    openFileDialog.CheckFileExists = true;
            //    openFileDialog.CheckPathExists = true;
            //    openFileDialog.InitialDirectory = Properties.Settings.Default.LastLoadPath;
            //    openFileDialog.Filter = "Binary files (*.sprx, *.bin, *.elf)|*.sprx;*.bin;*.elf";
            //    openFileDialog.FilterIndex = 1; // Set the default filter index to the first one (sprx files)
            //    openFileDialog.RestoreDirectory = true;
            //
            //    if (openFileDialog.ShowDialog() == true)
            //    {
            //        binaryPath = openFileDialog.FileName;
            //        Properties.Settings.Default.LastLoadPath = Path.GetDirectoryName(openFileDialog.FileName);
            //        Properties.Settings.Default.Save();
            //    }
            //    else
            //        return;
            //
            //    // Open a handle to the file.
            //    var binaryFile = File.Open(binaryPath, FileMode.Open);
            //    if (!binaryFile.CanRead)
            //    {
            //        SimpleMessageBox.ShowError(Window.GetWindow(this), "Failed read the file from the disk.", "Error: Failed to read the file.");
            //        binaryFile.Close();
            //        return;
            //    }
            //
            //    // Read the binary file data.
            //    byte[] PayloadBuffer = new byte[binaryFile.Length];
            //    if (binaryFile.Read(PayloadBuffer, 0, (int)binaryFile.Length) != binaryFile.Length)
            //    {
            //        SimpleMessageBox.ShowError(Window.GetWindow(this), "Failed read the file from the disk.", "Error: Failed to read the file.");
            //        binaryFile.Close();
            //        return;
            //    }
            //
            //    // Close the file handle were done reading it now.
            //    binaryFile.Close();
            //
            //    var fileExtension = Path.GetExtension(binaryPath).ToLower();
            //    if (fileExtension == ".sprx")
            //    {
            //        var tempSprxPath = $"/data/{Path.GetFileName(binaryPath)}";
            //        var result = await TargetManager.SelectedTarget.SendFile(PayloadBuffer, tempSprxPath);
            //
            //        if (!result.Succeeded)
            //        {
            //            SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
            //            return;
            //        }
            //
            //        // Get the Library list so we can check if its loaded already.
            //        (result, var libraryList) = await TargetManager.SelectedTarget.Debug.GetLibraries();
            //        if (!result.Succeeded)
            //        {
            //            SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
            //            return;
            //        }
            //
            //        // Search for the library in the list and unload the sprx if already loaded.
            //        var library = libraryList.Find(x => x.Path == tempSprxPath);
            //        if (library != null)
            //        {
            //            // Try to unload the library.
            //            (result, var handle) = await TargetManager.SelectedTarget.Debug.ReloadLibrary((int)library.Handle, tempSprxPath);
            //
            //            // If we failed abort here.
            //            if (!result.Succeeded)
            //            {
            //                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
            //                return;
            //            }
            //
            //            SprxLoadedEvent?.Invoke(this, new SprxLoadedEventArgs(handle));
            //
            //            SimpleMessageBox.ShowInformation(Window.GetWindow(this), $"The sprx {Path.GetFileName(binaryPath)} has been reloaded with handle {handle}.", "SPRX has been reloaded!");
            //        }
            //        else
            //        {
            //            (result, int handle) = await TargetManager.SelectedTarget.Debug.LoadLibrary(tempSprxPath);
            //
            //            if (!result.Succeeded || handle == -1)
            //            {
            //                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
            //                return;
            //            }
            //
            //            SprxLoadedEvent?.Invoke(this, new SprxLoadedEventArgs(handle));
            //
            //            SimpleMessageBox.ShowInformation(Window.GetWindow(this), $"The sprx {Path.GetFileName(binaryPath)} has been loaded with handle {handle}.", "SPRX has been loaded!");
            //
            //            // Remove the temp file.
            //            await TargetManager.SelectedTarget.DeleteFile(tempSprxPath);
            //        }
            //    }
            //    else if (fileExtension == ".bin")
            //    {
            //        if (!await TargetManager.SelectedTarget.Payload.InjectPayload(PayloadBuffer))
            //        {
            //            SimpleMessageBox.ShowError(Window.GetWindow(this), "Failed to send payload to target please try again.", "Error: Failed to inject payload.");
            //            return;
            //        }
            //
            //        PayloadLoadedEvent?.Invoke(this, EventArgs.Empty);
            //
            //        SimpleMessageBox.ShowInformation(Window.GetWindow(this), "The payload has been sucessfully sent.", "Payload Sent!");
            //    }
            //    else if (fileExtension == ".elf")
            //    {
            //        ElfLoadedEvent?.Invoke(this, EventArgs.Empty);
            //
            //        SimpleMessageBox.ShowInformation(Window.GetWindow(this), "ELF's are not currently supported.", "Failed to send ELF.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SimpleMessageBox.ShowError(Window.GetWindow(this), ex.Message, "Error: Failed to load binary.");
            //}
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
