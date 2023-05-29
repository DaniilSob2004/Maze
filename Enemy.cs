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

        private Point location;  // координаты
        private Point nextLocation;  // следующая координата
        private Timer t;
        private Direction direction;  // направление
        private bool isHitPlayer;  // попал ли на игрока


        public Enemy(Point location)
        {
            Location = location;
            SettingsTimer();
            direction = (Direction)Labirint.r.Next(4);  // рандомно задаём направление
            isHitPlayer = false;
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

        public void StopMoving()
        {
            t.Stop();  // остановка таймера
        }

        public void StartMoving()
        {
            t.Start();  // запуск таймера
        }


        private void SettingsTimer()
        {
            t = new Timer();
            t.Interval = Labirint.r.Next(500, 700);  // рандомный интервал
            t.Tick += T_Tick;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            NextLocation();  // для подсчёта следующих координат

            if (CheckCollision())  // проверка столкновения
            {
                ChangeDirection();  // меняем направление
            }

            if (isHitPlayer)  // если попали на игрока
            {
                Delete();  // удаляем врага
                l.EnemyHitPlayer(location);
            }
        }

        private void NextLocation()
        {
            // подсчёта следующих координат
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
                if (num != (int)direction) break;  // если это другое направление
            } while(num == (int)direction);  // пока направление такое же, которое и было

            direction = (Direction)num;
        }

        private bool CheckCollision()
        {
            MazeObjectType type = l.Maze[nextLocation.Y, nextLocation.X].Type;
            switch (type)
            {
                case MazeObjectType.Hall:  // если следующий объект это коридор и это не начало и не выход
                    if (nextLocation != l.startPoint && nextLocation != l.finalPoint)
                    {
                        Move();  // двигаем
                        return false;
                    }
                    return true;

                case MazeObjectType.Player:  // если игрок
                    isHitPlayer = true;
                    StopMoving();  // останавливаем таймер
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
    }
}
