using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisSuite.Dialog
{
    public class Dialogs
    {
        internal OrbisLib PS4;

        public Dialogs(OrbisLib PS4)
        {
            this.PS4 = PS4;
        }

        public System.Windows.Forms.DialogResult AddTarget(FormStartPosition startPosition = FormStartPosition.CenterParent)
        {
            AddTarget AddTarget = new AddTarget(PS4);
            AddTarget.StartPosition = startPosition;
            System.Windows.Forms.DialogResult Result = AddTarget.ShowDialog();
            AddTarget.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult EditTarget(string TargetName, FormStartPosition startPosition = FormStartPosition.CenterParent)
        {
            EditTarget EditTarget = new EditTarget(PS4, TargetName);
            EditTarget.StartPosition = startPosition;
            System.Windows.Forms.DialogResult Result = EditTarget.ShowDialog();
            EditTarget.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult TargetDetails(string TargetName, FormStartPosition startPosition = FormStartPosition.CenterParent)
        {
            TargetDetails TargetDetails = new TargetDetails(PS4, TargetName);
            TargetDetails.StartPosition = startPosition;
            System.Windows.Forms.DialogResult Result = TargetDetails.ShowDialog();
            TargetDetails.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult About(FormStartPosition startPosition = FormStartPosition.CenterParent)
        {
            About About = new About();
            About.StartPosition = startPosition;
            System.Windows.Forms.DialogResult Result = About.ShowDialog();
            About.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult Settings(FormStartPosition startPosition = FormStartPosition.CenterParent)
        {
            Settings Settings = new Settings(PS4);
            Settings.StartPosition = startPosition;
            DialogResult Result = Settings.ShowDialog();
            if (Result == DialogResult.OK)
                Settings.SaveSettings();
            Settings.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult SelectProcess(string TargetName, FormStartPosition startPosition = FormStartPosition.CenterParent)
        {
            SelectProcess SelectProcess = new SelectProcess(PS4, TargetName);
            SelectProcess.StartPosition = startPosition;
            DialogResult Result = SelectProcess.ShowDialog();

            if(Result == DialogResult.OK)
                SelectProcess.AttachtoSelected();

            SelectProcess.Close();
            return Result;
        }

        public DialogResult SelectTarget(FormStartPosition startPosition = FormStartPosition.CenterParent)
        {
            SelectTarget SelectTarget = new SelectTarget(PS4);
            SelectTarget.StartPosition = startPosition;
            DialogResult Result = SelectTarget.ShowDialog();

            if (Result == DialogResult.OK)
                SelectTarget.SelectCurrentTarget();

            SelectTarget.Close();
            return Result;
        }
    }
}
