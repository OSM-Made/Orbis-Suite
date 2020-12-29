using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisSuite.Dialog
{
    public partial class GameDetails : DarkDialog
    {
        public string TitleID { get; set; }
        public OrbisLib PS4;
        public TMDB TitleMetaData;

        public GameDetails(OrbisLib PS4, string TitleID)
        {
            InitializeComponent();

            this.PS4 = PS4;
            this.TitleID = TitleID;
            TitleMetaData = new TMDB(TitleID);
        }

        private void GameDetails_Load(object sender, EventArgs e)
        {
            GameIcon.Load(TitleMetaData.Icons[0]);

            Regex rgx = new Regex(@"[^0-9a-zA-Z +.:']");
            TitleName.Text = rgx.Replace(TitleMetaData.Names[0], "");
            NPTitleID.Text = TitleMetaData.NPTitleID;

            Category.Text = TitleMetaData.Category;
            PSVR.Text = TitleMetaData.PSVR > 0 ? "true" : "false";
            ProEnhanced.Text = TitleMetaData.NEOEnable > 0 ? "true" : "false";
            PatchRevision.Text = TitleMetaData.PatchRevision.ToString();

            ContentID.Text = TitleMetaData.ContentID;
        }
    }
}
