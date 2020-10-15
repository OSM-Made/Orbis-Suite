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
            foreach (TargetInfo Target in PS4.TargetManagement.TargetList)
            {
                if (Target.Default)
                    DefaultTargetComboBox.Text = Target.Name;

                DefaultTargetComboBox.Items.Add(Target.Name);
            }
        }

        public void UpdateSettings()
        {
            AutoLoadPayload.Checked = PS4.Settings.AutoLoadPayload;
            StartOnBoot.Checked = PS4.Settings.StartOnBoot;
            PromptAttach.Checked = PS4.Settings.DetectGame;

            RefreshTargetList();
            //DefaultCOMPort.Text = PS4.Settings.GetCOMPort(); //TODO: GetCOMPort List
            ServicePort.Text = PS4.Settings.ServicePort.ToString();
            APIPort.Text = PS4.Settings.APIPort.ToString();

            CensorIDPS.Checked = PS4.Settings.CensorIDPS;
            CensorPSID.Checked = PS4.Settings.CensorPSID;

            OrbisLibDebug.Checked = PS4.Settings.Debug;
            OrbisLibLogs.Checked = PS4.Settings.CreateLogs;

            ShowTimestamps.Checked = PS4.Settings.ShowTimestamps;
            WordWrap.Checked = PS4.Settings.WordWrap;
        }

        public void SaveSettings()
        {
            PS4.Settings.AutoLoadPayload = AutoLoadPayload.Checked;
            PS4.Settings.StartOnBoot = StartOnBoot.Checked;
            PS4.Settings.DetectGame = PromptAttach.Checked;

            PS4.TargetManagement.SetDefault(DefaultTargetComboBox.Text);
            //DefaultCOMPort.Text = PS4.Settings.SetCOMPort(); //TODO: SetCOMPort List
            PS4.Settings.ServicePort = Convert.ToInt32(ServicePort.Text);
            PS4.Settings.APIPort = Convert.ToInt32(APIPort.Text);

            PS4.Settings.CensorIDPS = CensorIDPS.Checked;
            PS4.Settings.CensorPSID = CensorPSID.Checked;

            PS4.Settings.Debug = OrbisLibDebug.Checked;
            PS4.Settings.CreateLogs = OrbisLibLogs.Checked;

            PS4.Settings.ShowTimestamps = ShowTimestamps.Checked;
            PS4.Settings.WordWrap = WordWrap.Checked;
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
