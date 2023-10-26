using OrbisLib2.Dialog.Controls;
using OrbisLib2.General;
using OrbisLib2.Targets;
using SimpleUI.Dialogs;
using System.Windows;

namespace OrbisLib2.Dialog
{
    /// <summary>
    /// Interaction logic for SelectTarget.xaml
    /// </summary>
    public partial class SelectTarget : SimpleDialog
    {
        public SelectTarget(Window Owner)
            : base(Owner, "Cancel", "Select Target")
        {
            InitializeComponent();

            Events.DBTouched += Events_DBTouched;
            Events.TargetStateChanged += Events_TargetStateChanged;

            RefreshTargets();
        }

        public static SimpleDialogResult ShowDialog(Window Owner)
        {
            var dlg = new SelectTarget(Owner);
            dlg.ShowDialog();
            return dlg.Result;
        }

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
                    var targetView = new MiniTargetPanel(Target.Name);
                    targetView.TargetChanged += Target_TargetChanged;
                    TargetList.Items.Add(targetView);
                }
            }
        }
    }
}
