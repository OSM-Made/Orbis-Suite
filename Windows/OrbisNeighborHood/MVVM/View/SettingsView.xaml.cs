using SimpleUI.Skins;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using OrbisLib2.Common.Database;

namespace OrbisNeighborHood.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();

            using (Stream? stream = assembly.GetManifestResourceStream("OrbisNeighborHood.Resources.BuildString.txt"))
            using (StreamReader? reader = new StreamReader(stream))
            {
                BuildString = reader.ReadToEnd();
            }
        }

        #region General

        private void AllowOnlyInteger(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void APIPort_LostFocus(object sender, RoutedEventArgs e)
        {
            var instance = Settings.CreateInstance();
            instance.APIPort = Convert.ToInt32(((SimpleUI.Controls.SimpleTextBox)sender).Text);
            instance.Save();
        }

        private void APIPort_Loaded(object sender, RoutedEventArgs e)
        {
            ((SimpleUI.Controls.SimpleTextBox)sender).Text = Settings.CreateInstance().APIPort.ToString();
        }

        private void FTPPort_Loaded(object sender, RoutedEventArgs e)
        {
            ((SimpleUI.Controls.SimpleTextBox)sender).Text = Settings.CreateInstance().FTPPort.ToString();
        }

        private void FTPPort_LostFocus(object sender, RoutedEventArgs e)
        {
            Settings.CreateInstance().FTPPort = Convert.ToInt32(((SimpleUI.Controls.SimpleTextBox)sender).Text);
        }

        private void KlogPort_Loaded(object sender, RoutedEventArgs e)
        {
            ((SimpleUI.Controls.SimpleTextBox)sender).Text = Settings.CreateInstance().KlogPort.ToString();
        }

        private void KlogPort_LostFocus(object sender, RoutedEventArgs e)
        {
            Settings.CreateInstance().KlogPort = Convert.ToInt32(((SimpleUI.Controls.SimpleTextBox)sender).Text);
        }

        private void COMPort_Loaded(object sender, RoutedEventArgs e)
        {
            ((SimpleUI.Controls.SimpleTextBox)sender).Text = Settings.CreateInstance().COMPort;
        }

        private void COMPort_LostFocus(object sender, RoutedEventArgs e)
        {
            Settings.CreateInstance().COMPort = ((SimpleUI.Controls.SimpleTextBox)sender).Text;
        }

        private void StartOnBoot_Loaded(object sender, RoutedEventArgs e)
        {
            ((SimpleUI.Controls.SimpleSwitch)sender).IsToggled = Settings.CreateInstance().StartOnBoot;
        }

        private void StartOnBoot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Settings.CreateInstance().StartOnBoot = ((SimpleUI.Controls.SimpleSwitch)sender).IsToggled;
        }

        #endregion

        #region Theme

        private void SelectTheme(string name)
        {
            var Theme = StringtoTheme(name);
            SkinResourceDictionary.ChangeTheme((SkinResourceDictionary.Themes)Theme);
            Settings.CreateInstance().Theme = Theme;
        }

        private int StringtoTheme(string Name)
        {
            switch (Name)
            {
                case "Red": return (int)SkinResourceDictionary.Themes.Red;
                case "Blue": return (int)SkinResourceDictionary.Themes.Blue;
                default: return (int)SkinResourceDictionary.Themes.Red;
            }
        }

        private string ThemetoString(int Theme)
        {
            switch (Theme)
            {
                case (int)SkinResourceDictionary.Themes.Red: return "Red";
                case (int)SkinResourceDictionary.Themes.Blue: return "Blue";
                default: return "Red";
            }
        }

        private void Theme_Loaded(object sender, RoutedEventArgs e)
        {
            var ThemeName = ThemetoString(Settings.CreateInstance().Theme);
            ((SimpleUI.Controls.SimpleComboBox)sender).SelectedValue = ThemeName;
            SelectTheme(ThemeName);
        }

        private void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectTheme((string)((SimpleUI.Controls.SimpleComboBox)sender).SelectedValue);
        }

        private void RainbowTheme_Loaded(object sender, RoutedEventArgs e)
        {
            ((SimpleUI.Controls.SimpleSwitch)sender).IsToggled = Settings.CreateInstance().RainbowColours;
        }

        private void RainbowTheme_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Settings.CreateInstance().RainbowColours = ((SimpleUI.Controls.SimpleSwitch)sender).IsToggled;
        }

        #endregion

        #region Target Details

        private void CensorIDPS_Loaded(object sender, RoutedEventArgs e)
        {
            ((SimpleUI.Controls.SimpleSwitch)sender).IsToggled = Settings.CreateInstance().CensorIDPS;
        }

        private void CensorIDPS_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Settings.CreateInstance().CensorIDPS = ((SimpleUI.Controls.SimpleSwitch)sender).IsToggled;
        }

        private void CensorPSID_Loaded(object sender, RoutedEventArgs e)
        {
            ((SimpleUI.Controls.SimpleSwitch)sender).IsToggled = Settings.CreateInstance().CensorPSID;
        }

        private void CensorPSID_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Settings.CreateInstance().CensorPSID = ((SimpleUI.Controls.SimpleSwitch)sender).IsToggled;
        }

        #endregion

        #region Build String
        public string BuildString
        {
            get { return (string)GetValue(BuildStringProperty); }
            set { SetValue(BuildStringProperty, value); }
        }

        public static readonly DependencyProperty BuildStringProperty =
            DependencyProperty.Register("BuildString", typeof(string), typeof(SettingsView), new PropertyMetadata(string.Empty));


        #endregion
    }
}
