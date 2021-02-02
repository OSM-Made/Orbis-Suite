using DarkUI.Docking;
using DarkUI.Forms;
using OrbisDebugger.Controls;
using OrbisSuite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OrbisDebugger
{
    public partial class OrbisDebugger : DarkForm
    {
        OrbisLib PS4 = new OrbisLib();

        public object SerializerHelper { get; private set; }

        public OrbisDebugger()
        {
            InitializeComponent();
        }

        private void OrbisDebugger_Load(object sender, EventArgs e)
        {
            //Update Targets.
            UpdateTarget();

            //Events
            PS4.SelectedTarget.Events.ProcAttach += Events_ProcAttach;
            PS4.SelectedTarget.Events.ProcDetach += Events_ProcDetach;
            PS4.SelectedTarget.Events.ProcDie += Events_ProcDie;
            PS4.Events.DBTouched += Events_DBTouched;
            PS4.Events.TargetAvailable += Events_TargetAvailable;
            PS4.Events.TargetUnAvailable += Events_TargetUnAvailable;
        }

        #region Target Availability

        private void EnableProgram(bool state, bool IsAttached = false)
        {
            if (state) //Enable controls when there is a target.
            {   //Controls Not dependent on Attached state.
                //Process Selection
                processAttachToolStripMenuItem.Enabled = true;
                Button_ProcessAttach.Enabled = true;

                //Target Controls
                restartTargetToolStripMenuItem.Enabled = true;
                Button_RebootTarget.Enabled = true;
                shutdownToolStripMenuItem.Enabled = true;
                suspendToolStripMenuItem.Enabled = true;

                if (IsAttached)
                {
                    //Process Selection
                    processDetachToolStripMenuItem.Enabled = true;
                    Button_ProcessDetach.Enabled = true;
                    CurrentProc.Text = string.Format("Process: {0}", PS4.SelectedTarget.Info.CurrentProc);

                    //Process Control
                    Button_StopDebugging.Enabled = true;
                    Button_StopDebugging.Enabled = true;
                    Button_PauseProcess.Enabled = true;
                    Button_ResumeProcess.Enabled = true;
                    Button_KillProcess.Enabled = true;
                    Process_StepInto.Enabled = true;
                    Process_StepOver.Enabled = true;
                    Process_StepOut.Enabled = true;
                    processKillToolStripMenuItem.Enabled = true;
                    stopToolStripMenuItem.Enabled = true;
                    pauseToolStripMenuItem.Enabled = true;
                    resumeToolStripMenuItem.Enabled = true;
                    stepOutToolStripMenuItem.Enabled = true;
                    stepIntoToolStripMenuItem.Enabled = true;
                    stepOverToolStripMenuItem.Enabled = true;

                    //Windows
                    Button_ConsoleWindow.Enabled = true;
                    Button_RegisterWindow.Enabled = true;
                    Button_ProcessCallStackWindow.Enabled = true;
                    Button_ProcessWatchpointsWindow.Enabled = true;
                    Button_ProcessBreakpointWindow.Enabled = true;
                    Button_ProcessInfoWindow.Enabled = true;
                    Button_MemoryWindow.Enabled = true;
                    Button_DisassemblyWindow.Enabled = true;
                    Button_ScratchWindow.Enabled = true;

                    consoletoolStripMenuItem.Enabled = true;
                    registerstoolStripMenuItem.Enabled = true;
                    callstacktoolStripMenuItem.Enabled = true;
                    watchpointstoolStripMenuItem.Enabled = true;
                    breakpointstoolStripMenuItem.Enabled = true;
                    threadstoolStripMenuItem.Enabled = true;
                    memorytoolStripMenuItem.Enabled = true;
                    disassemblytoolStripMenuItem.Enabled = true;
                    scratchtoolStripMenuItem.Enabled = true;
                    savetoolStripMenuItem.Enabled = true;
                    loadtoolStripMenuItem.Enabled = true;
                    cleartoolStripMenuItem.Enabled = true;
                    resettoolStripMenuItem.Enabled = true;
                }
                else
                {
                    //Process Selection
                    processDetachToolStripMenuItem.Enabled = false;
                    Button_ProcessDetach.Enabled = false;
                    CurrentProc.Text = "Process: -";

                    //Process Control
                    Button_StopDebugging.Enabled = false;
                    Button_StopDebugging.Enabled = false;
                    Button_PauseProcess.Enabled = false;
                    Button_ResumeProcess.Enabled = false;
                    Button_KillProcess.Enabled = false;
                    Process_StepInto.Enabled = false;
                    Process_StepOver.Enabled = false;
                    Process_StepOut.Enabled = false;
                    processKillToolStripMenuItem.Enabled = false;
                    stopToolStripMenuItem.Enabled = false;
                    pauseToolStripMenuItem.Enabled = false;
                    resumeToolStripMenuItem.Enabled = false;
                    stepOutToolStripMenuItem.Enabled = false;
                    stepIntoToolStripMenuItem.Enabled = false;
                    stepOverToolStripMenuItem.Enabled = false;                    

                    //Windows
                    Button_ConsoleWindow.Enabled = false;
                    Button_RegisterWindow.Enabled = false;
                    Button_ProcessCallStackWindow.Enabled = false;
                    Button_ProcessWatchpointsWindow.Enabled = false;
                    Button_ProcessBreakpointWindow.Enabled = false;
                    Button_ProcessInfoWindow.Enabled = false;
                    Button_MemoryWindow.Enabled = false;
                    Button_DisassemblyWindow.Enabled = false;
                    Button_ScratchWindow.Enabled = false;

                    consoletoolStripMenuItem.Enabled = false;
                    registerstoolStripMenuItem.Enabled = false;
                    callstacktoolStripMenuItem.Enabled = false;
                    watchpointstoolStripMenuItem.Enabled = false;
                    breakpointstoolStripMenuItem.Enabled = false;
                    threadstoolStripMenuItem.Enabled = false;
                    memorytoolStripMenuItem.Enabled = false;
                    disassemblytoolStripMenuItem.Enabled = false;
                    scratchtoolStripMenuItem.Enabled = false;
                    savetoolStripMenuItem.Enabled = false;
                    loadtoolStripMenuItem.Enabled = false;
                    cleartoolStripMenuItem.Enabled = false;
                    resettoolStripMenuItem.Enabled = false;
                }
            }
            else //Disable all controls when there is no target.
            {
                //Process Selection
                processAttachToolStripMenuItem.Enabled = false;
                Button_ProcessAttach.Enabled = false;
                processDetachToolStripMenuItem.Enabled = false;
                Button_ProcessDetach.Enabled = false;
                CurrentProc.Text = "Process: -";

                //Process Control
                Button_StopDebugging.Enabled = false;
                Button_StopDebugging.Enabled = false;
                Button_PauseProcess.Enabled = false;
                Button_ResumeProcess.Enabled = false;
                Button_KillProcess.Enabled = false;
                Process_StepInto.Enabled = false;
                Process_StepOver.Enabled = false;
                Process_StepOut.Enabled = false;
                processKillToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = false;
                pauseToolStripMenuItem.Enabled = false;
                resumeToolStripMenuItem.Enabled = false;
                stepOutToolStripMenuItem.Enabled = false;
                stepIntoToolStripMenuItem.Enabled = false;
                stepOverToolStripMenuItem.Enabled = false;

                //Target Controls
                restartTargetToolStripMenuItem.Enabled = false;
                Button_RebootTarget.Enabled = false;
                shutdownToolStripMenuItem.Enabled = false;
                suspendToolStripMenuItem.Enabled = false;

                //Windows
                Button_ConsoleWindow.Enabled = false;
                Button_RegisterWindow.Enabled = false;
                Button_ProcessCallStackWindow.Enabled = false;
                Button_ProcessWatchpointsWindow.Enabled = false;
                Button_ProcessBreakpointWindow.Enabled = false;
                Button_ProcessInfoWindow.Enabled = false;
                Button_MemoryWindow.Enabled = false;
                Button_DisassemblyWindow.Enabled = false;
                Button_ScratchWindow.Enabled = false;

                consoletoolStripMenuItem.Enabled = false;
                registerstoolStripMenuItem.Enabled = false;
                callstacktoolStripMenuItem.Enabled = false;
                watchpointstoolStripMenuItem.Enabled = false;
                breakpointstoolStripMenuItem.Enabled = false;
                threadstoolStripMenuItem.Enabled = false;
                memorytoolStripMenuItem.Enabled = false;
                disassemblytoolStripMenuItem.Enabled = false;
                scratchtoolStripMenuItem.Enabled = false;
                savetoolStripMenuItem.Enabled = false;
                loadtoolStripMenuItem.Enabled = false;
                cleartoolStripMenuItem.Enabled = false;
                resettoolStripMenuItem.Enabled = false;
            }
        }

        public void UpdateTarget()
        {
            Invoke((MethodInvoker)delegate
            {
                try
                {
                    //Clear the list when the count changes.
                    if (CurrentTarget.DropDownItems.Count != PS4.TargetManagement.GetTargetCount())
                        CurrentTarget.DropDownItems.Clear();

                    int LoopCount = 0;
                    foreach (TargetInfo Target in PS4.TargetManagement.TargetList)
                    {
                        //Set up the selection list
                        if (CurrentTarget.DropDownItems.Count <= LoopCount)
                            CurrentTarget.DropDownItems.Add(Target.Name, null, CurrentTarget_Click);
                        else
                            CurrentTarget.DropDownItems[LoopCount].Text = Target.Name;

                        LoopCount++;
                    }

                    if (PS4.SelectedTarget.Active)
                    {
                        if (PS4.SelectedTarget.Info.Available)
                            EnableProgram(true, PS4.SelectedTarget.Info.Attached);
                        else
                            EnableProgram(false);

                        CurrentTarget.Text = string.Format("Target: {0}", PS4.SelectedTarget.Info.Name);
                    }
                    else
                    {
                        CurrentTarget.Text = "Target: -";
                        CurrentProc.Text = "Process: -";
                        EnableProgram(false);
                    }
                }
                catch
                {

                }
            });
        }

        private void CurrentTarget_Click(object sender, EventArgs e)
        {
            try
            {
                string SelectedTarget = ((ToolStripMenuItem)sender).Text;
                CurrentTarget.Text = "Target: " + SelectedTarget;
                PS4.TargetManagement.SetSelected(SelectedTarget);
                UpdateTarget();
            }
            catch
            {

            }
        }

        #endregion

        #region Events

        private void Events_ProcDie(object sender, ProcDieEvent e)
        {
            UpdateTarget();
        }

        private void Events_ProcDetach(object sender, ProcDetachEvent e)
        {
            UpdateTarget();
        }

        private void Events_ProcAttach(object sender, ProcAttachEvent e)
        {
            UpdateTarget();
        }

        private void Events_TargetUnAvailable(object sender, TargetUnAvailableEvent e)
        {
            UpdateTarget();
        }

        private void Events_TargetAvailable(object sender, TargetAvailableEvent e)
        {
            UpdateTarget();
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
        {
            UpdateTarget();
        }

        #endregion

        #region File Dropdown

        private void processAttachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Process.SelectProcess();
        }

        private void processDetachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Process.Detach();
        }

        private void processKillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Process.Kill();
        }

        private void restartTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Reboot();
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Shutdown();
        }

        private void suspendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Suspend();
        }

        private void selectTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.Dialogs.SelectTarget();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.Dialogs.Settings();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.Dialogs.About();
        }

        #endregion

        #region Debug

        #region Windows

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void stepOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void stepIntoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void stepOverToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Tool Strip

        private void Button_ProcessAttach_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Process.SelectProcess();
        }

        private void Button_ProcessDetach_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Process.Detach();
        }

        private void Button_StopDebugging_Click(object sender, EventArgs e)
        {

        }

        //
        // Execution control
        //

        private void Button_PauseProcess_Click(object sender, EventArgs e)
        {
            //PS4.SelectedTarget.Process.
        }

        private void Button_ResumeProcess_Click(object sender, EventArgs e)
        {
            //PS4.SelectedTarget.Process.
        }

        private void Process_StepInto_Click(object sender, EventArgs e)
        {

        }

        private void Process_StepOver_Click(object sender, EventArgs e)
        {

        }

        private void Process_StepOut_Click(object sender, EventArgs e)
        {

        }

        //
        // Windows
        //

        private void Button_ConsoleWindow_Click(object sender, EventArgs e)
        {

        }

        private void Button_RegisterWindow_Click(object sender, EventArgs e)
        {

        }

        private void Button_ProcessCallStackWindow_Click(object sender, EventArgs e)
        {

        }

        private void Button_ProcessWatchpointsWindow_Click(object sender, EventArgs e)
        {

        }

        private void Button_ProcessBreakpointWindow_Click(object sender, EventArgs e)
        {

        }

        private void Button_ProcessInfoWindow_Click(object sender, EventArgs e)
        {

        }

        private void Button_MemoryWindow_Click(object sender, EventArgs e)
        {

        }

        private void Button_DisassemblyWindow_Click(object sender, EventArgs e)
        {

        }

        private void Button_ScratchWindow_Click(object sender, EventArgs e)
        {

        }

        //
        // Target Controls
        //

        private void Button_KillProcess_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Process.Kill();
        }

        private void Button_RebootTarget_Click(object sender, EventArgs e)
        {
            PS4.SelectedTarget.Reboot();
        }

        #endregion

        #region Windows
        
        //Left

        //Documents
        public ScratchPad ScratchPadWindow = new ScratchPad();


        //Bottom
        


        private void RegisterWindows()
        {

        }

        #endregion 

        #region MainDock

        private void SetupDockSize()
        {
            DockPanelState State = MainDock.GetDockPanelState();
            State.Regions.Add(new DockRegionState(DarkDockArea.Document));
            State.Regions.Add(new DockRegionState(DarkDockArea.Left, new Size(492, 811)));
            State.Regions.Add(new DockRegionState(DarkDockArea.Right, new Size(200, 100)));
            State.Regions.Add(new DockRegionState(DarkDockArea.Bottom, new Size(1040, 270)));
        }

        private void SetWindowButtons()
        {
            //Button_ConsoleWindow.Checked = MainDock.ContainsContent(_ConsoleWindow);
            //Button_RegisterWindow.Checked = MainDock.ContainsContent(_RegisterWindow);
            //Button_ProcessWatchpointsWindow.Checked = MainDock.ContainsContent(_WatchpointWindow);
            //Button_ProcessBreakpointWindow.Checked = MainDock.ContainsContent(_BreakpointWindow);
            //Button_ProcessInfoWindow.Checked = MainDock.ContainsContent(_ProcessInfoWindow);
            Button_ScratchWindow.Checked = MainDock.ContainsContent(ScratchPadWindow);
            //Button_ProcessCallStackWindow.Checked = MainDock.ContainsContent(_CallStackWindow);
        }

        void CloseAllWindows()
        {
            SetWindowButtons();

            foreach (DarkDockContent Content in MainDock.GetContent(DarkDockArea.Document))
                MainDock.RemoveContent(Content);

            foreach (DarkDockContent Content in MainDock.GetContent(DarkDockArea.Left))
                MainDock.RemoveContent(Content);

            foreach (DarkDockContent Content in MainDock.GetContent(DarkDockArea.Bottom))
                MainDock.RemoveContent(Content);

            //foreach (var toolWindow in _toolWindows)
            //    MainDock.RemoveContent(toolWindow);
        }

        bool AnyWindowsOpen()
        {
            return ((MainDock.GetContent(DarkDockArea.Document).Count > 0) || (MainDock.GetContent(DarkDockArea.Left).Count > 0) || (MainDock.GetContent(DarkDockArea.Bottom).Count > 0));
        }

        //Adding Windows

        private void AddLeftcontent(DarkToolWindow toolWindow)
        {
            DarkDockGroup Group = null;
            List<DarkDockContent> LeftContent = MainDock.GetContent(DarkDockArea.Left);

            foreach (DarkDockContent Content in LeftContent)
            {
                if ((Content != null))
                {
                    Group = Content.DockGroup;
                    break;
                }
            }

            if (Group != null)
                MainDock.AddContent(toolWindow, Group);
            else
                MainDock.AddContent(toolWindow);
        }

        private void AddBottomcontent(DarkToolWindow toolWindow)
        {
            DarkDockGroup Group = null;
            List<DarkDockContent> BottomContent = MainDock.GetContent(DarkDockArea.Bottom);

            foreach (DarkDockContent Content in BottomContent)
            {
                if ((Content != null))
                {
                    Group = Content.DockGroup;
                    break;
                }
            }

            if (Group != null)
                MainDock.AddContent(toolWindow, Group);
            else
                MainDock.AddContent(toolWindow);
        }

        #endregion

    }
}
