using System;
using System.Drawing;

namespace Maze
{
    public class MazeObject
    {
        public enum MazeObjectType { Hall, Wall, Medal, Enemy, Player, Pill, Energy };
        public static Bitmap[] images = {new Bitmap(@"image\hall.png"),
                                         new Bitmap(@"image\wall.png"),
                                         new Bitmap(@"image\medal.png"),
                                         new Bitmap(@"image\enemy.png"),
                                         new Bitmap(@"image\player.png"),
                                         new Bitmap(@"image\pill.png"),
                                         new Bitmap(@"image\energy.png")};

        private MazeObjectType type;
        private int width;
        private int height;
        private Image texture;

        public MazeObject(MazeObjectType type)
        {
            Type = type;
            Width = 16;
            Height = 16;
        }

        public MazeObjectType Type
        {
            get => type;
            set
            {
                type = value;
                Texture = images[(int)type];
            }
        }

        public int Width
        {
            get => width;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Error value (width)!");
                width = value;
            }
        }

        public int Height
        {
            get => height;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Error value (height)!");
                height = value;
            }
        }

        public Image Texture
        {
            get => texture;
            set => texture = value;
        }
    }
}
