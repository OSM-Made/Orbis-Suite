using OrbisLib2.Common.Database.Types;
using SQLite;
using System.Text.RegularExpressions;

namespace OrbisLib2.Common.Database
{
    public class StaticInfo
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int TargetId { get; set; }

        /// <summary>
        /// If the static data has been fetched.
        /// </summary>
        public bool IsSet { get; set; }

        /// <summary>
        /// The software version first installed on the target when sold.
        /// </summary>
        [NotNull]
        public string FactorySoftwareVersion { get; set; } = "-";

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
        /// Saves the current information about the target to the database.
        /// </summary>
        /// <returns>Returns true if any rows were effected.</returns>
        public bool Save()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<StaticInfo>();

            var result = db.Update(this);
            db.Close();
            return (result <= 0);
        }
    }
}
