using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShindaLibrary.Tools
{
    public class SecurityTools
    {
        public static bool IsTokenValid(string Token, string Account, string Password)
        {
            return Token == Password;
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public static string MD5encrypt(string phrase)
        {
            var encoder = new UTF8Encoding();
            var md5hasher = new MD5CryptoServiceProvider();
            var hashedDataBytes = md5hasher.ComputeHash(encoder.GetBytes(phrase));

            return StringTools.ByteArrayToString(hashedDataBytes);
        }

        /// <summary>
        /// SHA256 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SHA256encrypt(string input)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }


        }

    }
}
