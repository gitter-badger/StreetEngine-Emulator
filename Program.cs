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
namespace StreetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using System.Threading;
    using System.IO;
    using System.Reflection;

    class Program
    {
        public static StreetEngine.EngineDatabase.DatabaseManager MySQL = new StreetEngine.EngineDatabase.DatabaseManager();

        static Dictionary<string, Assembly> _libs = new Dictionary<string, Assembly>();           

        public static void infos(String msg) { EngineConsole.Log.Infos(msg); }
        public static void error(String msg) { EngineConsole.Log.Error(msg); }
        public static void debug(String msg) { EngineConsole.Log.Debug(msg); }
        public static void stage(String msg) { EngineConsole.Log.Stage(msg); }

        public static void DrawAscii() { EngineConsole.Log.DrawAscii(); } 

        /// <summary>
        /// Embed DLLs to the exe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static Assembly FindDLL(object sender, ResolveEventArgs args)
        {
            string keyName = new AssemblyName(args.Name).Name;

            if (_libs.ContainsKey(keyName)) return _libs[keyName];

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StreetEngine." + keyName + ".dll"))
            {
                byte[] buffer = new BinaryReader(stream).ReadBytes((int)stream.Length);
                Assembly assembly = Assembly.Load(buffer);
                _libs[keyName] = assembly;
                return assembly;
            }
        }

        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.AssemblyResolve += FindDLL;

            // Draw Ascii Art
            DrawAscii();

            // Draw informations and stuff
            Console.Beep();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("StreetEngine Dev Console [version " + EngineConsole.Log.fileVersionInfo.FileVersion + "]");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("");

            // Load settings
            stage("Loading Settings");
            EngineConfig.IniConfig.LoadSettings();

            // Load MySQL stuff
            stage("Loading '" + EngineConfig.IniConfig.IniValues.sql_backup + "'");
            MySQL.OpenConnection();

            // Start every servers
            stage("Sarting Server");
            Engine.Network.Server.Start();
            Engine.Network.msgServer.Start();
            Engine.Network.lobbyServer.Start();
            Engine.Network.mmoServer.Start();

            // Start StreetGears
            Process.Start("StreetGear.exe", "/enc /locale:" + EngineEnum.LauncherEnum.Locale.locale_fr + " /auth_ip:" + EngineConfig.IniConfig.IniValues.h_auth + " /auth_port:" + EngineConfig.IniConfig.IniValues.p_auth + " /window /debug");

            Console.ReadKey();
        }
    }
}
