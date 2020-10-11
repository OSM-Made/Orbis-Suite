using DarkUI.Forms;
using OrbisSuite.Classes;
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
    public partial class SelectProcess : DarkForm
    {
        public string SelectedProcess { get; set; }
        public OrbisLib PS4;
        bool ShouldStopThreads = false;
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
            ProcessList.Rows.Clear();

            List<ProcessInfo> List = PS4.Target[TargetName].Process.GetList();

            for (int i = 0; i < List.Count; i++)
            {
                object[] obj = { List[i].PID.ToString(), List[i].Name, List[i].TitleID, List[i].Attached ? OrbisSuite.Properties.Resources.Process_Attached : OrbisSuite.Properties.Resources.Process_Detached };
                ProcessList.Rows.Add(obj);
            }

            if (ProcessList.Rows.Count <= 16)
            {
                darkScrollBar1.Minimum = 0;
                darkScrollBar1.Maximum = 100;
                darkScrollBar1.ViewSize = 99;

                darkScrollBar1.Enabled = false;
            }
            else
            {
                darkScrollBar1.Minimum = 0;
                darkScrollBar1.Maximum = ProcessList.Rows.Count;
                darkScrollBar1.ViewSize = 17;
                darkScrollBar1.Enabled = true;
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

            PS4.Target[TargetName].Events.ProcDetach += Events_ProcDetach;
            PS4.Target[TargetName].Events.ProcAttach += Events_ProcAttach;
            PS4.Target[TargetName].Events.ProcDie += Events_ProcDie;
            PS4.Target[TargetName].Events.TargetNewTitle += Events_TargetNewTitle;
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
            //Setup Scroll Bar
            darkScrollBar1.Minimum = 0;
            darkScrollBar1.Maximum = 100;
            darkScrollBar1.ViewSize = 99;
            darkScrollBar1.Enabled = false;

            //Checking Scroll
            hScrollThread = new Thread(() => ScrollThread());
            hScrollThread.IsBackground = true;
            hScrollThread.Start();

            LoadProcList();
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void Button_SelectProcess_Click(object sender, EventArgs e)
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

        private void SelectProcess_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShouldStopThreads = true;
        }

        bool LockThread = false;
        public int SrollVal = 0;
        System.Threading.Thread hScrollThread;
        void ScrollThread()
        {
            while (!ShouldStopThreads)
            {
                if (!LockThread)
                {
                    if (SrollVal != darkScrollBar1.Value)
                    {
                        ExecuteSecure(() => ProcessList.FirstDisplayedScrollingRowIndex = darkScrollBar1.Value);
                        //Console.WriteLine(darkScrollBar1.Value);
                    }
                }

                Thread.Sleep(10);
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
            LockThread = true;
            //Console.WriteLine(e.NewValue);
            SrollVal = e.NewValue;
            darkScrollBar1.ScrollTo(e.NewValue);
            darkScrollBar1.UpdateScrollBar();

            LockThread = false;
        }

        private void ProcessList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 selectedCellCount = ProcessList.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                int index = ProcessList.SelectedRows[0].Index;
                SelectedProcess = Convert.ToString(ProcessList.Rows[index].Cells["ProcName"].Value);
                PS4.Target[TargetName].Process.Attach(SelectedProcess);
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
                DarkMessageBox.ShowError("Please Select a Process", "Error: No Process Selected!");
        }
    }
}
