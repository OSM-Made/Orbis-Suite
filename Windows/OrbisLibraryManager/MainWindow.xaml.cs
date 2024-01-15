using OrbisLib2.Common.Dispatcher;
using OrbisLib2.General;
using OrbisLib2.Targets;
using SimpleUI.Controls;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows.Threading;
using OrbisLib2.Common.Database.Types;

namespace OrbisLibraryManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SimpleWindow
    {
        private ILogger _logger;
        private TargetStatusType _currentState = TargetStatusType.None;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Show(ILogger logger)
        {
            base.Show();

            _logger = logger;

            // Subscribe to the target events feed.
            DispatcherClient.Subscribe(_logger);

            // Register events.
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
                    await UpdateProgramState();
                }
            });

            // Load settings.
            SPRXPath.FieldText = Properties.Settings.Default.SPRXPath;
            HideProcessBinaries.IsChecked = Properties.Settings.Default.HideProcessBinaries;
            HideSystemLibraries.IsChecked = Properties.Settings.Default.HideSystemLibraries;
        }

        private async Task RefreshLibraryList()
        {
            (var result, var libraryList) = await TargetManager.SelectedTarget.Debug.GetLibraries();

            // Check to see if the action succeeded.
            if (!result.Succeeded || libraryList == null || libraryList.Count == 0)
            {
                Dispatcher.Invoke(() =>
                {
                    LibraryList.ItemsSource = null;
                });

                return;
            }

            // Check for exclusions.
            if (Properties.Settings.Default.HideProcessBinaries)
                libraryList.RemoveAll(x => x.Path.Contains("/app0/"));

            if (Properties.Settings.Default.HideSystemLibraries)
                libraryList.RemoveAll(x => x.Path.Contains("/lib/"));

            // Make sure after exclusions we have some members left.
            if (libraryList.Count == 0)
            {
                Dispatcher.Invoke(() =>
                {
                    LibraryList.ItemsSource = null;
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    // Make the scroll bar move to the right a bit more make it look less awkward.
                    if (libraryList.Count >= 18)
                        LibraryList.Margin = new Thickness(6, 6, 3, 6);
                    else
                        LibraryList.Margin = new Thickness(6);

                    // Fill and refresh the list view.
                    LibraryList.ItemsSource = libraryList;
                    LibraryList.Items.Refresh();
                });
            }
        }

        #region Events

        private async Task UpdateProgramState()
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Abort if there is no saved targets.
            if (currentTarget == null)
                return;

            // If the current program stat matches the target state we do nothing here.
            var newTargetState = currentTarget.MutableInfo.Status;
            if (_currentState == newTargetState)
                return;

            _logger.LogInformation($"Target state has changed from {_currentState} to {newTargetState}.");

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
                        LoadPRX.IsEnabled = false;
                        UnloadPRX.IsEnabled = false;
                        ReloadPRX.IsEnabled = false;

                        RefreshLibraries.IsEnabled = false;
                        UnloadLibrary.IsEnabled = false;
                        ReloadLibrary.IsEnabled = false;
                    });

                    break;

                case TargetStatusType.APIAvailable:

                    Dispatcher.Invoke(() =>
                    {
                        LoadPRX.IsEnabled = false;
                        UnloadPRX.IsEnabled = false;
                        ReloadPRX.IsEnabled = false;

                        RefreshLibraries.IsEnabled = false;
                        UnloadLibrary.IsEnabled = false;
                        ReloadLibrary.IsEnabled = false;

                        LibraryList.ItemsSource = null;
                    });

                    break;

                case TargetStatusType.DebuggingActive:

                    Dispatcher.Invoke(() =>
                    {
                        LoadPRX.IsEnabled = true;
                        UnloadPRX.IsEnabled = true;
                        ReloadPRX.IsEnabled = true;

                        RefreshLibraries.IsEnabled = true;
                        UnloadLibrary.IsEnabled = true;
                        ReloadLibrary.IsEnabled = true;
                    });

                    await RefreshLibraryList();

                    break;
            }
        }

        private async void Events_TargetStateChanged(object? sender, TargetStateChangedEvent e)
        {
            var currentTarget = TargetManager.SelectedTarget;

            // Make sure a target is set.
            if (currentTarget == null)
                return;

            // Only accept events for the selected target.
            if (e.SendingTarget.IPAddress != currentTarget.IPAddress)
                return;

            await UpdateProgramState();
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
            await UpdateProgramState();
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
            await UpdateProgramState();
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
            await UpdateProgramState();
        }

        private async void Events_DBTouched(object? sender, DBTouchedEvent e)
        {
            await UpdateProgramState();
        }

        private async void Events_SelectedTargetChanged(object? sender, SelectedTargetChangedEvent e)
        {
            await UpdateProgramState();
        }

        #endregion

        #region Context Menu

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshLibraryList();
        }

        private async void HideProcessBinaries_Click(object sender, RoutedEventArgs e)
        {
            HideProcessBinaries.IsChecked = Properties.Settings.Default.HideProcessBinaries = !Properties.Settings.Default.HideProcessBinaries;
            Properties.Settings.Default.Save();

            await RefreshLibraryList();
        }

        private async void HideSystemLibraries_Click(object sender, RoutedEventArgs e)
        {
            HideSystemLibraries.IsChecked = Properties.Settings.Default.HideSystemLibraries = !Properties.Settings.Default.HideSystemLibraries;
            Properties.Settings.Default.Save();

            await RefreshLibraryList();
        }
        private void CopyHandle_Click(object sender, RoutedEventArgs e)
        {
            var selectedLibrary = LibraryList.SelectedItems.Cast<LibraryInfo>().FirstOrDefault();
            if (selectedLibrary == null)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), "No binary selected.", "Failed to copy handle.");
                return;
            }

            Clipboard.SetText($"{selectedLibrary.Handle}");
        }

        private void CopyName_Click(object sender, RoutedEventArgs e)
        {
            var selectedLibrary = LibraryList.SelectedItems.Cast<LibraryInfo>().FirstOrDefault();
            if (selectedLibrary == null)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), "No binary selected.", "Failed to copy name.");
                return;
            }

            Clipboard.SetText($"{Path.GetFileName(selectedLibrary.Path)}");
        }

        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            var selectedLibrary = LibraryList.SelectedItems.Cast<LibraryInfo>().FirstOrDefault();
            if (selectedLibrary == null)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), "No binary selected.", "Failed to copy path.");
                return;
            }

            Clipboard.SetText(selectedLibrary.Path);
        }

        private void CopyTextSegment_Click(object sender, RoutedEventArgs e)
        {
            var selectedLibrary = LibraryList.SelectedItems.Cast<LibraryInfo>().FirstOrDefault();
            if (selectedLibrary == null)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), "No binary selected.", "Failed to copy Text Segment.");
                return;
            }

            Clipboard.SetText($"0x{selectedLibrary.MapBase.ToString("X")}");
        }

        private void CopyDataSegment_Click(object sender, RoutedEventArgs e)
        {
            var selectedLibrary = LibraryList.SelectedItems.Cast<LibraryInfo>().FirstOrDefault();
            if (selectedLibrary == null)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), "No binary selected.", "Failed to copy Data Segment.");
                return;
            }

            Clipboard.SetText($"0x{selectedLibrary.DataBase.ToString("X")}");
        }

        private async void UnloadLibrary_Click(object sender, RoutedEventArgs e)
        {
            var selectedLibrary = LibraryList.SelectedItems.Cast<LibraryInfo>().FirstOrDefault();
            if(selectedLibrary != null)
            {
                await TargetManager.SelectedTarget.Debug.UnloadLibrary((int)selectedLibrary.Handle);
                await RefreshLibraryList();
            }
        }

        private async void ReloadLibrary_Click(object sender, RoutedEventArgs e)
        {
            var selectedLibrary = LibraryList.SelectedItems.Cast<LibraryInfo>().FirstOrDefault();
            if (selectedLibrary != null)
            {
                (var result, var _) = await TargetManager.SelectedTarget.Debug.ReloadLibrary((int)selectedLibrary.Handle, selectedLibrary.Path);

                if (!result.Succeeded)
                {
                    SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, $"Failed to reload library {Path.GetFileName(selectedLibrary.Path)}.");
                    return;
                }

                await RefreshLibraryList();
            }
        }

        #endregion

        #region Buttons

        private async void LoadPRX_Click(object sender, RoutedEventArgs e)
        {
            // Pull down the sprx we are trying to load.
            string sprxPath = string.Empty;
            sprxPath = SPRXPath.FieldText;

            // Get the Library list so we can check if its loaded already.
            (var result, var libraryList) = await TargetManager.SelectedTarget.Debug.GetLibraries();
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, $"Failed to load library {Path.GetFileName(sprxPath)}.");
                return;
            }

            // Search for the library in the list.
            var library = libraryList.Find(x => x.Path == sprxPath);
            if (library != null)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), $"Could not load \"{sprxPath}\" since it is already loaded.", "Error: Failed to load SPRX.");
            }

            // Try to load the library.
            (result, var _) = await TargetManager.SelectedTarget.Debug.LoadLibrary(sprxPath);

            // If we failed abort here.
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, $"Failed to load library {Path.GetFileName(sprxPath)}.");
                return;
            }

            // On success reload the library list.
            await RefreshLibraryList();
        }

        private async void UnloadPRX_Click(object sender, RoutedEventArgs e)
        {
            // Pull down the sprx we are trying to unload.
            string sprxPath = string.Empty;
            sprxPath = SPRXPath.FieldText;

            // Get the Library list so we can check if its loaded already.
            (var result, var libraryList) = await TargetManager.SelectedTarget.Debug.GetLibraries();
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, $"Failed to unload library {Path.GetFileName(sprxPath)}.");
                return;
            }

            // Search for the library in the list.
            var library = libraryList.Find(x => x.Path == sprxPath);
            if (library == null)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), $"Could not unload \"{sprxPath}\" since it is not loaded.", "Error: Failed to unload SPRX.");
                return;
            }

            // Try to unload the library.
            result = await TargetManager.SelectedTarget.Debug.UnloadLibrary((int)library.Handle);

            // If we failed abort here.
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, $"Failed to unload library {Path.GetFileName(sprxPath)}.");
                return;
            }

            // On success reload the library list.
            await RefreshLibraryList();
        }

        private async void ReloadPRX_Click(object sender, RoutedEventArgs e)
        {
            // Pull down the sprx we are trying to reload.
            string sprxPath = string.Empty;
            sprxPath = SPRXPath.FieldText;

            // Get the Library list so we can check if its loaded already.
            (var result, var libraryList) = await TargetManager.SelectedTarget.Debug.GetLibraries();
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, $"Failed to reload library {Path.GetFileName(sprxPath)}.");
                return;
            }

            // Search for the library in the list.
            var library = libraryList.Find(x => x.Path == sprxPath);
            if (library == null)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), $"Could not reload \"{sprxPath}\" since it is not loaded.", "Error: Failed to reload SPRX.");
            }

            // Try to unload the library.
            (result, var _) = await TargetManager.SelectedTarget.Debug.ReloadLibrary((int)library.Handle, sprxPath);

            // If we failed abort here.
            if (!result.Succeeded)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, $"Failed to reload library {Path.GetFileName(sprxPath)}.");
                return;
            }

            // On success reload the library list.
            await RefreshLibraryList();
        }

        

        

        private void SPRXPath_LostFocus(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.SPRXPath = SPRXPath.FieldText;
            Properties.Settings.Default.Save();
        }

        #endregion
    }
}
