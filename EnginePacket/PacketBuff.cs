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
namespace StreetEngine.EnginePacket
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    public class PacketBuff
    {
        public static StreetEngine.EngineDatabase.DatabaseManager MySQL = new StreetEngine.EngineDatabase.DatabaseManager();

        /// <summary>
        /// Handle the login.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="socket"></param>
        public static void HandleLogin(byte[] data, Engine.Network.Client socket)
        {
            var Reader = new BinaryReader(new MemoryStream(data)); // Initializing our reader

            // Both username and password are 16 lenght long

            byte[] username = new byte[16];
            byte[] password = new byte[16];

            Reader.BaseStream.Seek(14, SeekOrigin.Begin); // Seek where the username is located ( Position 14 )
            username = Reader.ReadBytes(16);

            Reader.BaseStream.Seek(32, SeekOrigin.Begin); // Seek where the password is located ( Position 32 )
            password = Reader.ReadBytes(16);

            // Convert both username and password byte array to string and remove all null bytes
            MySQL.CheckUserAccount(System.Text.Encoding.UTF8.GetString(username).Replace("\0", ""), System.Text.Encoding.UTF8.GetString(password).Replace("\0", "")); // Check the user in the database..

            if (EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].isConnected) { // We could actually put this directly in the CheckUser function
                 socket.Send(EnginePacket.PacketBuff.Login_Buff(EngineDatabase.DatabaseManager.Player[EngineDatabase.DatabaseManager.id].auth_key)); // Login the user with his session key
            }
            else { // Send an error message to client
                 socket.Send(EnginePacket.PacketBuff.Error_Buff(EngineEnum.HeadersEnum.Send.TM_SC_RESULT.MSG_SERVER_NOT_EXIST)); // Wrong password error
            }
        }

        /// <summary>
        /// Send an error message to the client.
        /// </summary>
        /// <param name="request_msg_id"></param>
        /// <returns></returns>
        public static byte[] Error_Buff(EngineEnum.HeadersEnum.Send.TM_SC_RESULT request_msg_id)
        {
            byte[] block = new byte[0x09]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x09, 1001); // Calc the packet header

            // Initialize the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteInt16(7, (Int16)request_msg_id); // Desired error message
            
            return block;
        }

        /// <summary>
        /// Used to log the user.
        /// </summary>
        /// <param name="auth_key"></param>
        /// <returns></returns>
        public static byte[] Login_Buff(string auth_key)
        {
            byte[] block = new byte[0x2F]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x2F, 0x3F1); // Calc our packet header..

            // Initialize the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header stack
            PW.WriteByteArray(0, c_block);
            
            // Write the packet's data stack
            PW.WriteSByte(5, 1); // I don't know what is it
            PW.WriteString(9, auth_key); // Write the player's auth key
            PW.WriteInt32(42, 700); // I don't know what is it
            PW.WriteSByte(46, 1); // I don't know what is it

            return block;
        }

        /// <summary>
        /// Create the server list.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="lobby_port"></param>
        /// <param name="msg_port"></param>
        /// <param name="mmo_port"></param>
        /// <param name="unk_clientnumb"></param>
        /// <param name="unk_clientnumb_2"></param>
        /// <returns></returns>
        public static byte[] ServerList_Buff(string ip, int msg_port, int lobby_port, int mmo_port, Int16 unk_clientnumb, Int16 unk_clientnumb_2)
        {
            byte[] block = new byte[0x4F]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x4F, 0x3F4); // Calc our packet header..

            // Initialize the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header stack
            PW.WriteByteArray(0, c_block);

            // Write the packet's data stack
            PW.WriteSByte(5, 1); // I don't know what is it
            PW.WriteSByte(7, 100); // I don't know what is it
            PW.WriteSByte(9, 2); // I don't know what is it
            PW.WriteSByte(11, 3); // I don't know what is it
            PW.WriteSByte(13, 4); // I don't know what is it
            PW.WriteString(15, ip); // Write the lobby ip
            PW.WriteString(31, ip); // Write the mmo ip
            PW.WriteString(47, ip); // Write the msg ip 
            PW.WriteInt16(63, (Int16)lobby_port); // Write the lobby port
            PW.WriteInt16(67, (Int16)msg_port); // Write the msg port
            PW.WriteInt16(71, (Int16)mmo_port); // Write the mmo port
            PW.WriteInt16(75, (Int16)unk_clientnumb); // Still don't know what is it, something to do with players
            PW.WriteInt16(77, (Int16)unk_clientnumb_2); // Still don't know what is it, something to do with players

            return block;
        }

        /// <summary>
        /// Select the server created.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static byte[] SelectServer_Buff(EngineEnum.LoginEnum.SelectServerStatus status)
        {
            byte[] block = new byte[0x12]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x12, 0x3EF); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteInt32(5, (int)status);

            return block;
        }

        /// <summary>
        /// Create a player character list.
        /// </summary>
        /// <param name="isFirstTime"></param>
        /// <param name="char_type"></param>
        /// <param name="char_name"></param>
        /// <returns></returns>
        public static byte[] PlayerCharacterList_Buff(bool isFirstTime, EngineEnum.WorldEnum.CharactersType char_type, string char_name)
        {
            byte[] block = new byte[0x120]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x120, 0x90E); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);
            PW.WriteString(30, char_name); // Write the character's name, you can use some html code like: <#ff0000> or <glow>
            PW.WriteInt32(73, isFirstTime ? 1 : 0); // Create a character/or no
            PW.WriteUInt32(75, 0); // I don't know what is it
            PW.WriteUInt32(79, (UInt32)char_type); // Choose the desired character (luna, etc..)
            PW.WriteUInt32(83, 0); // I don't know what is it

            return block;
        }

        /// <summary>
        /// Create mate's infos.
        /// </summary>
        /// <param name="char_type"></param>
        /// <param name="char_name"></param>
        /// <param name="clan_name"></param>
        /// <param name="clan_id"></param>
        /// <param name="age"></param>
        /// <param name="level"></param>
        /// <param name="license"></param>
        /// <param name="zone_info"></param>
        /// <param name="zone_id"></param>
        /// <param name="bio_str"></param>
        /// <returns></returns>
        public static byte[] InfoChar_Buff(int char_type, string char_name, string clan_name, string clan_id, int age, int level, short license, string zone_info, int zone_id, string bio_str)
        {
            byte[] block = new byte[0x253]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x253, 0x920); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);
            PW.WriteString(39, char_name); // Write the character's name, you can use some html code like: <#ff0000> or <glow>
            PW.WriteString(156, clan_id); // Write the clan's ID, still don't know what is it exactly, I just use "CL#1", its a 4 lenght string
            PW.WriteString(160, clan_name); // Write the clan name
            PW.WriteSByte(0x26, (SByte)char_type); // Choose the desired character (luna, etc..)
            PW.WriteSByte(247, (SByte)age); // Write the age
            PW.WriteInt16(248, (Int16)level); // Write the level
            PW.WriteInt16(250, (Int16)license); // Write the liscence
            PW.WriteInt32(252, (Int32)zone_id); // Write the zone (France, etc...) check the list in mate_zip.txt
            PW.WriteString(256, zone_info); // Write the zone info
            PW.WriteString(377, bio_str); // Write the bio

            return block;
        }

        /// <summary>
        /// Response used many times.
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static byte[] SetSuccess(short header)
        {
            byte[] block = new byte[0x0D]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x0D, header); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);

            return block;
        }

        /// <summary>
        /// Create a trick list.
        /// </summary>
        /// <param name="grind_level"></param>
        /// <param name="backflip_level"></param>
        /// <param name="frontflip_level"></param>
        /// <param name="airtwist_level"></param>
        /// <param name="powerswing_level"></param>
        /// <param name="gripturn_level"></param>
        /// <param name="dash_level"></param>
        /// <param name="backskating_level"></param>
        /// <param name="jumpingsteer_level"></param>
        /// <param name="butting_level"></param>
        /// <param name="powerslide_level"></param>
        /// <param name="powerjump_level"></param>
        /// <param name="wallride_level"></param>
        /// <returns></returns>
        public static byte[] TrickList_Buff(int grind_level, int backflip_level, int frontflip_level, int airtwist_level, int powerswing_level, int gripturn_level, int dash_level, int backskating_level, int jumpingsteer_level, int butting_level, int powerslide_level, int powerjump_level, int wallride_level) 
        {
            byte[] block = new byte[0x95]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x95, 0x839); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);

            // Write the trick list
            PW.WriteSByte(30, 0xD); // Tricks number
            PW.WriteInt16(32, (Int16)EngineEnum.WorldEnum.TrickCode.GRIND);
            PW.WriteSByte(40, 1); // Apply the trick
            PW.WriteInt16(41, (Int16)EngineEnum.WorldEnum.TrickCode.DASH);
            PW.WriteSByte(49, 1); // Apply the trick
            PW.WriteInt16(50, (Int16)EngineEnum.WorldEnum.TrickCode.BACK_SKATING);
            PW.WriteSByte(58, 1); // Apply the trick
            PW.WriteInt16(59, (Int16)EngineEnum.WorldEnum.TrickCode.BUTTING);
            PW.WriteSByte(67, 1); // Apply the trick
            PW.WriteInt16(68, (Int16)EngineEnum.WorldEnum.TrickCode.POWER_SLIDE);
            PW.WriteSByte(76, 1); // Apply the trick
            PW.WriteInt16(77, (Int16)EngineEnum.WorldEnum.TrickCode.BACK_FLIP);
            PW.WriteSByte(85, 1); // Apply the trick
            PW.WriteInt16(86, (Int16)EngineEnum.WorldEnum.TrickCode.FRONT_FLIP);
            PW.WriteSByte(94, 1); // Apply the trick
            PW.WriteInt16(95, (Int16)EngineEnum.WorldEnum.TrickCode.AIR_TWIST);
            PW.WriteSByte(103, 1); // Apply the trick
            PW.WriteInt16(104, (Int16)EngineEnum.WorldEnum.TrickCode.POWER_SWING);
            PW.WriteSByte(112, 1); // Apply the trick
            PW.WriteInt16(113, (Int16)EngineEnum.WorldEnum.TrickCode.GRIP_TURN);
            PW.WriteSByte(121, 1); // Apply the trick
            PW.WriteInt16(122, (Int16)EngineEnum.WorldEnum.TrickCode.JUMPING_STEER);
            PW.WriteSByte(130, 1); // Apply the trick
            PW.WriteInt16(131, (Int16)EngineEnum.WorldEnum.TrickCode.POWER_JUMP);
            PW.WriteSByte(139, 1); // Apply the trick
            PW.WriteInt16(140, (Int16)EngineEnum.WorldEnum.TrickCode.WALL_RIDE);
            PW.WriteSByte(148, 1); // Apply the trick

            // Write trick levels
            PW.WriteInt32(0x24, grind_level);
            PW.WriteInt32(0x2D, dash_level);
            PW.WriteInt32(0x36, backskating_level);
            PW.WriteInt32(0x3F, butting_level);
            PW.WriteInt32(0x48, powerslide_level);
            PW.WriteInt32(0x51, backflip_level);
            PW.WriteInt32(0x5A, frontflip_level);
            PW.WriteInt32(0x63, airtwist_level);
            PW.WriteInt32(0x6C, powerswing_level);
            PW.WriteInt32(0x75, gripturn_level);
            PW.WriteInt32(0x7E, powerjump_level);
            PW.WriteInt32(0x87, wallride_level);
            PW.WriteInt32(0x90, gripturn_level);

            return block;
        }

        /// <summary>
        /// Still don't really know what can do this packet.
        /// </summary>
        /// <returns></returns>
        public static byte[] PlayerInfo_Buff()
        {
            byte[] block = new byte[0xD7]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0xD7, 0x90A); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteSByte(5, 4);
            PW.WriteSByte(9, 0x2D);
            PW.WriteString(11, EngineEnum.PacketEnum.PacketCommand.success_0);
            PW.WriteSByte(41, 5);
            PW.WriteSByte(45, 1);
            PW.WriteSByte(47, 0x32);
            PW.WriteString(49, "ID1_Testo_2"); // Thanks itsexe :)
            PW.WriteString(90, "ID1_Test"); // Thanks itsexe :)
            PW.WriteSByte(99, 2);
            PW.WriteSByte(101, 0xC);
            PW.WriteSByte(103, 5);
            PW.WriteSByte(105, 5);
            PW.WriteSByte(107, 45);
            PW.WriteSByte(115, 4);
            PW.WriteSByte(117, 0xC);
            PW.WriteSByte(119, 5);
            PW.WriteInt32(123, 6);
            PW.WriteInt32(127, 7);
            PW.WriteSByte(131, 0x40);
            PW.WriteSByte(133, 0x10);
            PW.WriteSByte(135, 7);
            PW.WriteSByte(139, 8);
            PW.WriteSByte(143, 9);
            PW.WriteSByte(147, 10);
            PW.WriteInt16(152, 11266);
            PW.WriteInt16(200, 3076);
            PW.WriteSByte(203, 2);
            PW.WriteSByte(207, 3);
            PW.WriteSByte(211, 3);

            return block;
        }

        /// <summary>
        /// Enter in Park Town.
        /// </summary>
        /// <returns></returns>
        public static byte[] EnterChannel_Buff()
        {
            byte[] block = new byte[0x20]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x20, 0x7D8); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);
            PW.WriteSByte(29, 1);
            PW.WriteSByte(30, 1);

            return block;
        }

        /// <summary>
        /// Send balance infos to the client.
        /// </summary>
        /// <param name="coins"></param>
        /// <param name="rupees"></param>
        /// <param name="gpotatos"></param>
        /// <param name="trickpoints"></param>
        /// <returns></returns>
        public static byte[] BalanceInfo_Buff(Int32 coins, Int32 rupees, Int32 gpotatos, Int32 trickpoints)
        {
            byte[] block = new byte[0x2D]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x2D, 0x82F); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);
            PW.WriteSByte(12, 0);
            PW.WriteUInt32(29, (UInt32)rupees);
            PW.WriteUInt32(33, (UInt32)coins);
            PW.WriteUInt32(37, (UInt32)gpotatos);
            PW.WriteUInt32(41, (UInt32)trickpoints);

            return block;
        }

        /// <summary>
        /// Create a channel list (only one channel at the moment).
        /// </summary>
        /// <param name="channel_name"></param>
        /// <returns></returns>
        public static byte[] ChannelList_Buff(string channel_name)
        {
            byte[] block = new byte[0x40]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x40, 0x7D6); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);
            PW.WriteUInt16(29, 1);
            PW.WriteUInt16(31, 1);
            PW.WriteString(32, channel_name);
            PW.WriteUInt32(44, 5);
            PW.WriteUInt32(48, 6);
            PW.WriteUInt32(52, 7);
            PW.WriteUInt32(56, 9);
            PW.WriteUInt32(60, 10);

            if(channel_name.Length > 11) {
                EngineConsole.Log.Error("channel name too long");
            }

            return block;
        }

        /// <summary>
        /// Send balance info to the player.
        /// </summary>
        /// <param name="gpotatos"></param>
        /// <returns></returns>
        public static byte[] CashBalance_Buff(Int32 gpotatos)
        {
            byte[] block = new byte[0x21]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x21, 0x8E0); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);
            PW.WriteInt32(29, gpotatos);

            return block;
        }

        /// <summary>
        /// Big ass messy code to load inventory stuff of each character.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="D"></param>
        /// <param name="char_type"></param>
        /// <param name="char_item_list"></param>
        /// <param name="char_list_inventory_size"></param>
        public static void LoadItemList(byte[] packet, EngineEnum.WorldEnum.CharactersType char_type)
        {
            // Initializing basic stuff
            SByte unknow_number = 0x0A; // Still not sure about this
            Int32 item_number = 0; // To tell the loop where to stop

            // Initializing items list
            List<int> rookieItemList = new List<int>();
            List<int> lunaItemList = new List<int>();
            List<int> rushItemList = new List<int>();
            List<int> tippyItemList = new List<int>();

            // Initializing inventory enums
            EngineEnum.IventoryEnum.ROOKIE_ITEM_LIST[] rookie_items = (EngineEnum.IventoryEnum.ROOKIE_ITEM_LIST[])Enum.GetValues(typeof(EngineEnum.IventoryEnum.ROOKIE_ITEM_LIST));
            EngineEnum.IventoryEnum.LUNA_ITEM_LIST[] luna_items = (EngineEnum.IventoryEnum.LUNA_ITEM_LIST[])Enum.GetValues(typeof(EngineEnum.IventoryEnum.LUNA_ITEM_LIST));
            EngineEnum.IventoryEnum.RUSH_ITEM_LIST[] rush_items = (EngineEnum.IventoryEnum.RUSH_ITEM_LIST[])Enum.GetValues(typeof(EngineEnum.IventoryEnum.RUSH_ITEM_LIST));
            EngineEnum.IventoryEnum.TIPPY_ITEM_LIST[] tippy_items = (EngineEnum.IventoryEnum.TIPPY_ITEM_LIST[])Enum.GetValues(typeof(EngineEnum.IventoryEnum.TIPPY_ITEM_LIST));

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(packet);

            switch(char_type)
            {
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_ROOKIE: item_number = 150;
                    foreach (var itemId in rookie_items)
                        rookieItemList.Add((int)itemId);
                    for (int pItem = 0x24, feItem = 0; pItem < (item_number * 0x1D) && feItem < item_number; pItem += 0x1D, feItem++) 
                        PW.WriteInt16(pItem, (Int16)rookieItemList[feItem]);
                    break; // Load rookie items
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_LUNA: item_number = 150;
                    foreach (var itemId in luna_items) 
                        lunaItemList.Add((int)itemId);
                    for (int pItem = 0x24, feItem = 0; pItem < (item_number * 0x1D) && feItem < item_number; pItem += 0x1D, feItem++)
                        PW.WriteInt16(pItem, (Int16)lunaItemList[feItem]);
                    break; // Load luna items
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_RUSH: item_number = 100;
                    foreach (var itemId in rush_items) 
                        rushItemList.Add((int)itemId);
                    for (int pItem = 0x24, feItem = 0; pItem < (item_number * 0x1D) && feItem < item_number; pItem += 0x1D, feItem++)
                        PW.WriteInt16(pItem, (Int16)rushItemList[feItem]);
                    break; // Load rush items
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_TIPPY: item_number = 124;
                    foreach (var itemId in tippy_items) 
                        tippyItemList.Add((int)itemId);
                    for (int pItem = 0x24, feItem = 0; pItem < (item_number * 0x1D) && feItem < item_number; pItem += 0x1D, feItem++)
                        PW.WriteInt16(pItem, (Int16)tippyItemList[feItem]);
                    break; // Load tippy items
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_KLAUS: // There is still no avaiable items for klaus
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_KARA: // There is still no avaiable items for kara
                    break;
            }

            PW.WriteInt32(30, item_number); // I'm not sure if the packet really need this

            // Write the rank of each items (1,2,3,4, etc..)
            for (int pUnk = 0x2E; pUnk < (item_number * 0x1D); pUnk += 0x1D)
                PW.WriteSByte(pUnk, unknow_number);
            for (int nItem = 0x20, j = 0; nItem < (item_number * 0x1D) && j < (item_number * 0x1D); nItem += 0x1D, j++) 
                PW.WriteInt16(nItem, (Int16)j);

        }  

        /// <summary>
        /// Load inventory items.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static byte[] Inventory_Buff(EngineEnum.WorldEnum.CharactersType type)
        {
            byte[] block = new byte[0x118F]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x118F, 0x833); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);

            switch(type)
            {
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_ROOKIE:
                    LoadItemList(block, EngineEnum.WorldEnum.CharactersType.CHARACTER_ROOKIE);
                    break;
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_LUNA:
                    LoadItemList(block, EngineEnum.WorldEnum.CharactersType.CHARACTER_LUNA);
                    break;
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_RUSH:
                    LoadItemList(block, EngineEnum.WorldEnum.CharactersType.CHARACTER_RUSH);
                    break;
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_TIPPY:
                    LoadItemList(block, EngineEnum.WorldEnum.CharactersType.CHARACTER_TIPPY);
                    break;
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_KLAUS: // There is still no avaiable items for klaus
                case EngineEnum.WorldEnum.CharactersType.CHARACTER_KARA: // There is still no avaiable items for kara
                    break;
            }

            return block;
        }

        /// <summary>
        /// Send level infos to the client.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="license"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static byte[] LevelInfo_Buff(Int32 level, Int32 license, Int32 exp)
        {
            byte[] block = new byte[0x29]; // Create the packet's data base
            byte[] c_block = EngineUtils.PacketUtils.calcPacket(0x29, 0x831); // Calc the packet's header

            // Initializing the packet writer
            PacketWriter PW = new PacketWriter(block);

            // Write the packet's header
            PW.WriteByteArray(0, c_block);

            // Write the packet's data
            PW.WriteString(5, EngineEnum.PacketEnum.PacketCommand.success_0);
            PW.WriteInt32(29, exp);
            PW.WriteInt32(33, level);
            PW.WriteInt32(37, license);

            return block;
        }
    }
}
