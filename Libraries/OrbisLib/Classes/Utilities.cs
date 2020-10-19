using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace OrbisSuite.Classes
{
    class Utilities
    {
        public static string CleanByteToString(byte[] bIn)
        {
            try
            {
                if (bIn.Length <= 0)
                    return string.Empty;

                string Out = Encoding.Default.GetString(bIn);
                return Out.Substring(0, Out.IndexOf('\0'));
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string CensorString(string In, char CensorChar, int AllowedCount)
        {
            try
            {
                string Out = In.Substring(0, AllowedCount);
                Out += new string(CensorChar, In.Length - AllowedCount);
                return Out;
            }
            catch
            {
                return string.Empty;
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);
    }
}
