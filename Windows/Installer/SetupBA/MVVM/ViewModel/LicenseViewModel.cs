using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using SetupBA.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupBA.MVVM.ViewModel
{
    public class LicenseViewModel : PropertyNotifyBase
    {
        private bool _agreedToLicesnse;

        public LicenseViewModel(MainViewModel mainViewModel)
        {
            MainVM = mainViewModel;
        }

        public MainViewModel MainVM { get; set; }

        public bool AgreedToLicesnse 
        {
            get { return _agreedToLicesnse; }
            set
            {
                _agreedToLicesnse = value;
                OnPropertyChanged("AgreedToLicesnse");
            }
        }
    }
}
