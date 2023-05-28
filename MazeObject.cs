using System.Drawing;
using System.Windows.Forms;

namespace Maze
{
    public class MazeObject
    {
        public enum MazeObjectType { Hall, Wall, Medal, Enemy, Player, Pill, Energy, Bomb, Detonation };
        private static Size size = new Size(16, 16);
        public static Bitmap[] images = {new Bitmap(@"image\hall.png"),
                                         new Bitmap(@"image\wall.png"),
                                         new Bitmap(@"image\medal.png"),
                                         new Bitmap(@"image\enemy.png"),
                                         new Bitmap(@"image\player.png"),
                                         new Bitmap(@"image\pill.png"),
                                         new Bitmap(@"image\energy.png"),
                                         new Bitmap(@"image\bomb.png"),
                                         new Bitmap(@"image\detonation.png")};
       
        private MazeObjectType type;
        private Image texture;
        private PictureBox pictureBox;


        public MazeObject(MazeObjectType type)
        {
            pictureBox = new PictureBox();
            ChangeBackgroundImage(type);
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

        public static Size Size
        {
            get => size;
            set => size = value;
        }

        public Image Texture
        {
            get => texture;
            set => texture = value;
        }

        public PictureBox PictureBox => pictureBox;

        public void ChangeBackgroundImage(MazeObjectType type)
        {
            // меняем текстуру
            Type = type;
            pictureBox.BackgroundImage = Texture;
        }
    }
}
