using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite
{
    public class Events
    {
        internal OrbisLib PS4;

        //Events
        public event EventHandler<ProcPrintEvent> ProcPrint;
        public event EventHandler<ProcInterceptEvent> ProcIntercept;
        public event EventHandler<ProcContinueEvent> ProcContinue;
        public event EventHandler<ProcDieEvent> ProcDie;
        public event EventHandler<ProcAttachEvent> ProcAttach;
        public event EventHandler<ProcDetachEvent> ProcDetach;
        public event EventHandler<TargetSuspendEvent> TargetSuspend;
        public event EventHandler<TargetResumeEvent> TargetResume;
        public event EventHandler<TargetShutdownEvent> TargetShutdown;
        public event EventHandler<TargetAvailableEvent> TargetAvailable;
        public event EventHandler<TargetNewTitleEvent> TargetNewTitle;
        public event EventHandler<DBTouchedEvent> DBTouched;
        public event EventHandler<TargetUnAvailableEvent> TargetUnAvailable;

        #region CallBacks
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_Print_Callback(int Type, int Len, string Data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Intercept_Callback(int Reason, IntPtr Registers);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Continue_Callback();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Die_Callback();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Attach_Callback(string NewProcName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Detach_Callback();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_Suspend_Callback();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_Resume_Callback();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_Shutdown_Callback();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_NewTitle_Callback(string TitleID);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void DB_Touched_Callback();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_Availability_Callback(bool Available, string TargetName);


        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern void OrbisService_RegisterCallBacks(
            IntPtr Target_Print,
            IntPtr Proc_Intercept,
            IntPtr Proc_Continue,
            IntPtr Proc_Die,
            IntPtr Proc_Attach,
            IntPtr Proc_Detach,
            IntPtr Target_Suspend,
            IntPtr Target_Resume,
            IntPtr Target_Shutdown,
            IntPtr Target_NewTitle,
            IntPtr DB_Touched,
            IntPtr Target_Availability);

        //Initialize all the callbacks
        Target_Print_Callback pTarget_PrintCallback;
        Proc_Intercept_Callback pProc_InterceptCallback;
        Proc_Continue_Callback pProc_ContinueCallback;
        Proc_Die_Callback pProc_DieCallback;
        Proc_Attach_Callback pProc_AttachCallback;
        Proc_Detach_Callback pProc_DetachCallback;
        Target_Suspend_Callback pTarget_SuspendCallback;
        Target_Resume_Callback pTarget_ResumeCallback;
        Target_Shutdown_Callback pTarget_ShutdownCallback;
        Target_NewTitle_Callback pTarget_NewTitle_Callback;
        DB_Touched_Callback pDB_Touched_Callback;
        Target_Availability_Callback pTarget_AvailabilityCallback;

        #endregion

        public Events(OrbisLib InPS4)
        {
            PS4 = InPS4;

            pTarget_PrintCallback = new Target_Print_Callback(Target_PrintCallback);
            pProc_InterceptCallback = new Proc_Intercept_Callback(Proc_InterceptCallback);
            pProc_ContinueCallback = new Proc_Continue_Callback(Proc_ContinueCallback);
            pProc_DieCallback = new Proc_Die_Callback(Proc_DieCallback);
            pProc_AttachCallback = new Proc_Attach_Callback(Proc_AttachCallback);
            pProc_DetachCallback = new Proc_Detach_Callback(Proc_DetachCallback);
            pTarget_SuspendCallback = new Target_Suspend_Callback(Target_SuspendCallback);
            pTarget_ResumeCallback = new Target_Resume_Callback(Target_ResumeCallback);
            pTarget_ShutdownCallback = new Target_Shutdown_Callback(Target_ShutdownCallback);
            pTarget_NewTitle_Callback = new Target_NewTitle_Callback(Target_NewTitleCallback);
            pDB_Touched_Callback = new DB_Touched_Callback(DB_TouchedCallback);
            pTarget_AvailabilityCallback = new Target_Availability_Callback(Target_AvailabilityCallback);

            //Register the callbacks with the c++ dll.
            OrbisService_RegisterCallBacks(
                Marshal.GetFunctionPointerForDelegate(pTarget_PrintCallback),
                Marshal.GetFunctionPointerForDelegate(pProc_InterceptCallback),
                Marshal.GetFunctionPointerForDelegate(pProc_ContinueCallback),
                Marshal.GetFunctionPointerForDelegate(pProc_DieCallback),
                Marshal.GetFunctionPointerForDelegate(pProc_AttachCallback),
                Marshal.GetFunctionPointerForDelegate(pProc_DetachCallback),
                Marshal.GetFunctionPointerForDelegate(pTarget_SuspendCallback),
                Marshal.GetFunctionPointerForDelegate(pTarget_ResumeCallback),
                Marshal.GetFunctionPointerForDelegate(pTarget_ShutdownCallback),
                Marshal.GetFunctionPointerForDelegate(pTarget_NewTitle_Callback),
                Marshal.GetFunctionPointerForDelegate(pDB_Touched_Callback),
                Marshal.GetFunctionPointerForDelegate(pTarget_AvailabilityCallback));
        }

        internal void Target_PrintCallback(int Type, int Len, string Data)
        {
            ProcPrint?.Invoke(null, new ProcPrintEvent(Type, Len, Data));
        }

        internal void Proc_InterceptCallback(int Reason, IntPtr Registers)
        {
            OrbisLib.registers reg = (OrbisLib.registers)Marshal.PtrToStructure(Registers, typeof(OrbisLib.registers));

            ProcIntercept?.Invoke(null, new ProcInterceptEvent(Reason, reg));
        }

        internal void Proc_ContinueCallback()
        {
            ProcContinue?.Invoke(null, new ProcContinueEvent());
        }

        internal void Proc_DieCallback()
        {
            ProcDie?.Invoke(null, new ProcDieEvent());
        }

        internal void Proc_AttachCallback(string NewProcName)
        {
            ProcAttach?.Invoke(null, new ProcAttachEvent(NewProcName));
        }

        internal void Proc_DetachCallback()
        {
            ProcDetach?.Invoke(null, new ProcDetachEvent());
        }

        internal void Target_SuspendCallback()
        {
            TargetSuspend?.Invoke(null, new TargetSuspendEvent());
        }

        internal void Target_ResumeCallback()
        {
            TargetResume?.Invoke(null, new TargetResumeEvent());
        }

        internal void Target_ShutdownCallback()
        {
            TargetShutdown?.Invoke(null, new TargetShutdownEvent());
        }

        internal void Target_NewTitleCallback(string TitleID)
        {
            TargetNewTitle?.Invoke(null, new TargetNewTitleEvent(TitleID));
        }

        internal void DB_TouchedCallback()
        {
            DBTouched?.Invoke(null, new DBTouchedEvent());
        }

        internal void Target_AvailabilityCallback(bool Available, string TargetName)
        {
            if (Available)
            {
                TargetAvailable?.Invoke(null, new TargetAvailableEvent(TargetName));
            }
            else
            {
                TargetUnAvailable?.Invoke(null, new TargetUnAvailableEvent(TargetName));
            }
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

    public class DBTouchedEvent : EventArgs
    {
        public DBTouchedEvent() { }
    }

    public class TargetAvailableEvent : EventArgs
    {
        public string TargetName { get; private set; }

        public TargetAvailableEvent(string TargetName)
        {
            this.TargetName = TargetName;
        }
    }

    public class TargetUnAvailableEvent : EventArgs
    {
        public string TargetName { get; private set; }

        public TargetUnAvailableEvent(string TargetName)
        {
            this.TargetName = TargetName;
        }
    }
}
