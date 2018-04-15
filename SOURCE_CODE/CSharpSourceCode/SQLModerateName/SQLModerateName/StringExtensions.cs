using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLModerateName
{
    public static class StringExtensions
    {
        public static string FormatLength(this string input, int length, char padding)
        {
            if (input == null)
            {
                input = "null";
            }
            return input.Substring(0, Math.Min(length, input.Length)).PadRight(length);
        }
    }
}
