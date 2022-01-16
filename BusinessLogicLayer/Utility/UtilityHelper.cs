using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static DateTime ToDateTime(this string s, string format)
        {
            try
            {
                return DateTime.ParseExact(s, format, new CultureInfo("en-GB", false).DateTimeFormat);
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; 
            }
        }

        public static DateTime ToDateTime(this string s)
            => ToDateTime(s, "dd/MM/yyyy HH:mm:ss");

        public static DateTime ToUTCDateTime(this string s)
        {
            try
            {
                return DateTimeOffset.Parse(s, CultureInfo.InvariantCulture).ToUniversalTime().DateTime;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
