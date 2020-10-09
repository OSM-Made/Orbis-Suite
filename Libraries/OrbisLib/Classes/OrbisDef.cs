using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class TargetInfo
    {
        public bool Default;
        public string Name;
        public string IPAddr;
        public string Firmware;
        public bool Available;
        public string Title;
        public string SDKVersion;
        public string ConsoleName;
        public string ConsoleType;

        public TargetInfo(bool Default, string Name, string IPAddr, string Firmware, bool Available, string Title, string SDKVersion, string ConsoleName, string ConsoleType)
        {
            this.Default = Default;
            this.Name = Name;
            this.IPAddr = IPAddr;
            this.Firmware = Firmware;
            this.Available = Available;
            this.Title = Title;
            this.SDKVersion = SDKVersion;
            this.ConsoleName = ConsoleName;
            this.ConsoleType = ConsoleType;
        }
    }

    public class OrbisDef
    {
        public static string[] ConsoleTypesNames =
        {
            "-",
            "Diag",
            "Devkit",
            "Testkit",
            "Retail",
            "Kratos"
        };
    };

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct DB_TargetInfo
    {
        public bool Default;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] IPAddr;
        public int Firmware;
        public bool Available;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        public byte[] SDKVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] SoftwareVersion;
        public int CPUTemp;
        public int SOCTemp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] CurrentTitleID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public byte[] ConsoleName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] IDPS;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] PSID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] ConsoleType;
    };

    public enum ConsoleTypes
    {
        UNK,
        DIAG, //0x80
        DEVKIT, //0x81
        TESTKIT, //0x82
        RETAIL, //0x83 -> 0x8F
        KRATOS, //0xA0 IMPOSSIBLE??
    };

    public class DetailedTargetInfo
    {
        public string SDKVersion;
        public string SoftwareVersion;
        public int CPUTemp;
        public int SOCTemp;
        public string CurrentTitleID;
        public string ConsoleName;
        public string IDPS;
        public string PSID;
        public string ConsoleType;

        public DetailedTargetInfo(string SDKVersion, string SoftwareVersion, int CPUTemp, int SOCTemp, string CurrentTitleID, string ConsoleName, string IDPS, string PSID, string ConsoleType)
        {
            this.SDKVersion = SDKVersion;
            this.SoftwareVersion = SoftwareVersion;
            this.CPUTemp = CPUTemp;
            this.SOCTemp = SOCTemp;
            this.CurrentTitleID = CurrentTitleID;
            this.ConsoleName = ConsoleName;
            this.IDPS = IDPS;
            this.PSID = PSID;
            this.ConsoleType = ConsoleType;
        }

        public DetailedTargetInfo()
        {
            SDKVersion = "-";
            SoftwareVersion = "-";
            CPUTemp = 0;
            SOCTemp = 0;
            CurrentTitleID = "-";
            ConsoleName = "-";
            IDPS = "-";
            PSID = "-";
            ConsoleType = "-";
        }
    };
}
