using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class TargetManagement
    {
        public TargetManagement()
        {

        }
        public bool DoesDefaultTargetExist()
        {
            return Imports.TargetManagement.DoesDefaultTargetExist();
        }

        public bool DoesTargetExist(string TargetName)
        {
            return Imports.TargetManagement.DoesTargetExist(TargetName);
        }

        public bool DoesTargetExistIP(string IPAddr)
        {
            return Imports.TargetManagement.DoesTargetExistIP(IPAddr);
        }

        public bool GetTarget(string TargetName, out TargetInfo Out)
        {
            DB_TargetInfo RawTargetInfo;
            bool Result = Imports.TargetManagement.GetTarget(TargetName, out RawTargetInfo);

            Out = new TargetInfo(RawTargetInfo.Default,
                    Utilities.CleanByteToString(RawTargetInfo.Name),
                    Utilities.CleanByteToString(RawTargetInfo.IPAddr),
                    (RawTargetInfo.Firmware / 100.0).ToString(),
                    RawTargetInfo.PayloadPort,
                    RawTargetInfo.Available,
                    Utilities.CleanByteToString(RawTargetInfo.CurrentTitleID),
                    Utilities.CleanByteToString(RawTargetInfo.SDKVersion),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleName),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleType),
                    RawTargetInfo.Attached,
                    Utilities.CleanByteToString(RawTargetInfo.CurrentProc));

            return Result;
        }

        public bool SetTarget(string TargetName, bool Default, string NewTargetName, string IPAddr, int Firmware, int PayloadPort)
        {
            return Imports.TargetManagement.SetTarget(TargetName, Default, NewTargetName, IPAddr, Firmware, PayloadPort);
        }

        public bool DeleteTarget(string TargetName)
        {
            return Imports.TargetManagement.DeleteTarget(TargetName);
        }

        public bool NewTarget(bool Default, string TargetName, string IPAddr, int Firmware, int PayloadPort)
        {
            return Imports.TargetManagement.NewTarget(Default, TargetName, IPAddr, Firmware, PayloadPort);
        }

        public int GetTargetCount()
        {
            return Imports.TargetManagement.GetTargetCount();
        }

        public List<TargetInfo> GetTargetList()
        {
            List<TargetInfo> List = new List<TargetInfo>();
            IntPtr ptr = IntPtr.Zero;
            int TargetCount = Imports.TargetManagement.GetTargets(out ptr);

            for (int i = 0; i < TargetCount; i++)
            {
                //Convert the array of targets to a struct c# can use and incrementing the pointer by the size of the struct to get the next.
                DB_TargetInfo RawTargetInfo = (DB_TargetInfo)Marshal.PtrToStructure(ptr, typeof(DB_TargetInfo));
                ptr += Marshal.SizeOf(typeof(DB_TargetInfo));

                List.Add(new TargetInfo(RawTargetInfo.Default,
                    Utilities.CleanByteToString(RawTargetInfo.Name),
                    Utilities.CleanByteToString(RawTargetInfo.IPAddr),
                    (RawTargetInfo.Firmware / 100.0).ToString(),
                    RawTargetInfo.PayloadPort,
                    RawTargetInfo.Available,
                    Utilities.CleanByteToString(RawTargetInfo.CurrentTitleID),
                    Utilities.CleanByteToString(RawTargetInfo.SDKVersion),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleName),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleType),
                    RawTargetInfo.Attached,
                    Utilities.CleanByteToString(RawTargetInfo.CurrentProc)));
            }

            return List;
        }

        public TargetInfo GetDefault()
        {
            DB_TargetInfo RawTargetInfo;
            Imports.TargetManagement.GetDefaultTarget(out RawTargetInfo);

            return new TargetInfo(RawTargetInfo.Default,
                    Utilities.CleanByteToString(RawTargetInfo.Name),
                    Utilities.CleanByteToString(RawTargetInfo.IPAddr),
                    (RawTargetInfo.Firmware / 100.0).ToString(),
                    RawTargetInfo.PayloadPort,
                    RawTargetInfo.Available,
                    Utilities.CleanByteToString(RawTargetInfo.CurrentTitleID),
                    Utilities.CleanByteToString(RawTargetInfo.SDKVersion),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleName),
                    Utilities.CleanByteToString(RawTargetInfo.ConsoleType),
                    RawTargetInfo.Attached,
                    Utilities.CleanByteToString(RawTargetInfo.CurrentProc));
        }

        public void SetDefault(string TargetName)
        {
            Imports.TargetManagement.SetDefault(TargetName);
        }

        public DetailedTargetInfo GetInfo(string TargetName)
        {
            DB_TargetInfo RawTargetInfo;
            Imports.TargetManagement.GetTarget(TargetName, out RawTargetInfo);

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
    }
}
