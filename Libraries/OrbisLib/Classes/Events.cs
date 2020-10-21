using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite
{
    /// <summary>
    /// Events that are thrown from the Orbis Suite API
    /// </summary>
    public class Events
    {
        internal OrbisLib PS4;

        //Global Events
        /// <summary>
        /// The DBTouched Event gets invoked when the Database used to store target specific info is changed.
        /// </summary>
        public event EventHandler<DBTouchedEvent> DBTouched;
        /// <summary>
        /// The TargetAvailable Event gets invoked when a target becomes available.
        /// </summary>
        public event EventHandler<TargetAvailableEvent> TargetAvailable;
        /// <summary>
        /// The TargetUnAvailable Event gets invoked when a target becomes no longer available.
        /// </summary>
        public event EventHandler<TargetUnAvailableEvent> TargetUnAvailable;

        #region CallBacks
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_Print_Callback(string IPAddr, int Type, int Len, string Data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Intercept_Callback(string IPAddr, int Reason, IntPtr Registers);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Continue_Callback(string IPAddr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Die_Callback(string IPAddr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Attach_Callback(string IPAddr, string NewProcName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Proc_Detach_Callback(string IPAddr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_Suspend_Callback(string IPAddr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_Resume_Callback(string IPAddr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_Shutdown_Callback(string IPAddr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Target_NewTitle_Callback(string IPAddr, string TitleID);

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
        /// <summary>
        /// 
        /// </summary>
        public Events(OrbisLib PS4)
        {
            this.PS4 = PS4;

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

        internal void Target_PrintCallback(string IPAddr, int Type, int Len, string Data)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseProcPrintEvent(IPAddr, Type, Len, Data);
            PS4.SelectedTarget.Events.RaiseProcPrintEvent(IPAddr, Type, Len, Data);

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseProcPrintEvent(IPAddr, Type, Len, Data);
        }

        internal void Proc_InterceptCallback(string IPAddr, int Reason, IntPtr Registers)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseProcInterceptEvent(IPAddr, Reason, Registers);
            PS4.SelectedTarget.Events.RaiseProcInterceptEvent(IPAddr, Reason, Registers);

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseProcInterceptEvent(IPAddr, Reason, Registers);
        }

        internal void Proc_ContinueCallback(string IPAddr)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseProcContinueEvent(IPAddr);
            PS4.SelectedTarget.Events.RaiseProcContinueEvent(IPAddr);

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseProcContinueEvent(IPAddr);
        }

        internal void Proc_DieCallback(string IPAddr)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseProcDieEvent(IPAddr);
            PS4.SelectedTarget.Events.RaiseProcDieEvent(IPAddr);

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseProcDieEvent(IPAddr);
        }

        internal void Proc_AttachCallback(string IPAddr, string NewProcName)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseProcAttachEvent(IPAddr, NewProcName.Substring(4));
            PS4.SelectedTarget.Events.RaiseProcAttachEvent(IPAddr, NewProcName.Substring(4));

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseProcAttachEvent(IPAddr, NewProcName.Substring(4));
        }

        internal void Proc_DetachCallback(string IPAddr)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseProcDetachEvent(IPAddr);
            PS4.SelectedTarget.Events.RaiseProcDetachEvent(IPAddr);

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseProcDetachEvent(IPAddr);
        }

        internal void Target_SuspendCallback(string IPAddr)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseTargetSuspendEvent(IPAddr);
            PS4.SelectedTarget.Events.RaiseTargetSuspendEvent(IPAddr);

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseTargetSuspendEvent(IPAddr);
        }

        internal void Target_ResumeCallback(string IPAddr)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseTargetResumeEvent(IPAddr);
            PS4.SelectedTarget.Events.RaiseTargetResumeEvent(IPAddr);

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseTargetResumeEvent(IPAddr);
        }

        internal void Target_ShutdownCallback(string IPAddr)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseTargetShutdownEvent(IPAddr);
            PS4.SelectedTarget.Events.RaiseTargetShutdownEvent(IPAddr);

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseTargetShutdownEvent(IPAddr);
        }

        internal void Target_NewTitleCallback(string IPAddr, string TitleID)
        {
            //Raise the event for the Default target and the Selected Target.
            PS4.DefaultTarget.Events.RaiseTargetNewTitleEvent(IPAddr, TitleID.Substring(4));
            PS4.SelectedTarget.Events.RaiseTargetNewTitleEvent(IPAddr, TitleID.Substring(4));

            //Raise the event for all the conosles in the target list.
            foreach (KeyValuePair<string, Target> Target in PS4.Target)
                Target.Value.Events.RaiseTargetNewTitleEvent(IPAddr, TitleID.Substring(4));
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
