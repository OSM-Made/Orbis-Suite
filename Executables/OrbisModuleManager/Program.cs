using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbisModuleManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool MutexCreated = true;
            Mutex mutex = new Mutex(true, "{9E3B7C5A-C637-4509-AA54-15A04F510989}", out MutexCreated);

            if (MutexCreated && mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new OrbisModuleManager());
                mutex.ReleaseMutex();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_MODULEMANAGER,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }
        }
    }
}
