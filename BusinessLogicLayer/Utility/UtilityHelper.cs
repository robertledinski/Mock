using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public static class UtilityHelper
    {
        public static T ParseEnum<T>(this string stringToEnum) where T : struct
        { 
            try
            {
                T res = (T)Enum.Parse(typeof(T), stringToEnum);
                if (!Enum.IsDefined(typeof(T), res)) return default(T);
                return res;
            }
            catch
            {
                return default(T);
            }
        }

    }
}
