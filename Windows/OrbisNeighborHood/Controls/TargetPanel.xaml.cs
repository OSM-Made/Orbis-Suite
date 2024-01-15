using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OrbisNeighborHood.MVVM.ViewModel;
using SimpleUI.Controls;
using OrbisLib2.Common.Database.Types;
using OrbisLib2.Common.API;
using OrbisLib2.Targets;
using OrbisLib2.Dialog;
using OrbisLib2.General;

namespace OrbisNeighborHood.Controls
{
    /// <summary>
    /// Interaction logic for TargetView.xaml
    /// </summary>
    public partial class TargetPanel : UserControl
    {
        private readonly Target _thisTarget;

        public event EventHandler<RoutedEventArgs>? TargetChanged;

        public TargetPanel(string targetName)
        {
            InitializeComponent();

            _thisTarget = TargetManager.GetTarget(targetName);
            if(_thisTarget != null )
            {
                TargetName = _thisTarget.Name;
                TargetStatus = _thisTarget.MutableInfo.Status;
                ConsoleModel = _thisTarget.StaticInfo.ModelType;
                IsDefault = _thisTarget.IsDefault;
                FirmwareVersion = _thisTarget.MutableInfo.SoftwareVersion;
                SDKVersion = _thisTarget.MutableInfo.SdkVersion;
                IPAddress = _thisTarget.IPAddress;
                ConsoleName = _thisTarget.MutableInfo.ConsoleName;
                PayloadPort = _thisTarget.PayloadPort.ToString();

                var isApiAvailable = _thisTarget.MutableInfo.Status >= TargetStatusType.APIAvailable;
                var isNetworkAvailable = _thisTarget.MutableInfo.Status == TargetStatusType.Online || isApiAvailable;
                LocateTarget.IsEnabled = isApiAvailable;
                SendPayload.IsEnabled = isNetworkAvailable;
                RestartTarget.IsEnabled = isApiAvailable;
                ShutdownTarget.IsEnabled = isApiAvailable;
                SuspendTarget.IsEnabled = isApiAvailable;
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
                case TargetStatusType.DebuggingActive:
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

        private void SendPayload_Click(object sender, RoutedEventArgs e)
        {
            LoadSomething.ShowDialog(Window.GetWindow(this));
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
