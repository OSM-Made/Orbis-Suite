using SQLite;

namespace OrbisLib2.Common.Database
{
    /// <summary>
    /// Used to get/set the settings of Orbis Suite.
    /// </summary>
    [Table("Settings")]
    public class Settings
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        /// <summary>
        /// The API port that OrbisLib communicates on.
        /// </summary>
        [NotNull]
        public int APIPort { get; set; } = 6900;

        /// <summary>
        /// The port that will be used to access the targets file system using ftp
        /// </summary>
        [NotNull]
        public int FTPPort { get; set; } = 2121;

        /// <summary>
        /// The port of a klog server that will be used to print console output similar to UART.
        /// </summary>
        [NotNull]
        public int KlogPort { get; set; } = 3232;

        /// <summary>
        /// The serial COM port we will listen to for UART output.
        /// </summary>
        [NotNull]
        public string? COMPort { get; set; } = "-";

        /// <summary>
        /// Starts the Orbis Suite taskbar app when windows boots.
        /// </summary>
        [NotNull]
        public bool StartOnBoot { get; set; } = false;

        /// <summary>
        /// Choose which theme will be used across Orbis Suite.
        /// </summary>
        [NotNull]
        public int Theme { get; set; } = 0;

        /// <summary>
        /// Enables the accent colours to cycle through all colours of the rainbow.
        /// </summary>
        [NotNull]
        public bool RainbowColours { get; set; } = false;

        /// <summary>
        /// When viewd from the target details choose to censor the Target identifier.
        /// </summary>
        [NotNull]
        public bool CensorIDPS { get; set; } = false;

        /// <summary>
        /// When viewd from the target details choose to censor the Target identifier.
        /// </summary>
        [NotNull]
        public bool CensorPSID { get; set; } = false;

        /// <summary>
        /// SHow timestamps on the console output.
        /// </summary>
        [NotNull]
        public bool ShowTimestamps { get; set; } = false;

        /// <summary>
        /// Word wrap the console output window.
        /// </summary>
        [NotNull]
        public bool WordWrap { get; set; } = false;

        public static Settings CreateInstance()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<Settings>();

            // Try to pull the entries of the settings
            var list = db.Table<Settings>();
            if (list.Count() > 0)
            {
                var instance = list.First();
                db.Close();
                return instance;
            }
            else
            {
                // Create settings entry since it doesn't exist.
                var instance = new Settings();
                var result = db.Insert(instance);
                db.Close();

                if (result > 0)
                    return instance;
                else
                    throw new Exception("Failed to create settings row into database.");
            }
        }

        public void Save()
        {
            var db = new SQLiteConnection(Config.DataBasePath, SQLiteOpenFlags.ReadWrite);
            db.Update(this);
            db.Close();
        }
    }
}
