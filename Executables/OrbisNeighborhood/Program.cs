using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace nsOrbisNeighborhood
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
            Mutex mutex = new Mutex(true, "{7A669218-E33C-46E4-BEDD-1A3CCB6AFC2C}", out MutexCreated);

            if (MutexCreated && mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new OrbisNeighborhood());
                mutex.ReleaseMutex();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_NEIGHBORHOOD,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }
        }
    }
}
