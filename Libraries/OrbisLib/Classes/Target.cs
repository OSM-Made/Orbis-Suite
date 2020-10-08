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

                List.Add(new TargetInfo(Utilities.CleanByteToString(RawTargetInfo.Name), 
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

            return new TargetInfo(Utilities.CleanByteToString(RawTargetInfo.Name),
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

        public int GetInfo(string IPAddr, out DetailedTargetInfo TargetInfo)
        {
            RESP_TargetInfo RawTargetInfo;
            int Status = Imports.GetInfo(IPAddr, out RawTargetInfo);


            string SDKVersion = "-", SoftwareVersion = "-", CurrentTitleID = "-", ConsoleName = "-", IDPS = "-", PSID = "-";

            SDKVersion = string.Format("{0}.{1}.{2}", ((RawTargetInfo.SDKVersion >> 24) & 0xFF).ToString("X1"), ((RawTargetInfo.SDKVersion >> 12) & 0xFFF).ToString("X3"), (RawTargetInfo.SDKVersion & 0xFFF).ToString("X3"));
            SoftwareVersion = string.Format("{0}.{1}", ((RawTargetInfo.SoftwareVersion >> 24) & 0xFF).ToString("X1"), ((RawTargetInfo.SoftwareVersion >> 16) & 0xFF).ToString("X2"));

            CurrentTitleID = Encoding.Default.GetString(RawTargetInfo.CurrentTitleID);
            CurrentTitleID = CurrentTitleID.Substring(0, CurrentTitleID.IndexOf('\0'));

            ConsoleName = Encoding.Default.GetString(RawTargetInfo.ConsoleName);
            ConsoleName = ConsoleName.Substring(0, ConsoleName.IndexOf('\0'));

            Console.WriteLine("{0}.{1}", ((RawTargetInfo.SoftwareVersion >> 24) & 0xFF).ToString("X1"), ((RawTargetInfo.SoftwareVersion >> 16) & 0xFF).ToString("X2"));

            TargetInfo = new DetailedTargetInfo(SDKVersion, SoftwareVersion, RawTargetInfo.CPUTemp, RawTargetInfo.SOCTemp, CurrentTitleID, ConsoleName, IDPS, PSID, OrbisDef.ConsoleTypesNames[RawTargetInfo.ConsoleType]);

            return Status;
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
