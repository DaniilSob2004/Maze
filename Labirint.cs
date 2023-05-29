using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using static Maze.MazeObject;

namespace Maze
{
    public class Labirint
    {
        public static Random r = new Random();
        private static Labirint labirint = null;
        private static ToolTip toolTip = new ToolTip();

        public enum GameValue { MaxHealth = 100, LossHealth = 20, AddHealth = 10, MaxEnergy = 500, LossEnergy = 1, AddEnergy = 25, BombPlanted = 50 };
        public readonly Point startPoint;  // координаты начала лабиринта
        public readonly Point finalPoint;  // координаты конца лабиринта

        private int height;  // высота лабиринта
        private int width;  // ширина лабиринта

        private Form parent;  // родитель
        private Player player;
        private MazeObject[,] maze;  // массив элементов лабиринта
        private List<Enemy> enemies;  // список врагов


        private Labirint(Form parent, int width, int height, int sizeElem)
        {
            this.width = width;
            this.height = height;
            this.parent = parent;
            MazeObject.Size = new Size(sizeElem, sizeElem);

            finalPoint = new Point(width - 1, height - 3);
            startPoint = new Point(0, 2);

            player = new Player();
            maze = new MazeObject[height, width];
            enemies = new List<Enemy>();

            StartSettings();
        }


        public MazeObject[,] Maze => maze;
        public Player Player => player;


        // Паттерн ОДИНОЧКА
        public static Labirint GetInstance(Form parent = null, int width = 0, int height = 0, int sizeElem = 0)
        {
            if (labirint == null)
            {
                labirint = new Labirint(parent, width, height, sizeElem);
            }
            return labirint;
        }


        private void StartSettings()
        {
            player.Location = startPoint;
            player.StartSettings();
            enemies.Clear();

            Generate();
        }

        private void SettingToolTip()
        {
            toolTip.IsBalloon = true;
            toolTip.ToolTipIcon = ToolTipIcon.Info;
            toolTip.InitialDelay = 500;
            toolTip.ToolTipTitle = "Подсказка";
            toolTip.SetToolTip(maze[startPoint.Y, startPoint.X].PictureBox, "Старт игрока");  // настраиваем подсказку для старта
            toolTip.SetToolTip(maze[finalPoint.Y, finalPoint.X].PictureBox, "Выход");  // настраиваем подсказку для выхода
        }

        private void Generate()
        {
            // генерация лабиринта
            bool isEnemy = false;

            bool isFirstGen = false;  // является ли это первая генерация
            if (maze[0, 0] == null) isFirstGen = true;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    MazeObjectType current = MazeObjectType.Hall;

                    if (y == 0 || x == 0 || y == height - 1 || x == width - 1)  // периметр лабиринта
                    {
                        if (x == player.Location.X && y == player.Location.Y) current = MazeObjectType.Player;  // персонаж
                        else if (x == width - 1 && y == height - 3) current = MazeObjectType.Hall;  // пустота для выхода
                        else current = MazeObjectType.Wall;  // стена
                    }

                    else if ((x == player.Location.X + 1 && y == player.Location.Y) || (x == width - 2 && y == height - 3))  // пустота напротив игрока и выхода
                    {
                        current = MazeObjectType.Hall;
                    }

                    else
                    {
                        if (r.Next(140) == 0)  // враг
                        {
                            current = MazeObjectType.Enemy;
                            isEnemy = true;
                        }

                        else if (r.Next(200) == 0)  // энергетик
                        {
                            current = MazeObjectType.Energy;
                        }

                        else if (r.Next(200) == 0)  // лекарство
                        {
                            current = MazeObjectType.Pill;
                        }

                        else if (r.Next(75) == 0)  // медаль
                        {
                            current = MazeObjectType.Medal;
                            player.AllPlayersMedal++;
                        }

                        else if (r.Next(4) == 0)  // стена
                        {
                            current = MazeObjectType.Wall;
                        }
                    }

                    if (isFirstGen)  // если первая генерация
                    {
                        maze[y, x] = new MazeObject(current);
                        maze[y, x].PictureBox.Location = new Point(x * MazeObject.Size.Width, y * MazeObject.Size.Height);
                        maze[y, x].PictureBox.Parent = parent;
                        maze[y, x].PictureBox.Size = MazeObject.Size;
                        maze[y, x].PictureBox.Visible = false;
                    }
                    else maze[y, x].ChangeBackgroundImage(current);

                    if (isEnemy)  // если сгенерился враг
                    {
                        enemies.Add(new Enemy(new Point(x, y)));  // добавляем в список
                        isEnemy = false;
                    }
                }
            }
        }

        public void Show()
        {
            // отображаем лабиринт
            if (!maze[0, 0].PictureBox.Visible)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        maze[y, x].PictureBox.Visible = true;
                    }
                }
                SettingToolTip();  // настройка подсказки
            }
            ShowInfo();  // вывод статистики в заголовок окна
            StartMovingEnemies();  // запуск движения врагов
        }

        public void ShowInfo()
        {
            // статистика
            parent.Text = $"Maze   (Медалей:  {player.PlayersMedal}/{player.AllPlayersMedal},  Здоровье: {player.PlayersHealth}%,  Энергия: {player.PlayersEnergy},  Врагов: {enemies.Count})";
        }


        public void MovePLayer(KeyEventArgs e)
        {
            int playerX = player.Location.X;
            int playerY = player.Location.Y;

            // если на следующей координате нет столкновения, то двигаем и меняем координаты
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (player.CheckCollision(playerX, playerY - 1))
                    {
                        player.Move(playerX, playerY - 1);
                        player.SetY(playerY - 1);
                    }
                    break;

                case Keys.Down:
                    if (player.CheckCollision(playerX, playerY + 1))
                    {
                        player.Move(playerX, playerY + 1);
                        player.SetY(playerY + 1);
                    }
                    break;

                case Keys.Left:
                    if (player.CheckCollision(playerX - 1, playerY))
                    {
                        player.Move(playerX - 1, playerY);
                        player.SetX(playerX - 1);
                    }
                    break;

                case Keys.Right:
                    if (player.CheckCollision(playerX + 1, playerY))
                    {
                        player.Move(playerX + 1, playerY);
                        player.SetX(playerX + 1);
                    }
                    break;
            }

            if (player.IsHitEnemy)  // если игрок попал на врага
            {
                DelEnemy(player.Location);  // удаляем врага из списка
                player.IsHitEnemy = false;
            }

            if (!CheckEndGame())  // проверка проигрыша
            {
                CheckDrawingBomb();  // проверка отрисовки бомбы
            }
        }


        public void BombPlanted()
        {
            // если бомба устанавливается НЕ на одно и тоже место
            if (!player.IsBombPlanted)
            {
                player.BombPlanted();  // устанавливаем бомбу
            }
        }

        private void CheckDrawingBomb()
        {
            if (player.IsBombPlanted)  // если бомба установлена
            {
                player.DrawingBomb();  // отрисовка бомбы
                player.IsBombPlanted = false;

                GameSound.CreateBomb();
            }
        }


        public void StartMovingEnemies()
        {
            // запуск всех врагов
            foreach (Enemy enemy in enemies) enemy.StartMoving();
        }

        private void EndMovingEnemies()
        {
            // остановка всех врагов
            foreach (Enemy enemy in enemies) enemy.StopMoving();
        }

        public void EnemyHitPlayer(Point enemyPoint)
        {
            player.LossHealth();  // отнимаем жизнь
            DelEnemy(enemyPoint);  // удаляем врага
            CheckEndGame();  // проверка конца игры
        }

        public void AddEnemy()
        {
            int randX, randY;
            bool flag = false;

            do
            {
                randX = r.Next(width);
                randY = r.Next(height);
                if (maze[randY, randX].Type == MazeObjectType.Hall)  // если это коридор
                {
                    maze[randY, randX].ChangeBackgroundImage(MazeObjectType.Enemy);  // меняем текстуру на врага
                    enemies.Add(new Enemy(new Point(randX, randY)));  // добавляем в список
                    enemies[enemies.Count - 1].StartMoving();  // враг движется
                    flag = true;
                }
            } while (!flag);
        }

        public void DelEnemy(Point p)
        {
            Enemy delEnemy = GetEnemyByLoacation(p);  // получаем по координатам объект врага
            delEnemy.StopMoving();  // останавливаем
            enemies.Remove(delEnemy);  // удаляем из списка

            ShowInfo();
        }

        public Enemy GetEnemyByLoacation(Point p)
        {
            // по координатам получаем объект врага
            foreach (Enemy enemy in enemies)
            {
                if (enemy.Location == p) return enemy;
            }
            return null;
        }


        public bool CheckEndGame()
        {
            // проверка проигрыша
            ShowInfo();

            if (player.PlayersHealth <= 0)
            {
                GameSound.Loss();
                GameRestart("Поражение - закончилось здоровье!");
            }

            else if (player.PlayersEnergy <= 0)
            {
                GameSound.Loss();
                GameRestart("Поражение - закончилась энергия!");
            }

            else if (finalPoint.X == player.Location.X && finalPoint.Y == player.Location.Y)
            {
                GameSound.Winner();
                GameRestart("Победа - найден выход!");
            }

            else if (player.PlayersMedal == player.AllPlayersMedal && player.AllPlayersMedal != 0)
            {
                GameSound.Winner();
                GameRestart("Победа - медали собраны!");
            }

            else if (enemies.Count == 0)
            {
                GameSound.Winner();
                GameRestart("Победа - враги уничтожены!");
            }

            else return false;

            return true;
        }

        public void GameRestart(string text)
        {
            // перезапуск лабиринта
            EndMovingEnemies();
            MessageBox.Show(text, "Message");
            StartSettings();
            Show();
        }
    }
}
