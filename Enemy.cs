using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class Enemy
    {
        private static Labirint l;
        private Point location;

        public Enemy(Point location = new Point())
        {
            Location = location;
        }

        public Point Location
        {
            get => location;
            set => location = value;
        }

        public static void InitialLabirint()
        {
            l = Labirint.GetInstance();
        }
    }
}
