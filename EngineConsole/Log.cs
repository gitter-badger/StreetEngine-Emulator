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
namespace StreetEngine.EngineConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Diagnostics;
    using System.Text;

    public class Log
    {
        public static Object Locker = new object();
        public static Boolean DebugMode = true;

        public static Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
        public static FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(CurrentAssembly.Location);

        /// <summary>
        /// Draw Ascii art.
        /// </summary>
        public static void DrawAscii()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"                                                                            ");
            Console.WriteLine(@"            _____ _                 _   ______             _                ");
            Console.WriteLine(@"           / ____| |               | | |  ____|           (_)               ");
            Console.WriteLine(@"          | (___ | |_ _ __ ___  ___| |_| |__   _ __   __ _ _ _ __   ___     ");
            Console.WriteLine(@"           \___ \| __| '__/ _ \/ _ \ __|  __| | '_ \ / _` | | '_ \ / _ \    ");
            Console.WriteLine(@"          ____ ) | |_| | |  __/  __/ |_| |____| | | | (_| | | | | |  __/    ");
            Console.WriteLine(@"          |_____/ \__|_|  \___|\___|\__|______|_| |_|\__, |_|_| |_|\___|    ");
            Console.WriteLine(@"                                                      __/ |                 ");
            Console.WriteLine(@"                                                     |___/                  ");
            Console.WriteLine(@"                                                                            ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"                        StreetEngine Project Devs: iMaes                    ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"                      Credits: Epvp StreetGears Dev Emu Team                ");
            Console.WriteLine(@"                            Project Under Copyright                          ");
            Console.WriteLine(@"                                                                            ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"");
            Console.WriteLine(@"");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Append text on the console using Console.Write().
        /// </summary>
        /// <param name="header"></param>
        /// <param name="message"></param>
        /// <param name="headcolor"></param>
        /// <param name="line"></param>
        public static void Append(string header, string message, ConsoleColor headcolor, bool line = true)
        {
            lock (Locker)
            {
                if (line)
                    Console.Write("\n");

                Console.ForegroundColor = headcolor;
                Console.Write(header);
                Console.Write(" ");
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach (var c in message)
                {
                    if (c == 'è')
                    {
                        if (Console.ForegroundColor == ConsoleColor.Gray)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                    else
                    {
                        Console.Write(c);
                    }
                }
            }
        }

        /// <summary>
        /// Information message style.
        /// </summary>
        /// <param name="message"></param>
        public static void Infos(string message)
        {
            Append("Information>>", message, ConsoleColor.Yellow);//ConsoleColor.Green);
        }

        /// <summary>
        /// Error message style.
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            Append("Error>>", message, ConsoleColor.Red);
        }

        /// <summary>
        /// Debug message style.
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            if (DebugMode)
            {
                Append("Debug>>", message, ConsoleColor.Magenta);
            }
        }

        /// <summary>
        /// Stage message style.
        /// </summary>
        /// <param name="message"></param>
        public static void Stage(string stage)
        {
            Console.Write("\n\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("                 ================ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(stage);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" ================ ");
            Console.Write("\n");
        }
    }
}
