using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportAutomation
{
    public static class StringExtensions
    {
        public static bool IsTCValid(this string s)
        {
            if (s.Length != 11) return false;
            foreach (var c in s)
                if (!char.IsNumber(c))
                    return false;
            return true;
        }
    }
}
