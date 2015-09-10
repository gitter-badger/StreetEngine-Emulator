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

    public class WorldEnum
    {
        public enum CharactersType
        {
            CHARACTER_LUNA = 0,
            CHARACTER_ROOKIE = 3,
            CHARACTER_RUSH = 2,
            CHARACTER_KARA = 4,
            CHARACTER_KLAUS = 5,
            CHARACTER_TIPPY = 1,
        }

        public enum TrickCode
        {
            GRIND = 1000,
            BACK_FLIP = 1100,
            FRONT_FLIP = 1200,
            AIR_TWIST = 1300,
            POWER_SWING = 1400,
            GRIP_TURN = 1500,
            DASH = 1600,
            BACK_SKATING = 1700,
            JUMPING_STEER = 1800,
            BUTTING = 1900,
            POWER_SLIDE = 2000,
            POWER_JUMP = 2200,
            WALL_RIDE = 5000,
        }
    }
}
