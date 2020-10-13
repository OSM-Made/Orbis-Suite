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
    public partial class Settings : DarkDialog
    {
        OrbisLib PS4;

        public Settings(OrbisLib PS4)
        {
            InitializeComponent();

            btnOk.Text = "Save";

            this.PS4 = PS4;

            UpdateSettings();
        }

        public void RefreshTargetList()
        {
            DefaultTargetComboBox.Items.Clear();
            List<TargetInfo> TargetList = PS4.TargetManagement.GetTargetList();
            foreach (TargetInfo Target in TargetList)
            {
                if (Target.Default)
                    DefaultTargetComboBox.Text = Target.Name;

                DefaultTargetComboBox.Items.Add(Target.Name);
            }
        }

        public void UpdateSettings()
        {
            AutoLoadPayload.Checked = PS4.Settings.GetAutoLoadPayload();
            StartOnBoot.Checked = PS4.Settings.GetStartOnBoot();
            PromptAttach.Checked = PS4.Settings.GetDetectGame();

            RefreshTargetList();
            //DefaultCOMPort.Text = PS4.Settings.GetCOMPort(); //TODO: GetCOMPort List
            ServicePort.Text = PS4.Settings.GetServicePort().ToString();
            APIPort.Text = PS4.Settings.GetAPIPort().ToString();

            CensorIDPS.Checked = PS4.Settings.GetCensorIDPS();
            CensorPSID.Checked = PS4.Settings.GetCensorPSID();

            OrbisLibDebug.Checked = PS4.Settings.GetDebug();
            OrbisLibLogs.Checked = PS4.Settings.GetCreateLogs();

            ShowTimestamps.Checked = PS4.Settings.GetShowTimestamps();
            WordWrap.Checked = PS4.Settings.GetWordWrap();
        }

        public void SaveSettings()
        {
            PS4.Settings.SetAutoLoadPayload(AutoLoadPayload.Checked);
            PS4.Settings.SetStartOnBoot(StartOnBoot.Checked);
            PS4.Settings.SetDetectGame(PromptAttach.Checked);

            PS4.TargetManagement.SetDefault(DefaultTargetComboBox.Text);
            //DefaultCOMPort.Text = PS4.Settings.SetCOMPort(); //TODO: SetCOMPort List
            PS4.Settings.SetServicePort(Convert.ToInt32(ServicePort.Text));
            PS4.Settings.SetAPIPort(Convert.ToInt32(APIPort.Text));

            PS4.Settings.SetCensorIDPS(CensorIDPS.Checked);
            PS4.Settings.SetCensorPSID(CensorPSID.Checked);

            PS4.Settings.SetDebug(OrbisLibDebug.Checked);
            PS4.Settings.SetCreateLogs(OrbisLibLogs.Checked);

            PS4.Settings.SetShowTimestamps(ShowTimestamps.Checked);
            PS4.Settings.SetWordWrap(WordWrap.Checked);
        }

        private void ServicePort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void APIPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
