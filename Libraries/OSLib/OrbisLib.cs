using System;
using System.IO;
using System.Runtime.InteropServices;
using DarkUI.Forms;
using OSLib.Classes;

namespace OSLib
{
    public class OrbisLib
    {
        #region Definitions
        internal string OrbisLib_Dir;

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

        private TargetManagement Internal_TargetManagement;
        public TargetManagement TargetManagement
        {
            get { return Internal_TargetManagement ?? (Internal_TargetManagement = new TargetManagement(this)); }
        }

        #endregion

        #region Kernel Imports

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        #endregion

        #region ** DLL Creation **

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
            }
            catch
            {

            }
        }

        #endregion
    }
}
