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

namespace OrbisNeighborHood.Controls
{
    /// <summary>
    /// Interaction logic for SettingPanel.xaml
    /// </summary>
    public partial class SettingPanel : ContentControl
    {
        public SettingPanel()
        {
            InitializeComponent();
        }

        public string? SettingName { get; set; } = string.Empty;

        public static readonly DependencyProperty SettingNameProperty =
            DependencyProperty.Register("SettingName", typeof(string), typeof(SettingPanel), new PropertyMetadata(string.Empty, SettingNameChanged));

        private static void SettingNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SettingPanel)d).SettingName = e.NewValue as string;
        }

        public string? SettingDescription { get; set; } = string.Empty;

        public static readonly DependencyProperty SettingDescriptionProperty =
            DependencyProperty.Register("SettingDescription", typeof(string), typeof(SettingPanel), new PropertyMetadata(string.Empty, SettingDescriptionChanged));

        private static void SettingDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SettingPanel)d).SettingDescription = e.NewValue as string;
        }
    }
}
