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
        internal static extern int GetInfo(string IPAddr, [MarshalAs(UnmanagedType.Struct)] out RESP_TargetInfo TargetInfo);

        //OrbisLib
        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TestCommunications(string IPAddr);


    }
}
