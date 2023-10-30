using Microsoft.Win32;
using OrbisLib2.Common.Database;
using OrbisLib2.Targets;
using SimpleUI.Controls;
using SimpleUI.Dialogs;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace OrbisLib2.Dialog
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
    /// Interaction logic for LoadSomething.xaml
    /// </summary>
    public partial class LoadSomething : SimpleDialog
    {
        public event EventHandler<SprxLoadedEventArgs> SprxLoadedEvent;
        public event EventHandler PayloadLoadedEvent;
        public event EventHandler ElfLoadedEvent;

        public LoadSomething(Window Owner)
            : base(Owner, "Browse", "Remote", "Cancel", "Select something to load")
        {
            InitializeComponent();

            RefreshHistory();
        }

        public static SimpleDialogResult ShowDialog(Window Owner)
        {
            var dlg = new LoadSomething(Owner);
            dlg.ShowDialog();
            return dlg.Result;
        }

        #region History

        private async Task SaveToHistory(string path, bool isRemote)
        {
            // Check if we have saved this before, if we do update the last loaded time.
            var previousEntry = BinaryHistory.Find(x => x.Path == path);
            if (previousEntry != null)
            {
                previousEntry.LastLoaded = DateTime.UtcNow;
                previousEntry.Save();
                return;
            }

            if (Path.GetExtension(path) != ".bin")
            {
                // Get the process information.
                (var result, var procInfo) = await TargetManager.SelectedTarget.Debug.GetCurrentProcess();

                if (!result.Succeeded || procInfo == null || procInfo.ProcessId == -1)
                    return;

                var entry = new BinaryHistory() { IsRemote = isRemote, Path = path, TitleId = procInfo.TitleId, LastLoaded = DateTime.UtcNow };
                entry.Add();
            }
            else
            {
                var entry = new BinaryHistory() { IsRemote = isRemote, Path = path, TitleId = "N/A", LastLoaded = DateTime.UtcNow };
                entry.Add();
            }
            
        }

        private void RefreshHistory()
        {
            HistoryList.ItemsSource = BinaryHistory.AsList();
            HistoryList.Items.Refresh();
        }

        #endregion

        #region Loaders

        private async Task LoadSomethingRemote(string path)
        {
            // Save to history.
            await SaveToHistory(path, true);

            switch (Path.GetExtension(path).ToLower())
            {
                case ".sprx":
                    {
                        // Strip out the ftp part of the path.
                        var key = ":2121";
                        var tempSprxPath = (path.Contains(key)) ? path.Substring(path.IndexOf(key) + key.Length) : path;

                        // Get the Library list so we can check if its loaded already.
                        (var result, var libraryList) = await TargetManager.SelectedTarget.Debug.GetLibraries();
                        if (!result.Succeeded)
                        {
                            SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
                            return;
                        }

                        // Search for the library in the list and unload the sprx if already loaded.
                        var library = libraryList.Find(x => x.Path == tempSprxPath);
                        if (library != null)
                        {
                            // Try to unload the library.
                            (result, var handle) = await TargetManager.SelectedTarget.Debug.ReloadLibrary((int)library.Handle, tempSprxPath);

                            // If we failed abort here.
                            if (!result.Succeeded)
                            {
                                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
                                return;
                            }

                            SprxLoadedEvent?.Invoke(this, new SprxLoadedEventArgs(handle));

                            SimpleMessageBox.ShowInformation(Window.GetWindow(this), $"The sprx {Path.GetFileName(path)} has been reloaded with handle {handle}.", "SPRX has been reloaded!");
                        }
                        else
                        {
                            (result, int handle) = await TargetManager.SelectedTarget.Debug.LoadLibrary(tempSprxPath);

                            if (!result.Succeeded || handle == -1)
                            {
                                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
                                return;
                            }

                            SprxLoadedEvent?.Invoke(this, new SprxLoadedEventArgs(handle));

                            SimpleMessageBox.ShowInformation(Window.GetWindow(this), $"The sprx {Path.GetFileName(path)} has been loaded with handle {handle}.", "SPRX has been loaded!");

                            // Remove the temp file.
                            await TargetManager.SelectedTarget.DeleteFile(tempSprxPath);
                        }

                        break;
                    }

                case ".bin":
                    {
                        SimpleMessageBox.ShowInformation(Window.GetWindow(this), "Payload's are not currently supported from remote.", "Failed to send Payload.");

                        break;
                    }

                case ".elf":
                    {
                        ElfLoadedEvent?.Invoke(this, EventArgs.Empty);

                        SimpleMessageBox.ShowInformation(Window.GetWindow(this), "ELF's are not currently supported.", "Failed to send ELF.");

                        break;
                    }
            }
        }

        private async Task<bool> LoadSomethingLocal(string path)
        {
            // Save to history.
            await SaveToHistory(path, false);

            // Get the file data.
            var binaryData = await File.ReadAllBytesAsync(path);

            switch (Path.GetExtension(path).ToLower())
            {
                case ".sprx":
                    {
                        // Send the sprx to a temporary path.
                        var tempSprxPath = $"/data/Orbis Suite/{Path.GetFileName(path)}";
                        var result = await TargetManager.SelectedTarget.SendFile(binaryData, tempSprxPath);

                        if (!result.Succeeded)
                        {
                            SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
                            return false;
                        }

                        // Get the Library list so we can check if its loaded already.
                        (result, var libraryList) = await TargetManager.SelectedTarget.Debug.GetLibraries();
                        if (!result.Succeeded)
                        {
                            SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
                            return false;
                        }

                        // Search for the library in the list and unload the sprx if already loaded.
                        var library = libraryList.Find(x => x.Path == tempSprxPath);
                        if (library != null)
                        {
                            // Try to unload the library.
                            (result, var handle) = await TargetManager.SelectedTarget.Debug.ReloadLibrary((int)library.Handle, tempSprxPath);

                            // If we failed abort here.
                            if (!result.Succeeded)
                            {
                                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
                                return false;
                            }

                            SprxLoadedEvent?.Invoke(this, new SprxLoadedEventArgs(handle));
                        }
                        else
                        {
                            (result, int handle) = await TargetManager.SelectedTarget.Debug.LoadLibrary(tempSprxPath);

                            if (!result.Succeeded || handle == -1)
                            {
                                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
                                return false;
                            }

                            SprxLoadedEvent?.Invoke(this, new SprxLoadedEventArgs(handle));

                            // Remove the temp file.
                            await TargetManager.SelectedTarget.DeleteFile(tempSprxPath);
                        }

                        break;
                    }

                case ".bin":
                    {
                        if (!await TargetManager.SelectedTarget.Payload.InjectPayload(binaryData))
                        {
                            SimpleMessageBox.ShowError(Window.GetWindow(this), "Failed to send payload to target please try again.", "Error: Failed to inject payload.");
                            return false;
                        }

                        PayloadLoadedEvent?.Invoke(this, EventArgs.Empty);

                        break;
                    }

                case ".elf":
                    {
                        ElfLoadedEvent?.Invoke(this, EventArgs.Empty);

                        // TODO: Support ELF loading.
                        SimpleMessageBox.ShowInformation(Window.GetWindow(this), "ELF's are not currently supported.", "Failed to send ELF.");
                        return false;
                        // break;
                    }
            }

            return true;
        }

        private async Task LoadHisctoric(BinaryHistory item)
        {
            if (Path.GetExtension(item.Path) != ".bin")
            {
                // Get the process information.
                (var result, var procInfo) = await TargetManager.SelectedTarget.Debug.GetCurrentProcess();

                if (!result.Succeeded)
                {
                    SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Failed to get process info");
                    return;
                }

                // Make sure since this isnt the process we loaded this on last we want to do this.
                if (procInfo.TitleId != item.TitleId)
                {
                    var dialogResult = SimpleMessageBox.ShowInformation(
                        Window.GetWindow(this),
                        $"Are you sure you want to load {Path.GetFileName(item.Path)} on {procInfo.TitleId} it was last loaded on {item.TitleId}.",
                        "Are you sure?",
                        SimpleUI.SimpleMessageBoxButton.YesNo);

                    if (dialogResult != SimpleUI.SimpleMessageBoxResult.Yes)
                        return;
                }
            }

            // Load the item.
            var loadedSomething = false;
            if (item.IsRemote)
                await LoadSomethingRemote(item.Path);
            else
                loadedSomething = await LoadSomethingLocal(item.Path);

            // Close the window if we loaded something.
            if (loadedSomething)
                Close();
        }

        #endregion

        #region Buttons

        private async void HistoryList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as BinaryHistory;

            if (item == null || HistoryList.SelectedItem == null)
                return;

            await LoadHisctoric(item);
        }

        // Browse on PC
        public override async void Button1_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Open File...";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.InitialDirectory = Properties.Settings.Default.LastLoadPath;
            openFileDialog.Filter = "Binary files (*.sprx, *.bin, *.elf)|*.sprx;*.bin;*.elf";
            openFileDialog.FilterIndex = 1; // Set the default filter index to the first one (sprx files)
            openFileDialog.RestoreDirectory = true;

            // Make sure we found something and didnt cancel the dialog.
            if ((bool)!openFileDialog.ShowDialog())
                return;

            // Save the file path for the next time we look for something to load.
            var binaryPath = openFileDialog.FileName;
            Properties.Settings.Default.LastLoadPath = Path.GetDirectoryName(openFileDialog.FileName);
            Properties.Settings.Default.Save();

            // Try to load something.
            var loadedSomething = await LoadSomethingLocal(binaryPath);

            // Call the original if we loaded something.
            if (loadedSomething)
                base.Button1_Click(sender, e);
        }

        // Browse on Target
        public override async void Button2_Click(object sender, RoutedEventArgs e)
        {
            //var currentTarget = TargetManager.SelectedTarget;
            //var openFileDialog = new OpenFileDialog();
            //
            //openFileDialog.Title = "Open File...";
            //openFileDialog.CheckFileExists = true;
            //openFileDialog.CheckPathExists = true;
            //openFileDialog.InitialDirectory = $"ftp://anonymous:anonymous@{currentTarget.IPAddress}:2121";
            //openFileDialog.Filter = "Binary files (*.sprx, *.bin, *.elf)|*.sprx;*.bin;*.elf";
            //openFileDialog.FilterIndex = 1; // Set the default filter index to the first one (sprx files)
            //openFileDialog.RestoreDirectory = true;
            //
            //// Make sure we found something and didnt cancel the dialog.
            //if ((bool)!openFileDialog.ShowDialog())
            //    return;
            //
            //// Save the file path for the next time we look for something to load.
            //var binaryPath = openFileDialog.FileName;
            //
            //// Try to load something.
            //await LoadSomethingRemote(binaryPath);
            //
            //// Call the original.
            //base.Button2_Click(sender, e);
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryList.SelectedItem == null)
                return;

            var item = HistoryList.SelectedItem as BinaryHistory;

            if (item == null)
                return;

            await LoadHisctoric(item);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryList.SelectedItem == null)
                return;

            var item = HistoryList.SelectedItem as BinaryHistory;

            if (item == null)
                return;

            // Try to remove it from the db.
            if (!item.Remove())
            {
                SimpleMessageBox.ShowInformation(Window.GetWindow(this), "Failed to remove historic binary from db.", "Failed to remove historic binary.");
                return;
            }

            // Update the list.
            RefreshHistory();
        }

        #endregion

    }
}
