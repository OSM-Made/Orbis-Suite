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
        [DllImport("OrbisWindowsServiceLib.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dummy();

        public OrbisSuiteService()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            dummy();
            Console.WriteLine("Hello :)");
            while (true) { Thread.Sleep(10); }
        }

        protected override void OnStop()
        {

        }
    }
}
