using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using static Maze.MazeObject;

namespace Maze
{
    public class Bomb
    {
        private static Labirint l;

        private Point location;
        private Timer t;
        private bool killPlayer;

        public Bomb(Point location = new Point())
        {
            Location = location;
            t = new Timer();
            killPlayer = false;
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


        public void StartTimerBeforeDetonation()
        {
            t.Interval = 2500;
            t.Tick -= T_AfterTick;
            t.Tick += T_BeforeTick;
            t.Start();
        }

        public void StartTimerAfterDetonation()
        {
            t.Interval = 1500;
            t.Tick -= T_BeforeTick;
            t.Tick += T_AfterTick;
            t.Start();
        }

        private void Detonation()
        {
            List<Point> pAroundBomb = GetPointsAroundBomb();

            for (int i = 0; i < pAroundBomb.Count; i++)
            {
                MazeObjectType type = l.Maze[pAroundBomb[i].Y, pAroundBomb[i].X].Type;
                l.Maze[pAroundBomb[i].Y, pAroundBomb[i].X].ChangeBackgroundImage(MazeObjectType.Detonation);

                if (type == MazeObjectType.Enemy)
                {
                    l.DelEnemy(new Point(pAroundBomb[i].X, pAroundBomb[i].Y));
                }
                else if (type == MazeObjectType.Player)
                {
                    killPlayer = true;
                }
            }
            GameSound.Detonation();
            StartTimerAfterDetonation();
        }

        private void AfterDetonation()
        {
            List<Point> pAroundBomb = GetPointsAroundBomb();
            l.Player.Bombs.RemoveAt(0);  // удаляем бомбу

            for (int i = 0; i < pAroundBomb.Count; i++)
            {
                l.Maze[pAroundBomb[i].Y, pAroundBomb[i].X].ChangeBackgroundImage(MazeObjectType.Hall);
            }

            if (killPlayer)
            {
                l.Player.PlayersHealth = 0;
                l.CheckEndGame();
            }
        }

        private void T_BeforeTick(object sender, System.EventArgs e)
        {
            ((Timer)sender).Stop();
            Detonation();
        }

        private void T_AfterTick(object sender, System.EventArgs e)
        {
            ((Timer)sender).Stop();
            AfterDetonation();
        }

        private List<Point> GetPointsAroundBomb()
        {
            // все точки вокруг бомбы, начиная с верхней левой
            var allAroundPoints = new Point[] { new Point(location.X - 1, location.Y - 1),
                                                new Point(location.X, location.Y - 1),
                                                new Point(location.X + 1, location.Y - 1),
                                                new Point(location.X + 1, location.Y),
                                                new Point(location.X + 1, location.Y + 1),
                                                new Point(location.X, location.Y + 1),
                                                new Point(location.X - 1, location.Y + 1),
                                                new Point(location.X - 1, location.Y),
                                                location};

            // точки которые нужно уничтожить
            var pAroundBomb = new List<Point>();
            for (int i = 0; i < allAroundPoints.Length; i++)
            {
                switch (l.Maze[allAroundPoints[i].Y, allAroundPoints[i].X].Type)
                {
                    case MazeObjectType.Wall:
                        break;

                    default:
                        pAroundBomb.Add(allAroundPoints[i]);
                        break;
                }
            }

            return pAroundBomb;
        }
    }
}
