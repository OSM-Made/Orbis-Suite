using DarkUI.Controls;
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
            SPRXDirectory.Text = Properties.Settings.Default.SPRXDirectory;

            //Register Events
            PS4.DefaultTarget.Events.ProcAttach += Events_ProcAttach;
            PS4.DefaultTarget.Events.ProcDetach += Events_ProcDetach;
            PS4.DefaultTarget.Events.ProcDie += Events_ProcDie;
            PS4.Events.TargetAvailable += Events_TargetAvailable;
            PS4.Events.DBTouched += Events_DBTouched;

            //Make sure the target info is updated on start.
            UpdateTarget();

            //If the target is available and were attached update the module list.
            UpdateModuleList();

            //Update settings
            SPRXDirectory.Text = Properties.Settings.Default.SPRXDirectory;
            ELFDirectory.Text = Properties.Settings.Default.ELFDirectory;

            //Update the FTP on start up.
            UpdateFTP();
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

        private void SPRXDirectory_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SPRXDirectory = SPRXDirectory.Text;
            Properties.Settings.Default.Save();
        }

        private void ELFDirectory_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ELFDirectory = ELFDirectory.Text;
            Properties.Settings.Default.Save();
        }

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

                    FTPStrip_UnloadModule.Enabled = true;
                    FTPStrip_ReloadModule.Enabled = true;
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

                    FTPStrip_UnloadModule.Enabled = false;
                    FTPStrip_ReloadModule.Enabled = false;
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

                FTPStrip_UnloadModule.Enabled = false;
                FTPStrip_ReloadModule.Enabled = false;
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

        private void Events_TargetAvailable(object sender, TargetAvailableEvent e)
        {
            if (e.TargetName.Equals(PS4.TargetManagement.DefaultTarget.Name))
                ExecuteSecure(() => UpdateModuleList());
        }

        #endregion

        #region Module List

        public void UpdateModuleList()
        {
            if (PS4.DefaultTarget.Process.Current.Attached == false)
                return;

            SetStatus("Updating List...");

            try
            {
                int BackUpScroll = ModuleList.FirstDisplayedScrollingRowIndex;

                ModuleList.Rows.Clear();

                foreach (ModuleInfo Module in PS4.DefaultTarget.Process.ModuleList)
                {
                    object[] obj = { Module.Handle.ToString(), Module.Name, "0x" + Module.TextSegmentBase.ToString("X"), "0x" + Module.DataSegmentBase.ToString("X"), Utilities.SizeSuffix((long)(Module.TextSegmentLen + Module.DataSegmentLen)) };
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

                ModuleList.FirstDisplayedScrollingRowIndex = BackUpScroll;
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

        #region ModuleList MenuStrip

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

                    int Result = PS4.DefaultTarget.Process.UnloadSPRX(ModuleHandle, 0);

                    if (Result != 0)
                        DarkMessageBox.ShowError(string.Format("Result returned {0}.", Result.ToString()), "Module Failed to load.");
                    else
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
            try
            {
                Int32 selectedCellCount = ModuleList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = ModuleList.SelectedRows[0].Index;
                    int ModuleHandle = Convert.ToInt32(ModuleList.Rows[index].Cells["mHandle"].Value);
                    string ModuleName = Convert.ToString(ModuleList.Rows[index].Cells["ModuleName"].Value);

                    SetStatus("Reloading Module " + ModuleName + "...");

                    Int32 Handle = PS4.DefaultTarget.Process.ReloadSPRX(ModuleHandle, 0);

                    if (Handle == 0)
                        DarkMessageBox.ShowError("Handle returned 0.", "Module Failed to Reload.");
                    else
                        Thread.Sleep(100);

                    UpdateModuleList();

                    SetStatus("Ready");
                }
            }
            catch
            {

            }
        }

        private void MenuStrip_Dump_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedCellCount = ModuleList.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    int index = ModuleList.SelectedRows[0].Index;
                    int ModuleHandle = Convert.ToInt32(ModuleList.Rows[index].Cells["mHandle"].Value);
                    string ModuleName = Convert.ToString(ModuleList.Rows[index].Cells["ModuleName"].Value);

                    SetStatus("Dumping Module " + ModuleName + "...");

                    ModuleInfo Info = PS4.DefaultTarget.Process.ModuleList.Find(x => x.Name == ModuleName);

                    int Length = (int)(Info.TextSegmentLen + Info.DataSegmentLen);
                    byte[] Buffer = new byte[Length];
                    string FilePath = string.Empty;

                    //If its module 0 thats the process so we want to dump the process else we dump a module.
                    if (ModuleHandle == 0)
                    {
                        API_ERRORS Result = PS4.DefaultTarget.DumpProcess(ModuleName, Length, Buffer);
                        if (Result != API_ERRORS.API_OK)
                        {
                            DarkMessageBox.ShowError(string.Format("Failed to Dump Process \"{0}\".\n{1}", ModuleName, OrbisDef.API_ERROR_STR[(int)Result]), "Error dumping module.");

                            return;
                        }
                    }
                    else
                    {
                        API_ERRORS Result = PS4.DefaultTarget.Process.DumpModule(ModuleName, Length, Buffer);
                        if (Result != API_ERRORS.API_OK)
                        {
                            DarkMessageBox.ShowError(string.Format("Failed to Dump Module \"{0}\".\n{1}", ModuleName, OrbisDef.API_ERROR_STR[(int)Result]), "Error dumping module.");

                            return;
                        }
                    }

                    //Write the file some where.
                    FilePath = string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), Utilities.IndexedFilename(Path.GetFileNameWithoutExtension(ModuleName) + "-Dump", Path.GetExtension(ModuleName)));
                    using (FileStream fs = File.OpenWrite(FilePath))
                        fs.Write(Buffer, 0, (int)Length);

                    SetStatus("Ready");
                }
            }
            catch
            {

            }
        }

        private void MenuStrip_Refresh_Click(object sender, EventArgs e)
        {
            UpdateModuleList();
        }

        #endregion

        #region SPRX Section

        private void Button_ReloadModule_Click(object sender, EventArgs e)
        {
            SetStatus("ReLoading Module " + Path.GetFileName(SPRXDirectory.Text) + "...");

            Int32 Handle = PS4.DefaultTarget.Process.ReloadSPRX(Path.GetFileName(SPRXDirectory.Text), 0);

            if (Handle == 0)
                DarkMessageBox.ShowError("Handle returned 0.", "Module Failed to load.");
            else
                Thread.Sleep(100);

            UpdateModuleList();

            SetStatus("Ready");
        }

        private void Button_UnloadModule_Click(object sender, EventArgs e)
        {
            SetStatus("UnLoading Module " + Path.GetFileName(SPRXDirectory.Text) + "...");

            int Result = PS4.DefaultTarget.Process.UnloadSPRX(Path.GetFileName(SPRXDirectory.Text), 0);

            if(Result != 0)
                DarkMessageBox.ShowError(string.Format("Result returned {0}.", Result.ToString()), "Module Failed to load.");
            else
                Thread.Sleep(100);

            UpdateModuleList();

            SetStatus("Ready");
        }

        private void Button_LoadModule_Click(object sender, EventArgs e)
        {
            SetStatus("Loading Module " + Path.GetFileName(SPRXDirectory.Text) + "...");

            Int32 Handle = PS4.DefaultTarget.Process.LoadSPRX(SPRXDirectory.Text, 0);

            if (Handle == 0)
                DarkMessageBox.ShowError("Handle returned 0.", "Module Failed to load.");
            else
                Thread.Sleep(100);

            UpdateModuleList();

            SetStatus("Ready");
        }

        #endregion

        #region ELF Section

        private void Button_OpenELF_Click(object sender, EventArgs e)
        {
            try
            {
                string fileContent = string.Empty;
                string filePath = string.Empty;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Open ELF File...";
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.CheckPathExists = true;
                    openFileDialog.InitialDirectory = Properties.Settings.Default.ELFDirectory;
                    openFileDialog.Filter = "ELF files (*.ELF)|*.ELF";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ELFDirectory.Text = openFileDialog.FileName;

                        Properties.Settings.Default.ELFDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                        Properties.Settings.Default.Save();
                    }
                }
            }
            catch
            {

            }
        }

        private void Button_LoadELF_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("loading ELF...");
                byte[] rawData = File.ReadAllBytes(ELFDirectory.Text);
                PS4.DefaultTarget.Process.LoadELF(rawData, rawData.Length);
                SetStatus("Ready");
            }
            catch
            {

            }
        }

        #endregion

        #region FTP Section

        public List<DarkTreeNode> GetParentNodeList(DarkTreeNode Parent)
        {
            List<DarkTreeNode> ParentNodes = new List<DarkTreeNode>();
            DarkTreeNode CurrentNode = Parent;
            do
            {
                ParentNodes.Add(Parent.ParentNode);
                CurrentNode = Parent.ParentNode;
            } while (CurrentNode != null);

            return ParentNodes;
        }

        public void AddFolderNode(string LastDir, DarkTreeNode Parent, string FolderName)
        {
            DarkTreeNode node = new DarkTreeNode(FolderName);
            node.ExpandedIcon = Properties.Resources.folder_16x;
            node.Icon = Properties.Resources.folder_Closed_16xLG;

            List<FtpFileInfo> ChildFileList = PS4.DefaultTarget.FTP.GetDir($"{LastDir + FolderName}/");
            if (ChildFileList.Count > 1)
            {
                node.NodeCountBypass = true;
                node.NodeExpanded += Node_NodeExpanded;
            }

            Parent.Nodes.Add(node);
        }

        public void AddFileNode(DarkTreeNode Parent, string Name)
        {
            DarkTreeNode node = new DarkTreeNode(Name);
            node.Icon = Properties.Resources.document_16xLG;
            node.ExpandedIcon = null;
            Parent.Nodes.Add(node);
        }

        public void SetupDirectoryNode(string LastDir, DarkTreeNode Parent)
        {
            Parent.Nodes.Clear();

            List<FtpFileInfo> FileList = PS4.DefaultTarget.FTP.GetDir(LastDir);
            foreach (FtpFileInfo Info in FileList)
            {
                if (Info.FileName.Contains(".."))
                    continue;

                if (Info.Directory)
                    AddFolderNode(LastDir, Parent, Info.FileName);
                else
                    AddFileNode(Parent, Info.FileName);
            }
        }

        public void FetchFTP()
        {
            
        }

        bool FetchingFTP = false;
        System.Threading.Thread hFTPThread;
        public void FTPThread()
        {
            try
            {
                ExecuteSecure(() => FTPDataTree.Nodes.Clear());

                List<FtpFileInfo> FileList = PS4.DefaultTarget.FTP.GetDir("/mnt/");

                foreach (FtpFileInfo Info in FileList)
                {
                    if (!Info.FileName.Contains("usb"))
                        continue;

                    if (Info.Directory)
                    {
                        DarkTreeNode node = new DarkTreeNode(Info.FileName);
                        node.ExpandedIcon = Properties.Resources.folder_16x;
                        node.Icon = Properties.Resources.folder_Closed_16xLG;

                        List<FtpFileInfo> ChildFileList = PS4.DefaultTarget.FTP.GetDir($"/mnt/{Info.FileName}/");
                        if (ChildFileList.Count > 1)
                        {
                            node.NodeCountBypass = true;
                            node.NodeExpanded += Node_NodeExpanded;
                        }

                        ExecuteSecure(() => FTPDataTree.Nodes.Add(node));
                    }
                    else
                    {
                        DarkTreeNode node = new DarkTreeNode(Info.FileName);
                        node.Icon = Properties.Resources.document_16xLG;
                        node.ExpandedIcon = null;
                        ExecuteSecure(() => FTPDataTree.Nodes.Add(node));
                    }
                }

                ExecuteSecure(() => SetStatus("Ready"));
                FetchingFTP = false;
            }
            catch
            {

            }
        }

        public void UpdateFTP()
        {
            try
            {
                if (!FetchingFTP)
                {
                    SetStatus("Fetching FTP List...");

                    hFTPThread = new Thread(() => FTPThread());
                    hFTPThread.Priority = ThreadPriority.Highest;
                    hFTPThread.IsBackground = true;
                    hFTPThread.Start();
                    FetchingFTP = true;
                }
            }
            catch
            {

            }
        }

        private void FTPStrip_LoadModule_Click(object sender, EventArgs e)
        {
            try
            {
                DarkTreeNode SelectedNode = FTPDataTree.SelectedNodes.FirstOrDefault();
                string FilePath = "/mnt/" + SelectedNode.FullPath;

                if (!Path.GetExtension(FilePath).Equals(".sprx"))
                    DarkMessageBox.ShowError("This file is not an sprx module and can not be loaded...", "File not sprx module!");
                else
                {
                    SetStatus("Loading Module " + Path.GetFileName(FilePath) + "...");

                    Int32 Handle = PS4.DefaultTarget.Process.LoadSPRX(FilePath, 0);

                    if (Handle == 0)
                        DarkMessageBox.ShowError("Handle returned 0.", "Module Failed to load.");
                    else
                        Thread.Sleep(100);

                    UpdateModuleList();

                    SetStatus("Ready");
                }
            }
            catch
            {

            }
        }

        private void FTPStrip_UnloadModule_Click(object sender, EventArgs e)
        {
            try
            {
                DarkTreeNode SelectedNode = FTPDataTree.SelectedNodes.FirstOrDefault();
                string FilePath = "/mnt/" + SelectedNode.FullPath;

                if (!Path.GetExtension(FilePath).Equals(".sprx"))
                    DarkMessageBox.ShowError("This file is not an sprx module and can not be Reloaded...", "File not sprx module!");
                else
                {
                    if (PS4.DefaultTarget.Process.ModuleList.Find(x => x.Name == Path.GetFileName(FilePath)) == null)
                        DarkMessageBox.ShowError("This module couldnt be unloaded as it is not loaded.", "Module not loaded.");
                    {
                        SetStatus("UnLoading Module " + Path.GetFileName(FilePath) + "...");

                        int Result = PS4.DefaultTarget.Process.UnloadSPRX(Path.GetFileName(FilePath), 0);

                        if (Result != 0)
                            DarkMessageBox.ShowError("Failed to unload the module.", "Module Failed to Unload.");
                        else
                            Thread.Sleep(100);

                        UpdateModuleList();

                        SetStatus("Ready");
                    }
                }
            }
            catch
            {

            }
        }

        private void FTPStrip_ReloadModule_Click(object sender, EventArgs e)
        {
            try
            {
                DarkTreeNode SelectedNode = FTPDataTree.SelectedNodes.FirstOrDefault();
                string FilePath = "/mnt/" + SelectedNode.FullPath;

                if (!Path.GetExtension(FilePath).Equals(".sprx"))
                    DarkMessageBox.ShowError("This file is not an sprx module and can not be Reloaded...", "File not sprx module!");
                else
                {
                    if(PS4.DefaultTarget.Process.ModuleList.Find(x => x.Name == Path.GetFileName(FilePath)) == null)
                        DarkMessageBox.ShowError("This module couldnt be reloaded as it is not loaded.", "Module not loaded.");
                    {
                        SetStatus("ReLoading Module " + Path.GetFileName(FilePath) + "...");

                        Int32 Handle = PS4.DefaultTarget.Process.ReloadSPRX(Path.GetFileName(FilePath), 0);

                        if (Handle == 0)
                            DarkMessageBox.ShowError("Handle returned 0.", "Module Failed to Reload.");
                        else
                            Thread.Sleep(100);

                        UpdateModuleList();

                        SetStatus("Ready");
                    }
                }
            }
            catch
            {

            }
        }

        private void FTPStrip_Refresh_Click(object sender, EventArgs e)
        {
            UpdateFTP();
        }

        private void Node_NodeExpanded(object sender, EventArgs e)
        {
            DarkTreeNode Node = (DarkTreeNode)sender;
            SetupDirectoryNode("/mnt/" + Node.FullPath + "/", Node);
        }

        #endregion
    }
}
