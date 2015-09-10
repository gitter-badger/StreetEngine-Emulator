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
namespace StreetEngine.EngineDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using MySql.Data.MySqlClient;

    public class DatabaseManager
    {
        public static MySqlConnection mysql_connection;
        public static MySqlCommand mysql_command;
        public static MySqlDataReader Reader;

        public static List<String> usersList = new List<String>();
        public static List<String> tablesList = new List<String>();

        public static EngineWorld.Player.Info[] Player;

        public static Int32 id = -1;

        public static void infos(String message) { EngineConsole.Log.Infos(message); }
        public static void debug(String message) { EngineConsole.Log.Debug(message); }
        public static void error(String message) { EngineConsole.Log.Error(message); }
        public static void stage(String message) { EngineConsole.Log.Stage(message); }

        /// <summary>
        /// Execute a SQL command.
        /// </summary>
        /// <param name="command"></param>
        public void ExecuteCommand(string command)
        {
            mysql_command.CommandText = command;
            mysql_command.ExecuteNonQuery();
        }

        /// <summary>
        /// Update any row of the desired player's id with any value.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        public void UpdateTable(string row, string value)
        {
            ExecuteCommand("Update sg_account SET " + row + "='" 
                + value + "' WHERE id='" + Player[id].id + "'");
        }

        /// <summary>
        /// Open connection to the database and connect to the sg_account sql file.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="database_user"></param>
        /// <param name="database_password"></param>
        /// <param name="database_name"></param>
        public void OpenConnection()
        {
            mysql_connection = new MySqlConnection(EngineConfig.IniConfig.IniValues.sql_connection.Replace(":","=")); // Initialize connection
            mysql_command = new MySqlCommand("SELECT * FROM `" + EngineConfig.IniConfig.IniValues.sql_backup.Replace(".sql", "") + "`"); // You can change the sql file's name if you want

            mysql_command.Connection = mysql_connection; // Initalize connection

            Reader = null; // We must null the reader before opening any connection

            try { 
                mysql_connection.Open(); // Open the connection
                Reader = mysql_command.ExecuteReader(); // And finally initalize our reader

                if (Reader.Read()) { // Reader is reading so we must close it once its finished
                    usersList.Add(Reader["id"].ToString()); // Add every ids to our user's list
                    Player = new EngineWorld.Player.Info[usersList.Count]; // Initialize player's struct for existing users
                }

                infos("Connection to '" + EngineConfig.IniConfig.IniValues.sql_backup + "' etablished"); // Just to make sure we know what database we are using
                infos("'" + usersList.Count + "' Users loaded"); // Show how many people exist in the database

            Reader.Close(); // Close the reader
            }
            catch (Exception x) { error(x.ToString()); }
        }

        /// <summary>
        /// Check if the account exist in the database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void CheckUserAccount(string username, string password)
        {
            Reader.Close();

            try
            {
                CheckUser:
                {
                    Reader = mysql_command.ExecuteReader(); // Initalize our reader

                    if (!Reader.IsClosed)
                    {
                        if (Reader.Read())
                        { // Reader is reading so we must close it once its finished
                            if (username == (EngineDatabase.DatabaseManager.Reader["user"].ToString())
                                && password == (EngineDatabase.DatabaseManager.Reader["password"].ToString())) // Check the user in the database
                            {
                                id = (Int32)Reader["id"]; // Store the player's id into Int32 var

                                Player[id].id = (Int32)Reader["id"]; ; // Store the player's id into player struct
                                Player[id].isFirstLogin = (Int32)Reader["first_login"]; // Store the first_login boolean into the player struct

                                switch (Player[id].isFirstLogin) // Check if the player already connected before
                                {
                                    case 0: // He did not connected before
                                        InsertEntries(); // Create a new account in the database (ingame part is still in progress...)
                                        break;
                                    case 1: // He connected before
                                        ReadEntries();  // Load existing account data
                                        break;
                                }
                            }
                            else {
                                Player[id].isConnected =
                                    false;
                            }
                            mysql_connection.Close();
                        }
                    }
                }

                switch(mysql_connection.State)
                {
                    case ConnectionState.Open:
                        goto CheckUser; // Check user account
                    case ConnectionState.Closed:
                        if (Player[id].isConnected)
                            error("User already logged."); // Well its not considered as error anymore...
                        else {
                            mysql_connection.Open();
                            goto CheckUser; // Check user account
                        }
                        break;
                    case ConnectionState.Broken:
                        error("Connection between MySQL and StreetEngine is broken.");
                        break;     
                }
            }
            catch (Exception x) { error(x.ToString());  }
        }

        /// <summary>
        /// Read entries of the database and store them in the player struct.
        /// </summary>
        public void ReadEntries()
        {
            Player[id].id = (Int32)Reader["id"];
            Player[id].auth_key = (String)Reader["auth_key"].ToString();
            Player[id].isFirstLogin = (Int32)Reader["first_login"];
            Player[id].rank = (String)Reader["char_rank"].ToString();
            Player[id].level = (Int32)Reader["char_level"];
            Player[id].type = (Int32)Reader["char_type"];
            Player[id].exp = (Int32)Reader["char_exp"];
            Player[id].liscence = (Int32)Reader["char_liscence"];
            Player[id].gpotatos = (Int32)Reader["char_gpotatos"];
            Player[id].rupees = (Int32)Reader["char_rupees"];
            Player[id].coins = (Int32)Reader["char_coins"];
            Player[id].questpoints = (Int32)Reader["char_questpoints"];
            Player[id].bio = (String)Reader["char_bio"];
            Player[id].s_zone = (String)Reader["char_zoneinfo"];
            Player[id].clan_id = (String)Reader["char_clanid"];
            Player[id].clan_name = (String)Reader["char_clanname"];

            switch(Player[id].rank)
            {
                case "Admin":
                    Player[id].username = "[AM] " + (String)Reader["username"].ToString();
                    break;
                case "GameMaster":
                    Player[id].username = "[GM] " + (String)Reader["username"].ToString();
                    break;
                case "Player":
                    Player[id].username = "" + (String)Reader["username"].ToString();
                    break;
                case "Bot":
                    Player[id].username = "Bot";
                    break;
                case "Banned":
                    break; // Still in progress
            }

            stage("Auth Recv");
            debug("[" + Player[id].auth_key + "]" + " has logged");

            Player[id].isConnected = true; // Connect the player
        }

        /// <summary>
        /// Insert "start" values to the new account created.
        /// </summary>
        public void InsertEntries()
        {
            Player[id].auth_key = EngineWorld.Player.GenerateAuthKey(); // Generate player's unique session key

            Reader.Close(); // Close the reader to be able to use UpdateTable function

            UpdateTable("auth_key", Player[id].auth_key);
            UpdateTable("first_login", EngineEnum.LoginEnum.LoginStatus.second_time);
            UpdateTable("char_type", EngineConfig.IniConfig.IniValues.type.ToString());
            UpdateTable("char_level", EngineConfig.IniConfig.IniValues.level.ToString());
            UpdateTable("char_liscence", EngineConfig.IniConfig.IniValues.liscence.ToString());
            UpdateTable("char_gpotatos", EngineConfig.IniConfig.IniValues.gpotatos.ToString());
            UpdateTable("char_rupees", EngineConfig.IniConfig.IniValues.rupees.ToString());
            UpdateTable("char_coins", EngineConfig.IniConfig.IniValues.coins.ToString());
            UpdateTable("char_questpoints", EngineConfig.IniConfig.IniValues.questpoints.ToString());

            Player[id].isConnected = true; // Connect the player

            Reader = mysql_command.ExecuteReader(); // Open the MySQL Reader again

            stage("Auth Recv");
            debug("[" + Player[id].auth_key + "]" + " has logged"); 
        }
    }
}
