using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public static class CryptoHelper
    {
        public static string GetStringSha256Hash(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
                return String.Empty;

            using (var encrypter = new SHA256Managed())
                return BitConverter.ToString(encrypter.ComputeHash(Encoding.UTF8.GetBytes(inputString))).Replace("-", String.Empty);
        }
    }
}
