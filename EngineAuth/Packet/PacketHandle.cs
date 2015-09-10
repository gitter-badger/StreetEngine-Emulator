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
namespace StreetEngine.EngineAuth
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class PacketHandle
    {
        /// <summary>
        /// Handle socket's data from auth recv.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="Socket"></param>
        public static void HandleData(byte[] data, Engine.Network.Client Socket)
        {
            // That's a quick code I wrote to detect a header, it's a bit laggy but you can find a better way.
            Int16 header = 0;
            byte[] buff = new byte[0x255];
            var Reader = new BinaryReader(new MemoryStream(data));
            for (int Index = 0; Index < data.Length; Index++)
            {
                Reader.BaseStream.Seek((int)Index, SeekOrigin.Begin);
                byte[] headerBuff = Reader.ReadBytes(2);
                headerBuff.CopyTo(buff, 0);
                header = BitConverter.ToInt16(buff, 0);

                switch (header) // If my code detect a header
                {
                    case ((Int16)EngineEnum.HeadersEnum.Recv.GLOBAL_TCP_RECV.KEEP_ALIVE):
                        Thread.Sleep(345);
                        break; // I still don't know what to do there
                    case ((Int16)EngineEnum.HeadersEnum.Recv.TM_SC.TM_SC_WE_LOGIN_RECV):
                        EnginePacket.PacketBuff.HandleLogin(data, Socket);
                        break; // Connect or no the user
                    case ((Int16)EngineEnum.HeadersEnum.Recv.TM_SC.TM_SC_SERVER_LIST_RECV):
                        Socket.Send(EnginePacket.PacketBuff.ServerList_Buff(EngineConfig.IniConfig.IniValues.h_auth, EngineConfig.IniConfig.IniValues.p_msg, EngineConfig.IniConfig.IniValues.p_lobby, EngineConfig.IniConfig.IniValues.p_world, 800, 845));
                        break; // Connect the user's game to our server
                    case ((Int16)EngineEnum.HeadersEnum.Recv.TM_SC.TM_SC_SELECT_SERVER_RECV):
                        Socket.Send(EnginePacket.PacketBuff.SelectServer_Buff(EngineEnum.LoginEnum.SelectServerStatus.CONNECTION_SUCCESS));
                        break; // Select the server the user just connected
                }
            }
        }
    }
}
