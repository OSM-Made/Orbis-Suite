using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{

    class Imports
    {

        //Target
        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool DoesTargetExist(string TargetName);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool DoesTargetExistIP(string IPAddr);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetTarget(string TargetName, [MarshalAs(UnmanagedType.Struct)] out DB_TargetInfo Out);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetTarget(string TargetName, bool Default, string NewTargetName, string IPAddr, int Firmware);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool DeleteTarget(string TargetName);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool NewTarget(bool Default, string TargetName, string IPAddr, int Firmware);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTargetCount();

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTargets([Out] out IntPtr Targets);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetAutoLoadPayload();

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetAutoLoadPayload(bool Value);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetStartOnBoot();

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetStartOnBoot(bool Value);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void GetDefaultTarget([MarshalAs(UnmanagedType.Struct)] out DB_TargetInfo DefaultTarget);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetDefault(string TargetName);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Shutdown(string IPAddr);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Reboot(string IPAddr);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Suspend(string IPAddr);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Notify(string IPAddr, int Type, string Message);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DoBeep(string IPAddr, int Count);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetLED(string IPAddr, byte R, byte G, byte B, byte A);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLED(string IPAddr, [MarshalAs(UnmanagedType.U1)] out byte R, [MarshalAs(UnmanagedType.U1)] out byte G, [MarshalAs(UnmanagedType.U1)] out byte B, [MarshalAs(UnmanagedType.U1)] out byte A);

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DumpProcess(string IPAddr, string ProcName, [MarshalAs(UnmanagedType.U8)] out UInt64 Size, byte[] Out);

        //OrbisLib
        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TestCommunications(string IPAddr);


    }
}
