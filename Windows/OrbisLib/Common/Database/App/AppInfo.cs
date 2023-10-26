using SQLite;
using System.Linq.Expressions;

namespace OrbisLib2.Common.Database.App
{
    [Table("tbl_appinfo")]
    public class AppInfo
    {
        [PrimaryKey, NotNull]
        public string titleId { get; set; } = "";

        [PrimaryKey, NotNull]
        public string key { get; set; } = "";

        public string val { get; set; }

        public static string GetStringFromAppInfo(string DataBasePath, string TitleId, string Key)
        {
            var db = new SQLiteConnection(DataBasePath);

            var result = db.Find((Expression<Func<AppInfo, bool>>)(x => x.titleId.Equals(TitleId) && x.key.Equals(Key)));
            db.Close();
            if (result != null)
            {
                return result.val;
            }
            else
            {
                return "";
            }
        }
    }
}
