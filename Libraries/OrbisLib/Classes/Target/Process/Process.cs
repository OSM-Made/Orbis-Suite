using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class Process
    {
        public Memory Memory;
        private OrbisLib PS4;
        private Target Target;

        public Process(OrbisLib PS4, Target Target)
        {
            this.PS4 = PS4;
            this.Target = Target;

            Memory = new Memory(Target, this);
        }

        public void SelectProcess()
        {
            PS4.Dialogs.SelectProcess(Target.Info.Name);
        }

        public List<ProcessInfo> GetList()
        {
            //Allocate unmanaged memory.
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RESP_Proc)) * 100);

            int ProcCount = 0;
            List<ProcessInfo> ProcList = new List<ProcessInfo>();

            if (Imports.Process.GetProcList(Target.Info.IPAddr, out ProcCount, ptr) != (int)API_ERRORS.API_OK)
                return ProcList;

            if (ProcCount == 0)
                return ProcList;

            for (int i = 0; i < ProcCount; i++)
            {
                //Convert the array of targets to a struct c# can use and incrementing the pointer by the size of the struct to get the next.
                RESP_Proc ProcInfo = (RESP_Proc)Marshal.PtrToStructure(ptr, typeof(RESP_Proc));
                ptr += Marshal.SizeOf(typeof(RESP_Proc));

                ProcList.Add(
                    new ProcessInfo(
                        ProcInfo.ProcessID,
                        ProcInfo.Attached,
                        Utilities.CleanByteToString(ProcInfo.ProcName),
                        Utilities.CleanByteToString(ProcInfo.TitleID),
                        ProcInfo.TextSegmentBase,
                        ProcInfo.TextSegmentLen,
                        ProcInfo.DataSegmentBase,
                        ProcInfo.DataSegmentLen
                    ));
            }

            //Weird shit requires you to be at the start to free it.
            ptr -= Marshal.SizeOf(typeof(RESP_Proc)) * ProcCount;

            //free unmanageed memory.
            Marshal.FreeHGlobal(ptr);

            return ProcList;
        }

        public API_ERRORS Attach(string ProcName)
        {
            return Imports.Process.Attach(Target.Info.IPAddr, ProcName);
        }

        public API_ERRORS Detach(string ProcName)
        {
            return Imports.Process.Detach(Target.Info.IPAddr, ProcName);
        }

        public API_ERRORS Detach()
        {
            if (Target.Info.Attached)
                return Imports.Process.Detach(Target.Info.IPAddr, Target.Info.CurrentProc);
            else
                return API_ERRORS.API_ERROR_NOT_ATTACHED;
        }

        public API_ERRORS GetCurrent(out ProcessInfo Info)
        {
            //Allocate unmanaged memory.
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RESP_Proc)));

            //Call our unmanaged c++ import.
            API_ERRORS res = Imports.Process.GetCurrent(Target.Info.IPAddr, ptr);

            //Convert the unmanaged memory into a easy to digest structure.
            RESP_Proc ProcInfo = (RESP_Proc)Marshal.PtrToStructure(ptr, typeof(RESP_Proc));

            //Populate the c# class and clean strings for C# usage.
            Info = new ProcessInfo(
                        ProcInfo.ProcessID,
                        ProcInfo.Attached,
                        Utilities.CleanByteToString(ProcInfo.ProcName),
                        Utilities.CleanByteToString(ProcInfo.TitleID),
                        ProcInfo.TextSegmentBase,
                        ProcInfo.TextSegmentLen,
                        ProcInfo.DataSegmentBase,
                        ProcInfo.DataSegmentLen
                    );

            return res;
        }

        public API_ERRORS Read(UInt64 Address, Int32 Len, out byte[] List)
        {
            return Imports.Process.Read(Target.Info.IPAddr, Address, Len, out List);
        }

        public API_ERRORS Write(UInt64 Address, Int32 Len, byte[] List)
        {
            return Imports.Process.Write(Target.Info.IPAddr, Address, Len, List);
        }

        public API_ERRORS Kill(string ProcName)
        {
            return Imports.Process.Kill(Target.Info.IPAddr, ProcName);
        }

        public API_ERRORS Kill()
        {
            return Imports.Process.Kill(Target.Info.IPAddr, Target.Info.CurrentProc);
        }

        public API_ERRORS LoadELF(byte[] Data, Int32 Len)
        {
            return Imports.Process.LoadELF(Target.Info.IPAddr, Data, Len);
        }

        //
        // TODO: Add RPC Call
        //

        //Modules
        public Int32 LoadSPRX(string Path, Int32 Flags)
        {
            return Imports.Process.LoadSPRX(Target.Info.IPAddr, Path, Flags);
        }

        public int UnloadSPRX(Int32 Handle, Int32 Flags)
        {
            return Imports.Process.UnloadSPRX(Target.Info.IPAddr, Handle, Flags);
        }

        public int UnloadSPRX(string Name, Int32 Flags)
        {
            return Imports.Process.UnloadSPRXbyName(Target.Info.IPAddr, Name, Flags);
        }

        public Int32 ReloadSPRX(string Name, Int32 Flags)
        {
            return Imports.Process.ReloadSPRXbyName(Target.Info.IPAddr, Name, Flags);
        }

        public Int32 ReloadSPRX(Int32 Handle, Int32 Flags)
        {
            return Imports.Process.ReloadSPRX(Target.Info.IPAddr, Handle, Flags);
        }

        public API_ERRORS DumpModule(string Name, int Length, byte[] Out)
        {
            IntPtr ptr = Marshal.AllocHGlobal(Length);

            int RealLength = 0;
            API_ERRORS Result = Imports.Process.DumpModule(Target.Info.IPAddr, Name, out RealLength, ptr);

            Marshal.Copy(ptr, Out, 0, Length);

            //free unmanageed memory.
            Marshal.FreeHGlobal(ptr);

            return Result;
        }

        public API_ERRORS GetLibraryList(out List<ModuleInfo> List)
        {
            List<ModuleInfo> ModuleList = new List<ModuleInfo>();

            //Allocate unmanaged memory.
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RESP_ModuleList)) * 100);

            int ModuleCount = 0;
            API_ERRORS res = Imports.Process.GetLibraryList(Target.Info.IPAddr, out ModuleCount, ptr);

            if (ModuleCount == 0)
            {
                List = ModuleList;
                return API_ERRORS.API_ERROR_FAIL;
            }
                

            for (int i = 0; i < ModuleCount; i++)
            {
                //Convert the unmanaged memory into a easy to digest structure. Increment the pointer of memory to the nex struct in the array.
                RESP_ModuleList ModuleInfo = (RESP_ModuleList)Marshal.PtrToStructure(ptr, typeof(RESP_ModuleList));
                ptr += Marshal.SizeOf(typeof(RESP_ModuleList));

                ModuleList.Add(new ModuleInfo(
                        Utilities.CleanByteToString(ModuleInfo.Name),
                        Utilities.CleanByteToString(ModuleInfo.Path),
                        ModuleInfo.Handle,
                        ModuleInfo.TextSegmentBase,
                        ModuleInfo.TextSegmentLen,
                        ModuleInfo.DataSegmentBase,
                        ModuleInfo.DataSegmentLen
                        ));

            }

            //Set the output list to out new list.
            List = ModuleList;

            //Weird shit requires you to be at the start to free it.
            ptr -= Marshal.SizeOf(typeof(RESP_ModuleList)) * ModuleCount;

            //free unmanageed memory.
            Marshal.FreeHGlobal(ptr);

            return res;
        }

    }


}
