using DarkUI.Docking;
using OrbisSuite;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

            UpdateScrollBar();
        }

        public void Clear()
        {
            Output.Clear();
        }

        public void Append(PrintType prefix, string Data)
        {
            switch(prefix)
            {
                case PrintType.None:
                    break;

                case PrintType.Info:
                    Output.AppendText("[INFO] ", Color.FromArgb(75, 110, 175));
                    break;

                case PrintType.Warn:
                    Output.AppendText("[WARN] ", Color.FromArgb(176, 75, 75));
                    break;

                case PrintType.Error:
                    Output.AppendText("[ERROR] ", Color.FromArgb(176, 75, 75));
                    break;
            }

            if(prefix == PrintType.Error)
                Output.AppendText(Data, Color.FromArgb(176, 75, 75));
            else
                Output.AppendText(Data);
        }

        public void Append(string Data)
        {
            Output.AppendText(Data);
        }

        #region Custom Scroll Bar

        public int GetNumberOfVisibleLines()
        {
            return Output.Height / TextRenderer.MeasureText("A", Output.Font, Output.Size, TextFormatFlags.WordBreak).Height;
        }

        private void UpdateScrollBar()
        {
            if (Output.Lines.Count() > GetNumberOfVisibleLines())
            {
                darkScrollBar1.Minimum = 0;
                darkScrollBar1.Maximum = Output.MaxVScroll;
                darkScrollBar1.ViewSize = Output.DisplayRectangle.Height;
                darkScrollBar1.Enabled = true;
            }
            else
            {
                darkScrollBar1.Minimum = 0;
                darkScrollBar1.Maximum = 100;
                darkScrollBar1.ViewSize = 99;
                darkScrollBar1.Enabled = false;
            }
        }

        private void darkScrollBar1_ValueChanged(object sender, DarkUI.Controls.ScrollValueEventArgs e)
        {
            if (Output.Lines.Count() > GetNumberOfVisibleLines())
                Output.VScrollPos = darkScrollBar1.Value;
        }

        private void Output_Scroll(object sender, DarkUI.Controls.MyScrollEvent e)
        {
            if (Output.Lines.Count() > GetNumberOfVisibleLines())
                darkScrollBar1.Value = Output.VScrollPos;
        }

        private void Output_TextChanged(object sender, System.EventArgs e)
        {
            UpdateScrollBar();
        }

        private void Output_Resize(object sender, System.EventArgs e)
        {
            UpdateScrollBar();
        }

        #endregion

    }
}
