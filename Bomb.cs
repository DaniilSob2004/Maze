using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using static Maze.MazeObject;

namespace Maze
{
    public class Bomb
    {
        private static Labirint l;

        private Point location;  // координаты
        private Timer t;
        private bool killPlayer;  // убит ли игрок

        public Bomb(Point location)
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
            l = Labirint.GetInstance();  // получаем объект Labirint, паттерн Одиночка
        }


        public void StartTimerBeforeDetonation()
        {
            // настройки таймера до взрыва
            t.Interval = 2500;
            t.Tick -= T_AfterTick;
            t.Tick += T_BeforeTick;
            t.Start();
        }

        public void StartTimerAfterDetonation()
        {
            // настройки таймера после взрыва
            t.Interval = 1500;
            t.Tick -= T_BeforeTick;
            t.Tick += T_AfterTick;
            t.Start();
        }

        private void Detonation()
        {
            List<Point> pAroundBomb = GetPointsAroundBomb();  // получаем координаты вокруг бомбы

            for (int i = 0; i < pAroundBomb.Count; i++)
            {
                MazeObjectType type = l.Maze[pAroundBomb[i].Y, pAroundBomb[i].X].Type;  // получаем тип объекта
                l.Maze[pAroundBomb[i].Y, pAroundBomb[i].X].ChangeBackgroundImage(MazeObjectType.Detonation);  // меняем текстуру на взрыв

                if (type == MazeObjectType.Enemy)  // если это враг, то удаляем
                {
                    l.DelEnemy(pAroundBomb[i]);
                }
                else if (type == MazeObjectType.Player)  // если это игрок, то убит
                {
                    killPlayer = true;
                }
            }
            GameSound.Detonation();
            StartTimerAfterDetonation();  // запуск таймера после взрыва
        }

        private void AfterDetonation()
        {
            List<Point> pAroundBomb = GetPointsAroundBomb();  // получаем координаты вокруг бомбы
            l.Player.Bombs.RemoveAt(0);  // удаляем бомбу

            for (int i = 0; i < pAroundBomb.Count; i++)
            {
                MazeObjectType type = l.Maze[pAroundBomb[i].Y, pAroundBomb[i].X].Type;  // получаем тип объекта
                l.Maze[pAroundBomb[i].Y, pAroundBomb[i].X].ChangeBackgroundImage(MazeObjectType.Hall);  // меняем текстуру на коридор

                if (type == MazeObjectType.Enemy)  // если это враг, то удаляем
                {
                    l.DelEnemy(pAroundBomb[i]);
                }
                else if (type == MazeObjectType.Player)  // если это игрок, то убит
                {
                    killPlayer = true;
                }
            }

            if (killPlayer)  // если игрок убит
            {
                l.Player.PlayersHealth = 0;
                l.CheckEndGame();  // проверка проигрыша
            }
        }

        private void T_BeforeTick(object sender, System.EventArgs e)
        {
            ((Timer)sender).Stop();  // останавливаем таймер
            Detonation();
        }

        private void T_AfterTick(object sender, System.EventArgs e)
        {
            ((Timer)sender).Stop();  // останавливаем таймер
            AfterDetonation();
        }

        private List<Point> GetPointsAroundBomb()
        {
            // все точки вокруг бомбы, начиная с верхней левой
            List<Point> pAroundBomb = new List<Point> { new Point(location.X - 1, location.Y - 1),
                                                        new Point(location.X, location.Y - 1),
                                                        new Point(location.X + 1, location.Y - 1),
                                                        new Point(location.X + 1, location.Y),
                                                        new Point(location.X + 1, location.Y + 1),
                                                        new Point(location.X, location.Y + 1),
                                                        new Point(location.X - 1, location.Y + 1),
                                                        new Point(location.X - 1, location.Y),
                                                        location};

            for (int i = 0; i < pAroundBomb.Count; i++)
            {
                switch (l.Maze[pAroundBomb[i].Y, pAroundBomb[i].X].Type)
                {
                    case MazeObjectType.Wall:  // если по этой координате находится стена, то удаляем из списка
                        pAroundBomb.RemoveAt(i);
                        i--;
                        break;
                }
            }

            return pAroundBomb;  // точки которые нужно уничтожить
        }
    }
}
