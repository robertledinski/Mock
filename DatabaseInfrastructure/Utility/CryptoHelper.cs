using System;
using System.Security.Cryptography;
using System.Text;

namespace DatabaseInfrastructure
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
