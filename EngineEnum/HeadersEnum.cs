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
namespace StreetEngine.EngineEnum
{
    using System;

    public class HeadersEnum
    {
        public class Recv
        {
            public enum GLOBAL_TCP_RECV // Auth, Ingame, Lobby, Quests
            {
                KEEP_ALIVE = 2000,
            }

            public enum TM_SC // Auth Recv
            {
                TM_SC_HASH_CERTIFICATION_RECV = 1002,
                TM_SC_WE_LOGIN_RECV = 1003,
                TM_SC_SERVER_LIST_RECV = 1004,
                TM_SC_SELECT_SERVER_RECV = 1006,
            }

            public enum BM_SC // World Recv
            {
                BM_SC_LOGIN = 2144,
                BM_SC_CREATE_CHARACTER = 2068, // Not sure about the name
                BM_SC_CHAT_MESSAGE = 2206,
                BM_SC_PLAYER_CHARACTER_LIST = 2317,
                BM_SC_SELECT_CHARACTER = 2066,
                BM_SC_PLAYER_INFO = 2313,
                BM_SC_TRICK_LIST = 2104,
                BM_SC_BALANCE_INFO = 2094,
                BM_SC_GET_CHANNEL_LIST = 2005,
                BM_SC_CASH_BALANCE_INFO = 2271,
                BM_SC_ENTER_CHANNEL = 2007,
                BM_SC_LEAVE_CHANNEL = 2009,
                BM_SC_SET_SESSION_MESSAGE = 2120,
                BM_SC_MISSION_LIST = 2072,
                BM_SC_INVENTORY = 2098,
                BM_SC_MINI_GAME_START = 2048,
                BM_SC_MINI_GAME_END = 2050,
                BM_SC_LEAVE_INVENTORY = 2080,
                BM_SC_ENTER_INVENTORY = 2078,
                BM_SC_DELETE_ITEM = 2102,
                BM_SC_SELECT_ITEM = 2100,
                BM_SC_MATE_INFO = 2335,
                BM_SC_SELECT_TRICK = 2108,
                BM_SC_EXCHANGE_MONEY = 2152,
                BM_SC_CREATE_ROOM = 2173,
            }

            public enum ID_BZ_SC // Lobby Room Recv
            {
                ID_BZ_SC_ENTER_LOBBY = 2275, // Still not sure about the name
                ID_BZ_SC_ENTER_LOBBY_ROOM = 19018, // Still not sure about the name
                ID_BZ_SC_UNKNOW_LOBBY = 1298, // Still not sure about the name, something with lobby..
            }
        }

        public class Send
        {
            public enum TM_SC_RESULT // Error messages
            {
                NOTHING = 0,
                MSG_SERVER_NOT_EXIST = 1,
                MSG_SERVER_DENIED = 6,
                MSG_SERVER_ALREADY_EXIST = 9,
                MSG_SERVER_RETRY = 50,
                MSG_FAIL_WEB_ID = 60,
                AURORA_RESULT_REPETITION = 70,
            }

            public enum BM_SC // World Send
            {
                BM_SC_LOGIN = 2145,
                BM_SC_CREATE_CHARACTER = 2069,
                BM_SC_SELECT_CHARACTER = 2067,
                BM_SC_CHAT_MESSAGE = 2207,
                BM_SC_SELECT_TRICK = 2109,
                BM_SC_ENTER_INVENTORY = 2079,
                BM_SC_LEAVE_CHANNEL = 2010,
                BM_SC_LEAVE_INVENTORY = 2081,
                BM_SC_CREATE_ROOM = 2174,
            }
            
            public enum ID_BZ_SC // Lobby Send
            {
                ID_BZ_SC_ENTER_LOBBY = 2276,
                ID_BZ_SC_ENTER_LOBBY_ROOM = 19019,
                ID_BZ_SC_UNKNOW_LOBBY = 1299,
            }
        }
    }
}
