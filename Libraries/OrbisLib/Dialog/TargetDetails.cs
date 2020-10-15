using DarkUI.Forms;
using OrbisSuite.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisSuite.Dialog
{
    public partial class TargetDetails : DarkDialog
    {
        string TargetName;
        OrbisLib PS4;

        private void ExecuteSecure(Action a)
        {
            if (InvokeRequired)
                BeginInvoke(a);
            else
                a();
        }

        public void UpdateDetails()
        {
            DetailedTargetInfo Info = PS4.TargetManagement.GetInfo(TargetName);

            SDKVersion.Text = Info.SDKVersion;
            SoftwareVersion.Text = Info.SoftwareVersion;
            CPUTemp.Text = string.Format("CPU: {0} °C", Info.CPUTemp);
            SOCTemp.Text = string.Format("SOC: {0} °C", Info.SOCTemp);
            CurrentTitleID.Text = Info.CurrentTitleID;
            ConsoleName.Text = Info.ConsoleName;
            if (PS4.Settings.CensorIDPS)
                IDPS.Text = Utilities.CensorString(Info.IDPS, '#', 16);
            else
                IDPS.Text = Info.IDPS;
            if (PS4.Settings.CensorPSID)
                PSID.Text = Utilities.CensorString(Info.PSID, '#', 16);
            else
                PSID.Text = Info.PSID;
            ConsoleType.Text = Info.ConsoleType;
        }

        public TargetDetails(OrbisLib PS4, string TargetName)
        {
            InitializeComponent();

            this.PS4 = PS4;

            this.TargetName = TargetName;
            Text += " (" + TargetName + ")";

            UpdateDetails();

            PS4.Events.DBTouched += Events_DBTouched;
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
        {
            ExecuteSecure(() => UpdateDetails());
        }

        private void CurrentTitleID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
