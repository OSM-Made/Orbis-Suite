using OrbisLib2.Common.Database.Types;
using SQLite;

namespace OrbisLib2.Common.Database
{
    public class MutableInfo
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int TargetId { get; set; }

        public TargetStatusType Status { get; set; }

        public string SdkVersion { get; set; }

        public string SoftwareVersion { get; set; }

        public int BigAppPid { get; set; }

        public string BigAppProcessName { get; set; }

        public string BigAppTitleId { get; set; }

        public string ConsoleName { get; set; }

        public bool Uart { get; set; }

        public bool IduMode { get; set; }

        public int ForegroundAccountId { get; set; }

        public long HddUsedSpace { get; set; }

        public long HddFreeSpace { get; set; }

        public long HddTotalSpace { get; set; }

        public int CpuTemp { get; set; }

        public int SocTemp { get; set; }

        public int ThreadCount { get; set; }

        public float AverageCPUUsage { get; set; }

        public int BusyCore { get; set; }

        public int RamUsage { get; set; }

        public int VideoRamUsage { get; set; }

        public bool Save()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<MutableInfo>();

            var result = db.Update(this);
            db.Close();
            return (result <= 0);
        }

        public void UpdateStatus(TargetStatusType status)
        {
            Status = status;
            Save();
        }
    }
}
