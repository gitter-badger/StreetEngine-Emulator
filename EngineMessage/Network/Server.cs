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
namespace StreetEngine.Engine.Network
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SilverSock;

    public class msgServer
    {
        private static SilverServer server;

        public static List<msgClient> Clients = new List<msgClient>();

        public static String ip = EngineConfig.IniConfig.IniValues.h_msg;
        public static Int32 port = EngineConfig.IniConfig.IniValues.p_msg;

        public static void evnt(String msg) { EngineConsole.Log.Infos(msg); }
        public static void error(String msg) { EngineConsole.Log.Error(msg); }
        public static void debug(String msg) { EngineConsole.Log.Debug(msg); }

        /// <summary>
        /// Start the server on the desired ip and port.
        /// </summary>
        public static void Start()
        {
            try
            {
                server = new SilverServer(ip, port);
                server.OnAcceptSocketEvent += new SilverEvents.AcceptSocket(ServerOnAcceptSocketEvent);
                server.OnListeningEvent += new SilverEvents.Listening(ServerOnListeningEvent);
                server.WaitConnection();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        /// <summary>
        /// Server accept socket event, create a new session for the socket.
        /// </summary>
        /// <param name="socket"></param>
        public static void ServerOnAcceptSocketEvent(SilverSocket socket)
        {
            try
            {
                msgClient session = new msgClient(socket);
            }
            catch (Exception ex) { error(ex.ToString()); }
        }

        /// <summary>
        /// Server is up, send a message.
        /// </summary>
        public static void ServerOnListeningEvent()
        {
            try
            {
                evnt("msg_server@" + ip + ":" + " server started");
            }
            catch (Exception ex) { error(ex.ToString()); }
        }
    }
}
