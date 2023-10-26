using SetupBA.MVVM.ViewModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SetupBA.MVVM.View
{
    /// <summary>
    /// Interaction logic for LicenseView.xaml
    /// </summary>
    public partial class LicenseView : UserControl
    {
        public LicenseView()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as LicenseViewModel;
            dc.MainVM.CurrentView = dc.MainVM.InitialVM;
        }

        private void Install_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as LicenseViewModel;
            dc.MainVM.InstallExecute();
            dc.MainVM.CurrentView = dc.MainVM.InstallVM;
        }

        private void LicenseText_Loaded(object sender, RoutedEventArgs e)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SetupBA.Resources.License.rtf"))
            {
                LicenseText.Selection.Load(stream, DataFormats.Rtf);
            }
        }

        private void AgreementCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as LicenseViewModel;
            dc.AgreedToLicesnse = AgreementCheckBox.IsChecked ?? false;
        }
    }
}
