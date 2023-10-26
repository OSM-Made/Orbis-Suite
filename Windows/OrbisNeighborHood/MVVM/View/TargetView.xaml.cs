using OrbisNeighborHood.Controls;
using System.Windows.Controls;
using OrbisLib2.General;
using OrbisLib2.Targets;

namespace OrbisNeighborHood.MVVM.View
{
    /// <summary>
    /// Interaction logic for TargetView.xaml
    /// </summary>
    public partial class TargetView : UserControl
    {
        #region Constructor

        public TargetView()
        {
            InitializeComponent();
            RefreshTargets();

            Events.DBTouched += Events_DBTouched;
            Events.TargetStateChanged += Events_TargetStateChanged;
        }

        #endregion

        #region Events

        private void Events_TargetStateChanged(object? sender, TargetStateChangedEvent e)
        {
            Dispatcher.Invoke(() => { RefreshTargets(); });
        }

        private void Events_DBTouched(object? sender, DBTouchedEvent e)
        {
            Dispatcher.Invoke(() => { RefreshTargets(); });
        }

        private void Target_TargetChanged(object? sender, System.Windows.RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => { RefreshTargets(); });
        }

        #endregion

        public void RefreshTargets()
        {
            TargetList.Items.Clear();

            if (TargetManager.Targets.Count > 0)
            {
                foreach (var Target in TargetManager.Targets)
                {
                    var targetView = new TargetPanel(Target.Name);
                    targetView.TargetChanged += Target_TargetChanged;
                    TargetList.Items.Add(targetView);
                }
            }

            var newTargetView = new NewTargetPanel();
            newTargetView.TargetChanged += Target_TargetChanged;
            TargetList.Items.Add(newTargetView);
        }
    }
}
