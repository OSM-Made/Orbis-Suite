using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    class Utilities
    {
        public static string CleanByteToString(byte[] bIn)
        {
            string Out = Encoding.Default.GetString(bIn);
            return Out.Substring(0, Out.IndexOf('\0'));
        }
    }
}
