using H.Pipes;
using Microsoft.Extensions.Logging;
using OrbisLib2.General;

namespace OrbisLib2.Common.Dispatcher
{
    public class DispatcherClient
    {
        private static ILogger _Logger;
        private static IPipeClient<ForwardPacket> _Client;

        /// <summary>
        /// Call this to subscribe for events from the windows service.
        /// </summary>
        public static void Subscribe(ILogger logger)
        {
            _Logger = logger;
            _Client = new PipeClient<ForwardPacket>("OrbisSuite");
            _Client.MessageReceived += _Client_MessageReceived;
            _Client.ConnectAsync();
        }

        private static void _Client_MessageReceived(object? sender, H.Pipes.Args.ConnectionMessageEventArgs<ForwardPacket?> e)
        {
            _Logger.LogInformation($"Recieved Forward {e.Message.Type}");
            switch (e.Message.Type)
            {
                default:
                    Console.WriteLine("Invalid Packet...");
                    break;

                case ForwardPacket.PacketType.SerialCom:
                    // TODO:
                    break;

                case ForwardPacket.PacketType.Intercept:
                    Events.RaiseProcInterceptEvent(e.Message.SenderIPAddress);
                    break;

                case ForwardPacket.PacketType.Continue:
                    Events.RaiseProcContinueEvent(e.Message.SenderIPAddress);
                    break;

                // Process States
                case ForwardPacket.PacketType.ProcessDie:
                    Events.RaiseProcDieEvent(e.Message.SenderIPAddress);
                    break;

                case ForwardPacket.PacketType.ProcessAttach:
                    Events.RaiseProcAttachEvent(e.Message.SenderIPAddress, e.Message.ProcessId);
                    break;

                case ForwardPacket.PacketType.ProcessDetach:
                    Events.RaiseProcDetachEvent(e.Message.SenderIPAddress);
                    break;

                // Target State
                case ForwardPacket.PacketType.TargetSuspend:
                    Events.RaiseTargetSuspendEvent(e.Message.SenderIPAddress);
                    break;

                case ForwardPacket.PacketType.TargetResume:
                    Events.RaiseTargetResumeEvent(e.Message.SenderIPAddress);
                    break;

                case ForwardPacket.PacketType.TargetShutdown:
                    Events.RaiseTargetShutdownEvent(e.Message.SenderIPAddress);
                    break;

                case ForwardPacket.PacketType.TargetStateChanged:
                    Events.FireTargetStateChanged(e.Message.SenderIPAddress, e.Message.TargetStatus.PreviousState, e.Message.TargetStatus.NewState);
                    break;

                // Misc
                case ForwardPacket.PacketType.DBTouched:
                    Events.FireDBTouched();
                    break;

                case ForwardPacket.PacketType.MutableInfoUpdated:
                    // TODO: Mutable Info
                    break;
            }
        }
    }
}
