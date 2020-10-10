using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using DarkUI.Forms;
using OrbisSuite.Classes;
using OrbisSuite.Dialog;

namespace OrbisSuite
{
    public class OrbisLib
    {
        #region Definitions
        internal string OrbisLib_Dir;

        public enum API_ERRORS
        {
            API_OK = 0,
            API_ERROR_NOT_CONNECTED,
            API_ERROR_FAILED_TO_CONNNECT,
            API_ERROR_NOT_REACHABLE,
            API_ERROR_NOT_ATTACHED,
            API_ERROR_LOST_PROC,

            API_ERROR_FAIL,
            API_ERROR_INVALID_ADDRESS,

            //Debugger
            API_ERROR_PROC_RUNNING,
            API_ERROR_DEBUGGER_NOT_ATTACHED,
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
        public struct registers
        {
            public ulong r_r15;
            public ulong r_r14;
            public ulong r_r13;
            public ulong r_r12;
            public ulong r_r11;
            public ulong r_r10;
            public ulong r_r9;
            public ulong r_r8;
            public ulong r_rdi;
            public ulong r_rsi;
            public ulong r_rbp;
            public ulong r_rbx;
            public ulong r_rdx;
            public ulong r_rcx;
            public ulong r_rax;
            public uint r_trapno;
            public ushort r_fs;
            public ushort r_gs;
            public uint r_err;
            public ushort r_es;
            public ushort r_ds;
            public ulong r_rip;
            public ulong r_cs;
            public ulong r_rflags;
            public ulong r_rsp;
            public ulong r_ss;
        }

        public enum SIGNALS
        {
            SIGHUP = 1,
            SIGINT = 2,
            SIGQUIT = 3,
            SIGILL = 4,
            SIGTRAP = 5,
            SIGABRT = 6,
            SIGEMT = 7,
            SIGFPE = 8,
            SIGKILL = 9,
            SIGBUS = 10,
            SIGSEGV = 11,
            SIGSYS = 12,
            SIGPIPE = 13,
            SIGALRM = 14,
            SIGTERM = 15,
            SIGURG = 16,
            SIGSTOP = 17,
            SIGTSTP = 18,
            SIGCONT = 19,
            SIGCHLD = 20,
            SIGTTIN = 21,
            SIGTTOU = 22,
            SIGIO = 23,
            SIGXCPU = 24,
            SIGXFSZ = 25,
            SIGVTALRM = 26,
            SIGPROF = 27,
            SIGWINCH = 28,
            SIGINFO = 29,
            SIGUSR1 = 30,
            SIGUSR2 = 31,
        };

        public enum PRFLAGS
        {
            PR_ATTACHED = 0x800,
            PR_STOPPED = 0x20000,
        }

        #endregion

        #region Internal Class Defines

        private Events Internal_Events;
        public Events Events
        {
            get { return Internal_Events ?? (Internal_Events = new Events(this)); }
        }

        private Payload Internal_Payload;
        public Payload Payload
        {
            get { return Internal_Payload ?? (Internal_Payload = new Payload()); }
        }

        private Target Internal_Target;
        public Target Target
        {
            get { return Internal_Target ?? (Internal_Target = new Target(this)); }
        }

        private Classes.Settings Internal_Settings;
        public Classes.Settings Settings
        {
            get { return Internal_Settings ?? (Internal_Settings = new Classes.Settings(this)); }
        }

        private Dialogs Internal_Dialogs;
        public Dialogs Dialogs
        {
            get { return Internal_Dialogs ?? (Internal_Dialogs = new Dialogs(this)); }
        }

        #endregion

        #region Kernel Imports

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        #endregion

        #region ** DLL Startup **

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetupCPP(bool WinService);

        public OrbisLib()
        {
            try
            {
                OrbisLib_Dir = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\");

                if (!Directory.Exists(OrbisLib_Dir))
                {
                    DarkMessageBox.ShowError("In order to use the functionality of the OSLib.dll you need to first install Orbis Suite on this machine.", "Orbis Suite not Installed.");

                    throw new System.Exception("Orbis Suite not Installed.");
                }

                SetDllDirectory(OrbisLib_Dir);

                //Set up our instance of the OrbisLibCPP.dll.
                SetupCPP(false);

                //check with windows service to initialize Current Target and Proc
            }
            catch
            {

            }
        }

        

        #endregion
    }
}
