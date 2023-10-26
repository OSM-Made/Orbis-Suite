using Microsoft.Extensions.Logging;
using OrbisLib2.Common.Dispatcher;
using SimpleUI.Controls;

namespace OrbisNeighborHood
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SimpleWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Show(ILogger logger)
        {
            base.Show();
            DispatcherClient.Subscribe(logger);
        }
    }
}
