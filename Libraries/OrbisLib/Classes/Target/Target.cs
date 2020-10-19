using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class Target
    {
        public bool Active = false;
        public TargetInfo Info;

        private OrbisLib PS4;
        public TargetEvents Events;
        public Debug Debug;
        public Payload Payload;
        public Process Process;
        public FTP FTP;

        public Target(OrbisLib PS4)
        {
            Active = false;
            Info = new TargetInfo();
            this.PS4 = PS4;
            Events = new TargetEvents(this);
            Debug = new Debug(this);
            Payload = new Payload(this);
            Process = new Process(PS4, this);
            FTP = new FTP(this);
        }

        public Target(OrbisLib PS4, TargetInfo TargetInfo)
        {
            Active = true;
            Info = TargetInfo;
            this.PS4 = PS4;
            Events = new TargetEvents(this);
            Debug = new Debug(this);
            Payload = new Payload(this);
            Process = new Process(PS4, this);
            FTP = new FTP(this);
        }

        public bool Edit(string Name, string IPAddr, int Firmware, int PayloadPort)
        {
            return Imports.TargetManagement.SetTarget(Info.Name, Info.Default, Name, IPAddr, Firmware, PayloadPort);
        }

        public void SetDefault()
        {
            Imports.TargetManagement.SetDefault(Info.Name);
        }

        public bool Delete()
        {
            if (PS4.TargetManagement.DefaultTarget.Name.Equals(Info.Name))
            {
                DarkMessageBox.ShowError($"{Info.Name} is the Default Target and cant be deleted.", "Cant Delete Default Target");
                return false;
            }

            return Imports.TargetManagement.DeleteTarget(Info.Name);
        }

        public API_ERRORS Shutdown()
        {
            return Imports.Target.Shutdown(Info.IPAddr);
        }

        public API_ERRORS Reboot()
        {
            return Imports.Target.Reboot(Info.IPAddr);
        }

        public API_ERRORS Suspend()
        {
            return Imports.Target.Suspend(Info.IPAddr);
        }

        public API_ERRORS Notify(string Message)
        {
            return Imports.Target.Notify(Info.IPAddr, -1, Message);
        }

        public API_ERRORS Notify(int Type, string Message)
        {
            return Imports.Target.Notify(Info.IPAddr, Type, Message);
        }

        public API_ERRORS Beep(int Count)
        {
            return Imports.Target.DoBeep(Info.IPAddr, Count);
        }

        public API_ERRORS SetLED(byte R, byte G, byte B, byte A)
        {
            return Imports.Target.SetLED(Info.IPAddr, R, G, B, A);
        }

        public API_ERRORS GetLED(out byte R, out byte G, out byte B, out byte A)
        {
            return Imports.Target.GetLED(Info.IPAddr, out R, out G, out B, out A);
        }

        public API_ERRORS DumpProcess(string ProcName, int Length, byte[] Out)
        {
            IntPtr ptr = Marshal.AllocHGlobal(Length);

            UInt64 RealLength = 0;
            API_ERRORS Result = Imports.Target.DumpProcess(Info.IPAddr, ProcName, out RealLength, ptr);

            Marshal.Copy(ptr, Out, 0, Length);

            //free unmanageed memory.
            Marshal.FreeHGlobal(ptr);

            return Result;
        }
    }
}
