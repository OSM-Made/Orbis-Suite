using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using OrbisNeighborHood.MVVM.ViewModel;
using SimpleUI.Controls;
using OrbisLib2.Common.Database.Types;
using OrbisLib2.Common.API;
using OrbisLib2.Targets;
using OrbisLib2.Common.Database;
using System.Threading.Tasks;

namespace OrbisNeighborHood.Controls
{
    /// <summary>
    /// Interaction logic for TargetView.xaml
    /// </summary>
    public partial class TargetPanel : UserControl
    {
        private readonly Target _thisTarget;

        public event EventHandler<RoutedEventArgs>? TargetChanged;

        public TargetPanel(string TargetName)
        {
            InitializeComponent();

            _thisTarget = TargetManager.GetTarget(TargetName);
            if(_thisTarget != null )
            {
                this.TargetName = _thisTarget.Name;
                TargetStatus = _thisTarget.Info.Status;
                ConsoleModel = _thisTarget.Info.ModelType;
                IsDefault = _thisTarget.IsDefault;
                FirmwareVersion = _thisTarget.Info.SoftwareVersion;
                SDKVersion = _thisTarget.Info.SDKVersion;
                IPAddress = _thisTarget.IPAddress;
                ConsoleName = _thisTarget.Info.ConsoleName;
                PayloadPort = _thisTarget.PayloadPort.ToString();

                LocateTarget.IsEnabled = _thisTarget.Info.IsAPIAvailable;
                SendPayload.IsEnabled = _thisTarget.Info.IsAvailable;
                RestartTarget.IsEnabled = _thisTarget.Info.IsAPIAvailable;
                ShutdownTarget.IsEnabled = _thisTarget.Info.IsAPIAvailable;
                SuspendTarget.IsEnabled = _thisTarget.Info.IsAPIAvailable;
            }
            else
            {
                throw new Exception("TargetPanel(): Target not found when it should have been!");
            }    
        }

        #region Properties

        private string TargetName
        {
            get { return (string)GetValue(TargetNameProperty); }
            set { SetValue(TargetNameProperty, value); }
        }

        private static readonly DependencyProperty TargetNameProperty =
            DependencyProperty.Register("TargetName", typeof(string), typeof(TargetPanel), new PropertyMetadata(string.Empty));

        private TargetStatusType TargetStatus
        {
            get { return (TargetStatusType)GetValue(TargetStatusProperty); }
            set { SetValue(TargetStatusProperty, value); }
        }

        private static readonly DependencyProperty TargetStatusProperty =
            DependencyProperty.Register("TargetStatus", typeof(TargetStatusType), typeof(TargetPanel), new PropertyMetadata(TargetStatusType.None, TargetStatusProperty_Changed));

        private static void TargetStatusProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            switch ((TargetStatusType)e.NewValue)
            {
                case TargetStatusType.Offline:
                    ((TargetPanel)d).TargetStatusElement.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    ((TargetPanel)d).TargetStatusElement.ToolTip = "Offline";
                    break;

                case TargetStatusType.Online:
                    ((TargetPanel)d).TargetStatusElement.Fill = new SolidColorBrush(Color.FromRgb(255, 140, 0));
                    ((TargetPanel)d).TargetStatusElement.ToolTip = "Online";
                    break;

                case TargetStatusType.APIAvailable:
                    ((TargetPanel)d).TargetStatusElement.Fill = new SolidColorBrush(Color.FromRgb(0, 128, 0));
                    ((TargetPanel)d).TargetStatusElement.ToolTip = "Online & API Available";
                    break;

                default:
                    ((TargetPanel)d).TargetStatusElement.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    ((TargetPanel)d).TargetStatusElement.ToolTip = "Unknown";
                    break;
            }
        }

        private bool IsDefault
        {
            get { return (bool)GetValue(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        private static readonly DependencyProperty IsDefaultProperty =
            DependencyProperty.Register("IsDefault", typeof(bool), typeof(TargetPanel), new PropertyMetadata(false, IsDefaultProperty_Changed));

        private static void IsDefaultProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ((TargetPanel)d).DefaultTargetElement.Foreground = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                ((TargetPanel)d).DefaultTargetElement.Cursor = Cursors.Arrow;
            }
                
            else
            {
                ((TargetPanel)d).DefaultTargetElement.Foreground = new SolidColorBrush(Color.FromRgb(69, 73, 74));
                ((TargetPanel)d).DefaultTargetElement.Cursor = Cursors.Hand;
            }
        }

        private ConsoleModelType ConsoleModel
        {
            get { return (ConsoleModelType)GetValue(ConsoleModelProperty); }
            set { SetValue(ConsoleModelProperty, value); }
        }

        private static readonly DependencyProperty ConsoleModelProperty =
            DependencyProperty.Register("ConsoleModel", typeof(ConsoleModelType), typeof(TargetPanel), new PropertyMetadata(ConsoleModelType.Unknown, ConsoleModelProperty_Changed));

        private static void ConsoleModelProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            switch ((ConsoleModelType)e.NewValue)
            {
                case ConsoleModelType.Fat:
                    ((TargetPanel)d).ConsoleImageElement.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisNeighborHood;component/Images/Consoles/Fat.png"));
                    break;

                case ConsoleModelType.Slim:
                    ((TargetPanel)d).ConsoleImageElement.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisNeighborHood;component/Images/Consoles/Slim.png"));
                    break;

                case ConsoleModelType.Pro:
                    ((TargetPanel)d).ConsoleImageElement.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisNeighborHood;component/Images/Consoles/Pro.png"));
                    break;

                default:
                    ((TargetPanel)d).ConsoleImageElement.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisNeighborHood;component/Images/Consoles/Fat.png"));
                    break;
            }
        }

        private string FirmwareVersion
        {
            get { return (string)GetValue(FirmwareVersionProperty); }
            set { SetValue(FirmwareVersionProperty, value); }
        }

        public static readonly DependencyProperty FirmwareVersionProperty =
            DependencyProperty.Register("FirmwareVersion", typeof(string), typeof(TargetPanel), new PropertyMetadata(string.Empty));

        private string SDKVersion
        {
            get { return (string)GetValue(SDKVersionProperty); }
            set { SetValue(SDKVersionProperty, value); }
        }

        private static readonly DependencyProperty SDKVersionProperty =
            DependencyProperty.Register("SDKVersion", typeof(string), typeof(TargetPanel), new PropertyMetadata(string.Empty));

        private string IPAddress
        {
            get { return (string)GetValue(IPAddressProperty); }
            set { SetValue(IPAddressProperty, value); }
        }

        private static readonly DependencyProperty IPAddressProperty =
            DependencyProperty.Register("IPAddress", typeof(string), typeof(TargetPanel), new PropertyMetadata(string.Empty));

        private string ConsoleName
        {
            get { return (string)GetValue(ConsoleNameProperty); }
            set { SetValue(ConsoleNameProperty, value); }
        }

        private static readonly DependencyProperty ConsoleNameProperty =
            DependencyProperty.Register("ConsoleName", typeof(string), typeof(TargetPanel), new PropertyMetadata(string.Empty));

        public string PayloadPort
        {
            get { return (string)GetValue(PayloadPortProperty); }
            set { SetValue(PayloadPortProperty, value); }
        }

        public static readonly DependencyProperty PayloadPortProperty =
            DependencyProperty.Register("PayloadPort", typeof(string), typeof(TargetPanel), new PropertyMetadata(string.Empty));

        #endregion

        #region Buttons

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var result = SimpleMessageBox.ShowWarning(Window.GetWindow(this), "Are you sure you want to delete this target?", "Delete this Target?", SimpleUI.SimpleMessageBoxButton.YesNo);
            if(result == SimpleUI.SimpleMessageBoxResult.Yes)
            {
                TargetManager.DeleteTarget(_thisTarget.Name);
                TargetChanged?.Invoke(this, e);
            }  
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (MainViewModel.Instance != null)
            {
                var editTargetViewModel = MainViewModel.Instance.EditTargetVM;
                editTargetViewModel.TargetChanged += EditTargetVM_TargetChanged;
                editTargetViewModel.CurrentTarget = _thisTarget.SavedTarget.Clone();
                editTargetViewModel.CallingVM = MainViewModel.Instance.TargetVM;
                MainViewModel.Instance.CurrentView = editTargetViewModel;
            }
        }

        private void EditTargetVM_TargetChanged(object? sender, RoutedEventArgs e)
        {
            TargetChanged?.Invoke(this, e);
        }

        private void DefaultTargetElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var savedTarget = _thisTarget.SavedTarget;
            if (!savedTarget.IsDefault)
            {
                savedTarget.IsDefault = true;
                savedTarget.Save();
                TargetChanged?.Invoke(this, e);
            }
        }

        private async void LocateTarget_Click(object sender, RoutedEventArgs e)
        {
            await _thisTarget.Buzzer(BuzzerType.RingThree);
        }

        private async void SendPayload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var binaryPath = string.Empty;
                var openFileDialog = new Microsoft.Win32.OpenFileDialog();

                openFileDialog.Title = "Open File...";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.InitialDirectory = Properties.Settings.Default.LastLoadPath;
                openFileDialog.Filter = "Binary files (*.sprx, *.bin, *.elf)|*.sprx;*.bin;*.elf";
                openFileDialog.FilterIndex = 1; // Set the default filter index to the first one (BIN files)
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    binaryPath = openFileDialog.FileName;
                    Properties.Settings.Default.LastLoadPath = Path.GetDirectoryName(openFileDialog.FileName);
                    Properties.Settings.Default.Save();
                }
                else
                    return;

                // Open a handle to the file.
                var binaryFile = File.Open(binaryPath, FileMode.Open);
                if (!binaryFile.CanRead)
                {
                    SimpleMessageBox.ShowError(Window.GetWindow(this), "Failed read the file from the disk.", "Error: Failed to read the file.");
                    binaryFile.Close();
                    return;
                }

                // Read the binary file data.
                byte[] PayloadBuffer = new byte[binaryFile.Length];
                if (binaryFile.Read(PayloadBuffer, 0, (int)binaryFile.Length) != binaryFile.Length)
                {
                    SimpleMessageBox.ShowError(Window.GetWindow(this), "Failed read the file from the disk.", "Error: Failed to read the file.");
                    binaryFile.Close();
                    return;
                }

                // Close the file handle were done reading it now.
                binaryFile.Close();

                var fileExtension = Path.GetExtension(binaryPath).ToLower();
                if (fileExtension == ".sprx")
                {
                    var tempSprxPath = $"/data/{Path.GetFileName(binaryPath)}";
                    var result = await TargetManager.SelectedTarget.SendFile(PayloadBuffer, tempSprxPath);

                    if (!result.Succeeded)
                    {
                        SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
                        return;
                    }

                    // Get the Library list so we can check if its loaded already.
                    (result, var libraryList) = await TargetManager.SelectedTarget.Debug.GetLibraries();
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

                        SimpleMessageBox.ShowInformation(Window.GetWindow(this), $"The sprx {Path.GetFileName(binaryPath)} has been reloaded with handle {handle}.", "SPRX has been reloaded!");
                    }
                    else
                    {
                        (result, int handle) = await TargetManager.SelectedTarget.Debug.LoadLibrary(tempSprxPath);

                        if (!result.Succeeded || handle == -1)
                        {
                            SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Error: Failed to load sprx.");
                            return;
                        }

                        SimpleMessageBox.ShowInformation(Window.GetWindow(this), $"The sprx {Path.GetFileName(binaryPath)} has been loaded with handle {handle}.", "SPRX has been loaded!");

                        // Remove the temp file.
                        await TargetManager.SelectedTarget.DeleteFile(tempSprxPath);
                    }
                }
                else if (fileExtension == ".bin")
                {
                    if (!await TargetManager.SelectedTarget.Payload.InjectPayload(PayloadBuffer))
                    {
                        SimpleMessageBox.ShowError(Window.GetWindow(this), "Failed to send payload to target please try again.", "Error: Failed to inject payload.");
                        return;
                    }

                    SimpleMessageBox.ShowInformation(Window.GetWindow(this), "The payload has been sucessfully sent.", "Payload Sent!");
                }
                else if (fileExtension == ".elf")
                {
                    SimpleMessageBox.ShowInformation(Window.GetWindow(this), "ELF's are not currently supported.", "Failed to send ELF.");
                }
            }
            catch (Exception ex)
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), ex.Message, "Error: Failed to load binary.");
            }
        }

        private async void RestartTarget_Click(object sender, RoutedEventArgs e)
        {
            await _thisTarget.Reboot();
        }

        private async void ShutdownTarget_Click(object sender, RoutedEventArgs e)
        {
            await _thisTarget.Shutdown();
        }

        private async void SuspendTarget_Click(object sender, RoutedEventArgs e)
        {
            await _thisTarget.Suspend();
        }

        #endregion
    }
}
