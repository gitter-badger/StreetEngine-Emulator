/*
 *****************************************************************
 *                     Street Engine Project                     *
 *                                                               *
 * Author: greatmaes (2015)                                      *
 * URL: https://github.com/greatmaes                             *
 *                                                               *
 * Notes:                                                        *
 * StreetEngine is a non-profit server side emulator for the ga- *
 * -me StreetGear. This is mostly a project to learn how to make *
 * your own emulator as I don't have the knowledge to finish th- *
 * -is project. Feel free to contribute.                         *
 *                                                               *
 * Credits:                                                      *
 * https://github.com/itsexe (help, original project)            *
 * http://www.elitepvpers.com/forum/members/4193997-k1ramox.html *
 *                                                               *
 ***************************************************************** 
*/
namespace StreetEngine.EngineSecurity
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class Encryption
    {
        /// <summary>
        /// I do not own Crypto class encryption.
        /// </summary>
        public class Crypto
        {
            private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

            public static string EncryptStringAES(string plainText, string sharedSecret)
            {
                if (string.IsNullOrEmpty(plainText))
                    throw new ArgumentNullException("plainText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");

                string outStr = null;                    
                RijndaelManaged aesAlg = null;              

                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
 
                        msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }
                        outStr = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
                finally
                {
                    if (aesAlg != null)
                        aesAlg.Clear();
                }

                return outStr;
            }

            public static string DecryptStringAES(string cipherText, string sharedSecret)
            {
                if (string.IsNullOrEmpty(cipherText))
                    throw new ArgumentNullException("cipherText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");

                RijndaelManaged aesAlg = null;

                string plaintext = null;

                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
           
                    byte[] bytes = Convert.FromBase64String(cipherText);
                    using (MemoryStream msDecrypt = new MemoryStream(bytes))
                    {
                        aesAlg = new RijndaelManaged();
                        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                        aesAlg.IV = ReadByteArray(msDecrypt);

                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                                plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
                finally
                {
                    if (aesAlg != null)
                        aesAlg.Clear();
                }

                return plaintext;
            }

            private static byte[] ReadByteArray(Stream s)
            {
                byte[] rawLength = new byte[sizeof(int)];
                if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
                {
                    throw new SystemException("Stream did not contain properly formatted byte array");
                }

                byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
                if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
                {
                    throw new SystemException("Did not read byte array properly");
                }

                return buffer;
            }
        }

        /// <summary>
        /// I do not own Jenkins class encryption.
        /// </summary>
        public class Jenkins
        {
            /// <summary>
            /// C++ Jenkins Hash Encryption to C# (GTA5 Encryption). This can be cracked.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="len"></param>
            /// <returns></returns>
            public static UInt32 jenkins_one_at_a_time_hash(string key, int len)
            {
                UInt32 hash, i;
                for (hash = i = 0; i < len; ++i) {
                    hash += key[(int)i];
                    hash += (hash << 10);
                    hash ^= (hash >> 6);
                }
                hash += (hash << 3);
                hash ^= (hash >> 11);
                hash += (hash << 15);
                return hash;
            }
        }
    }
}
