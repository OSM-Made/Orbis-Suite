using DarkUI.Forms;
using OrbisSuite;
using OrbisSuite.Classes;
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
        }

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

        public void UpdateTargetList()
        {
            try
            {
                int Count = 0;
                DarkContextMenu_ConsoleList.Items.Clear();
                List<TargetInfo> TargetList = PS4.TargetManagement.GetTargetList();

                foreach(TargetInfo Target in TargetList)
                {
                    DarkContextMenu_ConsoleList.Items.Add(Target.Name, null, ConsoleList_Click);

                    if (Target.Default)
                    {
                        ((ToolStripMenuItem)DarkContextMenu_ConsoleList.Items[Count]).Checked = true;
                        ToolStrip_TargetList.Text = "Default Target: " + Target.Name;
                    }

                    Count++;
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

        private void Neigborhood_Launcher_Click(object sender, EventArgs e)
        {

        }

        private void ConsoleOutput_Launcher_Click(object sender, EventArgs e)
        {

        }

        private void Debugger_Launcher_Click(object sender, EventArgs e)
        {

        }

        private void ModuleManager_Launcher_Click(object sender, EventArgs e)
        {

        }

        private void TargetSettings_Launcher_Click(object sender, EventArgs e)
        {

        }

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
                DialogResult FileOpened;
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
                            DarkMessageBox.ShowError("Failed to send payload to target please try again.", "Error: Failed to inject payload.");
                    }
                    else
                        DarkMessageBox.ShowError("Failed read payload from disc to target please try again.", "Error: Failed to inject payload.");
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
                    DarkMessageBox.ShowError("Failed to send payload to target please try again.", "Error: Failed to inject payload.");
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
                PS4.Settings.SetAutoLoadPayload(ToolStrip_AutoLoadPayload.Checked);
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
                PS4.Settings.SetStartOnBoot(ToolStrip_AutoLaunch.Checked);
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
