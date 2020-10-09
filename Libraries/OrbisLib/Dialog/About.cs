using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisSuite.Dialog
{
    public partial class About : DarkDialog
    {
        public About()
        {
            InitializeComponent();

            blVersion.Text = $"Version: { Assembly.GetExecutingAssembly().GetName().Version.Major }.{ Assembly.GetExecutingAssembly().GetName().Version.Minor}.{ Assembly.GetExecutingAssembly().GetName().Version.Build }";
        }
    }
}
