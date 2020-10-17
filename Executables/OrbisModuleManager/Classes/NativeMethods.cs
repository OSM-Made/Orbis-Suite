using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace OrbisModuleManager
{
    class NativeMethods
    {
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_MODULEMANAGER = RegisterWindowMessage("WM_MODULEMANAGER");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}
