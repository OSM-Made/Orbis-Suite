using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisLib2.Common
{
    public class Config
    {
        /// <summary>
        /// The prort that is used to interact with the Target Console.
        /// </summary>
        public static readonly int APIPort = 6900;

        /// <summary>
        /// Port used to communicate events to the host machine from the Target Console.
        /// </summary>
        public static readonly int EventPort = 6901;

        /// <summary>
        /// The Port used to recieve debug logs from the Target Console.
        /// </summary>
        public static readonly int DebugPort = 6902;

        /// <summary>
        /// The default port for FTP.
        /// </summary>
        public static readonly int FTPPort = 2121;

        /// <summary>
        /// 
        /// </summary>
        public static readonly int DispatcherPort = 6919;

        /// <summary>
        /// 
        /// </summary>
        public static readonly int DispatcherClientPort = 6920;

        /// <summary>
        /// Name of the data base used to store the user data / Target List.
        /// </summary>
        public static readonly string DataBaseName = "OrbisSuiteUserData.db";

        /// <summary>
        /// The Path to the Orbis Suite data.
        /// </summary>
        public static readonly string OrbisPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\Orbis Suite";

        /// <summary>
        /// Path of the data base used to store the user data / Target List.
        /// </summary>
        public static readonly string DataBasePath = $@"{OrbisPath}\{DataBaseName}";

        /// <summary>
        /// Maximum number of targets we can store.
        /// </summary>
        public static readonly int MaxTargets = 20;

        /// <summary>
        /// The version of the packets used to communicate with the Target Console.
        /// </summary>
        public static readonly int PacketVersion = 5;
    }
}
