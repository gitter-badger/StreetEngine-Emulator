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
namespace StreetEngine.EngineUtils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ByteUtils
    {
        public static readonly Random _rng = new Random();

        public const String _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcedfghijklmnopqrstuvwxyz";

        /// <summary>
        /// Gets a random string.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GenerateRandomKey(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        /// <summary>
        /// Convert a byte array to a string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", " ");
        }

        /// <summary>
        /// Remove '00' off byte array to string.
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        public static string RemoveNullBytes(string _string)
        {
            return _string.Replace("\0", "");
        }

        /// <summary>
        /// Write a '.bin' file filled with byte array.
        /// </summary>
        /// <param name="data"></param>
        public static void writeBytesToBin(byte[] data)
        {
            string[] _writeBytesToBin = new string[] { "-", EngineUtils.ByteUtils.GenerateRandomKey(3), ".bin" };
            BinaryWriter writer = new BinaryWriter(new StreamWriter("dump" + _writeBytesToBin[0] + _writeBytesToBin[1] + _writeBytesToBin[2]).BaseStream);
            writer.Write(data);
            writer.BaseStream.Close();
            writer.Close();
        }
    }
}
