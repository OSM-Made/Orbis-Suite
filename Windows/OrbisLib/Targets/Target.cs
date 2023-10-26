using OrbisLib2.Common.API;
using OrbisLib2.Common.Database;
using OrbisLib2.Common.Helpers;
using System.Net.Sockets;

namespace OrbisLib2.Targets
{
    public record ProcInfo(int AppId, int ProcessId, string Name, string TitleId);

    public class Target
    {
        private int _SavedTargetId = 0;

        public SavedTarget SavedTarget
        {
            get
            {
                var savedTarget = SavedTarget.FindTarget(x => x.Id == _SavedTargetId);

                if (savedTarget == null)
                    savedTarget = new SavedTarget();

                return savedTarget;
            }
        }

        /// <summary>
        /// Weather or not this is our default target to be selected on start up.
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return SavedTarget.IsDefault;
            }
        }

        /// <summary>
        /// The name given to the target.
        /// </summary>
        public string Name
        {
            get
            {
                return SavedTarget.Name;
            }
        }

        /// <summary>
        /// The IP Address as a string.
        /// </summary>
        public string IPAddress
        {
            get
            {
                return SavedTarget.IPAddress;
            }
        }

        /// <summary>
        /// The port used to send payloads to the saved IP Address.
        /// </summary>
        public int PayloadPort
        {
            get
            {
                return SavedTarget.PayloadPort;
            }
        }


        public TargetInfo Info
        {
            get
            {
                return SavedTarget.Info;
            }
        }

        public Debug Debug;
        public Payload Payload;
        public FTP FTP;
        public Application Application;

        public Target(SavedTarget SavedTarget)
        {
            _SavedTargetId = SavedTarget.Id;

            Debug = new Debug(this);
            Payload = new Payload(this);
            FTP = new FTP(this);
            Application = new Application(this);
        }

        public Target Clone()
        {
            return new Target(SavedTarget.Clone());
        }

        public async Task<ResultState> Shutdown()
        {
            return await API.SendCommand(this, 400, APICommand.ApiTargetShutdown);
        }

        public async Task<ResultState> Reboot()
        {
            return await API.SendCommand(this, 400, APICommand.ApiTargetReboot);
        }

        public async Task<ResultState> Suspend()
        {
            return await API.SendCommand(this, 400, APICommand.ApiAppsSuspend);
        }

        public async Task<ResultState> Notify(string Message)
        {
            return await API.SendCommand(this, 400, APICommand.ApiTargetNotify, async (Socket Sock) =>
            {
                return await API.SendNextPacket(Sock, new TargetNotifyPacket { Message = Message });
            });
        }

        public async Task<ResultState> Notify(string IconURI, string Message)
        {
            return await API.SendCommand(this, 400, APICommand.ApiTargetNotify, async (Socket Sock) =>
            {
                return await API.SendNextPacket(Sock, new TargetNotifyPacket { IconURI = IconURI, Message = Message });
            });
        }

        public async Task<ResultState> Buzzer(BuzzerType Type)
        {
            return await API.SendCommand(this, 400, APICommand.ApiTargetBuzzer, async (Socket Sock) =>
            {
                await Sock.SendInt32Async((int)Type);

                // Set the result state of the call.
                return await API.GetState(Sock);
            });
        }

        public async Task<ResultState> SetLED(ConsoleLEDColours Colour)
        {
            return await API.SendCommand(this, 400, APICommand.ApiTargetSetLed, async (Socket Sock) =>
            {
                await Sock.SendInt32Async((int)Colour);

                // Set the result state of the call.
                return await API.GetState(Sock);
            });
        }

        public bool SetSettings(bool ShowDebugTitleIdLabel, bool ShowDevkitPanel, bool ShowDebugSettings, bool ShowAppHome)
        {
            //var result = API.SendCommand(this, 5, APICommand.ApiTargetSetSettings, (Socket Sock) =>
            //{
            //    Result = API.SendNextPacket(Sock, new TargetSettingsPacket()
            //    {
            //        ShowDebugTitleIdLabel = Convert.ToInt32(ShowDebugTitleIdLabel),
            //        ShowDevkitPanel = Convert.ToInt32(ShowDevkitPanel),
            //        ShowDebugSettings = Convert.ToInt32(ShowDebugSettings),
            //        ShowAppHome = Convert.ToInt32(ShowAppHome)
            //    });
            //});
            //
            //return result == APIResults.API_OK;

            return false;
        }

        public async Task<(ResultState, List<ProcInfo>)> GetProcList()
        {
            var result = new ResultState { Succeeded = true };
            var tempList = new List<ProcInfo>();

            try
            {
                result = await API.SendCommand(this, 400, APICommand.ApiTargetGetProcList, async (Socket Sock) =>
                {
                    var rawPacket = await Sock.ReceiveSizeAsync();
                    var packet = ProcListPacket.Parser.ParseFrom(rawPacket);

                    foreach (var process in packet.Processes)
                    {
                        tempList.Add(new ProcInfo(process.AppId, process.ProcessId, process.Name, process.TitleId));
                    }

                    return new ResultState { Succeeded = true };
                });
            }
            catch (Exception ex)
            {
                result = new ResultState { Succeeded = false, ErrorMessage = ex.Message };
            }

            return (result, tempList);
        }

        public async Task<byte[]> GetFile(string filePath)
        {
            int bytesRecieved = 0;
            var file = new byte[0];
            await API.SendCommand(this, 400, APICommand.ApiTargetSendFile, async (Socket Sock) =>
            {
                var result = await API.SendNextPacket(Sock, new FilePacket { FilePath = filePath });

                if (!result.Succeeded)
                    return result;

                var fileSize = await Sock.RecvInt32Async();
                file = new byte[fileSize];
                bytesRecieved = await Sock.RecvLargeAsync(file);

                return result;
            });

            return bytesRecieved > 0 ? file : new byte[0];
        }

        public async Task<ResultState> SendFile(byte[] data, string filePath)
        {
            return await API.SendCommand(this, 400, APICommand.ApiTargetRecieveFile, async (Socket Sock) =>
            {
                var result = await API.SendNextPacket(Sock, new FilePacket { FilePath = filePath });

                if (!result.Succeeded)
                    return result;

                // Send the file.
                Sock.SendSize(data);

                return result;
            });
        }

        public async Task<ResultState> DeleteFile(string filePath)
        {
            return await API.SendCommand(this, 400, APICommand.ApiTargetDeleteFile, async (Socket Sock) =>
            {
                return await API.SendNextPacket(Sock, new FilePacket { FilePath = filePath });
            });
        }
    }
}
