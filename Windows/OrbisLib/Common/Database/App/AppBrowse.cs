using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisLib2.Common.Database.App
{
    public class AppBrowse
    {
        [PrimaryKey, NotNull]
        [Column("titleId")]
        public string TitleId { get; set; } = "";

        [Column("contentId")]
        public string ContentId { get; set; } = "";

        [Column("titleName")]
        public string TitleName { get; set; } = "";

        [Column("metaDataPath")]
        public string MetaDataPath { get; set; } = "";

        [Column("visible")]
        public int Visible { get; set; } = 0;

        [Column("sortPriority")]
        public int SortPriority { get; set; } = 0;

        [Column("dispLocation")]
        public int DispLocation { get; set; } = 0;

        [Column("canRemove")]
        public int CanRemove { get; set; } = 0;

        [Column("category")]
        public string Category { get; set; } = "";

        [Column("contentSize")]
        public long ContentSize { get; set; } = 0;

        [Column("installDate")]
        public string InstallDate { get; set; } = "";

        [Column("uiCategory")]
        public string UICategory { get; set; } = "";

        public static List<AppBrowse> GetAppBrowseList(string dbPath, int UserId)
        {
            var db = new SQLiteConnection(dbPath);

            // The name of the table with the user id appended to the end.
            var tableName = $"tbl_appbrowse_{UserId.ToString().PadLeft(10, '0')}";

            // Query the table with the userId that we want.
            var result = db.Query<AppBrowse>($"SELECT * FROM {tableName}");

            // Close the handle.
            db.Close();

            // Return the Results.
            return result;
        }
    }
}
