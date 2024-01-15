using OrbisLib2.Common;
using OrbisLib2.Common.API;
using OrbisLib2.Common.Database.App;
using OrbisLib2.Common.Helpers;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace OrbisLib2.Targets
{
    public class Application
    {
        public enum VisibilityType : int
        {
            VT_NONE,
            VT_VISIBLE,
            VT_INVISIBLE,
        };

        private Target Target;

        public Application(Target Target)
        {
            this.Target = Target;
        }

        public string GetAppDBPath()
        {
            var foregroundAccountId = Target.MutableInfo.ForegroundAccountId;
            if (foregroundAccountId <= 0)
                return string.Empty;

            // Create the db cache folder if it does not exist.
            var dbCachePath = @$"{Config.OrbisPath}\DBCache";
            if (!Directory.Exists(dbCachePath))
            {
                Directory.CreateDirectory(dbCachePath);
            }

            // create a folder for this target if it does not exist yet.
            var targetFolder = @$"{dbCachePath}\{Target.StaticInfo.MACAddressLAN.Replace(":", "-")}";
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            return @$"{targetFolder}\app.db";
        }

        public async Task<(ResultState, bool)> IsOutOfDate()
        {
            bool tempIsOutOfDate = false;
            var databasePath = GetAppDBPath();

            if (!File.Exists(databasePath))
            {
                return (new ResultState { Succeeded = true, ErrorMessage = "Database file does not exist." }, true);
            }

            var result = await API.SendCommand(Target, 400, APICommand.ApiAppsCheckVer, async (Socket Sock) =>
            {
                // Send the current app version.
                Sock.SendInt32(AppBrowseVersion.GetAppBrowseVersion(databasePath));

                // Get the state from API.
                var tempIsOutOfDate = await Sock.RecvInt32Async() == 1;

                return new ResultState { Succeeded = true };
            });

            return (result, tempIsOutOfDate);
        }

        public async Task<ResultState> UpdateLocalDB()
        {
            (var result, var isOutOfDate) = await IsOutOfDate();

            // If the out of date check failed we need to abort.
            if (!result.Succeeded)
                return result;

            // If the DB is up to date we have nothing to do here, lets get out of here!
            if (!isOutOfDate)
                return new ResultState { Succeeded = true };

            return await API.SendCommand(Target, 400, APICommand.ApiAppsGetDb, async (Socket Sock) =>
            {
                var fileSize = await Sock.RecvInt32Async();
                var newDatabaseBytes = new byte[fileSize];
                if (await Sock.RecvLargeAsync(newDatabaseBytes) < fileSize)
                    return new ResultState { Succeeded = false, ErrorMessage = "Failed to recieve the data." };

                var databasePath = GetAppDBPath();
                var oldDatabasePath = @$"{databasePath}.old";

                // If we already have a db back it up.
                if (File.Exists(databasePath))
                {
                    File.Copy(databasePath, oldDatabasePath, true);

                    // Remove the last db
                    File.Delete(databasePath);
                }

                // Write the new DB.
                File.WriteAllBytes(databasePath, newDatabaseBytes);

                return new ResultState { Succeeded = true };
            });
        }

        public async Task<List<AppBrowse>> GetAppListAsync()
        {
            var databasePath = GetAppDBPath();

            // Update the DB if needed.
            await UpdateLocalDB();

            // Make sure we actually have a DB now.
            if (!File.Exists(databasePath))
            {
                return new List<AppBrowse>();
            }

            return AppBrowse.GetAppBrowseList(databasePath, Target.MutableInfo.ForegroundAccountId);
        }

        public string GetAppInfoString(string TitleId, string Key)
        {
            var databasePath = GetAppDBPath();

            if (!File.Exists(databasePath))
            {
                return string.Empty;
            }

            return AppInfo.GetStringFromAppInfo(databasePath, TitleId, Key);
        }

        public async Task<(ResultState, AppState State)> GetAppState(string TitleId)
        {
            var tempAppState = AppState.StateNotRunning;
            if (!Regex.IsMatch(TitleId, @"[a-zA-Z]{4}\d{5}"))
            {
                return (new ResultState { Succeeded = false, ErrorMessage = $"Invaild titleId format {TitleId}" }, tempAppState);
            }

            var result = await API.SendCommand(Target, 400, APICommand.ApiAppsStatus, async (Socket Sock) => 
            {
                var result = await API.SendNextPacket(Sock, new AppPacket { TitleId = TitleId });

                // Get the state from API.
                if (result.Succeeded)
                    tempAppState = (AppState)await Sock.RecvInt32Async();

                return result;
            });

            return (result, tempAppState);
        }

        public async Task<ResultState> Start(string TitleId)
        {
            if (!Regex.IsMatch(TitleId, @"[a-zA-Z]{4}\d{5}"))
                return new ResultState { Succeeded = false, ErrorMessage = $"Invaild titleId format {TitleId}" };

            return await API.SendCommand(Target, 400, APICommand.ApiAppsStart, async (Socket Sock) => 
            {
                return await API.SendNextPacket(Sock, new AppPacket { TitleId = TitleId });
            });
        }

        public async Task<ResultState> Stop(string TitleId)
        {
            if (!Regex.IsMatch(TitleId, @"[a-zA-Z]{4}\d{5}"))
                return new ResultState { Succeeded = false, ErrorMessage = $"Invaild titleId format {TitleId}" };

            return await API.SendCommand(Target, 400, APICommand.ApiAppsStop, async (Socket Sock) =>
            {
                return await API.SendNextPacket(Sock, new AppPacket { TitleId = TitleId });
            });
        }

        public async Task<ResultState> Suspend(string TitleId)
        {
            if (!Regex.IsMatch(TitleId, @"[a-zA-Z]{4}\d{5}"))
                return new ResultState { Succeeded = false, ErrorMessage = $"Invaild titleId format {TitleId}" };

            return await API.SendCommand(Target, 400, APICommand.ApiAppsSuspend, async (Socket Sock) =>
            {
                return await API.SendNextPacket(Sock, new AppPacket { TitleId = TitleId });
            });
        }

        public async Task<ResultState> Resume(string TitleId)
        {
            if (!Regex.IsMatch(TitleId, @"[a-zA-Z]{4}\d{5}"))
                return new ResultState { Succeeded = false, ErrorMessage = $"Invaild titleId format {TitleId}" };

            return await API.SendCommand(Target, 400, APICommand.ApiAppsResume, (Socket Sock) =>
            {
                return API.SendNextPacket(Sock, new AppPacket { TitleId = TitleId });
            });
        }

        public async Task<ResultState> Delete(string TitleId)
        {
            if (!Regex.IsMatch(TitleId, @"[a-zA-Z]{4}\d{5}"))
                return new ResultState { Succeeded = false, ErrorMessage = $"Invaild titleId format {TitleId}" };

            return await API.SendCommand(Target, 5, APICommand.ApiAppsDelete, async (Socket Sock) =>
            {
                return await API.SendNextPacket(Sock, new AppPacket { TitleId = TitleId });
            });
        }

        public async Task<ResultState> SetVisibility(string TitleId, VisibilityType Visibility)
        {
            if (!Regex.IsMatch(TitleId, @"[a-zA-Z]{4}\d{5}"))
                return new ResultState { Succeeded = false, ErrorMessage = $"Invaild titleId format {TitleId}" };

            return await API.SendCommand(Target, 400, APICommand.ApiAppsSetVisibility, async (Socket Sock) =>
            {
                var result = await API.SendNextPacket(Sock, new AppPacket { TitleId = TitleId });

                if (result.Succeeded)
                {
                    // Send the visibility state.
                    await Sock.SendInt32Async((int)Visibility);

                    result = await API.GetState(Sock);
                }

                return result;
            });
        }

        public async Task<(ResultState, VisibilityType)> GetVisibility(string TitleId)
        {
            var tempType = VisibilityType.VT_NONE;

            if (!Regex.IsMatch(TitleId, @"[a-zA-Z]{4}\d{5}"))
            {
                return (new ResultState { Succeeded = false, ErrorMessage = $"Invaild titleId format {TitleId}" }, tempType);
            }

            
            var result = await API.SendCommand(Target, 400, APICommand.ApiAppsGetVisibility, async (Socket Sock) =>
            {
                var result = await API.SendNextPacket(Sock, new AppPacket { TitleId = TitleId });

                // Get the state from API.
                if (result.Succeeded)
                    tempType = (VisibilityType)await Sock.RecvInt32Async();

                return result;
            });

            return (result, tempType);
        }
    }
}
