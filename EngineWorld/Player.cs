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
namespace StreetEngine.EngineWorld
{
    using System;

    public class Player
    {
        public static string GenerateAuthKey() 
        {
            return EngineUtils.ByteUtils.GenerateRandomKey((int)17);
        }

        public struct Info
        {
            public String auth_key { get; set; }
            public String username { get; set; }
            public String password { get; set; }
            public String email { get; set; }
            public String rank { get; set; }
            public String charname { get; set; }
            public String clan_id { get; set; }
            public String clan_name { get; set; }
            public String bio { get; set; }
            public String s_zone { get; set; }
            public Int32 isFirstLogin { get; set; }
            public Boolean isConnected { get; set; }
            public Boolean isInWorld { get; set; }
            public Int32 id { get; set; }
            public Int32 type { get; set; }
            public Int32 level { get; set; }
            public Int32 exp { get; set; }
            public Int32 liscence { get; set; }
            public Int32 gpotatos { get; set; }
            public Int32 rupees { get; set; }
            public Int32 coins { get; set; }
            public Int32 questpoints { get; set; }
            public Int32 age { get; set; }
            public Int32 c_zone { get; set; }
        };

        public class RankInfo
        {
            public const string
            rank_a = "Admin",
            rank_b = "GameMaster",
            rank_c = "Player",
            rank_d = "Banned",
            rank_e = "Bot";
        }
    }
}
