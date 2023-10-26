using OrbisLib2.Targets;

namespace OrbisLib2.General
{
    public class ProcInterceptEvent : EventArgs
    {
        public Target SendingTarget { get; private set; }

        public ProcInterceptEvent(Target SendingTarget)
        {
            this.SendingTarget = SendingTarget;
        }
    }

    public class ProcContinueEvent : EventArgs
    {
        public Target SendingTarget { get; private set; }

        public ProcContinueEvent(Target SendingTarget)
        {
            this.SendingTarget = SendingTarget;
        }
    }

    public class ProcDieEvent : EventArgs
    {
        public Target SendingTarget { get; private set; }

        public ProcDieEvent(Target SendingTarget)
        {
            this.SendingTarget = SendingTarget;
        }
    }

    public class ProcAttachEvent : EventArgs
    {
        public Target SendingTarget { get; private set; }

        public int NewProcessId { get; private set; }

        public ProcAttachEvent(Target SendingTarget, int NewProcessId)
        {
            this.SendingTarget = SendingTarget;
            this.NewProcessId = NewProcessId;
        }
    }

    public class ProcDetachEvent : EventArgs
    {
        public Target SendingTarget { get; private set; }

        public ProcDetachEvent(Target SendingTarget)
        {
            this.SendingTarget = SendingTarget;
        }
    }

    public class TargetSuspendEvent : EventArgs
    {
        public Target SendingTarget { get; private set; }

        public TargetSuspendEvent(Target SendingTarget)
        {
            this.SendingTarget = SendingTarget;
        }
    }

    public class TargetResumeEvent : EventArgs
    {
        public Target SendingTarget { get; private set; }

        public TargetResumeEvent(Target SendingTarget)
        {
            this.SendingTarget = SendingTarget;
        }
    }

    public class TargetShutdownEvent : EventArgs
    {
        public Target SendingTarget { get; private set; }

        public TargetShutdownEvent(Target SendingTarget)
        {
            this.SendingTarget = SendingTarget;
        }
    }

    public class DBTouchedEvent : EventArgs
    {
        public DBTouchedEvent() { }
    }

    public class TargetStateChangedEvent : EventArgs
    {
        public enum TargetState
        {
            None,
            Available,
            UnAvailable,
            APIAvailable,
            APIUnAvailable,
        };

        public TargetState State { get; private set; }

        public string Name { get; private set; }

        public TargetStateChangedEvent(string Name, TargetState State)
        {
            this.Name = Name;
            this.State = State;
        }
    }

    public class SelectedTargetChangedEvent : EventArgs
    {
        public string Name { get; private set; }

        public SelectedTargetChangedEvent(string Name) 
        {
            this.Name = Name;
        }
    }

    public class Events
    {
        /// <summary>
        /// Event fired when the current proccess the debugger is attached to thows an exception.
        /// </summary>
        public static event EventHandler<ProcInterceptEvent> ProcIntercept;

        /// <summary>
        /// Event fired when the current proccess the debugger is attached to continues.
        /// </summary>
        public static event EventHandler<ProcContinueEvent> ProcContinue;

        /// <summary>
        /// Event fired when the current proccess the debugger is attached to is dying.
        /// </summary>
        public static event EventHandler<ProcDieEvent> ProcDie;

        /// <summary>
        /// Event fired when the current proccess the debugger is attached to has changed.
        /// </summary>
        public static event EventHandler<ProcAttachEvent> ProcAttach;

        /// <summary>
        /// Event fired when debugging of the last attached process has stopped.
        /// </summary>
        public static event EventHandler<ProcDetachEvent> ProcDetach;

        /// <summary>
        /// Event fired when the corisponding target has changed states to suspend.
        /// </summary>
        public static event EventHandler<TargetSuspendEvent> TargetSuspend;

        /// <summary>
        /// Event fired when the corisponding target has changed states to resume.
        /// </summary>
        public static event EventHandler<TargetResumeEvent> TargetResume;

        /// <summary>
        /// Event fired when the corisponding target has changed states to shutdown.
        /// </summary>
        public static event EventHandler<TargetShutdownEvent> TargetShutdown;

        /// <summary>
        /// The DBTouched Event gets invoked when the Database used to store target specific info is changed.
        /// </summary>
        public static event EventHandler<DBTouchedEvent>? DBTouched;

        /// <summary>
        /// Event is fired when ever the state of the target changes.
        /// </summary>
        public static event EventHandler<TargetStateChangedEvent>? TargetStateChanged;

        /// <summary>
        /// Event thrown when ever the currently selected target has changed.
        /// </summary>
        public static event EventHandler<SelectedTargetChangedEvent>? SelectedTargetChanged;

        /// <summary>
        /// Fires an event for when the Target's proccess were attached to has reached an intercepted state.
        /// </summary>
        /// <param name="IPAddr">The sending Target Address.</param>
        internal static void RaiseProcInterceptEvent(string IPAddr)
        {
            var sendingTarget = TargetManager.GetTarget(x => x.IPAddress == IPAddr);
            if(sendingTarget != null)
            {
                ProcIntercept?.Invoke(null, new ProcInterceptEvent(sendingTarget));
            }
        }

        /// <summary>
        /// Fires an event when a procees on the Target has gotten the signal to continue execution.
        /// </summary>
        /// <param name="IPAddr">The sending Target Address.</param>
        internal static void RaiseProcContinueEvent(string IPAddr)
        {
            var sendingTarget = TargetManager.GetTarget(x => x.IPAddress == IPAddr);
            if (sendingTarget != null)
            {
                ProcContinue?.Invoke(null, new ProcContinueEvent(sendingTarget));
            }
        }

        /// <summary>
        /// Fires an event for when a proccess is going to be shutting down on the Target.
        /// </summary>
        /// <param name="IPAddr">The sending Target Address.</param>
        internal static void RaiseProcDieEvent(string IPAddr)
        {
            var sendingTarget = TargetManager.GetTarget(x => x.IPAddress == IPAddr);
            if (sendingTarget != null)
            {
                ProcDie?.Invoke(null, new ProcDieEvent(sendingTarget));
            }
        }

        /// <summary>
        /// Fires an event for when the OrbisLib API attaches to a process.
        /// </summary>
        /// <param name="IPAddr">The sending Target Address.</param>
        /// <param name="NewProcName">The name of the process were attaching to.</param>
        internal static void RaiseProcAttachEvent(string IPAddr, int NewProcessId)
        {
            var sendingTarget = TargetManager.GetTarget(x => x.IPAddress == IPAddr);
            if (sendingTarget != null)
            {
                ProcAttach?.Invoke(null, new ProcAttachEvent(sendingTarget, NewProcessId));
            }
        }

        /// <summary>
        /// Fires an event for when the OrbisLib API detaches to a process.
        /// </summary>
        /// <param name="IPAddr">The sending Target Address.</param>
        internal static void RaiseProcDetachEvent(string IPAddr)
        {
            var sendingTarget = TargetManager.GetTarget(x => x.IPAddress == IPAddr);
            if (sendingTarget != null)
            {
                ProcDetach?.Invoke(null, new ProcDetachEvent(sendingTarget));
            }
        }

        /// <summary>
        /// Fires event for when Target is entering the suspended state.
        /// </summary>
        /// <param name="IPAddr">The sending Target Address.</param>
        internal static void RaiseTargetSuspendEvent(string IPAddr)
        {
            var sendingTarget = TargetManager.GetTarget(x => x.IPAddress == IPAddr);
            if (sendingTarget != null)
            {
                TargetSuspend?.Invoke(null, new TargetSuspendEvent(sendingTarget));
            }
        }

        /// <summary>
        /// Fires event for when the Target is resuming from a suspended state.
        /// </summary>
        /// <param name="IPAddr">The sending Target Address.</param>
        internal static void RaiseTargetResumeEvent(string IPAddr)
        {
            var sendingTarget = TargetManager.GetTarget(x => x.IPAddress == IPAddr);
            if (sendingTarget != null)
            {
                TargetResume?.Invoke(null, new TargetResumeEvent(sendingTarget));
            }
        }

        /// <summary>
        /// Fires an event for when the Target is shutting down.
        /// </summary>
        /// <param name="IPAddr">The sending Target Address.</param>
        internal static void RaiseTargetShutdownEvent(string IPAddr)
        {
            var sendingTarget = TargetManager.GetTarget(x => x.IPAddress == IPAddr);
            if (sendingTarget != null)
            {
                TargetShutdown?.Invoke(null, new TargetShutdownEvent(sendingTarget));
            }
        }

        /// <summary>
        /// Will Fire the event for when the Database has been updated.
        /// </summary>
        internal static void FireDBTouched()
        {
            DBTouched?.Invoke(null, new DBTouchedEvent());
        }

        /// <summary>
        /// Will Fire the event for Target Availability.
        /// </summary>
        /// <param name="Available"></param>
        /// <param name="TargetName"></param>
        internal static void FireTargetAvailability(bool Available, string TargetName)
        {
            if (Available)
                TargetStateChanged?.Invoke(null, new TargetStateChangedEvent(TargetName, TargetStateChangedEvent.TargetState.Available));
            else
                TargetStateChanged?.Invoke(null, new TargetStateChangedEvent(TargetName, TargetStateChangedEvent.TargetState.UnAvailable));
        }

        /// <summary>
        /// Will Fire the event for Target API Availability.
        /// </summary>
        /// <param name="Available"></param>
        /// <param name="TargetName"></param>
        internal static void FireTargetAPIAvailability(bool Available, string TargetName)
        {
            if (Available)
                TargetStateChanged?.Invoke(null, new TargetStateChangedEvent(TargetName, TargetStateChangedEvent.TargetState.APIAvailable));
            else
                TargetStateChanged?.Invoke(null, new TargetStateChangedEvent(TargetName, TargetStateChangedEvent.TargetState.APIUnAvailable));
        }

        /// <summary>
        /// Will fire the event whent the selected target changes.
        /// </summary>
        /// <param name="Name">The name of the target.</param>
        internal static void FireSelectedTargetChanged(string Name)
        {
            SelectedTargetChanged?.Invoke(null, new SelectedTargetChangedEvent(Name));
        }
    }
}
