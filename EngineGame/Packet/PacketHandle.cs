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
namespace StreetEngine.EngineGame.Packet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.IO;

    public class PacketHandle
    {
        /// <summary>
        /// Handle socket's data from world recv.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="Socket"></param>
        public static void HandleData(byte[] data, Engine.Network.mmoClient Socket)
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
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_LOGIN):
                        Socket.Send(EnginePacket.PacketBuff.SetSuccess((Int16)EngineEnum.HeadersEnum.Send.BM_SC.BM_SC_LOGIN));
                        Socket.Send(EnginePacket.PacketBuff.PlayerCharacterList_Buff(Convert.ToBoolean(EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].isFirstLogin), (EngineEnum.WorldEnum.CharactersType)EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].type, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].username));
                        Thread.Sleep(1650); // I don't know why but the server need this..
                        break; // Connect the player with his saved character
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_CREATE_CHARACTER):
                        //Socket.Send(EnginePacket.PacketBuff.SetSuccess((Int16)EngineEnum.HeadersEnum.Send.BM_SC.BM_SC_CREATE_CHARACTER));
                        //Socket.Send(EnginePacket.PacketBuff.PlayerCharacterList_Buff(false, (EngineEnum.WorldEnum.CharactersType)EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].type, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].username));
                        break; // Create the desired character (still in progress..)
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_SELECT_CHARACTER):
                        Socket.Send(EnginePacket.PacketBuff.SetSuccess((Int16)EngineEnum.HeadersEnum.Send.BM_SC.BM_SC_SELECT_CHARACTER));
                        break; // Select character response
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_CHAT_MESSAGE):
                        string cmd = EngineUtils.PacketUtils.readCommand(data);
                        switch(EngineUtils.ByteUtils.RemoveNullBytes(cmd))
                        {
                            case ".setlevel":
                                int level = EngineUtils.PacketUtils.readLevel(data);
                                Socket.Send(EnginePacket.PacketBuff.LevelInfo_Buff(level, 4, 102400));
                                break; // You can check the database and set current exp and liscence
                            default:
                                Socket.Send(EnginePacket.PacketBuff.SetSuccess((Int16)EngineEnum.HeadersEnum.Send.BM_SC.BM_SC_CHAT_MESSAGE));
                                break; // Chat response to send the message
                        }
                        break; // Send a command or a message
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_PLAYER_INFO):
                        Socket.Send(EnginePacket.PacketBuff.PlayerInfo_Buff());
                        break; // The server need this we don't actually know what this packet can do
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_TRICK_LIST):
                        Socket.Send(EnginePacket.PacketBuff.TrickList_Buff(5, 4, 5, 5, 4, 3, 3, 3, 3, 5, 0, 3, 4)); // This is the maximum level you can put for each trick
                        break; // Send each tricks level to the player
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_BALANCE_INFO):
                        Socket.Send(EnginePacket.PacketBuff.BalanceInfo_Buff(EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].coins, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].rupees, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].gpotatos, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].questpoints));
                        break; // Send player's database balance stuff to the world server
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_GET_CHANNEL_LIST):
                        Socket.Send(EnginePacket.PacketBuff.ChannelList_Buff((String)EngineWorld.Server.Channel.channel_a)); // You can put any name there
                        break; // Create our channel list, still only one channel coded
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_ENTER_CHANNEL):
                        Socket.Send(EnginePacket.PacketBuff.EnterChannel_Buff());
                        Socket.Send(EnginePacket.PacketBuff.CashBalance_Buff(EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].gpotatos));
                        Socket.Send(EnginePacket.PacketBuff.LevelInfo_Buff(EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].level, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].liscence, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].exp));
                        break; // Enter in Parktown and some server-need stuff (cash, level stuff..)
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_INVENTORY):
                        Socket.Send(EnginePacket.PacketBuff.Inventory_Buff((EngineEnum.WorldEnum.CharactersType)EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].type));
                        break; // Send every items to the player's character type inventory
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_SELECT_TRICK):
                        Socket.Send(EnginePacket.PacketBuff.SetSuccess((Int16)EngineEnum.HeadersEnum.Send.BM_SC.BM_SC_SELECT_TRICK));
                        break; // Select the desired trick (still in progress, you can't unselect..)
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_ENTER_INVENTORY):
                        Socket.Send(EnginePacket.PacketBuff.SetSuccess((Int16)EngineEnum.HeadersEnum.Send.BM_SC.BM_SC_ENTER_INVENTORY));
                        break; // Enter in the inventory room
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_MATE_INFO):
                        Socket.Send(EnginePacket.PacketBuff.InfoChar_Buff(EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].type, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].username, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].clan_name, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].clan_id, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].age, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].level, (Int16)EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].liscence, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].s_zone, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].c_zone, EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].bio));
                        break; // Show up infos of the selected player (still in progress, it actually only work for yourself..) TODO: Read the data the client receive in the packet and check the username he clicked on, in the database and load the info. Next step: update the info
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_LEAVE_CHANNEL):
                        Socket.Send(EnginePacket.PacketBuff.SetSuccess((Int16)EngineEnum.HeadersEnum.Send.BM_SC.BM_SC_LEAVE_CHANNEL));
                        break; // Leave the current channel
                    case ((Int16)EngineEnum.HeadersEnum.Recv.ID_BZ_SC.ID_BZ_SC_ENTER_LOBBY):
                        Socket.Send(EnginePacket.PacketBuff.SetSuccess((Int16)EngineEnum.HeadersEnum.Send.ID_BZ_SC.ID_BZ_SC_ENTER_LOBBY));
                        break; // Enter lobby room response
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_LEAVE_INVENTORY):
                        Socket.Send(EnginePacket.PacketBuff.SetSuccess((Int16)EngineEnum.HeadersEnum.Send.BM_SC.BM_SC_LEAVE_INVENTORY));
                        break; // Leave inventory and enter the tuning shop
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_SELECT_ITEM): // Equip the desired item (still in progress)
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_DELETE_ITEM): // Delete the desired item (still in progress)
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_MINI_GAME_START): // Start the mini game with infos (still in progress)
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_MINI_GAME_END):  // End the mini game with infos (still in progress)
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_EXCHANGE_MONEY): // Exchange coins for ruppees (still in progress)
                    case ((Int16)EngineEnum.HeadersEnum.Recv.BM_SC.BM_SC_PLAYER_CHARACTER_LIST): // Multiple characters list (still in progress)
                        break;
                }
            }
        }
    }
}
