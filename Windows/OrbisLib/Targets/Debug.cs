using OrbisLib2.Common.API;
using OrbisLib2.Common.Helpers;
using System.ComponentModel;
using System.Net.Sockets;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace OrbisLib2.Targets
{
    public record LibraryInfo(long Handle, string Path, ulong MapBase, ulong TextSize, ulong MapSize, ulong DataBase, ulong dataSize);

    public class Debug
    {
        private Target Target;

        public Debug(Target Target)
        {
            this.Target = Target;
        }

        public async Task<bool> IsDebugging()
        {
            (var result, var currentTarget) = await GetCurrentProcessId();

            if (!result.Succeeded)
                return false;

            if (currentTarget == -1)
                return false;

            return true;
        }

        public async Task<ResultState> Attach(int pid)
        {
            return await API.SendCommand(Target, 1000, APICommand.ApiDbgAttach, async (Socket Sock) =>
            {
                await Sock.SendInt32Async(pid);

                return await API.GetState(Sock);
            });
        }

        public async Task<ResultState> Detach()
        {
            return await API.SendCommand(Target, 400, APICommand.ApiDbgDetach, async (Socket Sock) =>
            {
                return await API.GetState(Sock);
            });
        }

        public async Task<(ResultState, int ProcessId)> GetCurrentProcessId()
        {
            var tempProcessId = -1;
            var result = await API.SendCommand(Target, 400, APICommand.ApiDbgGetCurrent, async (Socket Sock) =>
            {
                tempProcessId = await Sock.RecvInt32Async();

                return new ResultState { Succeeded = true };
            });

            return (result, tempProcessId);
        }

        public async Task<(ResultState, ProcInfo)> GetCurrentProcess()
        {
            // Check if were debugging.
            (var result, var currentProcessId) = await GetCurrentProcessId();

            if (!result.Succeeded || currentProcessId == -1)
                return (result, null);

            // Pull the process list.
            (result, var procList) = await Target.GetProcList();

            // If for what ever reason getting the proc list fails just abort.
            if (!result.Succeeded)
                return (result, null);

            // Try to find the process in the process list and if by some reason we cant abort.
            var proc = procList.Find(x => x.ProcessId == currentProcessId);
            if (proc == null)
                return (new ResultState { Succeeded = false }, null); ;

            return (result, proc);
        }

        public async Task<(ResultState, int)> LoadLibrary(string Path)
        {
            int tempHandle = -1;

            var result = await API.SendCommand(Target, 4000, APICommand.ApiDbgLoadLibrary, async (Socket Sock) =>
            {
                if (await Sock.RecvInt32Async() != 1)
                    return new ResultState { Succeeded = false, ErrorMessage = $"The target {Target.Name} ({Target.IPAddress}) is not currently debugging any process." };
                else
                {
                    var result = await API.SendNextPacket(Sock, new SPRXPacket { Path = Path });

                    if (result.Succeeded)
                        tempHandle = await Sock.RecvInt32Async();

                    return result;
                }
            });

            return (result, tempHandle);
        }

        public async Task<ResultState> UnloadLibrary(int Handle)
        {
            return await API.SendCommand(Target, 4000, APICommand.ApiDbgUnloadLibrary, async (Socket Sock) =>
            {
                if (await Sock.RecvInt32Async() != 1)
                    return new ResultState { Succeeded = false, ErrorMessage = $"The target {Target.Name} ({Target.IPAddress}) is not currently debugging any process." };
                else
                    return await API.SendNextPacket(Sock, new SPRXPacket { Handle = Handle });
            });
        }

        public async Task<(ResultState, int)> ReloadLibrary(int Handle, string Path)
        {
            int tempHandle = -1;
            var result = await API.SendCommand(Target, 4000, APICommand.ApiDbgReloadLibrary, async (Socket Sock) =>
            {
                if (await Sock.RecvInt32Async() != 1)
                    return new ResultState { Succeeded = false, ErrorMessage = $"The target {Target.Name} ({Target.IPAddress}) is not currently debugging any process." };
                else
                {
                    var result = await API.SendNextPacket(Sock, new SPRXPacket { Path = Path, Handle = Handle });
                    tempHandle = Sock.RecvInt32();

                    return result;
                }
            });

            return (result, tempHandle);
        }

        public async Task<(ResultState, List<LibraryInfo>)> GetLibraries()
        {
            var tempLibraryList = new List<LibraryInfo>();

            var result = await API.SendCommand(Target, 400, APICommand.ApiDbgLibraryList, async (Socket Sock) =>
            {
                if (await Sock.RecvInt32Async() != 1)
                    return new ResultState { Succeeded = false, ErrorMessage = $"The target {Target.Name} ({Target.IPAddress}) is not currently debugging any process." };
                else
                {
                    var rawPacket = await Sock.ReceiveSizeAsync();
                    var Packet = LibraryListPacket.Parser.ParseFrom(rawPacket);

                    foreach(var library in Packet.Libraries)
                    {
                        tempLibraryList.Add(new LibraryInfo(library.Handle, library.Path, library.MapBase, library.MapSize, library.TextSize, library.DataBase, library.TextSize));
                    }

                    return new ResultState { Succeeded = true };
                }
            });

            return (result, tempLibraryList);
        }


        public async Task<(ResultState, byte[])> ReadMemory(ulong address, ulong length)
        {
            var tempData = new byte[length];
            var result = await API.SendCommand(Target, 1000, APICommand.ApiDbgRead, async (Socket Sock) =>
            {
                if (await Sock.RecvInt32Async() != 1)
                    return new ResultState { Succeeded = false, ErrorMessage = $"The target {Target.Name} ({Target.IPAddress}) is not currently debugging any process." };
                else
                {
                    var result = await API.SendNextPacket(Sock, new RWPacket { Address = address, Length = length });

                    if (result.Succeeded)
                        await Sock.RecvLargeAsync(tempData);

                    return result;
                }
            });

            return (result, tempData);
        }

        public async Task<ResultState> WriteMemory(ulong Address, byte[] Data)
        {
            return await API.SendCommand(Target, 2000, APICommand.ApiDbgWrite, async (Socket Sock) =>
            {
                if (await Sock.RecvInt32Async() != 1)
                    return new ResultState { Succeeded = false, ErrorMessage = $"The target {Target.Name} ({Target.IPAddress}) is not currently debugging any process." };
                else
                {
                    var result = await API.SendNextPacket(Sock, new RWPacket { Address = Address, Length = (ulong)Data.Length });

                    if (result.Succeeded)
                    {
                        await Sock.SendLargeAsync(Data);

                        result = await API.GetState(Sock);
                    }

                    return result;
                }
            });
        }
    }
}
