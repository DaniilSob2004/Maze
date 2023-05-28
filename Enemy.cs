using System;
using System.Drawing;
using System.Windows.Forms;
using static Maze.MazeObject;

namespace Maze
{
    public class Enemy
    {
        public enum Direction { Up, Down, Left, Right };
        private static Labirint l;

        private Point location;
        private Point nextLocation;
        private Timer t;
        private Direction direction;
        private bool isHitPlayer;


        public Enemy(Point location = new Point())
        {
            Location = location;
            SettingsTimer();
            direction = (Direction)Labirint.r.Next(4);
            isHitPlayer = false;
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

        public void StopMoving()
        {
            t.Stop();
        }


        private void SettingsTimer()
        {
            t = new Timer();
            t.Interval = Labirint.r.Next(500, 700);
            t.Tick += T_Tick;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            NextLocation();

            if (CheckCollision())
            {
                ChangeDirection();
            }

            if (isHitPlayer)
            {
                Delete();
                isHitPlayer = false;
                l.CheckEndGame();
            }
        }

        private void NextLocation()
        {
            switch (direction)
            {
                case Direction.Up:
                    nextLocation.X = location.X;
                    nextLocation.Y = location.Y - 1;
                    break;

                case Direction.Down:
                    nextLocation.X = location.X;
                    nextLocation.Y = location.Y + 1;
                    break;

                case Direction.Left:
                    nextLocation.X = location.X - 1;
                    nextLocation.Y = location.Y;
                    break;

                case Direction.Right:
                    nextLocation.X = location.X + 1;
                    nextLocation.Y = location.Y;
                    break;
            }
        }

        private void ChangeDirection()
        {
            int num;
            do
            {
                num = Labirint.r.Next(4);
                if (num != (int)direction) break;
            } while(num == (int)direction);  // пока направление которое было, присвоилось снова
            direction = (Direction)num;
        }

        private bool CheckCollision()
        {
            MazeObjectType type = l.Maze[nextLocation.Y, nextLocation.X].Type;
            switch (type)
            {
                case MazeObjectType.Hall:
                    Move();
                    return false;

                case MazeObjectType.Player:
                    l.EnemyHitPlayer(location);
                    t.Stop();
                    isHitPlayer = true;
                    return false;

                default:
                    return true;
            }
        }

        private void Move()
        {
            l.Maze[location.Y, location.X].ChangeBackgroundImage(MazeObjectType.Hall);  // очищаем
            l.Maze[nextLocation.Y, nextLocation.X].ChangeBackgroundImage(MazeObjectType.Enemy);  // отображаем
            location.X = nextLocation.X;  // обновляем координаты врага
            location.Y = nextLocation.Y;
        }

        private void Delete()
        {
            l.Maze[location.Y, location.X].ChangeBackgroundImage(MazeObjectType.Hall);  // очищаем
        }

        public void StartMoving()
        {
            t.Start();
        }
    }
}
