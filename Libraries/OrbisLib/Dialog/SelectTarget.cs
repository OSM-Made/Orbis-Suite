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

namespace OrbisSuite
{
    public partial class SelectTarget : DarkDialog
    {
        OrbisLib PS4;

        public SelectTarget(OrbisLib PS4)
        {
            InitializeComponent();

            this.PS4 = PS4;
        }

        private void SelectTarget_Load(object sender, EventArgs e)
        {
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

            btnOk.Text = "Select";

            UpdateTargetList();
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
        {
            UpdateTargetList();
        }

        #region TargetList

        List<TargetInfo> InternalList = new List<TargetInfo>();
        private void UpdateTargetList()
        {
            Invoke((MethodInvoker)delegate
            {
                try
                {
                    int BackUpScroll = TargetList.FirstDisplayedScrollingRowIndex;

                    if (TargetList.Rows.Count != PS4.TargetManagement.GetTargetCount())
                        TargetList.Rows.Clear();

                    int LoopCount = 0;
                    foreach (TargetInfo Target in PS4.TargetManagement.TargetList)
                    {
                        object[] obj = {
                        Target.Name.Equals(PS4.TargetManagement.DefaultTarget.Name) ? OrbisSuite.Properties.Resources.Default : OrbisSuite.Properties.Resources.NotDefault,
                        Target.Name, Target.Firmware,
                        Target.IPAddr,
                        Target.Available ? "Available" : "Not Available"
                        };

                        if (TargetList.Rows.Count <= LoopCount)
                            TargetList.Rows.Add(obj);
                        else
                            TargetList.Rows[LoopCount].SetValues(obj);

                        LoopCount++;
                    }

                    if (TargetList.Rows.Count <= 17)
                    {
                        darkScrollBar1.Minimum = 0;
                        darkScrollBar1.Maximum = 100;
                        darkScrollBar1.ViewSize = 99;

                        darkScrollBar1.Enabled = false;
                    }
                    else
                    {
                        darkScrollBar1.Minimum = 0;
                        darkScrollBar1.Maximum = TargetList.Rows.Count;
                        darkScrollBar1.ViewSize = 17;
                        darkScrollBar1.Enabled = true;
                    }

                    TargetList.FirstDisplayedScrollingRowIndex = BackUpScroll;
                    PS4.Events.DBTouched += Events_DBTouched;
                }
                catch
                {

                }
            });
        }

        private void TargetList_Leave(object sender, EventArgs e)
        {
            TargetList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 92, 92);
        }

        private void TargetList_Enter(object sender, EventArgs e)
        {
            TargetList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 75, 75);
        }

        private void TargetList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectCurrentTarget();
        }

        private void TargetList_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                darkScrollBar1.ScrollTo(e.NewValue);
                darkScrollBar1.UpdateScrollBar();
            }
            catch
            {

            }
        }

        private void darkScrollBar1_ValueChanged(object sender, DarkUI.Controls.ScrollValueEventArgs e)
        {
            try
            {
                TargetList.FirstDisplayedScrollingRowIndex = e.Value;
            }
            catch
            {

            }
        }

        #endregion

        public void SelectCurrentTarget()
        {
            Int32 selectedCellCount = TargetList.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                int index = TargetList.SelectedRows[0].Index;
                string SelectedTarget = Convert.ToString(TargetList.Rows[index].Cells["TargetName"].Value);
                PS4.TargetManagement.SetSelected(SelectedTarget);
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
