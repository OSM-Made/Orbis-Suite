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
    /// Interaction logic for InputPanel.xaml
    /// </summary>
    public partial class InputPanel : UserControl
    {
        public InputPanel()
        {
            InitializeComponent();
        }

        public string InputName
        {
            get { return (string)GetValue(InputNameProperty); }
            set { SetValue(InputNameProperty, value); }
        }

        public static readonly DependencyProperty InputNameProperty =
            DependencyProperty.Register("InputName", typeof(string), typeof(InputPanel), new PropertyMetadata(string.Empty));

        public string InputDescription
        {
            get { return (string)GetValue(InputDescriptionProperty); }
            set { SetValue(InputDescriptionProperty, value); }
        }

        public static readonly DependencyProperty InputDescriptionProperty =
            DependencyProperty.Register("InputDescription", typeof(string), typeof(InputPanel), new PropertyMetadata(string.Empty));
    }
}
