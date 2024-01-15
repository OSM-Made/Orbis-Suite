using OrbisLib2.Common.Database;
using OrbisLib2.Common.Database.Types;
using OrbisLib2.Common.Dispatcher;
using OrbisLib2.Common.Helpers;
using OrbisLib2.Targets;

namespace OrbisSuiteService.Service
{
    public class TargetWatcher
    {
        private Dispatcher _dispatcher;

        public TargetWatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            Task.Run(() => DoTargetWatcher());
        }

        private async Task DoTargetWatcher()
        {
            while (true)
            {
                if(TargetManager.Targets.Count <= 0)
                {
                    await Task.Delay(1000);
                }

                Parallel.ForEach(TargetManager.Targets, async Target =>
                {
                    var previousState = Target.MutableInfo.Status;
                    var pingable = Sockets.PingHost(Target.IPAddress);
                    var apiPingable = Sockets.TestTcpConnection(Target.IPAddress, Settings.CreateInstance().APIPort);

                    if (!pingable && !apiPingable)
                    {
                        Target.MutableInfo.UpdateStatus(TargetStatusType.Offline);
                    }
                    else if (pingable && !apiPingable)
                    {
                        Target.MutableInfo.UpdateStatus(TargetStatusType.Online);
                    }
                    else if (apiPingable)
                    {
                        if (await Target.Debug.IsDebugging())
                        {
                            Target.MutableInfo.UpdateStatus(TargetStatusType.DebuggingActive);
                        }
                        else
                        {
                            Target.MutableInfo.UpdateStatus(TargetStatusType.APIAvailable);
                        }
                    }

                    // Depending on our current state check for a state change.
                    //switch (Target.MutableInfo.Status)
                    //{
                    //    case TargetStatusType.None:
                    //    case TargetStatusType.Offline:
                    //
                    //        if ()
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.Online);
                    //        else
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.Offline);
                    //
                    //        break;
                    //
                    //    case TargetStatusType.Online:
                    //
                    //        if (!Sockets.PingHost(Target.IPAddress))
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.Offline);
                    //
                    //        if (Sockets.TestTcpConnection(Target.IPAddress, Settings.CreateInstance().APIPort))
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.APIAvailable);
                    //
                    //        break;
                    //
                    //    case TargetStatusType.APIAvailable:
                    //
                    //        if (!Sockets.PingHost(Target.IPAddress))
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.Offline);
                    //
                    //        if (!Sockets.TestTcpConnection(Target.IPAddress, Settings.CreateInstance().APIPort))
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.Online);
                    //
                    //        if (await Target.Debug.IsDebugging())
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.DebuggingActive);
                    //
                    //        break;
                    //
                    //    case TargetStatusType.DebuggingActive:
                    //
                    //        if (!Sockets.PingHost(Target.IPAddress))
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.Offline);
                    //
                    //        if (!Sockets.TestTcpConnection(Target.IPAddress, Settings.CreateInstance().APIPort))
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.Online);
                    //
                    //        if (!await Target.Debug.IsDebugging())
                    //            Target.MutableInfo.UpdateStatus(TargetStatusType.APIAvailable);
                    //
                    //        break;
                    //
                    //}

                    // A state change occured fire the event!
                    if (previousState != Target.MutableInfo.Status)
                    {
                        var statePacket = new ForwardPacket(ForwardPacket.PacketType.TargetStateChanged, Target.IPAddress);
                        statePacket.TargetStatus.PreviousState = previousState;
                        statePacket.TargetStatus.NewState = Target.MutableInfo.Status;
                        _dispatcher.PublishEvent(statePacket);
                    }

                    // Update Mutable Information
                    var result = await Target.UpdateMutableInfo();
                    if (result.Succeeded)
                    {
                        var mutableInfoPacket = new ForwardPacket(ForwardPacket.PacketType.MutableInfoUpdated, Target.IPAddress);
                        _dispatcher.PublishEvent(mutableInfoPacket);
                    }

                    // Update Static Information if not set.
                    await Target.UpdateStaticInfo();
                });

                await Task.Delay(500);
            }
        }
    }
}
