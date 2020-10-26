using DarkUI.Forms;
using OrbisSuite;
using OrbisConsoleOutput.Controls;
using static OrbisConsoleOutput.Controls.OutputControl;
using DarkUI.Docking;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

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

        private void Events_ProcPrint(object sender, ProcPrintEvent e)
        {
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
