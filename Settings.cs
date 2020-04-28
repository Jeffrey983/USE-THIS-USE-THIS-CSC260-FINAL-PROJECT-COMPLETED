using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    public class Settings
    {
        public static int Width { get; set; } //these are pretty self explanatory
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static bool GameOver { get; set; }
        public static Direction Snakedirection { get; set; } //snake direction

        public Settings()
        {
            Width = 16;
            Height = 16;
            Speed = 15;
            Score = 0;
            Points = 100;
            GameOver = false;
            Snakedirection = Direction.Down;
        }
    }


}