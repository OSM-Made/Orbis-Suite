using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using SetupBA.Helpers;

namespace SetupBA.MVVM.ViewModel
{
    public class MainViewModel : PropertyNotifyBase
    {
        private object _currentView;
        private string _installationAction;
        private InstallState _currentInstallState;
        private bool _uninstallEnabled;
        private bool _installEnabled;
        private bool _isThinking;
        private int _progressPercentage;
        private string _message;
        private DetectionState _detectState;

        public MainViewModel(BootstrapperApplication bootstrapper)
        {
            Bootstrapper = bootstrapper;

            // View Models.
            InitialVM = new InitialViewModel(this);
            LicenseVM = new LicenseViewModel(this);
            InstallVM = new InstallViewModel(this);
            SummaryVM = new SummaryViewModel(this);

            // Set Current View.
            CurrentView = InitialVM;

            Bootstrapper.ExecuteMsiMessage += Bootstrapper_ExecuteMsiMessage;
            Bootstrapper.ApplyComplete += Bootstrapper_ApplyComplete;
            Bootstrapper.DetectPackageComplete += Bootstrapper_DetectPackageComplete;
            Bootstrapper.PlanComplete += Bootstrapper_PlanComplete;
            Bootstrapper.Progress += Bootstrapper_Progress;
        }

        public BootstrapperApplication Bootstrapper { get; private set; }

        // View Models
        public InitialViewModel InitialVM { get; set; }
        public LicenseViewModel LicenseVM { get; set; }
        public InstallViewModel InstallVM { get; set; }
        public SummaryViewModel SummaryVM { get; set; }

        
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        public string InstallationAction
        {
            get { return _installationAction; }
            set
            {
                _installationAction = value;
                OnPropertyChanged("InstallationAction");
            }
        }

        public DetectionState DetectState
        {
            get { return _detectState; }
            set 
            { 
                _detectState = value;
                OnPropertyChanged("DetectState");
            }
        }

        public InstallState CurrentInstallState
        {
            get { return _currentInstallState; }
            set 
            { 
                _currentInstallState = value;
                OnPropertyChanged("CurrentInstallState");
            }
        }

        public bool UnInstallEnabled
        {
            get { return _uninstallEnabled; }
            set
            {
                _uninstallEnabled = value;
                OnPropertyChanged("UninstallEnabled");
            }
        }

        public bool InstallEnabled
        {
            get { return _installEnabled; }
            set
            {
                _installEnabled = value;
                OnPropertyChanged("InstallEnabled");
            }
        }

        public bool IsThinking
        {
            get { return _isThinking; }
            set
            {
                _isThinking = value;
                OnPropertyChanged("IsThinking");
            }
        }

        public int ProgressPercentage
        {
            get { return _progressPercentage; }
            set
            {
                _progressPercentage = value;
                OnPropertyChanged("ProgressPercentage");
            }
        }

        public string Message
        {
            get { return _message; }
            set 
            { 
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        private void Bootstrapper_ExecuteMsiMessage(object sender, ExecuteMsiMessageEventArgs e)
        {
            if (e.MessageType == InstallMessage.ActionStart)
            {
                Message = e.Message;
            }
        }

        private void Bootstrapper_PlanComplete(object sender, PlanCompleteEventArgs e)
        {
            Bootstrapper.Engine.Log(LogLevel.Verbose, "PlanComplete.");

            if (e.Status >= 0)
                Bootstrapper.Engine.Apply(System.IntPtr.Zero);
        }

        private void Bootstrapper_DetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            if (e.PackageId == "DummyInstallationPackageId")
            {
                if (e.State == PackageState.Absent)
                {
                    Bootstrapper.Engine.Log(LogLevel.Verbose, "Absent.");
                    CurrentInstallState = InstallState.Install;
                    DetectState = DetectionState.Absent;
                    InstallEnabled = true;
                    UnInstallEnabled = false;
                }
                else if (e.State == PackageState.Present)
                {
                    Bootstrapper.Engine.Log(LogLevel.Verbose, "Present.");
                    CurrentInstallState = InstallState.UnInstall;
                    DetectState = DetectionState.Present;
                    UnInstallEnabled = true;
                    InstallEnabled = false;
                }
            }

            // Silent Uninstall for upgrade.
            if (Bootstrapper.Command.Display == Display.None || Bootstrapper.Command.Display == Display.Embedded)
            {
                Bootstrapper.Engine.Log(LogLevel.Verbose, "Silent.");
                Bootstrapper.Engine.Plan(LaunchAction.Uninstall);
            }
        }

        private void Bootstrapper_ApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            Message = "Complete.";

            // Done...
            IsThinking = false;

            // Shutdown for Silent Uninstall for upgrade.
            if (Bootstrapper.Command.Display == Display.None || Bootstrapper.Command.Display == Display.Embedded)
            {
                Bootstrapper.Engine.Log(LogLevel.Verbose, "Shutdown.");
                SetupBA.BootstrapperDispatcher.InvokeShutdown();
            }
        }

        private void Bootstrapper_Progress(object sender, ProgressEventArgs e)
        {
            if(IsThinking)
            {
                Bootstrapper.Engine.Log(LogLevel.Verbose, $"Progres.... {e.ProgressPercentage}");
                ProgressPercentage = e.ProgressPercentage;
            }
        }

        public void InstallExecute()
        {
            IsThinking = true;
            Bootstrapper.Engine.Plan(LaunchAction.Install);
        }

        public void UninstallExecute()
        {
            IsThinking = true;
            Bootstrapper.Engine.Plan(LaunchAction.Uninstall);
        }

        public void ExitExecute()
        {
            SetupBA.BootstrapperDispatcher.InvokeShutdown();
        }

        #region RelayCommands

        private RelayCommand installCommand;
        public RelayCommand InstallCommand
        {
            get
            {
                if (installCommand == null)
                    installCommand = new RelayCommand(() => InstallExecute(), () => InstallEnabled == true);

                return installCommand;
            }
        }

        private RelayCommand uninstallCommand;
        public RelayCommand UninstallCommand
        {
            get
            {
                if (uninstallCommand == null)
                    uninstallCommand = new RelayCommand(() => UninstallExecute(), () => UnInstallEnabled == true);

                return uninstallCommand;
            }
        }

        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                    exitCommand = new RelayCommand(() => ExitExecute());

                return exitCommand;
            }
        }

        #endregion
    }
}