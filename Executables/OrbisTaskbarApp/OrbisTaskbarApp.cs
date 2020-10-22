using DarkUI.Forms;
using OrbisSuite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisTaskbarApp
{
    public partial class OrbisTaskbarApp : DarkForm
    {
        OrbisLib PS4 = new OrbisLib();

        private void ExecuteSecure(Action a)
        {
            if (InvokeRequired)
                BeginInvoke(a);
            else
                a();
        }

        public OrbisTaskbarApp()
        {
            InitializeComponent();

            //Start up notify icon.
            NotifyIcon_TaskBarApp.BalloonTipTitle = "Orbis Taskbar App running in the background!";
            NotifyIcon_TaskBarApp.BalloonTipText = "Click the icon in the system tray to get started.";
            NotifyIcon_TaskBarApp.ShowBalloonTip(2000);
            NotifyIcon_TaskBarApp.Visible = true;

            //Update the Target List on run.
            UpdateTargetList();

            //Set Events
            PS4.Events.DBTouched += Events_DBTouched;
            PS4.Events.TargetAvailable += Events_TargetAvailable;
            PS4.Events.TargetUnAvailable += Events_TargetUnAvailable;

            //Update settings
            ToolStrip_AutoLoadPayload.Checked = PS4.Settings.AutoLoadPayload;
            ToolStrip_AutoLaunch.Checked = PS4.Settings.StartOnBoot;
        }

        #region One Instance

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_TASKBARAPP)
            {
                ShowMe();
            }
            base.WndProc(ref m);
        }
        private void ShowMe()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            // get our current "TopMost" value (ours will always be false though)
            bool top = TopMost;
            // make our form jump to the top of everything
            TopMost = true;
            // set it back to whatever it was
            TopMost = top;
        }

        #endregion

        private void Events_TargetUnAvailable(object sender, TargetUnAvailableEvent e)
        {
            ExecuteSecure(() => UpdateTargetList());
        }

        private void Events_TargetAvailable(object sender, TargetAvailableEvent e)
        {
            ExecuteSecure(() => UpdateTargetList());
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
        {
            ExecuteSecure(() => UpdateTargetList());
        }

        public static int LastTargetCount;
        public void UpdateTargetList()
        {
            try
            {
                //If new count we need to clear so we can remove what was removed.
                if (DarkContextMenu_ConsoleList.Items.Count != PS4.TargetManagement.TargetList.Count)
                    DarkContextMenu_ConsoleList.Items.Clear();

                //Only Update the list when there is a target to store.
                if (PS4.TargetManagement.TargetList.Count > 0)
                {
                    int Count = 0;

                    foreach (TargetInfo Target in PS4.TargetManagement.TargetList)
                    {
                        if (DarkContextMenu_ConsoleList.Items.Count <= Count)
                            DarkContextMenu_ConsoleList.Items.Add(Target.Name, null, ConsoleList_Click);
                        else
                            DarkContextMenu_ConsoleList.Items[Count].Text = Target.Name;

                        if (Target.Default)
                        {
                            ((ToolStripMenuItem)DarkContextMenu_ConsoleList.Items[Count]).Checked = true;
                            ToolStrip_TargetList.Text = "Default Target: " + Target.Name;
                        }

                        Count++;
                    }

                    LastTargetCount = PS4.TargetManagement.TargetList.Count;
                }
                else
                    ToolStrip_TargetList.Text = "Default Target: N/A";

                if (PS4.DefaultTarget.Active)
                {
                    ToolStrip_Reboot.Enabled = true;
                    ToolStrip_Shutdown.Enabled = true;
                    ToolStrip_Suspend.Enabled = true;

                    ToolStrip_SendPayload.Enabled = true;
                    if (PS4.DefaultTarget.Info.Available)
                        ToolStrip_SendOrbisPayload.Enabled = false;
                    else
                        ToolStrip_SendOrbisPayload.Enabled = true;

                    TargetSettings_Launcher.Enabled = true;
                }
                else
                {
                    
                    ToolStrip_Reboot.Enabled = false;
                    ToolStrip_Shutdown.Enabled = false;
                    ToolStrip_Suspend.Enabled = false;

                    ToolStrip_SendPayload.Enabled = false;
                    ToolStrip_SendOrbisPayload.Enabled = false;

                    TargetSettings_Launcher.Enabled = false;
                }
            }
            catch
            {

            }
        }

        private void ConsoleList_Click(object sender, EventArgs e)
        {
            try
            {
                string DefaultConsole = ((ToolStripMenuItem)sender).Text;
                ToolStrip_TargetList.Text = "Default Target: " + DefaultConsole;
                PS4.TargetManagement.SetDefault(((ToolStripMenuItem)sender).Text);
                UpdateTargetList();
            }
            catch
            {

            }
        }

        private void ToolStrip_AddTarget_Click(object sender, EventArgs e)
        {
            try
            {
                if (PS4.Dialogs.AddTarget(FormStartPosition.CenterScreen) == DialogResult.OK)
                    UpdateTargetList();
            }
            catch
            {

            }
        }

        #region Orbis Program Launcher

        private void Neigborhood_Launcher_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Orbis Suite\\OrbisNeighborhood.exe");
            }
            catch
            {

            }
        }

        private void ConsoleOutput_Launcher_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Orbis Suite\\OrbisConsoleOutput.exe");
            }
            catch
            {

            }
        }

        private void Debugger_Launcher_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Orbis Suite\\OrbisDebugger.exe");
            }
            catch
            {

            }
        }

        private void ModuleManager_Launcher_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Orbis Suite\\OrbisModuleManager.exe");
            }
            catch
            {

            }
        }

        private void TargetSettings_Launcher_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Orbis Suite\\OrbisTargetSettings.exe");
            }
            catch
            {

            }
        }

        #endregion

        private void ToolStrip_Reboot_Click(object sender, EventArgs e)
        {
            try
            {
                PS4.DefaultTarget.Reboot();
            }
            catch
            {

            }
        }

        private void ToolStrip_Shutdown_Click(object sender, EventArgs e)
        {
            try
            {
                PS4.DefaultTarget.Shutdown();
            }
            catch
            {

            }
        }

        private void ToolStrip_Suspend_Click(object sender, EventArgs e)
        {
            try
            {
                PS4.DefaultTarget.Suspend();
            }
            catch
            {

            }
        }

        private void ToolStrip_SendPayload_Click(object sender, EventArgs e)
        {
            try
            {
                string PayloadPath = string.Empty;
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Open BIN File...";
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.CheckPathExists = true;
                    openFileDialog.InitialDirectory = Properties.Settings.Default.BINDirectory;
                    openFileDialog.Filter = "BIN files (*.BIN)|*.BIN";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PayloadPath = openFileDialog.FileName;
                        Properties.Settings.Default.BINDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                        Properties.Settings.Default.Save();
                    }
                    else
                        return;
                }

                FileStream fPayload = File.Open(PayloadPath, FileMode.Open);
                if (fPayload.CanRead)
                {
                    byte[] PayloadBuffer = new byte[fPayload.Length];

                    if (fPayload.Read(PayloadBuffer, 0, (int)fPayload.Length) == fPayload.Length)
                    {
                        if (!PS4.DefaultTarget.Payload.InjectPayload(PayloadBuffer))
                            DarkMessageBox.ShowError("Failed to send payload to target please try again.", "Error: Failed to inject payload.", DarkDialogButton.Ok, FormStartPosition.CenterScreen);
                    }
                    else
                        DarkMessageBox.ShowError("Failed read payload from disc to target please try again.", "Error: Failed to inject payload.", DarkDialogButton.Ok, FormStartPosition.CenterScreen);
                }

                fPayload.Close();
            }
            catch
            {

            }
        }

        private void ToolStrip_SendOrbisPayload_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PS4.DefaultTarget.Payload.InjectPayload())
                    DarkMessageBox.ShowError("Failed to send payload to target please try again.", "Error: Failed to inject payload.", DarkDialogButton.Ok, FormStartPosition.CenterScreen);
            }
            catch
            {

            }
        }

        private void ToolStrip_AutoLoadPayload_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStrip_AutoLoadPayload.Checked = !ToolStrip_AutoLoadPayload.Checked;
                PS4.Settings.AutoLoadPayload = ToolStrip_AutoLoadPayload.Checked;
            }
            catch
            {

            }
        }

        private void ToolStrip_AutoLaunch_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStrip_AutoLaunch.Checked = !ToolStrip_AutoLaunch.Checked;
                PS4.Settings.StartOnBoot = ToolStrip_AutoLaunch.Checked;
            }
            catch
            {

            }
        }

        private void ToolStrip_Settings_Click(object sender, EventArgs e)
        {
            try
            {
                PS4.Dialogs.Settings(FormStartPosition.CenterScreen);
            }
            catch
            {

            }
        }

        private void ToolStrip_Help_Click(object sender, EventArgs e)
        {
            try
            {
                PS4.Dialogs.About(FormStartPosition.CenterScreen);
            }
            catch
            {

            }
        }

        private void ToolStrip_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch
            {

            }
        }
    }
}
