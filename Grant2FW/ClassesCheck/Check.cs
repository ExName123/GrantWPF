using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Grant2FW.ClassesCheck
{
    static internal class Check
    {
        static public string CheckNull(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return string.Empty;
            }
        }
        static public bool IsIntNull(int? value)
        {
            return value == null;
        }
        static public bool IsIntValid(string input)
        {
            int result;
            return int.TryParse(input, out result);
        }
    }
}
