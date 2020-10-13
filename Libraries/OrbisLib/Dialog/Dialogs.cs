using DarkUI.Forms;
using OrbisSuite.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Dialog
{
    public class Dialogs
    {
        internal OrbisLib PS4;

        public Dialogs(OrbisLib PS4)
        {
            this.PS4 = PS4;
        }

        public System.Windows.Forms.DialogResult AddTarget()
        {
            AddTarget AddTarget = new AddTarget(PS4);
            System.Windows.Forms.DialogResult Result = AddTarget.ShowDialog();
            AddTarget.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult EditTarget(string TargetName)
        {
            EditTarget EditTarget = new EditTarget(PS4, TargetName);
            System.Windows.Forms.DialogResult Result = EditTarget.ShowDialog();
            EditTarget.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult TargetDetails(string TargetName)
        {
            TargetDetails TargetDetails = new TargetDetails(PS4, TargetName);
            System.Windows.Forms.DialogResult Result = TargetDetails.ShowDialog();
            TargetDetails.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult About()
        {
            About About = new About();
            System.Windows.Forms.DialogResult Result = About.ShowDialog();
            About.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult Settings()
        {
            Settings Settings = new Settings(PS4);
            System.Windows.Forms.DialogResult Result = Settings.ShowDialog();
            if (Result == System.Windows.Forms.DialogResult.OK)
                Settings.SaveSettings();
            Settings.Close();
            return Result;
        }

        public System.Windows.Forms.DialogResult SelectProcess(string TargetName)
        {
            SelectProcess SelectProcess = new SelectProcess(PS4, TargetName);
            System.Windows.Forms.DialogResult Result = SelectProcess.ShowDialog();

            if(Result == System.Windows.Forms.DialogResult.OK)
                SelectProcess.AttachtoSelected();

            SelectProcess.Close();
            return Result;
        }
    }
}
