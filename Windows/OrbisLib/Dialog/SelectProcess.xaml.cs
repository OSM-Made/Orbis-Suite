using OrbisLib2.Targets;
using SimpleUI.Controls;
using SimpleUI.Dialogs;
using System.Windows;

namespace OrbisLib2.Dialog
{
    /// <summary>
    /// Interaction logic for SelectProcess.xaml
    /// </summary>
    public partial class SelectProcess : SimpleDialog
    {
        private static string[] DangerList = { "Fusion", "SceCdlgApp", "SceRemotePlay", "klogproc", "ScePfs", "SceNKWebProcess", "SceNKNetworkProcess", "SceNKUIProcess", "SceSpkProcess", "SceSpkService", "SceVencProxy.elf", "SceVoiceAndAgent", "webrtc_daemon.self", "SceMusicCoreServer", "SceSocialScreenMgr", "SceSpZeroConf", "SceCloudClientDaemon", "SceVideoCoreServer", "ScePartyDaemon", "SceGameLiveStreaming", "SceAvCapture", "GnmCompositor.elf", "SceSysCore.elf", "SceSysAvControl.elf", "usb", "SceSwd", "SceVnlru", "SceSyncer", "SceBufdaemon0", "SceBufdaemon1", "SceBufdaemon2", "SceBtDriver", "SceTrpwCtrl", "SceTrpwIntr", "SceTrpwReq", "SceSbram", "SceDaAtcev", "SceDaThrd", "SceIccThermal", "SceMd0", "SceIccnvs", "SceXptThrd", "SceGbeMtsPhyCtrl", "SceGbeMtsCtrl", "SceSflash", "SceIccNotification", "SceHdmiEvent", "SceCameraSdma", "SceCameraDriverMain", "SceHidMain", "SceHidAuth", "SceYarrow", "geom", "intr", "idle", "mini-syscore.elf", "SceAudit", "kernel", "orbis-jsc-compiler.self", "SecureWebProcess.self", "SecureUIProcess.self", "SceVdecProxy.elf", "fs_cleaner.elf", "SceVmdaemon", "ScePagedaemonX", "orbis_audiod.elf" };

        public SelectProcess(Window Owner)
            : base(Owner, "Select", "Favourites", "Cancel", "Select Process")
        {
            InitializeComponent();

            // Get initial process list.
            Task.Run(RefreshProcessList);

            AdvancedMode.IsChecked = Properties.Settings.Default.ProcessAdcancedMode;
        }

        public static async Task<SimpleDialogResult> ShowDialog(Window Owner)
        {
            var dlg = new SelectProcess(Owner);
            dlg.ShowDialog();

            // Attach to the process if one is selected and we clicked the attach button.
            var selectedProc = (ProcInfo)dlg.ProcessList.SelectedItem;
            if(selectedProc != null && dlg.Result == SimpleDialogResult.Button1)
            {
                await TargetManager.SelectedTarget.Debug.Attach(selectedProc.ProcessId);
            }

            return dlg.Result;
        }

        private async Task RefreshProcessList()
        {
            (var result, var procList) = await TargetManager.SelectedTarget.GetProcList();

            // Print the error that occured if we failed to get the process list.
            if (!result.Succeeded)
            {
                // Reset the item list.
                ProcessList.ItemsSource = null;

                SimpleMessageBox.ShowError(Window.GetWindow(this), result.ErrorMessage, "Failed to refresh process list.");
                return;
            }

            // Remove self.
            procList.RemoveAll(s => s.Name == "OrbisAPIDaemon");

            // Remove potentially dangerious processes.
            if (!Properties.Settings.Default.ProcessAdcancedMode)
                procList.RemoveAll(x => DangerList.Any(item => item.Contains(x.Name)));

            // Refresh the list.
            Dispatcher.Invoke(() => 
            {
                // Make the scroll bar move to the right a bit more make it look less awkward.
                if (procList.Count >= 11)
                    ProcessList.Margin = new Thickness(6, 6, 3, 6);
                else
                    ProcessList.Margin = new Thickness(6);

                ProcessList.ItemsSource = procList;
                ProcessList.Items.Refresh();
            });
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshProcessList();
        }

        private void ProcessList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as ProcInfo;
            if (item != null && ProcessList.SelectedItem != null)
            {
                Result = SimpleDialogResult.Button1;
                Close();
            }
        }

        private async void AdvancedMode_Click(object sender, RoutedEventArgs e)
        {
            AdvancedMode.IsChecked = Properties.Settings.Default.ProcessAdcancedMode = !AdvancedMode.IsChecked;
            Properties.Settings.Default.Save();

            await RefreshProcessList();
        }

        public override void Button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowOnlyFavourites_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddToFavourites_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
