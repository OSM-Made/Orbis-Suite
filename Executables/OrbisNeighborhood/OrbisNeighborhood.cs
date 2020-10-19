using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrbisSuite;
using static OrbisSuite.Classes.Target;
using OrbisSuite.Classes;
using System.IO;

namespace nsOrbisNeighborhood
{
    public partial class OrbisNeighborhood : DarkForm
    {
        OrbisLib PS4 = new OrbisLib();

        void SetStatus(string Val)
        {
            StatusLabel.Text = Val;
        }

        private void ExecuteSecure(Action a)
        {
            if (InvokeRequired)
                BeginInvoke(a);
            else
                a();
        }

        public OrbisNeighborhood()
        {
            InitializeComponent();

            TargetList.RowsDefaultCellStyle.BackColor = Color.FromArgb(57, 60, 62);
            TargetList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(60, 63, 65);

            TargetList.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
            TargetList.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(57, 60, 62);
            TargetList.EnableHeadersVisualStyles = false;
            TargetList.DefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
            TargetList.DefaultCellStyle.BackColor = Color.FromArgb(57, 60, 62);
            TargetList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 75, 75);
            TargetList.DefaultCellStyle.SelectionForeColor = Color.FromArgb(220, 220, 220);

            TargetList.BackColor = Color.FromArgb(122, 128, 132);

            UpdateTargetList();
            UpdateSettings();

            //Register Global Events
            PS4.Events.TargetAvailable += PS4_TargetAvailable;
            PS4.Events.TargetUnAvailable += PS4_TargetUnAvailable;
            PS4.Events.DBTouched += Events_DBTouched;

            PS4.SelectedTarget.Events.ProcAttach += Events_ProcAttach;
            PS4.SelectedTarget.Events.ProcDetach += Events_ProcDetach;
        }

        private void Events_ProcDetach(object sender, ProcDetachEvent e)
        {
            
        }

        #region One Instance

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_NEIGHBORHOOD)
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

        #region Events

        private void Events_ProcAttach(object sender, ProcAttachEvent e)
        {
            ExecuteSecure(() => CurrentProc.Text = "Process: " + e.NewProcName);
        }

        private void Button_Attach_Click(object sender, EventArgs e)
        {
            if(PS4.SelectedTarget.Info.Available)
                PS4.SelectedTarget.Process.SelectProcess();
        }

        private void Button_Detach_Click(object sender, EventArgs e)
        {
            if (PS4.SelectedTarget.Info.Available && PS4.SelectedTarget.Info.Attached)
                PS4.SelectedTarget.Process.Detach();
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
        {
            Console.WriteLine("Data Base Touched.");
            ExecuteSecure(() => UpdateSettings());
            ExecuteSecure(() => UpdateTargetList());
        }

        private void PS4_TargetUnAvailable(object sender, TargetUnAvailableEvent e)
        {
            ExecuteSecure(() => UpdateTargetList());
        }

        private void PS4_TargetAvailable(object sender, TargetAvailableEvent e)
        {
            ExecuteSecure(() => UpdateTargetList());
        }

        public void UpdateTargetList()
        {
            SetStatus("Updating List...");

            try
            {
                if(TargetList.Rows.Count > PS4.TargetManagement.GetTargetCount())
                    TargetList.Rows.Clear();

                int LoopCount = 0;
                foreach (TargetInfo Target in PS4.TargetManagement.TargetList)
                {
                    object[] obj = { Target.Name.Equals(PS4.TargetManagement.DefaultTarget.Name) ? nsOrbisNeighborhood.Properties.Resources.Default : nsOrbisNeighborhood.Properties.Resources.NotDefault, Target.Name, Target.Firmware, Target.IPAddr, Target.Available ? "Available" : "Not Available", Target.Title, Target.SDKVersion, Target.ConsoleName, Target.ConsoleType };
                    if (TargetList.Rows.Count <= LoopCount) //todo redo for contains.
                        TargetList.Rows.Add(obj);
                    else
                        TargetList.Rows[LoopCount].SetValues(obj);

                    LoopCount++;
                }

                if (PS4.SelectedTarget.Active)
                {
                    if (PS4.SelectedTarget.Info.Available)
                        Button_Attach.Enabled = true;
                    else
                        Button_Attach.Enabled = false;

                    CurrentTarget.Text = string.Format("Target: {0}", PS4.SelectedTarget.Info.Name);

                    if (PS4.SelectedTarget.Info.Attached)
                    {
                        CurrentProc.Text = "Process: " + PS4.SelectedTarget.Info.CurrentProc;
                        Button_Detach.Enabled = true;
                    }
                    else
                    {
                        CurrentProc.Text = "Process: N/A";
                        Button_Detach.Enabled = false;
                    }
                }
                else
                {
                    CurrentTarget.Text = "Target: N/A";
                    CurrentProc.Text = "Process: N/A";
                    Button_Detach.Enabled = false;
                }
            }
            catch
            {

            }

            SetStatus("Ready");
        }

        public void UpdateSettings()
        {
            SetStatus("Updating Settings...");

            try
            {
                AutoLoadPayload_Button.Checked = PS4.Settings.AutoLoadPayload;
                LoadOnBoot_Button.Checked = PS4.Settings.StartOnBoot;
            }
            catch
            {

            }

            SetStatus("Ready");
        }

        #endregion

        private void TargetList_Enter(object sender, EventArgs e)
        {
            TargetList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 75, 75);
        }

        private void TargetList_Leave(object sender, EventArgs e)
        {
            TargetList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 92, 92);
        }

        private void AutoLoadPayload_Button_Click(object sender, EventArgs e)
        {
            try
            {
                AutoLoadPayload_Button.Checked = !AutoLoadPayload_Button.Checked;
                PS4.Settings.AutoLoadPayload = AutoLoadPayload_Button.Checked;
            }
            catch
            {

            }
        }

        private void LoadOnBoot_Button_Click(object sender, EventArgs e)
        {
            try
            {
                LoadOnBoot_Button.Checked = !LoadOnBoot_Button.Checked;
                PS4.Settings.StartOnBoot = LoadOnBoot_Button.Checked;
            }
            catch
            {

            }
        }

        private void AddTarget_Button_Click(object sender, EventArgs e)
        {
            if (PS4.Dialogs.AddTarget() == DialogResult.OK)
                UpdateTargetList();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                PS4.Dialogs.Settings(); 
            }
            catch
            {

            }
        }

        private void About_Button_Click(object sender, EventArgs e)
        {
            PS4.Dialogs.About();
        }

        //
        // Target Context Menu
        //
        private void SendPayload_Click(object sender, EventArgs e)
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
                        Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
                        if (selectedCellCount > 0)
                        {
                            int index = TargetList.SelectedRows[0].Index;
                            string TargetName = Convert.ToString(TargetList.Rows[index].Cells["mTargetName"].Value);
                            if (!PS4.Target[TargetName].Payload.InjectPayload(PayloadBuffer))
                                DarkMessageBox.ShowError("Failed to send payload to target please try again.", "Error: Failed to inject payload.");
                        }
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

        private void SendOrbisPayload_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = TargetList.SelectedRows[0].Index;
                    string TargetName = Convert.ToString(TargetList.Rows[index].Cells["mTargetName"].Value);
                    if (!PS4.Target[TargetName].Payload.InjectPayload())
                        DarkMessageBox.ShowError("Failed to send payload to target please try again.", "Error: Failed to inject payload.");
                }
            }
            catch
            {

            }
        }

        private void Target_Reboot_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = TargetList.SelectedRows[0].Index;
                    string TargetName = Convert.ToString(TargetList.Rows[index].Cells["mTargetName"].Value);

                    PS4.Target[TargetName].Reboot();
                }
            }
            catch
            {

            }
        }

        private void Target_Shutdown_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = TargetList.SelectedRows[0].Index;
                    string TargetName = Convert.ToString(TargetList.Rows[index].Cells["mTargetName"].Value);

                    PS4.Target[TargetName].Shutdown();
                }
            }
            catch
            {

            }
        }

        private void Target_Suspend_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = TargetList.SelectedRows[0].Index;
                    string TargetName = Convert.ToString(TargetList.Rows[index].Cells["mTargetName"].Value);

                    PS4.Target[TargetName].Suspend();
                }
            }
            catch
            {

            }
        }

        private void Target_SetDefault_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = TargetList.SelectedRows[0].Index;
                    string TargetName = Convert.ToString(TargetList.Rows[index].Cells["mTargetName"].Value);

                    PS4.Target[TargetName].SetDefault();

                    UpdateTargetList();
                }
            }
            catch
            {

            }
        }

        private void Target_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = TargetList.SelectedRows[0].Index;
                    string TargetName = Convert.ToString(TargetList.Rows[index].Cells["mTargetName"].Value);

                    if (PS4.Dialogs.EditTarget(TargetName) == DialogResult.OK)
                        UpdateTargetList();
                }
            }
            catch
            {

            }
        }

        private void Target_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = TargetList.SelectedRows[0].Index;
                    string TargetName = Convert.ToString(TargetList.Rows[index].Cells["mTargetName"].Value);

                    if (DarkMessageBox.ShowInformation("Are you sure you want to delete Target \"" + TargetName + "\"?", "Delete Target?", DarkDialogButton.YesNo) == DialogResult.Yes)
                    {
                        PS4.Target[TargetName].Delete();

                        UpdateTargetList();
                    }
                }
            }
            catch
            {

            }
        }

        private void Target_Details_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = TargetList.SelectedRows[0].Index;
                    string TargetName = Convert.ToString(TargetList.Rows[index].Cells["mTargetName"].Value);
                    PS4.Dialogs.TargetDetails(TargetName);
                }
            }
            catch
            {

            }
        }

        private void TargetContextMenu_Opening(object sender, CancelEventArgs e)
        {
            Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                int index = TargetList.SelectedRows[0].Index;
                bool Available = TargetList.Rows[index].Cells["mStatus"].Value.Equals("Available");

                if(Available)
                {
                    Target_Details.Enabled = true;
                    SendOrbisPayload.Enabled = false;
                    Target_Reboot.Enabled = true;
                    Target_Shutdown.Enabled = true;
                    Target_Suspend.Enabled = true;
                }
                else
                {
                    Target_Details.Enabled = false;
                    SendOrbisPayload.Enabled = true;
                    Target_Reboot.Enabled = false;
                    Target_Shutdown.Enabled = false;
                    Target_Suspend.Enabled = false;
                }
            }
        }
    }
}
