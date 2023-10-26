using OrbisLib2.Common.Database.Types;
using OrbisLib2.Targets;
using SimpleUI.Dialogs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static SQLite.SQLite3;

namespace OrbisLib2.Dialog.Controls
{
    /// <summary>
    /// Interaction logic for MiniTargetPanel.xaml
    /// </summary>
    public partial class MiniTargetPanel : UserControl
    {
        private readonly Target _thisTarget;

        public event EventHandler<RoutedEventArgs>? TargetChanged;

        public MiniTargetPanel(string TargetName)
        {
            InitializeComponent();

            _thisTarget = TargetManager.GetTarget(TargetName);
            if (_thisTarget != null)
            {
                this.TargetName = _thisTarget.Name;
                TargetStatus = _thisTarget.Info.Status;
                ConsoleModel = _thisTarget.Info.ModelType;
                IsDefault = _thisTarget.IsDefault;
                FirmwareVersion = _thisTarget.Info.SoftwareVersion;
                SDKVersion = _thisTarget.Info.SDKVersion;
                IPAddress = _thisTarget.IPAddress;
                PayloadPort = _thisTarget.PayloadPort.ToString();
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
            DependencyProperty.Register("TargetName", typeof(string), typeof(MiniTargetPanel), new PropertyMetadata(string.Empty));

        private TargetStatusType TargetStatus
        {
            get { return (TargetStatusType)GetValue(TargetStatusProperty); }
            set { SetValue(TargetStatusProperty, value); }
        }

        private static readonly DependencyProperty TargetStatusProperty =
            DependencyProperty.Register("TargetStatus", typeof(TargetStatusType), typeof(MiniTargetPanel), new PropertyMetadata(TargetStatusType.None, TargetStatusProperty_Changed));

        private static void TargetStatusProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            switch ((TargetStatusType)e.NewValue)
            {
                case TargetStatusType.Offline:
                    ((MiniTargetPanel)d).TargetStatusElement.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    ((MiniTargetPanel)d).TargetStatusElement.ToolTip = "Offline";
                    break;

                case TargetStatusType.Online:
                    ((MiniTargetPanel)d).TargetStatusElement.Fill = new SolidColorBrush(Color.FromRgb(255, 140, 0));
                    ((MiniTargetPanel)d).TargetStatusElement.ToolTip = "Online";
                    break;

                case TargetStatusType.APIAvailable:
                    ((MiniTargetPanel)d).TargetStatusElement.Fill = new SolidColorBrush(Color.FromRgb(0, 128, 0));
                    ((MiniTargetPanel)d).TargetStatusElement.ToolTip = "Online & API Available";
                    break;

                default:
                    ((MiniTargetPanel)d).TargetStatusElement.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    ((MiniTargetPanel)d).TargetStatusElement.ToolTip = "Unknown";
                    break;
            }
        }

        private bool IsDefault
        {
            get { return (bool)GetValue(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        private static readonly DependencyProperty IsDefaultProperty =
            DependencyProperty.Register("IsDefault", typeof(bool), typeof(MiniTargetPanel), new PropertyMetadata(false, IsDefaultProperty_Changed));

        private static void IsDefaultProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ((MiniTargetPanel)d).DefaultTargetElement.Foreground = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                ((MiniTargetPanel)d).DefaultTargetElement.Cursor = Cursors.Arrow;
            }

            else
            {
                ((MiniTargetPanel)d).DefaultTargetElement.Foreground = new SolidColorBrush(Color.FromRgb(69, 73, 74));
                ((MiniTargetPanel)d).DefaultTargetElement.Cursor = Cursors.Hand;
            }
        }

        private ConsoleModelType ConsoleModel
        {
            get { return (ConsoleModelType)GetValue(ConsoleModelProperty); }
            set { SetValue(ConsoleModelProperty, value); }
        }

        private static readonly DependencyProperty ConsoleModelProperty =
            DependencyProperty.Register("ConsoleModel", typeof(ConsoleModelType), typeof(MiniTargetPanel), new PropertyMetadata(ConsoleModelType.Unknown, ConsoleModelProperty_Changed));

        private static void ConsoleModelProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            switch ((ConsoleModelType)e.NewValue)
            {
                case ConsoleModelType.Fat:
                    ((MiniTargetPanel)d).ConsoleImageElement.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisLib2;component/Common/Images/Consoles/Fat.png"));
                    break;

                case ConsoleModelType.Slim:
                    ((MiniTargetPanel)d).ConsoleImageElement.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisLib2;component/Common/Images/Consoles/Slim.png"));
                    break;

                case ConsoleModelType.Pro:
                    ((MiniTargetPanel)d).ConsoleImageElement.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisLib2;component/Common/Images/Consoles/Pro.png"));
                    break;

                default:
                    ((MiniTargetPanel)d).ConsoleImageElement.Source = new BitmapImage(new Uri("pack://application:,,,/OrbisLib2;component/Common/Images/Consoles/Fat.png"));
                    break;
            }
        }

        private string FirmwareVersion
        {
            get { return (string)GetValue(FirmwareVersionProperty); }
            set { SetValue(FirmwareVersionProperty, value); }
        }

        public static readonly DependencyProperty FirmwareVersionProperty =
            DependencyProperty.Register("FirmwareVersion", typeof(string), typeof(MiniTargetPanel), new PropertyMetadata(string.Empty));

        private string SDKVersion
        {
            get { return (string)GetValue(SDKVersionProperty); }
            set { SetValue(SDKVersionProperty, value); }
        }

        private static readonly DependencyProperty SDKVersionProperty =
            DependencyProperty.Register("SDKVersion", typeof(string), typeof(MiniTargetPanel), new PropertyMetadata(string.Empty));

        private string IPAddress
        {
            get { return (string)GetValue(IPAddressProperty); }
            set { SetValue(IPAddressProperty, value); }
        }

        private static readonly DependencyProperty IPAddressProperty =
            DependencyProperty.Register("IPAddress", typeof(string), typeof(MiniTargetPanel), new PropertyMetadata(string.Empty));

        public string PayloadPort
        {
            get { return (string)GetValue(PayloadPortProperty); }
            set { SetValue(PayloadPortProperty, value); }
        }

        public static readonly DependencyProperty PayloadPortProperty =
            DependencyProperty.Register("PayloadPort", typeof(string), typeof(MiniTargetPanel), new PropertyMetadata(string.Empty));

        #endregion

        #region Buttons

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

        #endregion

        private void MiniTargetPanelElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TargetManager.SelectedTarget = _thisTarget;

            var parentWindow = (SelectTarget)Window.GetWindow(this);
            if(parentWindow != null)
            {
                parentWindow.Result = SimpleDialogResult.None;
                parentWindow.Close();
            }
        }
    }
}
