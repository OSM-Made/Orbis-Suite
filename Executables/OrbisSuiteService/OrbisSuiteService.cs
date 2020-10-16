using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrbisSuiteService
{
    public partial class OrbisSuiteService : ServiceBase
    {
        bool RunService = true;

        public OrbisSuiteService()
        {
            InitializeComponent();
        }

        [DllImport("OrbisWindowsServiceLib.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StartLib();

        [DllImport("OrbisWindowsServiceLib.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StopLib();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        System.Threading.Thread hServiceThread;

        void ServiceThread()
        {
            StartLib();
            while (RunService) { Thread.Sleep(10); }
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            
            SetDllDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Orbis Suite\\");

            hServiceThread = new Thread(() => ServiceThread());
            hServiceThread.Priority = ThreadPriority.Highest;
            hServiceThread.IsBackground = true;
            hServiceThread.Start();
        }

        protected override void OnStop()
        {
            StopLib();

            RunService = false;
        }
    }
}
