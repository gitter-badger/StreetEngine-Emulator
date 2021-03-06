﻿/*
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
    using System.IO;
    using System.Threading;
    using SilverSock;

    public class mmoClient
    {
        private static SilverSocket Socket { get; set; }

        public static void evnt(String msg) { EngineConsole.Log.Infos(msg); }
        public static void error(String msg) { EngineConsole.Log.Error(msg); }
        public static void debug(String msg) { EngineConsole.Log.Debug(msg); }

        public mmoClient(SilverSocket socket)
        {
            Socket = socket;
            Socket.OnDataArrivalEvent += new SilverEvents.DataArrival(SocketOnDataArrivalEvent);
            Socket.OnSocketClosedEvent += new SilverEvents.SocketClosed(SocketOnClosedEvent);
            Socket.OnConnected += new SilverEvents.Connected(SocketOnConnected);
        }

        /// <summary>
        /// Socket connected event, you can put anything there.
        /// </summary>
        public void SocketOnConnected() { }

        /// <summary>
        /// Socket closed event, you can put anything there.
        /// </summary>
        public void SocketOnClosedEvent()
        {
            if (mmoServer.Clients.Contains(this))
                mmoServer.Clients.Remove(this);
            Socket.CloseSocket();
        }

        /// <summary>
        /// Socket data arrival event, handle game's data.
        /// </summary>
        /// <param name="data"></param>
        private void SocketOnDataArrivalEvent(byte[] data)
        {
            try 
            { 
                EngineGame.Packet.PacketHandle.HandleData(data, this); 
            }
            catch (Exception ex) { error(ex.ToString()); }
        }

        /// <summary>
        /// Send a packet buff.
        /// </summary>
        /// <param name="Packet"></param>
        public void Send(byte[] Packet)
        {
            try 
            { 
                Socket.Send(Packet); 
            }
            catch (Exception ex) { error(ex.ToString()); }
        }
    }
}