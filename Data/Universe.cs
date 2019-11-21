using System.Collections.Generic;

namespace BlazapyBird.Universe
{
    public class Universe
    {
        public const int FPS = 30;
        public const int SCREENWIDTH  = 288;

        public const int SCREENHEIGHT = 512;

        public const int PIPEGAPSIZE  = 100; // gap between upper and lower part of pipe
        public static double BASEY => SCREENHEIGHT * 0.79;

        //# image, sound and hitmask  dicts
        //IMAGES, SOUNDS, HITMASKS = {}, {}, {}

        //list of all possible players (tuple of 3 positions of flap)
        public IEnumerable<IEnumerable<string>> PLAYERS_LIST = new []
        {
            // red bird
            new []
            {
                "assets/sprites/redbird-upflap.png",
                "assets/sprites/redbird-midflap.png",
                "assets/sprites/redbird-downflap.png",
            },

            // blue bird
            new []
            {
                "assets/sprites/bluebird-upflap.png",
                "assets/sprites/bluebird-midflap.png",
                "assets/sprites/bluebird-downflap.png",
            },

            // yellow bird
            new []
            {
                "assets/sprites/yellowbird-upflap.png",
                "assets/sprites/yellowbird-midflap.png",
                "assets/sprites/yellowbird-downflap.png",
            },

        };


        // list of backgrounds
        IEnumerable<string> BACKGROUNDS_LIST = new []
        {
            "assets/sprites/background-day.png",
            "assets/sprites/background-night.png",
        };

        // list of pipes
        IEnumerable<string> PIPES_LIST = new []
        {
            "assets/sprites/pipe-green.png",
            "assets/sprites/pipe-red.png",
        };
    }
}