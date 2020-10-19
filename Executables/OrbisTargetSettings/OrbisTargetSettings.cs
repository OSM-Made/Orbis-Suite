using DarkUI.Forms;
using OrbisSuite;
using OrbisSuite.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisTargetSettings
{
    public partial class OrbisTargetSettings : DarkForm
    {
        OrbisLib PS4 = new OrbisLib();
        string OriginalIPAddr;
        bool IsDefaultTarget;

        private void ExecuteSecure(Action a)
        {
            if (InvokeRequired)
                BeginInvoke(a);
            else
                a();
        }

        private void SelectTarget(string TargetName)
        {
            //Back up the original name for db lookup.
            SelectedTarget.Text = TargetName;

            //get the target information we want to edit
            TargetInfo targetInfo;
            PS4.TargetManagement.GetTarget(TargetName, out targetInfo);

            //Bacl up the IP for db test.
            OriginalIPAddr = targetInfo.IPAddr;

            //populate the elements with the data.
            this.TargetName.Text = targetInfo.Name;
            TargetIPAddress.Text = targetInfo.IPAddr;

            switch (Convert.ToDouble(targetInfo.Firmware) * 100)
            {
                case 176:
                    TargetFW176.Checked = true;
                    break;
                case 405:
                    TargetFW405.Checked = true;
                    break;
                case 455:
                    TargetFW455.Checked = true;
                    break;
                case 505:
                    TargetFW505.Checked = true;
                    break;
                case 672:
                    TargetFW672.Checked = true;
                    break;
                case 702:
                    TargetFW702.Checked = true;
                    break;
            }

            IsDefaultTarget = targetInfo.Default;
        }

        private void UpdateTargetList()
        {
            try
            {
                if (PS4.TargetManagement.TargetList.Count <= 0)
                {
                    DarkMessageBox.ShowError("Target list is empty nothing to show.", "Target List Empty", DarkDialogButton.Ok, FormStartPosition.CenterScreen);
                    Environment.Exit(0);
                }

                SelectedTarget.Items.Clear();
                foreach (TargetInfo Target in PS4.TargetManagement.TargetList)
                    SelectedTarget.Items.Add(Target.Name);

                if(PS4.TargetManagement.TargetList.Find(x => x.Name == SelectedTarget.Text) == null)
                {
                    SelectTarget(PS4.TargetManagement.TargetList[0].Name);
                }
            }
            catch
            {

            }
        }

        #region One Instance

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_TARGETSETTINGS)
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

        public OrbisTargetSettings()
        {
            InitializeComponent();

            if (PS4.TargetManagement.TargetList.Count <= 0) //TODO: Could add a "Add Target" Option instead and disable the other options.
            {
                DarkMessageBox.ShowError("Target list is empty nothing to show.", "Target List Empty", DarkDialogButton.Ok, FormStartPosition.CenterScreen);
                Environment.Exit(0);
            }

            //Select the inital first member.
            SelectTarget(PS4.TargetManagement.TargetList[0].Name);

            //Update initial list.
            UpdateTargetList();

            //Events.
            PS4.Events.DBTouched += Events_DBTouched;
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
        {
            ExecuteSecure(() => UpdateTargetList());
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            PS4.TargetManagement.DeleteTarget(SelectedTarget.Text);

            UpdateTargetList();
        }

        private void Button_SaveTarget_Click(object sender, EventArgs e)
        {
            IPAddress IPAddr;
            if (!IPAddress.TryParse(TargetIPAddress.Text, out IPAddr))
            {
                DarkMessageBox.ShowError("A valid IP Address must be entered!", "Invalid Target IP Address");
                return;
            }

            if (!TargetIPAddress.Text.Equals(OriginalIPAddr) && PS4.TargetManagement.DoesTargetExistIP(TargetIPAddress.Text))
            {
                DarkMessageBox.ShowError("A Target with this IP Address already exists!", "Invalid Target IP Address");
                return;
            }

            if (TargetName.Text.Equals("") || TargetName.Text.Length == 0)
            {
                DarkMessageBox.ShowError("The target name must not be empty!", "Invalid Target Name");
                return;
            }

            if (TargetName.Text.Length >= 0x100)
            {
                DarkMessageBox.ShowError("The target name must not be longer than 256 characters!", "Invalid Target Name");
                return;
            }

            if (!TargetName.Text.Equals(SelectedTarget.Text) && PS4.TargetManagement.DoesTargetExist(TargetName.Text))
            {
                DarkMessageBox.ShowError("A Target with this Name already exists!", "Invalid Target Name");
                return;
            }

            int Firmware = 0;
            if (TargetFW176.Checked)
                Firmware = 176;
            else if (TargetFW405.Checked)
                Firmware = 405;
            else if (TargetFW455.Checked)
                Firmware = 455;
            else if (TargetFW505.Checked)
                Firmware = 505;
            else if (TargetFW672.Checked)
                Firmware = 672;
            else if (TargetFW702.Checked)
                Firmware = 702;

            if (PS4.TargetManagement.SetTarget(SelectedTarget.Text, IsDefaultTarget, TargetName.Text, TargetIPAddress.Text, Firmware, Convert.ToInt32(PayloadPort.Text)))
            {
                SelectedTarget.Text = TargetName.Text;
            }
            else
                DarkMessageBox.ShowError("An unknown error caused the target to not be saved. Please try again.", "Failed to save target.");
        }

        private void SelectedTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PS4.TargetManagement.TargetList.Find(x => x.Name == SelectedTarget.Text) == null)
            {
                SelectTarget(PS4.TargetManagement.TargetList[0].Name);
            }
            else
            {
                SelectTarget(SelectedTarget.Text);
            }
        }
    }
}
