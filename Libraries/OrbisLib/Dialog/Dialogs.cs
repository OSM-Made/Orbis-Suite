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

        public Dialogs(OrbisLib InPS4)
        {
            PS4 = InPS4;
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
    }
}
