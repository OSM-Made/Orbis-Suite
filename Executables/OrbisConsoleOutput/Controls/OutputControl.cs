using DarkUI.Docking;
using OrbisSuite;

namespace OrbisConsoleOutput.Controls
{
    public partial class OutputControl : DarkDocument
    {
        OrbisLib PS4;

        public OutputControl(OrbisLib PS4, string SenderName)
        {
            InitializeComponent();

            this.PS4 = PS4;
            this.DockText = SenderName;
        }
    }
}
