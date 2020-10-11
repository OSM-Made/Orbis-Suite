﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class TargetEvents
    {
        private Target Target;

        public event EventHandler<ProcPrintEvent> ProcPrint;
        public event EventHandler<ProcInterceptEvent> ProcIntercept;
        public event EventHandler<ProcContinueEvent> ProcContinue;
        public event EventHandler<ProcDieEvent> ProcDie;
        public event EventHandler<ProcAttachEvent> ProcAttach;
        public event EventHandler<ProcDetachEvent> ProcDetach;
        public event EventHandler<TargetSuspendEvent> TargetSuspend;
        public event EventHandler<TargetResumeEvent> TargetResume;
        public event EventHandler<TargetShutdownEvent> TargetShutdown;
        public event EventHandler<TargetNewTitleEvent> TargetNewTitle;

        public TargetEvents(Target Target)
        {
            this.Target = Target;
        }

        internal void RaiseProcPrintEvent(string IPAddr, int Type, int Len, string Data)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
                ProcPrint?.Invoke(null, new ProcPrintEvent(Type, Len, Data));
        }

        internal void RaiseProcInterceptEvent(string IPAddr, int Reason, IntPtr Registers)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
            {
                OrbisLib.registers reg = (OrbisLib.registers)Marshal.PtrToStructure(Registers, typeof(OrbisLib.registers));

                ProcIntercept?.Invoke(null, new ProcInterceptEvent(Reason, reg));
            }
        }

        internal void RaiseProcContinueEvent(string IPAddr)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
                ProcContinue?.Invoke(null, new ProcContinueEvent());
        }

        internal void RaiseProcDieEvent(string IPAddr)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
                ProcDie?.Invoke(null, new ProcDieEvent());
        }

        internal void RaiseProcAttachEvent(string IPAddr, string NewProcName)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
                ProcAttach?.Invoke(null, new ProcAttachEvent(NewProcName));
        }

        internal void RaiseProcDetachEvent(string IPAddr)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
                ProcDetach?.Invoke(null, new ProcDetachEvent());
        }

        internal void RaiseTargetSuspendEvent(string IPAddr)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
                TargetSuspend?.Invoke(null, new TargetSuspendEvent());
        }

        internal void RaiseTargetResumeEvent(string IPAddr)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
                TargetResume?.Invoke(null, new TargetResumeEvent());
        }

        internal void RaiseTargetShutdownEvent(string IPAddr)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
                TargetShutdown?.Invoke(null, new TargetShutdownEvent());
        }

        internal void RaiseTargetNewTitleEvent(string IPAddr, string TitleID)
        {
            if (IPAddr.Equals(Target.Info.IPAddr))
                TargetNewTitle?.Invoke(null, new TargetNewTitleEvent(TitleID));
        }
    }

    public class ProcPrintEvent : EventArgs
    {
        public int Type { get; private set; }
        public int Len { get; private set; }
        public string Data { get; private set; }

        public ProcPrintEvent(int Type, int Len, string Data)
        {
            this.Type = Type;
            this.Len = Len;
            this.Data = Data;
        }
    }

    public class ProcInterceptEvent : EventArgs
    {
        public int Reason { get; private set; }
        public OrbisLib.registers reg { get; private set; }

        public ProcInterceptEvent(int Reason, OrbisLib.registers reg)
        {
            this.Reason = Reason;
            this.reg = reg;
        }
    }

    public class ProcContinueEvent : EventArgs
    {
        public ProcContinueEvent() { }
    }

    public class ProcDieEvent : EventArgs
    {
        public ProcDieEvent() { }
    }

    public class ProcAttachEvent : EventArgs
    {
        public string NewProcName { get; private set; }

        public ProcAttachEvent(string NewProcName)
        {
            this.NewProcName = NewProcName;
        }
    }

    public class ProcDetachEvent : EventArgs
    {
        public ProcDetachEvent() { }
    }

    public class TargetSuspendEvent : EventArgs
    {
        public TargetSuspendEvent() { }
    }

    public class TargetResumeEvent : EventArgs
    {
        public TargetResumeEvent() { }
    }

    public class TargetShutdownEvent : EventArgs
    {
        public TargetShutdownEvent() { }
    }

    public class TargetNewTitleEvent : EventArgs
    {
        public string TitleID { get; private set; }

        public TargetNewTitleEvent(string TitleID)
        {
            this.TitleID = TitleID;
        }
    }
}
