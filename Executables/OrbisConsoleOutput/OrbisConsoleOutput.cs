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

namespace OrbisConsoleOutput
{
    public partial class OrbisConsoleOutput : DarkForm
    {
        public OrbisConsoleOutput()
        {
            InitializeComponent();
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
