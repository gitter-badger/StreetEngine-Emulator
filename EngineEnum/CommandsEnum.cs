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

    public class CommandsEnum
    {
        public class CommandName
        {
            public static string
            command_a = ".setlevel",
            command_b = ".setname",
            command_c = ".setchartype",
            command_d = ".setgpotatos",
            command_e = ".setruppees",
            command_f = ".setcoins",
            command_g = ".setquestpoints";
        }

        public enum CommandLenght
        {
            command_a = 9,
        }
    }
}
