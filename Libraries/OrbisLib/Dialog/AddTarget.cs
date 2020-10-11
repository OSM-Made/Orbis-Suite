using DarkUI.Forms;
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
    public partial class AddTarget : DarkForm
    {
        private OrbisLib PS4;

        public AddTarget(OrbisLib PS4)
        {
            InitializeComponent();

            this.PS4 = PS4;
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void Button_AddTarget_Click(object sender, EventArgs e)
        {
            IPAddress IPAddr;
            if (!IPAddress.TryParse(TargetIPAddress.Text, out IPAddr))
            {
                DarkMessageBox.ShowError("A valid IP Address must be entered!", "Invalid Target IP Address");
                return;
            }

            if (PS4.TargetManagement.DoesTargetExistIP(TargetIPAddress.Text))
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

            if (PS4.TargetManagement.DoesTargetExist(TargetName.Text))
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

            if (PS4.TargetManagement.NewTarget(IsDefaultTarget.Checked, TargetName.Text, TargetIPAddress.Text, Firmware))
                DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                DarkMessageBox.ShowError("An unknown error caused the target to not be saved. Please try again.", "Failed to save target.");
        }
    }
}
