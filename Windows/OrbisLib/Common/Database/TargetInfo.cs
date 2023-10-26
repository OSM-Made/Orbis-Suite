using OrbisLib2.Common.Database.Types;
using SQLite;
using System.Text.RegularExpressions;

namespace OrbisLib2.Common.Database
{
    [Table("TargetInfo")]
    public class TargetInfo
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int TargetId { get; set; }

        /// <summary>
        /// Will be true if a tcp connection can be achieved on the saved IP Address.
        /// </summary>
        [NotNull]
        [Column("Available")]
        public bool IsAvailable { get; set; } = false;

        /// <summary>
        /// Will be true if the Orbis Suite API is running on the target.
        /// </summary>
        [NotNull]
        [Column("APIAvailable")]
        public bool IsAPIAvailable { get; set; } = false;

        /// <summary>
        /// The power/API status of the target.
        /// </summary>
        public TargetStatusType Status
        {
            get
            {
                if (IsAPIAvailable)
                    return TargetStatusType.APIAvailable;
                else if (IsAvailable)
                    return TargetStatusType.Online;
                else
                    return TargetStatusType.Offline;
            }
        }

        /// <summary>
        /// The SDK Version of the software installed on the target.
        /// </summary>
        [NotNull]
        public string SDKVersion { get; set; } = "-";

        /// <summary>
        /// The full software version installed on the target.
        /// </summary>
        [NotNull]
        public string SoftwareVersion { get; set; } = "-";

        /// <summary>
        /// The software version first installed on the target when sold.
        /// </summary>
        [NotNull]
        public string FactorySoftwareVersion { get; set; } = "-";

        /// <summary>
        /// The process index of the current big app.
        /// </summary>
        [NotNull]
        public int BigAppPid { get; set; } = -1;

        /// <summary>
        /// The process name of the current big app.
        /// </summary>
        [NotNull]
        public string BigAppProcessName { get; set; } = "-";

        /// <summary>
        /// The current big game titleId running on the target.
        /// </summary>
        [NotNull]
        public string BigAppTitleID { get; set; } = "-";

        /// <summary>
        /// The name of the target as set on the target.
        /// </summary>
        [NotNull]
        public string ConsoleName { get; set; } = "-";

        /// <summary>
        /// The serial number of the targets motherboard.
        /// </summary>
        [NotNull]
        public string MotherboardSerial { get; set; } = "-";

        /// <summary>
        /// The seraial number of the target.
        /// </summary>
        [NotNull]
        public string Serial { get; set; } = "-";

        /// <summary>
        /// The model number of the target.
        /// </summary>
        [NotNull]
        public string Model { get; set; } = "-";

        public ConsoleModelType ModelType
        {
            get
            {
                // ConsoleModel
                // CUH-1XXXX Fat
                // CUH-2XXXX Slim
                // CUH-7XXXX Pro

                if (Model == null || !Regex.Match(Model, @"CUH-\d{1}\w{4}").Success)
                    return ConsoleModelType.Fat;

                switch (char.IsDigit(Model[4]) ? int.Parse(Model[4].ToString()) : 0)
                {
                    case 1:
                        return ConsoleModelType.Fat;

                    case 2:
                        return ConsoleModelType.Slim;

                    case 7:
                        return ConsoleModelType.Pro;


                    default:
                        return ConsoleModelType.Fat;
                }
            }
        }

        /// <summary>
        /// The MAC address of the target LAN adapter.
        /// </summary>
        [NotNull]
        public string MACAddressLAN { get; set; } = "-";

        /// <summary>
        /// The MAC address of the target WIFI adapter.
        /// </summary>
        [NotNull]
        public string MACAddressWIFI { get; set; } = "-";

        /// <summary>
        /// Will be true if the UART flag is set in the targets flash.
        /// </summary>
        [NotNull]
        public bool UART { get; set; } = false;

        /// <summary>
        /// Will return true if the IDUMode flag is set int he targets flash.
        /// </summary>
        [NotNull]
        public bool IDUMode { get; set; } = false;

        /// <summary>
        /// A unique string used to identify the target.
        /// </summary>
        [NotNull]
        public string IDPS { get; set; } = "-";

        /// <summary>
        /// A unique string used to identify the target.
        /// </summary>
        [NotNull]
        public string PSID { get; set; } = "-";

        /// <summary>
        /// The console type like Retail/TestKit/Devkit.
        /// </summary>
        [NotNull]
        public ConsoleType ConsoleType { get; set; } = 0;

        /// <summary>
        /// Will be true if the Orbis Suite Debugger is attached to a process.
        /// </summary>
        [NotNull]
        [Column("Attached")]
        public bool IsAttached { get; set; } = false;

        /// <summary>
        /// The current processId being debugged by the OrbisSuite Debugger.
        /// </summary>
        [NotNull]
        public int CurrentProcessId { get; set; } = 0;


        /// <summary>
        /// The current foreground account identifier.
        /// </summary>
        [NotNull]
        public int ForegroundAccountId { get; set; } = 0;

        /// <summary>
        /// The used space on the targets hard disk.
        /// </summary>
        [NotNull]
        public long HDDUsedSpace { get; set; } = 0;

        /// <summary>
        /// The free space on the targets hard disk.
        /// </summary>
        [NotNull]
        public long HDDFreeSpace { get; set; } = 0;

        /// <summary>
        /// The usable size of the targets hard disk.
        /// </summary>
        [NotNull]
        public long HDDTotalSpace { get; set; } = 0;

        /// <summary>
        /// The current temperature of the CPU.
        /// </summary>
        public int CPUTemp { get; set; } = 0;

        /// <summary>
        /// The current tempurature of the SOC.
        /// </summary>
        public int SOCTemp { get; set; } = 0;

        /// <summary>
        /// The current thread count.
        /// </summary>
        public int ThreadCount { get; set; } = 0;

        /// <summary>
        /// The current average cpu usage.
        /// </summary>
        public float AverageCPUUsage { get; set; } = 0.0f;

        /// <summary>
        /// The current core that is the most busy.
        /// </summary>
        public int BusyCore { get; set; } = 0;

        /// <summary>
        /// The current system ram usage.
        /// </summary>
        public int RamUsage { get; set; } = 0;

        /// <summary>
        /// The current video ram usage.
        /// </summary>
        public int VRamUsage { get; set; } = 0;

        /// <summary>
        /// Shows the Title number of the games on the home screen of this target.
        /// </summary>
        [NotNull]
        public bool ShowTitleId { get; set; } = false;

        /// <summary>
        /// Shows the devkit information display panel that will show some information about the target like the IP Address.
        /// </summary>
        [NotNull]
        public bool ShowDevkitPanel { get; set; } = false;

        /// <summary>
        /// Shows a shortcut on the Home screen that can be used to quickly access the Orbis Toolbox menu.
        /// </summary>
        [NotNull]
        public bool ShowToolboxShortcut { get; set; } = false;

        /// <summary>
        /// Shows the '★APP_HOME' on the home screen that can be used to quickly launch home brew to debug it with out a pkg.
        /// </summary>
        [NotNull]
        public bool ShowAppHome { get; set; } = false;

        /// <summary>
        /// Saves the current information about the target to the database.
        /// </summary>
        /// <returns>Returns true if any rows were effected.</returns>
        public bool Save()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<TargetInfo>();

            var result = db.Update(this);
            db.Close();
            return (result <= 0);
        }
    }
}
