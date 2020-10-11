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

            if (Imports.GetProcList(Target.Info.IPAddr, out ProcCount, ptr) != (int)API_ERRORS.API_OK)
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
            return Imports.Attach(Target.Info.IPAddr, ProcName);
        }

        public API_ERRORS Detach(string ProcName)
        {
            return Imports.Detach(Target.Info.IPAddr, ProcName);
        }

    }

    
}
