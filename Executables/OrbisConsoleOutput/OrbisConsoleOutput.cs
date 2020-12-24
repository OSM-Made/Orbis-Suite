using DarkUI.Forms;
using OrbisSuite;
using OrbisConsoleOutput.Controls;
using static OrbisConsoleOutput.Controls.OutputControl;
using DarkUI.Docking;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

namespace OrbisConsoleOutput
{
    public partial class OrbisConsoleOutput : DarkForm
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

        public OrbisConsoleOutput()
        {
            InitializeComponent();

            //Events
            PS4.Events.TargetAvailable += Events_TargetAvailable;
            PS4.Events.TargetUnAvailable += Events_TargetUnAvailable;
            PS4.Events.DBTouched += Events_DBTouched;

            PS4.SelectedTarget.Events.ProcAttach += Events_ProcAttach;
            PS4.SelectedTarget.Events.ProcDetach += Events_ProcDetach;

            PS4.SelectedTarget.Events.ProcPrint += Events_ProcPrint;
        }

        #region Events

        private void Events_ProcDetach(object sender, ProcDetachEvent e)
        {
            ExecuteSecure(() => CurrentProc.Text = "Process: -");
        }

        private void Events_ProcAttach(object sender, ProcAttachEvent e)
        {
            ExecuteSecure(() => CurrentProc.Text = "Process: " + e.NewProcName);
        }

        private void Events_DBTouched(object sender, DBTouchedEvent e)
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

        private static Mutex mut = new Mutex();
        private void Events_ProcPrint(object sender, ProcPrintEvent e)
        {
            // Wait until it is safe to enter.
            mut.WaitOne();

            OutputControl Tab = (OutputControl)MainDockPanel.GetDocuments().Find(x => x.DockText == e.Sender);
            if (Tab != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    Tab.Append(e.Type, e.Data);
                });
            }
            else
            {
                Invoke((MethodInvoker)delegate
                {
                    OutputControl NewDocument = new OutputControl(PS4, e.Sender);
                    MainDockPanel.AddContent(NewDocument);
                    NewDocument.Append(e.Type, e.Data);
                });
            }

            // Release the Mutex.
            mut.ReleaseMutex();
        }

        #endregion

        #region Target List

        private void EnableProgram(bool state, bool IsAttached = false)
        {
            if (state)
            {
                Button_Attach.Enabled = true;

                if (IsAttached)
                {
                    //Enable Controls
                    Button_Detach.Enabled = true;
                    CurrentProc.Text = string.Format("Process: {0}", PS4.SelectedTarget.Info.CurrentProc);

                    Button_Detach.Enabled = true;
                }
                else
                {
                    CurrentProc.Text = "Process: -";
                    Button_Detach.Enabled = false;
                }
            }
            else
            {
                CurrentProc.Text = "Process: -";

                //Disable Controls
                Button_Attach.Enabled = false;
                Button_Detach.Enabled = false;
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
                        Button_Detach.Enabled = false;
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

        #region Tool Strip

        private void selectTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS4.Dialogs.SelectTarget();
            UpdateTarget();
        }

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
            if (PS4.SelectedTarget.Info.Available)
                PS4.SelectedTarget.Process.SelectProcess();
        }

        private void Button_Detach_Click(object sender, EventArgs e)
        {
            if (PS4.SelectedTarget.Info.Available && PS4.SelectedTarget.Info.Attached)
                PS4.SelectedTarget.Process.Detach();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //MainDockPanel.rem
        }

        #endregion

        /*
        
            Should make the output dynamic. So make one output window class where we just instance it and have the output name dictated by the event.
            This could make it so in som respect we can have a sort of channel setup. Where whem we add the custom system calls we can break down the prints by process.

            string ConsoleIP / Name?
            string SenderName;
            string Data;


        */
    }
}
