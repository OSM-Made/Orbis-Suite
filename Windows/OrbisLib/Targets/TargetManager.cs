using OrbisLib2.Common.API;
using OrbisLib2.Common.Database;
using OrbisLib2.Common.Database.Types;
using OrbisLib2.Common.Helpers;
using OrbisLib2.General;
using System.Linq.Expressions;

namespace OrbisLib2.Targets
{
    public class TargetManager
    {

        /// <summary>
        /// Returns a list of the Targets saved.
        /// </summary>
        public static List<Target> Targets
        {
            get
            {
                var temporaryList = new List<Target>();
                var savedTargets = SavedTarget.GetTargetList();

                if (savedTargets == null)
                    return temporaryList;

                foreach (var target in savedTargets)
                {
                    temporaryList.Add(new Target(target));
                }

                return temporaryList;
            }
        }

        private static Target _SelectedTarget;

        /// <summary>
        /// Gets the currently selected target.
        /// </summary>
        public static Target SelectedTarget
        {
            get
            {
                // Set initially as the default target.
                if (_SelectedTarget == null)
                {
                    var defaultTarget = SavedTarget.FindDefaultTarget();

                    if (defaultTarget != null)
                    {
                        _SelectedTarget = new Target(defaultTarget);
                    }
                }

                return _SelectedTarget;
            }
            set 
            { 
                _SelectedTarget = value;
                Events.FireSelectedTargetChanged(_SelectedTarget.Name);
            }
        }

        /// <summary>
        /// Gets the specified target by name.
        /// </summary>
        /// <param name="TargetName">The specified target name.</param>
        /// <returns>Returns true if target is found.</returns>
        public static Target? GetTarget(string TargetName)
        {
            var saveedTarget = SavedTarget.FindTarget(x => x.Name == TargetName);

            if (saveedTarget == null)
            {
                return null;
            }

            return new Target(saveedTarget);
        }

        /// <summary>
        /// Gets the specified target by predicate.
        /// </summary>
        /// <param name="predicate">The predicate to be used to find the target.</param>
        /// <returns></returns>
        public static Target? GetTarget(Expression<Func<SavedTarget, bool>> predicate)
        {
            var saveedTarget = SavedTarget.FindTarget(predicate);

            if (saveedTarget == null)
            {
                return null;
            }

            return new Target(saveedTarget);
        }

        /// <summary>
        /// Deletes the specified target.
        /// </summary>
        /// <param name="TargetName">The specified target name.</param>
        /// <returns>Returns true if the operation was successful.</returns>
        public static bool DeleteTarget(string TargetName)
        {
            var Target = SavedTarget.FindTarget(x => x.Name == TargetName);
            return Target.Remove();
        }

        /// <summary>
        /// Adds a new target.
        /// </summary>
        /// <param name="Default">If this target the new default.</param>
        /// <param name="TargetName">The mame for this target.</param>
        /// <param name="IPAddress">IP Address of this target.</param>
        /// <param name="PayloadPort">The payload port to be used for this target.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool NewTarget(bool Default, string TargetName, string IPAddress, int PayloadPort)
        {
            return new SavedTarget { IsDefault = Default, Name = TargetName, IPAddress = IPAddress, PayloadPort = PayloadPort }.Add();
        }

        /// <summary>
        /// Updates extended information about the Target. *Requires the Target to be on and the API running.
        /// </summary>
        /// <param name="savedTarget">The identifier of the target to update.</param>
        /// <returns>Returns weather or not the action was successful or not.</returns>
        public static ResultState UpdateTargetInfo(SavedTarget savedTarget)
        {
            if (!savedTarget.Info.IsAPIAvailable)
            {
                savedTarget.Info.CPUTemp = 0;
                savedTarget.Info.SOCTemp = 0;
                savedTarget.Info.ThreadCount = 0;
                savedTarget.Info.AverageCPUUsage = 0;
                savedTarget.Info.BusyCore = 0;
                savedTarget.Info.RamUsage = 0;
                savedTarget.Info.VRamUsage = 0;
                savedTarget.Info.BigAppTitleID = "-";
                savedTarget.Info.Save();
                return new ResultState { Succeeded = true };
            }

            var Target = GetTarget(savedTarget.Name);
            if (Target == null)
                return new ResultState { Succeeded = false, ErrorMessage = $"Couldn't Find Target \"{savedTarget.Name}\"." };

            return API.SendCommand(Target, 5, APICommand.ApiTargetInfo, async (Sock) =>
            {
                var rawPacket = Sock.ReceiveSize();
                var Packet = TargetInfoPacket.Parser.ParseFrom(rawPacket);

                if (Packet == null)
                {
                    return new ResultState { Succeeded = false, ErrorMessage = $"Protobuf packet was null." };
                }

                savedTarget.Info.SDKVersion = $"{(Packet.SDKVersion >> 24 & 0xFF).ToString("X1")}.{(Packet.SDKVersion >> 12 & 0xFFF).ToString("X3")}.{(Packet.SDKVersion & 0xFFF).ToString("X3")}";
                savedTarget.Info.SoftwareVersion = $"{(Packet.SoftwareVersion >> 24 & 0xFF).ToString("X1")}.{(Packet.SoftwareVersion >> 16 & 0xFF).ToString("X2")}";
                savedTarget.Info.FactorySoftwareVersion = $"{(Packet.FactorySoftwareVersion >> 24 & 0xFF).ToString("X1")}.{(Packet.FactorySoftwareVersion >> 12 & 0xFFF).ToString("X3")}.{(Packet.FactorySoftwareVersion & 0xFFF).ToString("X3")}";
                savedTarget.Info.BigAppPid = Packet.BigApp.Pid;
                savedTarget.Info.BigAppProcessName = Packet.BigApp.Name;
                savedTarget.Info.BigAppTitleID = Packet.BigApp.TitleId;
                savedTarget.Info.ConsoleName = Packet.ConsoleName;
                savedTarget.Info.MotherboardSerial = Packet.MotherboardSerial;
                savedTarget.Info.Serial = Packet.Serial;
                savedTarget.Info.Model = Packet.Model;
                savedTarget.Info.MACAddressLAN = Packet.MACAddressLAN.ToUpper();
                savedTarget.Info.MACAddressWIFI = Packet.MACAddressWIFI.ToUpper();
                savedTarget.Info.UART = Packet.UART;
                savedTarget.Info.IDUMode = Packet.IDUMode;
                savedTarget.Info.IDPS = Packet.IDPS;
                savedTarget.Info.PSID = Packet.PSID;
                savedTarget.Info.ConsoleType = (ConsoleType)Packet.ConsoleType;

                // Debugging.
                savedTarget.Info.IsAttached = Packet.Attached > 0;
                // TODO: Implement this into the API.
                savedTarget.Info.CurrentProcessId = 0;// TODO: Update this to process Id Packet.CurrentProc;

                // Misc
                savedTarget.Info.ForegroundAccountId = Packet.ForegroundAccountId;

                // Storage.
                savedTarget.Info.HDDUsedSpace = (long)(Packet.TotalSpace - Packet.FreeSpace);
                savedTarget.Info.HDDFreeSpace = (long)Packet.FreeSpace;
                savedTarget.Info.HDDTotalSpace = (long)Packet.TotalSpace;

                // Perf Stats.
                savedTarget.Info.CPUTemp = Packet.CPUTemp;
                savedTarget.Info.SOCTemp = Packet.SOCTemp;
                savedTarget.Info.ThreadCount = Packet.ThreadCount;
                savedTarget.Info.AverageCPUUsage = Packet.AverageCPUUsage;
                savedTarget.Info.BusyCore = Packet.BusyCore;
                if (Packet.Ram != null)
                    savedTarget.Info.RamUsage = Packet.Ram.Used;
                if (Packet.VRam != null)
                    savedTarget.Info.VRamUsage = Packet.VRam.Used;

                // Save the updated info.
                savedTarget.Info.Save();

                return new ResultState { Succeeded = true };
            }).Result;
        }
    }
}
