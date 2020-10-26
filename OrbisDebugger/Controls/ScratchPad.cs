using System.Drawing;
using System.Windows.Forms;
using DarkUI.Docking;

namespace OrbisDebugger.Controls
{
    public partial class ScratchPad : DarkDocument
    {
        public ScratchPad()
        {
            InitializeComponent();
        }

        private int GetNumberOfVisibleLines()
        {
            int topIndex = ScratchPadTextBox.GetCharIndexFromPosition(new Point(1, 1));
            int bottomIndex = ScratchPadTextBox.GetCharIndexFromPosition(new Point(1, ScratchPadTextBox.Height - 1));

            int topLine = ScratchPadTextBox.GetLineFromCharIndex(topIndex);
            int bottomLine = ScratchPadTextBox.GetLineFromCharIndex(bottomIndex);

            return (bottomLine - topLine);
        }
    }
}
