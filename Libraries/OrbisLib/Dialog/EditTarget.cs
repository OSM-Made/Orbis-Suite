using DarkUI.Forms;
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

namespace OrbisSuite.Dialog
{
    public partial class EditTarget : DarkForm
    {
        private OrbisLib PS4;
        string OriginalTargetName;
        string OriginalIPAddr;

        public EditTarget(OrbisLib PS4, string TargetName)
        {
            InitializeComponent();

            //Pass the original dll instance here.
            this.PS4 = PS4;

            //Back up the original name for db lookup.
            OriginalTargetName = TargetName;

            //get the target information we want to edit
            TargetInfo targetInfo;
            PS4.TargetManagement.GetTarget(TargetName, out targetInfo);

            //Bacl up the IP for db test.
            OriginalIPAddr = targetInfo.IPAddr;

            //populate the elements with the data.
            this.TargetName.Text = targetInfo.Name;
            TargetIPAddress.Text = targetInfo.IPAddr;

            switch(Convert.ToDouble(targetInfo.Firmware) * 100)
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

            IsDefaultTarget.Checked = targetInfo.Default;
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
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

            if(!TargetName.Text.Equals(OriginalTargetName) && PS4.TargetManagement.DoesTargetExist(TargetName.Text))
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

            if(PS4.TargetManagement.SetTarget(OriginalTargetName, IsDefaultTarget.Checked, TargetName.Text, TargetIPAddress.Text, Firmware))
                DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                DarkMessageBox.ShowError("An unknown error caused the target to not be saved. Please try again.", "Failed to save target.");
        }
    }
}
