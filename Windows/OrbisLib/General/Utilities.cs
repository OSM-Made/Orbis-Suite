using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisLib2.General
{
    public static class Utilities
    {
        /// <summary>
        /// Will censor a part of an input string with the desired sensor char.
        /// </summary>
        /// <param name="In">The string we would like to censor.</param>
        /// <param name="CensorChar">The character we would like to use as the sensor.</param>
        /// <param name="AllowedCount">The allowed number of characters starting from 0.</param>
        /// <returns>Returns the censored string.</returns>
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

        /// <summary>
        /// Converts a number of bytes to a formated string.
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes to be converted.</param>
        /// <returns>Formatted string of the number of bytes represented in B, KB, MB or GB.</returns>
        public static string BytesToString(long numberOfBytes)
        {
            string[] typeStrings = new string[]
            {
                "{0} B",
                "{0:f1} KB",
                "{0:f1} MB",
                "{0:N2} GB"
            };

            double num = numberOfBytes;
            for (int i = 0; i < typeStrings.Length; i++)
            {
                if (num <= 10000.0 || i >= 3)
                {
                    return string.Format(typeStrings[i], num);
                }
                num /= 1000.0;
            }
            return "N/A";
        }
    }
}
