using SetupBA.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SetupBA.MVVM.View
{
    /// <summary>
    /// Interaction logic for InitialView.xaml
    /// </summary>
    public partial class InitialView : UserControl
    {
        public InitialView()
        {
            InitializeComponent();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as InitialViewModel;
            dc.MainVM.CurrentView = dc.MainVM.LicenseVM;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SetupBA.BootstrapperDispatcher.InvokeShutdown();
        }

        private void UnInstall_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as InitialViewModel;
            dc.MainVM.UninstallExecute();
            dc.MainVM.CurrentView = dc.MainVM.InstallVM;
        }
    }
}
