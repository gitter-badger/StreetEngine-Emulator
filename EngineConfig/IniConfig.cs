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
namespace StreetEngine.EngineConfig
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    public class IniConfig
    {
        public static List<String> keysList = new List<String>();
        public static List<String> sectionsList = new List<String>();

        public static void infos(String message) { EngineConsole.Log.Infos(message); }
        public static void debug(String message) { EngineConsole.Log.Debug(message); }
        public static void error(String message) { EngineConsole.Log.Error(message); }

        public struct IniValues
        {
            public static Int32 gpotatos { get; set; } 
            public static Int32 rupees { get; set; }
            public static Int32 questpoints { get; set; }
            public static Int32 coins { get; set; }
            public static Int32 level { get; set; }
            public static Int32 liscence { get; set; }
            public static Int32 type { get; set; }
            public static String h_auth { get; set; }
            public static String h_world { get; set; }
            public static String h_lobby { get; set; }
            public static String h_msg { get; set; }
            public static Int32 p_auth { get; set; }
            public static Int32 p_world { get; set; }
            public static Int32 p_lobby { get; set; }
            public static Int32 p_msg { get; set; }
            public static String sql_connection { get; set; }
            public static String sql_backup { get; set; }
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public class IniKeys
        {
            public static string
            gpotatos = "startGpotatos",
            rupees = "startRupees",
            questpoints = "startQuestPoints",
            coins = "startCoins",
            level = "startLevel",
            liscence = "startLiscence",
            type = "startType",
            host = "Host",
            port = "Port",
            connection = "SQLConnection",
            backup = "SQLBackup";
        }

        [StructLayout(LayoutKind.Sequential)]
        public class IniSections
        {
            public static string
            world = "WorldSettings",
            auth = "Auth",
            game = "World",
            lobby = "Lobby",
            msg = "Msg",
            db = "Database";
        }

        /// <summary>
        /// Load confing.ini settings into a struct.
        /// </summary>
        public static void LoadSettings()
        {
            var INI = new IniFile("StreetEngine.ini"); // Load the settings file

            IniKeys keys = new IniKeys();
            IniSections sections = new IniSections();

            FieldInfo[] f_keys = keys.GetType().GetFields();
            FieldInfo[] f_sections = sections.GetType().GetFields();

            // IPs
            IniValues.h_auth = INI.Read(IniKeys.host, IniSections.auth); // IP of "Auth Server"
            IniValues.h_world = INI.Read(IniKeys.host, IniSections.game); // IP of "World Server"
            IniValues.h_lobby = INI.Read(IniKeys.host, IniSections.lobby); // IP of "Lobby Server"
            IniValues.h_msg = INI.Read(IniKeys.host, IniSections.msg); // IP of "Msg Server"

            // Ports
            IniValues.p_auth = int.Parse(INI.Read(IniKeys.port, IniSections.auth)); // Port of "Auth Server"
            IniValues.p_world = int.Parse(INI.Read(IniKeys.port, IniSections.game)); // Port of "World Server"
            IniValues.p_lobby = int.Parse(INI.Read(IniKeys.port, IniSections.lobby)); // Port of "Lobby Server"
            IniValues.p_msg = int.Parse(INI.Read(IniKeys.port, IniSections.msg)); // Port of "Msg Server"

            // Ingame stuff
            IniValues.gpotatos = int.Parse(INI.Read(IniKeys.gpotatos, IniSections.world)); // Gpotatos start number
            IniValues.rupees = int.Parse(INI.Read(IniKeys.rupees, IniSections.world)); // Rupees start number
            IniValues.questpoints = int.Parse(INI.Read(IniKeys.questpoints, IniSections.world)); // Questpoints start number
            IniValues.coins = int.Parse(INI.Read(IniKeys.coins, IniSections.world)); // Coins start number
            IniValues.level = int.Parse(INI.Read(IniKeys.level, IniSections.world)); // Level start number
            IniValues.type = int.Parse(INI.Read(IniKeys.type, IniSections.world)); // Char type start number
            IniValues.liscence = int.Parse(INI.Read(IniKeys.liscence, IniSections.world)); // Liscence start number

            // SQL Stuff
            IniValues.sql_backup = INI.Read(IniKeys.backup, IniSections.db); // SQL Backup file name
            IniValues.sql_connection = INI.Read(IniKeys.connection, IniSections.db); // SQL Connection string

            // I just use this to know how much keys are loaded
            foreach (var _keys in f_keys) keysList.Add(_keys.GetValue(keys).ToString());

            // I just use this to know how much sections are loaded
            foreach (var _sections in f_sections) sectionsList.Add(_sections.GetValue(sections).ToString());

            // Message... (Feel free to delete them)
            infos("'" + keysList.Count + "' Keys loaded");
            infos("'" + sectionsList.Count + "' Sections loaded");
        }
    }
}
