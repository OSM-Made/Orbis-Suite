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

        public OrbisConsoleOutput()
        {
            InitializeComponent();

            //Events
            PS4.SelectedTarget.Events.ProcPrint += Events_ProcPrint;
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

        /*
        
            Should make the output dynamic. So make one output window class where we just instance it and have the output name dictated by the event.
            This could make it so in som respect we can have a sort of channel setup. Where whem we add the custom system calls we can break down the prints by process.

            string ConsoleIP / Name?
            string SenderName;
            string Data;


        */
    }
}
