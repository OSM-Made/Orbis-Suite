using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisSuite.Dialog
{
    public partial class SelectProcess : DarkDialog
    {
        public string SelectedProcess { get; set; }
        public OrbisLib PS4;
        public string TargetName;

        private void ExecuteSecure(Action a)
        {
            if (InvokeRequired)
                BeginInvoke(a);
            else
                a();
        }

        void LoadProcList()
        {
            try
            {
                ProcessList.Rows.Clear();

                foreach(ProcessInfo Info in PS4.Target[TargetName].Process.List)
                {
                    object[] obj = { Info.PID.ToString(), Info.Name, Info.TitleID, Info.Attached ? OrbisSuite.Properties.Resources.Process_Attached : OrbisSuite.Properties.Resources.Process_Detached };
                    ProcessList.Rows.Add(obj);
                }

                //The number of rows is less than or equal to the number displayed on screen.
                if (ProcessList.Rows.Count <= 17) //Diables the scroll bar
                {
                    darkScrollBar1.Minimum = 0;
                    darkScrollBar1.Maximum = 100;
                    darkScrollBar1.ViewSize = 99;

                    darkScrollBar1.Enabled = false;
                }
                else //enables the scroll bar
                {
                    darkScrollBar1.Minimum = 0;
                    darkScrollBar1.Maximum = ProcessList.Rows.Count;
                    darkScrollBar1.ViewSize = 17;
                    darkScrollBar1.Enabled = true;
                }
            }
            catch
            {

            }
        }

        public SelectProcess(OrbisLib PS4, string TargetName)
        {
            InitializeComponent();

            ProcessList.RowsDefaultCellStyle.BackColor = Color.FromArgb(57, 60, 62);
            ProcessList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(60, 63, 65);

            ProcessList.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
            ProcessList.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(57, 60, 62);
            ProcessList.EnableHeadersVisualStyles = false;
            ProcessList.DefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
            ProcessList.DefaultCellStyle.BackColor = Color.FromArgb(57, 60, 62);
            ProcessList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 75, 75);
            ProcessList.DefaultCellStyle.SelectionForeColor = Color.FromArgb(220, 220, 220);

            ProcessList.BackColor = Color.FromArgb(122, 128, 132);

            this.PS4 = PS4;
            this.TargetName = TargetName;

            btnOk.Text = "Select";

            PS4.Target[TargetName].Events.ProcDetach += Events_ProcDetach;
            PS4.Target[TargetName].Events.ProcAttach += Events_ProcAttach;
            PS4.Target[TargetName].Events.ProcDie += Events_ProcDie;
            PS4.Target[TargetName].Events.TargetNewTitle += Events_TargetNewTitle;

            PS4.Events.TargetUnAvailable += Events_TargetUnAvailable;
        }

        private void Events_TargetUnAvailable(object sender, TargetUnAvailableEvent e)
        {
            if(e.TargetName == TargetName)
                ExecuteSecure(() => Close());
        }

        private void Events_TargetNewTitle(object sender, TargetNewTitleEvent e)
        {
            ExecuteSecure(() => LoadProcList());
        }

        private void Events_ProcDie(object sender, ProcDieEvent e)
        {
            ExecuteSecure(() => LoadProcList());
        }

        private void Events_ProcAttach(object sender, ProcAttachEvent e)
        {
            ExecuteSecure(() => LoadProcList());
        }

        private void Events_ProcDetach(object sender, ProcDetachEvent e)
        {
            ExecuteSecure(() => LoadProcList());
        }

        private void SelectProcess_Load(object sender, EventArgs e)
        {
            LoadProcList();
        }

        private void SelectProcess_FormClosing(object sender, FormClosingEventArgs e)
        {
            PS4.Target[TargetName].Events.ProcDetach -= Events_ProcDetach;
            PS4.Target[TargetName].Events.ProcAttach -= Events_ProcAttach;
            PS4.Target[TargetName].Events.ProcDie -= Events_ProcDie;
            PS4.Target[TargetName].Events.TargetNewTitle -= Events_TargetNewTitle;

            Int32 selectedCellCount = ProcessList.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                int index = ProcessList.SelectedRows[0].Index;
                SelectedProcess = Convert.ToString(ProcessList.Rows[index].Cells["ProcName"].Value);
            }
        }

        private void ProcessList_Enter(object sender, EventArgs e)
        {
            ProcessList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 75, 75);
        }

        private void ProcessList_Leave(object sender, EventArgs e)
        {
            ProcessList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 92, 92);
        }

        private void ToolStrip_DetachProcess_Click(object sender, EventArgs e)
        {
            Int32 selectedCellCount = ProcessList.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                int index = ProcessList.SelectedRows[0].Index;
                string ProcName = Convert.ToString(ProcessList.Rows[index].Cells["ProcName"].Value);
                PS4.Target[TargetName].Process.Detach(ProcName);
                Thread.Sleep(400);
                LoadProcList();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadProcList();
        }

        private void ProcessList_Scroll(object sender, ScrollEventArgs e)
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
                ProcessList.FirstDisplayedScrollingRowIndex = e.Value;
            }
            catch
            {

            }
        }

        public void AttachtoSelected()
        {
            Int32 selectedCellCount = ProcessList.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                int index = ProcessList.SelectedRows[0].Index;
                SelectedProcess = Convert.ToString(ProcessList.Rows[index].Cells["ProcName"].Value);
                API_ERRORS res = PS4.Target[TargetName].Process.Attach(SelectedProcess);

                if (res == API_ERRORS.API_OK)
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                else
                    DarkMessageBox.ShowError(OrbisDef.API_ERROR_STR[(int)res], "Error: Failed to Attach!");

            }
            else
                DarkMessageBox.ShowError("Please Select a Process", "Error: No Process Selected!");
        }

        private void ProcessList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AttachtoSelected();
        }

        private void ToolStrip_KillProcess_Click(object sender, EventArgs e)
        {
            Int32 selectedCellCount = ProcessList.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                int index = ProcessList.SelectedRows[0].Index;
                SelectedProcess = Convert.ToString(ProcessList.Rows[index].Cells["ProcName"].Value);

                PS4.Target[TargetName].Process.Kill(SelectedProcess);
            }
        }
    }
}
