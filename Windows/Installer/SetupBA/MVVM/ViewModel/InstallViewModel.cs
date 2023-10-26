using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using SetupBA.Helpers;
using System;

namespace SetupBA.MVVM.ViewModel
{
    /// <summary>
    /// The states of detection.
    /// </summary>
    public enum DetectionState
    {
        Absent,
        Present,
        Newer,
    }

    public enum InstallState
    {
        Install,
        Installing,
        UnInstall,
        UnInstalling,
        Complete,
        Error,
    }

    /// <summary>
    /// The model of the installation view in SetupBA.
    /// </summary>
    public class InstallViewModel : PropertyNotifyBase
    {
        

        public InstallViewModel(MainViewModel mainViewModel)
        {
            MainVM = mainViewModel;
        }

        public MainViewModel MainVM { get; set; }

        //TODO: Move to main vm and set when changes happen so view is updated.
        public string Title { 
            get
            {
                switch (MainVM.CurrentInstallState)
                {
                    case InstallState.Install:
                        if (MainVM.IsThinking)
                            return "Installing Orbis Suite...";
                        else
                            return "Installation Complete.";

                    case InstallState.UnInstall:
                        if (MainVM.IsThinking)
                            return "Un-Installing Orbis Suite...";
                        else
                            return "Un-Installation Complete.";
                    
                    default:
                        return "Unknown";
                }
            }
        }
    }
}
