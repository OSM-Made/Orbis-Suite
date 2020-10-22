using DarkUI.Forms;
using OrbisSuite;
using OrbisConsoleOutput.Controls;

namespace OrbisConsoleOutput
{
    public partial class OrbisConsoleOutput : DarkForm
    {
        OrbisLib PS4 = new OrbisLib();

        public OrbisConsoleOutput()
        {
            InitializeComponent();

            MainDockPanel.AddContent(new OutputControl(PS4, "OrbisLib"));
            MainDockPanel.AddContent(new OutputControl(PS4, "Test2"));
            MainDockPanel.AddContent(new OutputControl(PS4, "Frost_Engine.sprx"));
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
