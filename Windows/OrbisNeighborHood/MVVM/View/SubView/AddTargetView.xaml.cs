using OrbisLib2.Common.Database;
using OrbisNeighborHood.MVVM.ViewModel;
using OrbisNeighborHood.MVVM.ViewModel.SubView;
using SimpleUI.Controls;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrbisNeighborHood.MVVM.View.SubView
{
    /// <summary>
    /// Interaction logic for NewTargetView.xaml
    /// </summary>
    public partial class AddTargetView : UserControl
    {
        private SavedTarget _newTarget;

        #region Constructor

        public AddTargetView()
        {
            InitializeComponent();

            _newTarget = new SavedTarget();
        }

        #endregion

        #region Target Info

        private void TargetName_Loaded(object sender, RoutedEventArgs e)
        {
            var Textbox = (SimpleTextBox)sender;
            Textbox.Text = "";
        }

        private void TargetName_LostFocus(object sender, RoutedEventArgs e)
        {
            var Textbox = (SimpleTextBox)sender;
            _newTarget.Name = Textbox.Text;
        }

        private void TargetIPAddress_Loaded(object sender, RoutedEventArgs e)
        {
            var Textbox = (SimpleTextBox)sender;
            Textbox.Text = "";
        }

        private void TargetIPAddress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9^.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TargetIPAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            var Textbox = (SimpleTextBox)sender;

            if (regex.IsMatch(Textbox.Text))
                _newTarget.IPAddress = Textbox.Text;
            else
                Textbox.Text = "";
        }

        private void TargetPayloadPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TargetPayloadPort_Loaded(object sender, RoutedEventArgs e)
        {
            var Textbox = (SimpleTextBox)sender;
            Textbox.Text = "";
        }

        private void TargetPayloadPort_LostFocus(object sender, RoutedEventArgs e)
        {
            var Textbox = (SimpleTextBox)sender;
            int value = 0;
            if(int.TryParse(Textbox.Text, out value))
                _newTarget.PayloadPort = value;
        }

        #endregion

        #region Target Settings

        private void ShowTitleIdLabels_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var Switch = (SimpleSwitch)sender;
            _newTarget.Info.ShowTitleId = Switch.IsToggled;
        }

        private void ShowDevkitPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var Switch = (SimpleSwitch)sender;
            _newTarget.Info.ShowDevkitPanel = Switch.IsToggled;
        }

        private void ShowToolboxShortcut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var Switch = (SimpleSwitch)sender;
            _newTarget.Info.ShowToolboxShortcut = Switch.IsToggled;
        }

        private void ShowAppHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var Switch = (SimpleSwitch)sender;
            _newTarget.Info.ShowAppHome = Switch.IsToggled;
        }

        #endregion

        #region Buttons

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (MainViewModel.Instance != null)
                MainViewModel.Instance.CurrentView = MainViewModel.Instance.TargetVM;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_newTarget.Name == string.Empty || _newTarget.Name == "-")
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), "You must give the target a name.", "Failed to add target to the Database!");
                return;
            }
                

            if (_newTarget.IPAddress == string.Empty || _newTarget.IPAddress == "-")
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), "You must give the target an IP Address.", "Failed to add target to the Database!");
                return;
            }
               
            if (_newTarget.Add())
            {
                var dc = DataContext as AddTargetViewModel;
                if (dc != null)
                    dc.DoTargetChanged();

                if (MainViewModel.Instance != null)
                    MainViewModel.Instance.CurrentView = MainViewModel.Instance.TargetVM;
            }
            else
            {
                SimpleMessageBox.ShowError(Window.GetWindow(this), "Maybe a target with this Name or IP Address already exists.", "Failed to add target to the Database!");
            }
        }

        #endregion
    }
}
