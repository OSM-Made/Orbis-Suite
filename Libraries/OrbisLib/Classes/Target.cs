using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        internal OrbisLib PS4;

        public Target(OrbisLib InPS4)
        {
            PS4 = InPS4;

        }

        public bool DoesTargetExist(string TargetName)
        {
            return Imports.DoesTargetExist(TargetName);
        }

        public bool DoesTargetExistIP(string IPAddr)
        {
            return Imports.DoesTargetExistIP(IPAddr);
        }

        public bool GetTarget(string TargetName, out TargetInfo Out)
        {
            DB_TargetInfo RawTargetInfo;
            bool Result = Imports.GetTarget(TargetName, out RawTargetInfo);

            Out = new TargetInfo(RawTargetInfo.Default, 
                    Utilities.CleanByteToString(RawTargetInfo.Name),
                    Utilities.CleanByteToString(RawTargetInfo.IPAddr),
                    (RawTargetInfo.Firmware / 100.0).ToString(),
                    RawTargetInfo.Available,
                    Utilities.CleanByteToString(RawTargetInfo.CurrentTitleID),
                    Utilities.CleanByteToString(RawTargetInfo.SDKVersion),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleName),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleType));

            return Result;
        }

        public bool SetTarget(string TargetName, bool Default, string NewTargetName, string IPAddr, int Firmware)
        {
            return Imports.SetTarget(TargetName, Default, NewTargetName, IPAddr, Firmware);
        }

        public bool DeleteTarget(string TargetName)
        {
            return Imports.DeleteTarget(TargetName);
        }

        public bool NewTarget(bool Default, string TargetName, string IPAddr, int Firmware)
        {
            return Imports.NewTarget(Default, TargetName, IPAddr, Firmware);
        }

        public int GetTargetCount()
        {
            return Imports.GetTargetCount();
        }

        public List<TargetInfo> GetTargetList()
        {
            List<TargetInfo> List = new List<TargetInfo>();
            IntPtr ptr = IntPtr.Zero;
            int TargetCount = Imports.GetTargets(out ptr);

            for (int i = 0; i < TargetCount; i++)
            {
                //Convert the array of targets to a struct c# can use and incrementing the pointer by the size of the struct to get the next.
                DB_TargetInfo RawTargetInfo = (DB_TargetInfo)Marshal.PtrToStructure(ptr, typeof(DB_TargetInfo));
                ptr += Marshal.SizeOf(typeof(DB_TargetInfo));

                List.Add(new TargetInfo(RawTargetInfo.Default, 
                    Utilities.CleanByteToString(RawTargetInfo.Name), 
                    Utilities.CleanByteToString(RawTargetInfo.IPAddr), 
                    (RawTargetInfo.Firmware / 100.0).ToString(), 
                    RawTargetInfo.Available, 
                    Utilities.CleanByteToString(RawTargetInfo.CurrentTitleID), 
                    Utilities.CleanByteToString(RawTargetInfo.SDKVersion), 
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleName), 
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleType)));
            }

            return List;
        }

        public bool GetAutoLoadPayload()
        {
            return Imports.GetAutoLoadPayload();
        }

        public void SetAutoLoadPayload(bool Value)
        {
            Imports.SetAutoLoadPayload(Value);
        }

        public bool GetStartOnBoot()
        {
            return Imports.GetStartOnBoot();
        }

        public void SetStartOnBoot(bool Value)
        {
            Imports.SetStartOnBoot(Value);
        }

        public TargetInfo GetDefault()
        {
            DB_TargetInfo RawTargetInfo;
            Imports.GetDefaultTarget(out RawTargetInfo);

            return new TargetInfo(RawTargetInfo.Default, 
                    Utilities.CleanByteToString(RawTargetInfo.Name),
                    Utilities.CleanByteToString(RawTargetInfo.IPAddr),
                    (RawTargetInfo.Firmware / 100.0).ToString(),
                    RawTargetInfo.Available,
                    Utilities.CleanByteToString(RawTargetInfo.CurrentTitleID),
                    Utilities.CleanByteToString(RawTargetInfo.SDKVersion),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleName),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleType));
        }

        public void SetDefault(string TargetName)
        {
            Imports.SetDefault(TargetName);
        }

        public DetailedTargetInfo GetInfo(string TargetName)
        {
            DB_TargetInfo RawTargetInfo;
            Imports.GetTarget(TargetName, out RawTargetInfo);

            return new DetailedTargetInfo(Utilities.CleanByteToString(RawTargetInfo.SDKVersion), 
                Utilities.CleanByteToString(RawTargetInfo.SoftwareVersion), 
                RawTargetInfo.CPUTemp, 
                RawTargetInfo.SOCTemp,
                Utilities.CleanByteToString(RawTargetInfo.CurrentTitleID),
                Utilities.CleanByteToString(RawTargetInfo.ConsoleName),
                Utilities.CleanByteToString(RawTargetInfo.IDPS),
                Utilities.CleanByteToString(RawTargetInfo.PSID),
                Utilities.CleanByteToString(RawTargetInfo.ConsoleType));
        }

        public int Shutdown(string IPAddr)
        {
            return Imports.Shutdown(IPAddr);
        }

        public int Reboot(string IPAddr)
        {
            return Imports.Reboot(IPAddr);
        }

        public int Suspend(string IPAddr)
        {
            return Imports.Suspend(IPAddr);
        }

        public int Notify(string IPAddr, string Message)
        {
            return Imports.Notify(IPAddr, -1, Message);
        }

        public int Notify(string IPAddr, int Type, string Message)
        {
            return Imports.Notify(IPAddr, Type, Message);
        }

        public int Beep(string IPAddr, int Count)
        {
            return Imports.DoBeep(IPAddr, Count);
        }

        public int SetLED(string IPAddr, byte R, byte G, byte B, byte A)
        {
            return Imports.SetLED(IPAddr, R, G, B, A);
        }

        public int GetLED(string IPAddr, out byte R, out byte G, out byte B, out byte A)
        {
            return Imports.GetLED(IPAddr, out R, out G, out B, out A);
        }

        public int DumpProcess(string IPAddr, string ProcName, out UInt64 Size, byte[] Out)
        {
            return Imports.DumpProcess(IPAddr, ProcName, out Size, Out);
        }
    }

    public class Targets
    {
        public string Name;
        public string IPAddress;
        public int FirmWare;

        public Targets(string Name, string IPAddress, int FirmWare)
        {
            this.Name = Name;
            this.IPAddress = IPAddress;
            this.FirmWare = FirmWare;
        }
    }
}
