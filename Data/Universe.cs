using System.Collections.Generic;
using System.Linq;

namespace BlazapyBird.Data
{
    public class Universe
    {
        public string CssStyle => $"width: {SCREENWIDTH}px; height: {SCREENHEIGHT}px; ";
        public const int FPS = 30;
        public const int SCREENWIDTH  = 288;

        public const int SCREENHEIGHT = 512;

        public const int PIPEGAPSIZE  = 100; // gap between upper and lower part of pipe
        public static double BASEY => SCREENHEIGHT * 0.79;

        //# image, sound and hitmask  dicts
        //IMAGES, SOUNDS, HITMASKS = {}, {}, {}

        public string CurrentBackgroundImage {get; set; } = BACKGROUNDS_LIST[0];

        public static Dictionary<string, string[]> IMAGESS =new Dictionary<string, string[]>() 
        {
            ["numbers"] = new [] {
                "assets/sprites/0.png",
                "assets/sprites/1.png",
                "assets/sprites/2.png",
                "assets/sprites/3.png",
                "assets/sprites/4.png",
                "assets/sprites/5.png",
                "assets/sprites/6.png",
                "assets/sprites/7.png",
                "assets/sprites/8.png",
                "assets/sprites/9.png",
            },            
        };

        public static Dictionary<string, string> IMAGES = new Dictionary<string, string>() 
        {
            ["gameover"] ="assets/sprites/gameover.png",
            ["message"] ="assets/sprites/gameover.png",
            ["base"] ="assets/sprites/gameover.png",
        };

        public static Dictionary<string, string> SOUNDS = new Dictionary<string, string>() 
        {
            ["die"] ="assets/audio/die.ogg",
            ["hit"] ="assets/audio/hit.ogg",
            ["point"] ="assets/audio/point.ogg",
            ["swoosh"] ="assets/audio/swoosh.ogg",
            ["swoosh"] ="assets/audio/wing.ogg",            
        };

        //list of all possible players (tuple of 3 positions of flap)
        public static string[][] PLAYERS_LIST = new []
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
        public static string[] BACKGROUNDS_LIST = new []
        {
            "assets/sprites/background-day.png",
            "assets/sprites/background-night.png",
        };

        // list of pipes
        public static string[] PIPES_LIST = new []
        {
            "assets/sprites/pipe-green.png",
            "assets/sprites/pipe-red.png",
        };
    }
}