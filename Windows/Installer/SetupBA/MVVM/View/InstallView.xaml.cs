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
    /// Interaction logic for InstallView.xaml
    /// </summary>
    public partial class InstallView : UserControl
    {
        public InstallView()
        {
            InitializeComponent();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as InstallViewModel;
            dc.MainVM.CurrentView = dc.MainVM.SummaryVM;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SetupBA.BootstrapperDispatcher.InvokeShutdown();
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            SetupBA.BootstrapperDispatcher.InvokeShutdown();
        }
    }
}
