using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class OrbisDef
    {
        public static string[] API_ERROR_STR =
        {
            "API_OK",
            "API_ERROR_NOT_CONNECTED",
            "API_ERROR_FAILED_TO_CONNNECT",
            "API_ERROR_NOT_REACHABLE",
            "API_ERROR_NOT_ATTACHED",
            "API_ERROR_LOST_PROC",

            "API_ERROR_FAIL",
            "API_ERROR_INVALID_ADDRESS",

            //Debugger
            "API_ERROR_PROC_RUNNING",
            "API_ERROR_DEBUGGER_NOT_ATTACHED",

            "API_ERROR_NOTARGET",
        };
    }

    public class TargetInfo
    {
        public bool Default;
        public string Name;
        public string IPAddr;
        public string Firmware;
        public int PayloadPort;
        public bool Available;
        public string Title;
        public string SDKVersion;
        public string ConsoleName;
        public string ConsoleType;
        public bool Attached;
        public string CurrentProc;

        public TargetInfo(bool Default, string Name, string IPAddr, string Firmware, int PayloadPort, bool Available, string Title, string SDKVersion, string ConsoleName, string ConsoleType, bool Attached, string CurrentProc)
        {
            this.Default = Default;
            this.Name = Name;
            this.IPAddr = IPAddr;
            this.Firmware = Firmware;
            this.PayloadPort = PayloadPort;
            this.Available = Available;
            this.Title = Title;
            this.SDKVersion = SDKVersion;
            this.ConsoleName = ConsoleName;
            this.ConsoleType = ConsoleType;
            this.Attached = Attached;
            this.CurrentProc = CurrentProc;
        }
    }

    public class ProcessInfo
    {
        public Int32 PID;
        public bool Attached;
        public string Name;
        public string TitleID;
        public UInt64 TextSegmentBase;
        public UInt64 TextSegmentLen;
        public UInt64 DataSegmentBase;
        public UInt64 DataSegmentLen;

        public ProcessInfo(Int32 PID, bool Attached, string Name, string TitleID, UInt64 TextSegmentBase, UInt64 TextSegmentLen, UInt64 DataSegmentBase, UInt64 DataSegmentLen)
        {
            this.PID = PID;
            this.Attached = Attached;
            this.Name = Name;
            this.TitleID = TitleID;
            this.TextSegmentBase = TextSegmentBase;
            this.TextSegmentLen = TextSegmentLen;
            this.DataSegmentBase = DataSegmentBase;
            this.DataSegmentLen = DataSegmentLen;
        }

        public ProcessInfo()
        {
            this.Attached = false;
        }
    }



    [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Ansi)]
    public struct RESP_Proc
    {
        public Int32 ProcessID;
        public bool Attached;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ProcName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] TitleID;
        public UInt64 TextSegmentBase;
        public UInt64 TextSegmentLen;
        public UInt64 DataSegmentBase;
        public UInt64 DataSegmentLen;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct DB_TargetInfo
    {
        public bool Default;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] IPAddr;
        public int Firmware;
        public int PayloadPort;
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
        public bool Attached;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] CurrentProc;
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

    public enum API_ERRORS
    {
        API_OK = 0,
        API_ERROR_NOT_CONNECTED,
        API_ERROR_FAILED_TO_CONNNECT,
        API_ERROR_NOT_REACHABLE,
        API_ERROR_NOT_ATTACHED,
        API_ERROR_LOST_PROC,

        API_ERROR_FAIL,
        API_ERROR_INVALID_ADDRESS,

        //Debugger
        API_ERROR_PROC_RUNNING,
        API_ERROR_DEBUGGER_NOT_ATTACHED,

        API_ERROR_NOTARGET,
    };

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct registers
    {
        public ulong r_r15;
        public ulong r_r14;
        public ulong r_r13;
        public ulong r_r12;
        public ulong r_r11;
        public ulong r_r10;
        public ulong r_r9;
        public ulong r_r8;
        public ulong r_rdi;
        public ulong r_rsi;
        public ulong r_rbp;
        public ulong r_rbx;
        public ulong r_rdx;
        public ulong r_rcx;
        public ulong r_rax;
        public uint r_trapno;
        public ushort r_fs;
        public ushort r_gs;
        public uint r_err;
        public ushort r_es;
        public ushort r_ds;
        public ulong r_rip;
        public ulong r_cs;
        public ulong r_rflags;
        public ulong r_rsp;
        public ulong r_ss;
    }

    public enum SIGNALS
    {
        SIGHUP = 1,
        SIGINT = 2,
        SIGQUIT = 3,
        SIGILL = 4,
        SIGTRAP = 5,
        SIGABRT = 6,
        SIGEMT = 7,
        SIGFPE = 8,
        SIGKILL = 9,
        SIGBUS = 10,
        SIGSEGV = 11,
        SIGSYS = 12,
        SIGPIPE = 13,
        SIGALRM = 14,
        SIGTERM = 15,
        SIGURG = 16,
        SIGSTOP = 17,
        SIGTSTP = 18,
        SIGCONT = 19,
        SIGCHLD = 20,
        SIGTTIN = 21,
        SIGTTOU = 22,
        SIGIO = 23,
        SIGXCPU = 24,
        SIGXFSZ = 25,
        SIGVTALRM = 26,
        SIGPROF = 27,
        SIGWINCH = 28,
        SIGINFO = 29,
        SIGUSR1 = 30,
        SIGUSR2 = 31,
    };

    public enum PRFLAGS
    {
        PR_ATTACHED = 0x800,
        PR_STOPPED = 0x20000,
    }

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

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RESP_ModuleList
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x24)]
        public byte[] Name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Path;
        public Int32 Handle;
        public UInt64 TextSegmentBase;
        public UInt64 TextSegmentLen;
        public UInt64 DataSegmentBase;
        public UInt64 DataSegmentLen;
    };

    public class ModuleInfo
    {
        public string Name;
        public string Path;
        public Int32 Handle;
        public UInt64 TextSegmentBase;
        public UInt64 TextSegmentLen;
        public UInt64 DataSegmentBase;
        public UInt64 DataSegmentLen;

        public ModuleInfo(string Name, string Path, Int32 Handle, UInt64 TextSegmentBase, UInt64 TextSegmentLen, UInt64 DataSegmentBase, UInt64 DataSegmentLen)
        {
            this.Name = Name;
            this.Path = Path;
            this.Handle = Handle;
            this.TextSegmentBase = TextSegmentBase;
            this.TextSegmentLen = TextSegmentLen;
            this.DataSegmentBase = DataSegmentBase;
            this.DataSegmentLen = DataSegmentLen;
        }
    }
}
