using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace OrbisTaskbarApp
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
            Mutex mutex = new Mutex(true, "{968E65CA-28AE-4E85-B7BF-25B967A9A265}", out MutexCreated);

            if (MutexCreated && mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new OrbisTaskbarApp());
                mutex.ReleaseMutex();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_TASKBARAPP,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }
        }
    }
}
