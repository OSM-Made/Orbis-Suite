namespace OrbisLib2.Common.Dispatcher
{
    [Serializable]
    internal sealed class DispatcherClientPacket
    {
        public enum PacketType
        {
            None,
            NewClient,
            RemoveClient,
            HeartBeat
        };

        public PacketType Type { get; set; }
        public string? ClientName { get; set; }
        public int Port { get; set; }

        public DispatcherClientPacket() { }

        public DispatcherClientPacket(PacketType Type, string ClientName, int Port)
        {
            this.Type = Type;
            this.ClientName = ClientName;
            this.Port = Port;
        }
    }

    [Serializable]
    public class TitleChange
    {
        public string? TitleID { get; set; }
    }

    [Serializable]
    public class Print
    {
        public string? Sender { get; set; }
        public string? Data { get; set; }
    }

    [Serializable]
    public class SerialCom
    {
        public byte[]? Data { get; set; }
    }

    [Serializable]
    public class Break
    {
        public int Reason { get; set; }

        //Regiseters
    }

    [Serializable]
    public class TargetAvailability
    {
        public bool Available { get; set; }

        public string? Name { get; set; }
    }

    [Serializable]
    public class ForwardPacket
    {
        public enum PacketType
        {
            None,

            // Debugging
            Print,
            SerialCom,
            Intercept,
            Continue,

            // Process States
            ProcessDie,
            ProcessAttach,
            ProcessDetach,

            // Target State
            TargetSuspend,
            TargetResume,
            TargetShutdown,
            TargetAvailability,
            TargetAPIAvailability,

            // Misc
            DBTouched,
        };

        /// <summary>
        /// The event the packet is firing for.
        /// </summary>
        public PacketType Type { get; set; }

        /// <summary>
        /// The Target's IP Address which the Event belongs to.
        /// </summary>
        public string? SenderIPAddress { get; set; }

        /// <summary>
        /// The process id for the event triggered.
        /// </summary>
        public int ProcessId { get; set; }

        public TitleChange? TitleChange { get; set; }

        public Print? Print { get; set; }

        public SerialCom? SerialCom { get; set; }

        public Break? Break { get; set; }

        public TargetAvailability? TargetAvailability { get; set; }

        public TargetAvailability? TargetAPIAvailability { get; set; }

        public ForwardPacket(PacketType Type, string SenderIPAddress)
        {
            this.Type = Type;
            this.SenderIPAddress = SenderIPAddress;

            //TODO: Maybe add logic to the getter/setter so that it will create these...
            TitleChange = new TitleChange();
            Print = new Print();
            SerialCom = new SerialCom();
            Break = new Break();
            TargetAvailability = new TargetAvailability();
            TargetAPIAvailability = new TargetAvailability();
        }
    }
}
