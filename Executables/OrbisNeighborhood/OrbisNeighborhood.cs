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

        public OrbisNeighborhood()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

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

            //Register Events
            PS4.Events.TargetAvailable += PS4_TargetAvailable;
            PS4.Events.TargetUnAvailable += PS4_TargetUnAvailable;
            PS4.Events.TargetNewTitle += Events_TargetNewTitle;
            //PS4.Events.DBTouched += Events_DBTouched;
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
        {
            Console.WriteLine("Events_DBTouched");
            //UpdateSettings();
            //UpdateTargetList();
        }

        private void Events_TargetNewTitle(object sender, TargetNewTitleEvent e)
        {
            UpdateTargetList();
        }

        private void PS4_TargetUnAvailable(object sender, TargetUnAvailableEvent e)
        {
            UpdateTargetList();
        }

        private void PS4_TargetAvailable(object sender, TargetAvailableEvent e)
        {
            UpdateTargetList();
        }

        public void UpdateTargetList()
        {
            SetStatus("Updating List...");

            try
            {
                //TargetList.Rows.Clear();
                DefaultTargetLabel.Text = "N/A";

                int LoopCount = 0;
                List<TargetInfo> Targets = PS4.Target.GetTargetList();

                foreach (TargetInfo Target in Targets)
                {
                    object[] obj = { Target.Name.Equals(PS4.Target.GetDefault().Name) ? nsOrbisNeighborhood.Properties.Resources.Default : nsOrbisNeighborhood.Properties.Resources.NotDefault, Target.Name, Target.Firmware, Target.IPAddr, Target.Available ? "Available" : "Not Available", Target.Title, Target.SDKVersion, Target.ConsoleName, Target.ConsoleType };
                    if (TargetList.Rows.Count <= LoopCount)
                        TargetList.Rows.Add(obj);
                    else
                        TargetList.Rows[LoopCount].SetValues(obj);

                    /*if (Target.Name.Equals(PS4.Target.GetDefault().Name))
                    {
                        DefaultTargetLabel.Text = Target.Name;
                        TargetList.Rows[LoopCount].Cells["mDefault"].Value = nsOrbisNeighborhood.Properties.Resources.Default;
                    }
                    else
                        TargetList.Rows[LoopCount].Cells["mDefault"].Value = nsOrbisNeighborhood.Properties.Resources.NotDefault;*/

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
                AutoLoadPayload_Button.Checked = PS4.Target.GetAutoLoadPayload();
                LoadOnBoot_Button.Checked = PS4.Target.GetStartOnBoot();
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

        private void TargetList_Click(object sender, EventArgs e)
        {
            //check if currently selecting then hide or show options
        }

        private void AutoLoadPayload_Button_Click(object sender, EventArgs e)
        {
            try
            {
                AutoLoadPayload_Button.Checked = !AutoLoadPayload_Button.Checked;
                PS4.Target.SetAutoLoadPayload(AutoLoadPayload_Button.Checked);
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
                PS4.Target.SetStartOnBoot(LoadOnBoot_Button.Checked);
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

                    PS4.Target.SetDefault(TargetName);

                    UpdateTargetList();
                }
            }
            catch
            {

            }
            //PS4.Target.
        }
    }
}
