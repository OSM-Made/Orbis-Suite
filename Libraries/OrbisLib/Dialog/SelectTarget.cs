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

namespace OrbisSuite.Dialog
{
    public partial class SelectTarget : DarkDialog
    {
        OrbisLib PS4;

        public SelectTarget(OrbisLib PS4)
        {
            InitializeComponent();

            this.PS4 = PS4;
        }
    }
}
