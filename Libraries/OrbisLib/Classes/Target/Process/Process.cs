using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite
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

        private ProcessInfo _Current;
        public ProcessInfo Current
        {
            get
            {
                //Allocate unmanaged memory.
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RESP_Proc)));

                //Call our unmanaged c++ import.
                API_ERRORS Result = Imports.Process.GetCurrent(Target.Info.IPAddr, ptr);

                if (Result != API_ERRORS.API_OK)
                    return _Current = new ProcessInfo();

                //Convert the unmanaged memory into a easy to digest structure.
                RESP_Proc ProcInfo = (RESP_Proc)Marshal.PtrToStructure(ptr, typeof(RESP_Proc));

                //Populate the c# class and clean strings for C# usage.
                _Current = new ProcessInfo(
                            ProcInfo.ProcessID,
                            ProcInfo.Attached,
                            Utilities.CleanByteToString(ProcInfo.ProcName),
                            Utilities.CleanByteToString(ProcInfo.TitleID),
                            ProcInfo.TextSegmentBase,
                            ProcInfo.TextSegmentLen,
                            ProcInfo.DataSegmentBase,
                            ProcInfo.DataSegmentLen
                        );

                return _Current;
            }
        }

        private List<ProcessInfo> _List = new List<ProcessInfo>();
        public List<ProcessInfo> List
        {
            get
            {
                //Clear the list for update
                _List.Clear();

                //Allocate unmanaged memory.
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RESP_Proc)) * 100);

                int ProcCount = 0;
                if (Imports.Process.GetProcList(Target.Info.IPAddr, out ProcCount, ptr) != (int)API_ERRORS.API_OK)
                    return _List;

                if (ProcCount == 0)
                    return _List;

                for (int i = 0; i < ProcCount; i++)
                {
                    //Convert the array of targets to a struct c# can use and incrementing the pointer by the size of the struct to get the next.
                    RESP_Proc ProcInfo = (RESP_Proc)Marshal.PtrToStructure(ptr, typeof(RESP_Proc));
                    ptr += Marshal.SizeOf(typeof(RESP_Proc));

                    _List.Add(
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

                return _List;
            }
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

        private List<ModuleInfo> _ModuleList = new List<ModuleInfo>();
        public List<ModuleInfo> ModuleList
        {
            get
            {
                //clear the list for update.
                _ModuleList.Clear();

                //Allocate unmanaged memory.
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RESP_ModuleList)) * 200);

                int ModuleCount = 0;
                API_ERRORS res = Imports.Process.GetLibraryList(Target.Info.IPAddr, out ModuleCount, ptr);

                if (ModuleCount == 0)
                    return _ModuleList;

                for (int i = 0; i < ModuleCount; i++)
                {
                    

                    //Convert the unmanaged memory into a easy to digest structure. Increment the pointer of memory to the nex struct in the array.
                    RESP_ModuleList ModuleInfo = (RESP_ModuleList)Marshal.PtrToStructure(ptr, typeof(RESP_ModuleList));
                    ptr += Marshal.SizeOf(typeof(RESP_ModuleList));

                    Console.WriteLine($"{ModuleInfo.Handle} {Utilities.CleanByteToString(ModuleInfo.Path)}");

                    _ModuleList.Add(new ModuleInfo(
                            Path.GetFileName(Utilities.CleanByteToString(ModuleInfo.Path)),
                            Utilities.CleanByteToString(ModuleInfo.Path),
                            ModuleInfo.Handle,
                            ModuleInfo.TextSegmentBase,
                            ModuleInfo.TextSegmentLen,
                            ModuleInfo.DataSegmentBase,
                            ModuleInfo.DataSegmentLen
                            ));

                }

                //Weird shit requires you to be at the start to free it.
                ptr -= Marshal.SizeOf(typeof(RESP_ModuleList)) * ModuleCount;

                //free unmanageed memory.
                Marshal.FreeHGlobal(ptr);

                return _ModuleList;
            }
        }
    }
}
