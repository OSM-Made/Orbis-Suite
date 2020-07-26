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
        }

        public void UpdateTargetList()
        {
            //PS4.Payload.InjectPayload()

            SetStatus("Updating List...");

            try
            {
                TargetList.Rows.Clear();

                object[] obj = { "", "OSM's Console", "5.05", "192.168.1.142", "Not Connected" };
                TargetList.Rows.Add(obj);

                //if (List[i].Default == 1)
                TargetList.Rows[0].Cells["mDefault"].Value = nsOrbisNeighborhood.Properties.Resources.Default;
                //else
                //    ProcessList.Rows[i].Cells["Attached"].Value = null;

                object[] obj2 = { "", "OSM's Console 2", "6.72", "192.168.1.168", "Not Connected" };
                TargetList.Rows.Add(obj2);

                //if (List[i].Default == 1)
                TargetList.Rows[1].Cells["mDefault"].Value = nsOrbisNeighborhood.Properties.Resources.NotDefault;

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
    }
}
