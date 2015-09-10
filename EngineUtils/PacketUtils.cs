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
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>
    /// I do not use my packet structure to read here because it doesn't works like I thought it will works.
    /// </summary>
    public class PacketUtils
    {
        /// <summary>
        /// Read a command from the chat text data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string readCommand(byte[] data)
        {
            var Reader = new BinaryReader(new MemoryStream(data));
            Reader.BaseStream.Seek(41, SeekOrigin.Begin);
            byte[] _string = Reader.ReadBytes(9);
            return System.Text.Encoding.UTF8.GetString(_string);
        }

        /// <summary>
        /// Read the level from the .setlevel command.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int readLevel(byte[] data)
        {
            var Reader = new BinaryReader(new MemoryStream(data));
            Reader.BaseStream.Seek(51, SeekOrigin.Begin);
            byte[] level = Reader.ReadBytes(2);
            string level_numb = System.Text.Encoding.UTF8.GetString(level).Replace("\0","");
            return int.Parse(level_numb);
        }

        /// <summary>
        /// Calc packet header (first 5 bytes). Credits: itsexe - https://github.com/itsexe/.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static byte[] calcPacket(int size, Int16 header)
        {
            byte[] calc_packet = new byte[size];
            EnginePacket.PacketWriter PW = new EnginePacket.PacketWriter(calc_packet);
            PW.WriteInt32(0, size);
            PW.WriteInt16(2, header);
            byte[] len = BitConverter.GetBytes((UInt16)(calc_packet.Length));
            byte[] head = BitConverter.GetBytes(header);
            byte[] hash = BitConverter.GetBytes((UInt32)(Convert.ToDouble(len[0]) + Convert.ToDouble(len[1]) + Convert.ToDouble(head[0]) + Convert.ToDouble(head[1])));
            PW.WriteByteArray(4, hash);
            return calc_packet;
        }
    }
}
