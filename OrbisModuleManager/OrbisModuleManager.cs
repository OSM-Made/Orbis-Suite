using DarkUI.Forms;
using OrbisSuite;
using OrbisSuite.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisModuleManager
{
    public partial class OrbisModuleManager : DarkForm
    {
        OrbisLib PS4 = new OrbisLib();

        void SetStatus(string Val)
        {
            StatusLabel.Text = Val;
        }

        private void ExecuteSecure(Action a)
        {
            if (InvokeRequired)
                BeginInvoke(a);
            else
                a();
        }

        public OrbisModuleManager()
        {
            InitializeComponent();

            ModuleList.RowsDefaultCellStyle.BackColor = Color.FromArgb(57, 60, 62);
            ModuleList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(60, 63, 65);

            ModuleList.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
            ModuleList.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(57, 60, 62);
            ModuleList.EnableHeadersVisualStyles = false;
            ModuleList.DefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
            ModuleList.DefaultCellStyle.BackColor = Color.FromArgb(57, 60, 62);
            ModuleList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 75, 75);
            ModuleList.DefaultCellStyle.SelectionForeColor = Color.FromArgb(220, 220, 220);

            ModuleList.BackColor = Color.FromArgb(122, 128, 132);

            //Load Stored Path
            SPRXDirectory.Text = Properties.Settings.Default.SPRXDir;

            //Register Events
            PS4.DefaultTarget.Events.ProcAttach += Events_ProcAttach;
            PS4.DefaultTarget.Events.ProcDetach += Events_ProcDetach;
            PS4.DefaultTarget.Events.ProcDie += Events_ProcDie;
            PS4.Events.DBTouched += Events_DBTouched;

            //Make sure the target info is updated on start.
            UpdateTarget();

            //If the target is available and were attached update the module list.
            UpdateModuleList();
        }


        #region Tool Strip

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            PS4.Dialogs.Settings();
        }

        private void About_Button_Click(object sender, EventArgs e)
        {
            PS4.Dialogs.About();
        }

        private void Button_Attach_Click(object sender, EventArgs e)
        {
            if (PS4.DefaultTarget.Info.Available)
                PS4.DefaultTarget.Process.SelectProcess();
        }

        private void Button_Detach_Click(object sender, EventArgs e)
        {
            if (PS4.DefaultTarget.Info.Available && PS4.DefaultTarget.Info.Attached)
                PS4.DefaultTarget.Process.Detach();
        }

        #endregion

        #region PS4 Events

        private void EnableProgram(bool state, bool IsAttached = false)
        {
            if (state)
            {
                Button_Attach.Enabled = true;
                FTPStrip_Refresh.Enabled = true;

                if (IsAttached)
                {
                    //Enable Controls
                    Button_Detach.Enabled = true;
                    CurrentProc.Text = string.Format("Process: {0}", PS4.DefaultTarget.Info.CurrentProc);

                    Button_Detach.Enabled = true;

                    MenuStrip_Unload.Enabled = true;
                    MenuStrip_Reload.Enabled = true;
                    MenuStrip_Dump.Enabled = true;
                    MenuStrip_Refresh.Enabled = true;

                    FTPStrip_LoadModule.Enabled = true;

                    Button_ReloadModule.Enabled = true;
                    Button_UnloadModule.Enabled = true;
                    Button_LoadModule.Enabled = true;

                    Button_OpenELF.Enabled = true;
                    Button_LoadELF.Enabled = true;
                }
                else
                {
                    CurrentProc.Text = "Process: N/A";
                    Button_Detach.Enabled = false;

                    //Initialize Module List
                    ModuleList.Rows.Clear();
                    for (int i = 0; i < 17; i++)
                        ModuleList.Rows.Add();

                    //Setup Scroll Bar
                    darkScrollBar1.Minimum = 0;
                    darkScrollBar1.Maximum = 100;
                    darkScrollBar1.ViewSize = 99;
                    darkScrollBar1.Enabled = false;

                    Button_Detach.Enabled = false;

                    MenuStrip_Unload.Enabled = false;
                    MenuStrip_Reload.Enabled = false;
                    MenuStrip_Dump.Enabled = false;
                    MenuStrip_Refresh.Enabled = false;

                    FTPStrip_LoadModule.Enabled = false;

                    Button_ReloadModule.Enabled = false;
                    Button_UnloadModule.Enabled = false;
                    Button_LoadModule.Enabled = false;


                    Button_OpenELF.Enabled = false;
                    Button_LoadELF.Enabled = false;
                }
            }
            else
            {
                FTPStrip_Refresh.Enabled = false;

                //Initialize Module List
                ModuleList.Rows.Clear();
                for (int i = 0; i < 17; i++)
                    ModuleList.Rows.Add();

                //Setup Scroll Bar
                darkScrollBar1.Minimum = 0;
                darkScrollBar1.Maximum = 100;
                darkScrollBar1.ViewSize = 99;
                darkScrollBar1.Enabled = false;

                CurrentProc.Text = "Process: N/A";

                //Disable Controls
                Button_Attach.Enabled = false;
                Button_Detach.Enabled = false;

                MenuStrip_Unload.Enabled = false;
                MenuStrip_Reload.Enabled = false;
                MenuStrip_Dump.Enabled = false;
                MenuStrip_Refresh.Enabled = false;

                FTPStrip_LoadModule.Enabled = false;

                Button_ReloadModule.Enabled = false;
                Button_UnloadModule.Enabled = false;
                Button_LoadModule.Enabled = false;
                

                Button_OpenELF.Enabled = false;
                Button_LoadELF.Enabled = false;
            }
        }

        public void UpdateTarget()
        {
            try
            {
                if (PS4.TargetManagement.DoesDefaultTargetExist())
                {
                    if (PS4.DefaultTarget.Info.Available)
                        EnableProgram(true, PS4.DefaultTarget.Info.Attached);
                    else
                        EnableProgram(false);

                    CurrentTarget.Text = string.Format("Target: {0}", PS4.DefaultTarget.Info.Name);
                }
                else
                {
                    CurrentTarget.Text = "Target: N/A";
                    CurrentProc.Text = "Process: N/A";
                    Button_Detach.Enabled = false;
                    EnableProgram(false);
                }
            }
            catch
            {

            }
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
        {
            ExecuteSecure(() => UpdateTarget());
        }

        private void Events_ProcDie(object sender, OrbisSuite.Classes.ProcDieEvent e)
        {
            ExecuteSecure(() => UpdateTarget());
        }

        private void Events_ProcDetach(object sender, OrbisSuite.Classes.ProcDetachEvent e)
        {
            ExecuteSecure(() => UpdateTarget());
        }

        private void Events_ProcAttach(object sender, OrbisSuite.Classes.ProcAttachEvent e)
        {
            ExecuteSecure(() => UpdateTarget());
            ExecuteSecure(() => UpdateModuleList());
        }

        #endregion

        #region Module List

        public void UpdateModuleList()
        {
            ProcessInfo Info;
            if (PS4.DefaultTarget.Process.GetCurrent(out Info) != API_ERRORS.API_OK)
                return;

            SetStatus("Updating List...");

            try
            {
                List<ModuleInfo> List = new List<ModuleInfo>();
                PS4.DefaultTarget.Process.GetLibraryList(out List);

                ModuleList.Rows.Clear();

                foreach (ModuleInfo Module in List)
                {
                    object[] obj = { Module.Handle.ToString(), Module.Name, "0x" + Module.TextSegmentBase.ToString("X"), "0x" + Module.DataSegmentBase.ToString("X") };
                    ModuleList.Rows.Add(obj);
                }

                if (ModuleList.Rows.Count <= 16)
                {
                    darkScrollBar1.Minimum = 0;
                    darkScrollBar1.Maximum = 100;
                    darkScrollBar1.ViewSize = 99;

                    darkScrollBar1.Enabled = false;
                }
                else
                {
                    darkScrollBar1.Minimum = 0;
                    darkScrollBar1.Maximum = ModuleList.Rows.Count;
                    darkScrollBar1.ViewSize = 17;
                    darkScrollBar1.Enabled = true;
                }
            }
            catch
            {

            }

            SetStatus("Ready");
        }

        private void ModuleList_Enter(object sender, EventArgs e)
        {
            ModuleList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 75, 75);
        }

        private void ModuleList_Leave(object sender, EventArgs e)
        {
            ModuleList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(92, 92, 92);
        }

        private void darkScrollBar1_ValueChanged(object sender, DarkUI.Controls.ScrollValueEventArgs e)
        {
            try
            {
                ModuleList.FirstDisplayedScrollingRowIndex = e.Value;
            }
            catch
            {

            }
        }

        private void ModuleList_Scroll(object sender, ScrollEventArgs e)
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



        #endregion

        private void MenuStrip_Unload_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = ModuleList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = ModuleList.SelectedRows[0].Index;
                    int ModuleHandle = Convert.ToInt32(ModuleList.Rows[index].Cells["mHandle"].Value);
                    string ModuleName = Convert.ToString(ModuleList.Rows[index].Cells["ModuleName"].Value);

                    SetStatus("Unloading Module " + ModuleName + "...");

                    PS4.DefaultTarget.Process.UnloadSPRX(ModuleHandle, 0);

                    Thread.Sleep(100);

                    UpdateModuleList();

                    SetStatus("Ready");
                }
            }
            catch
            {

            }
        }

        private void MenuStrip_Reload_Click(object sender, EventArgs e)
        {

        }

        private void MenuStrip_Dump_Click(object sender, EventArgs e)
        {

        }

        private void MenuStrip_Refresh_Click(object sender, EventArgs e)
        {
            UpdateModuleList();
        }


        Int32 ModuleHandle = 0;
        private void Button_ReloadModule_Click(object sender, EventArgs e)
        {
            SetStatus("ReLoading Module " + Path.GetFileName(SPRXDirectory.Text) + "...");

            PS4.DefaultTarget.Process.ReloadSPRX(Path.GetFileName(SPRXDirectory.Text), 0);

            Thread.Sleep(100);

            UpdateModuleList();

            SetStatus("Ready");
        }

        private void Button_UnloadModule_Click(object sender, EventArgs e)
        {
            SetStatus("UnLoading Module " + Path.GetFileName(SPRXDirectory.Text) + "...");

            PS4.DefaultTarget.Process.UnloadSPRX(Path.GetFileName(SPRXDirectory.Text), 0);

            Thread.Sleep(100);

            UpdateModuleList();

            SetStatus("Ready");
        }

        private void Button_LoadModule_Click(object sender, EventArgs e)
        {
            SetStatus("Loading Module " + Path.GetFileName(SPRXDirectory.Text) + "...");

            PS4.DefaultTarget.Process.LoadSPRX(SPRXDirectory.Text, 0);

            Thread.Sleep(100);

            UpdateModuleList();

            SetStatus("Ready");
        }
    }
}
