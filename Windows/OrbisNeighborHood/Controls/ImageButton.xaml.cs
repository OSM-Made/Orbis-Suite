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
    /// Interaction logic for ImageButton.xaml
    /// </summary>
    public partial class ImageButton : UserControl
    {
        public event EventHandler<RoutedEventArgs>? Click;

        public ImageButton()
        {
            InitializeComponent();
        }

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ImageButton), new PropertyMetadata("/OrbisNeighborHood;component/Images/UnAvailable.png", ImageSource_Changed));

        private static void ImageSource_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var currentControl = (ImageButton)d;
            currentControl.ButtonImage.Source = new BitmapImage(new Uri($"pack://application:,,,{(string)e.NewValue}"));
            currentControl.ButtonImage.Opacity = currentControl.IsEnabled ? 1 : 0.5;
        }

        public int ImageMargin
        {
            get { return (int)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }

        public static readonly DependencyProperty ImageMarginProperty =
            DependencyProperty.Register("ImageMargin", typeof(int), typeof(ImageButton), new PropertyMetadata(0));

        private void ImageButtonElement_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ButtonImage.Source = new BitmapImage(new Uri($"pack://application:,,,{ImageSource}"));
            ButtonImage.Opacity = IsEnabled ? 1 : 0.5;
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(sender, e);
        }
    }
}
