using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AppLauncherButton.xaml
    /// </summary>
    public partial class AppLauncherButton : UserControl
    {
        public AppLauncherButton()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AppLauncherButton), new PropertyMetadata(string.Empty));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(AppLauncherButton), new PropertyMetadata("/OrbisNeighborHood;component/Images/Icons/OrbisTaskbarApp.ico", Source_Changed));

        private static void Source_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var currentControl = ((AppLauncherButton)d);
            var bitmapImage = new BitmapImage(new Uri($"pack://application:,,,{(string)e.NewValue}"));
            if (currentControl.IsEnabled)
            {
                currentControl.IconImage.Source = bitmapImage;
                currentControl.IconText.Opacity = 1;
            }
            else
            {
                var grayBitmapSource = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray8, null, 1);
                currentControl.IconImage.Source = grayBitmapSource;
                currentControl.IconText.Opacity = 0.5;
            }
        }

        private void AppLauncherButtonElement_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void AppLauncherButtonElement_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var bitmapImage = new BitmapImage(new Uri($"pack://application:,,,{Source}"));
            if (IsEnabled)
            {
                IconImage.Source = bitmapImage;
                IconText.Opacity = 1;
            }
            else
            {
                var grayBitmapSource = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray8, null, 1);
                IconImage.Source = grayBitmapSource;
                IconText.Opacity = 0.5;
            }
        }
    }
}
