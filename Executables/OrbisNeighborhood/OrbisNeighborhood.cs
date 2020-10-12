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
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
        {
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

        public void UpdateCurrentProc()
        {
            
        }

        public void UpdateTargetList()
        {
            SetStatus("Updating List...");

            try
            {
                if(TargetList.Rows.Count > PS4.TargetManagement.GetTargetCount())
                    TargetList.Rows.Clear();

                CurrentTarget.Text = "Target: N/A";

                int LoopCount = 0;
                List<TargetInfo> Targets = PS4.TargetManagement.GetTargetList();

                foreach (TargetInfo Target in Targets)
                {
                    object[] obj = { Target.Name.Equals(PS4.TargetManagement.GetDefault().Name) ? nsOrbisNeighborhood.Properties.Resources.Default : nsOrbisNeighborhood.Properties.Resources.NotDefault, Target.Name, Target.Firmware, Target.IPAddr, Target.Available ? "Available" : "Not Available", Target.Title, Target.SDKVersion, Target.ConsoleName, Target.ConsoleType };
                    if (TargetList.Rows.Count <= LoopCount)
                        TargetList.Rows.Add(obj);
                    else
                        TargetList.Rows[LoopCount].SetValues(obj);

                    if (Target.Name.Equals(PS4.TargetManagement.GetDefault().Name))
                    {
                        CurrentTarget.Text = Target.Name;
                        if (Target.Attached)
                            CurrentProc.Text = "Process: " + Target.CurrentProc;
                        else
                            CurrentProc.Text = "Process: N/A";
                    }

                    LoopCount++;
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
                AutoLoadPayload_Button.Checked = PS4.Settings.GetAutoLoadPayload();
                LoadOnBoot_Button.Checked = PS4.Settings.GetStartOnBoot();
            }
            catch
            {

            }

            SetStatus("Ready");
        }

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
                PS4.Settings.SetAutoLoadPayload(AutoLoadPayload_Button.Checked);
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
                PS4.Settings.SetStartOnBoot(LoadOnBoot_Button.Checked);
            }
            catch
            {

            }
        }

        private void AddTarget_Button_Click(object sender, EventArgs e)
        {
            PS4.Target["OSM's Console"].Process.SelectProcess();
            //if (PS4.Dialogs.AddTarget() == DialogResult.OK)
            //    UpdateTargetList();
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
        private void Target_Payload_Click(object sender, EventArgs e)
        {
            //TODO:Add the send payload function.

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
                    Target_Payload.Enabled = false;
                    Target_Reboot.Enabled = true;
                    Target_Shutdown.Enabled = true;
                    Target_Suspend.Enabled = true;
                }
                else
                {
                    Target_Details.Enabled = false;
                    Target_Payload.Enabled = true;
                    Target_Reboot.Enabled = false;
                    Target_Shutdown.Enabled = false;
                    Target_Suspend.Enabled = false;
                }
            }
        }
    }
}
