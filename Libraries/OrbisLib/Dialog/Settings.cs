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

            RefreshTargetList();
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

        private void darkSectionPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void darkTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void darkTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void darkTitle3_Click(object sender, EventArgs e)
        {

        }

        private void darkTitle4_Click(object sender, EventArgs e)
        {

        }

        private void darkComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void darkSectionPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void darkTitle2_Click(object sender, EventArgs e)
        {

        }
    }
}
