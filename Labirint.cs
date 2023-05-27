﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using static Maze.MazeObject;

namespace Maze
{
    public class Labirint
    {
        public static Random r = new Random();

        public enum GameValue { MaxHealth = 100, LossHealth = 25, AddHealth = 10, MaxEnergy = 500, LossEnergy = 1, AddEnergy = 25, BombPlanted = 50 };
        public readonly Point finalPoint;  // координаты конца лабиринта
        private static Labirint labirint = null;

        private int height;  // высота лабиринта
        private int width;  // ширина лабиринта

        private Form parent;  // родитель
        private Player player;
        private MazeObject[,] maze;
        private List<Enemy> enemies;


        private Labirint(Form parent, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.parent = parent;

            player = new Player();
            maze = new MazeObject[height, width];
            finalPoint = new Point(width - 1, height - 3);
            enemies = new List<Enemy>();

            StartSettings();
        }


        public MazeObject[,] Maze => maze;
        public Point FinalPoint => finalPoint;
        public Player Player => player;


        public static Labirint GetInstance(Form parent = null, int width = 0, int height = 0)
        {
            if (labirint == null)
            {
                labirint = new Labirint(parent, width, height);
            }
            return labirint;
        }


        private void StartSettings()
        {
            parent.Controls.Clear();

            player.Location = new Point(0, 2);
            player.StartSettings();
            enemies.Clear();

            Generate();
        }

        private void Generate()
        {
            bool isEnemy = false;

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

                    maze[y, x] = new MazeObject(current);
                    maze[y, x].PictureBox.Location = new Point(x * maze[y, x].Width, y * maze[y, x].Height);
                    maze[y, x].PictureBox.Parent = parent;
                    maze[y, x].PictureBox.Size = new Size(maze[y, x].Width, maze[y, x].Height);
                    maze[y, x].PictureBox.BackgroundImage = maze[y, x].Texture;
                    maze[y, x].PictureBox.Visible = false;

                    if (isEnemy)
                    {
                        enemies.Add(new Enemy(new Point(x, y)));
                        isEnemy = false;
                    }
                }
            }
            ShowInfo();
        }

        public void Show()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maze[y, x].PictureBox.Visible = true;
                }
            }
            Player.InitialLabirint();
            Enemy.InitialLabirint();
            Bomb.InitialLabirint();
            GC.Collect();
        }

        public void ShowInfo()
        {
            parent.Text = $"Maze   (Медалей:  {player.PlayersMedal}/{player.AllPlayersMedal},  Здоровье: {player.PlayersHealth}%,  Энергия: {player.PlayersEnergy},  Врагов: {enemies.Count})";
        }


        public void MovePLayer(KeyEventArgs e)
        {
            int playerX = player.Location.X;
            int playerY = player.Location.Y;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (player.CheckCollision(playerX, playerY - 1))
                    {
                        player.Move(playerX, playerY - 1);
                        player.Location = new Point(playerX, playerY - 1);
                    }
                    break;

                case Keys.Down:
                    if (player.CheckCollision(playerX, playerY + 1))
                    {
                        player.Move(playerX, playerY + 1);
                        player.Location = new Point(playerX, playerY + 1);
                    }
                    break;

                case Keys.Left:
                    if (player.CheckCollision(playerX - 1, playerY))
                    {
                        player.Move(playerX - 1, playerY);
                        player.Location = new Point(playerX - 1, playerY);
                    }
                    break;

                case Keys.Right:
                    if (player.CheckCollision(playerX + 1, playerY))
                    {
                        player.Move(playerX + 1, playerY);
                        player.Location = new Point(playerX + 1, playerY);
                    }
                    break;
            }

            if (player.IsHitEnemy)  // если игрок попал на врага
            {
                DelEnemy(player.Location);  // удаляем врага из списка
                player.IsHitEnemy = false;
            }

            if (!CheckEndGame())
            {
                CheckDrawingBomb();  // проверка отрисовки бомбы
            }
        }

        public void BombPlanted()
        {
            // если бомба устанавливается НЕ на одно и тоже место
            if (!player.IsBombPlanted)
            {
                player.BombPlanted();
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


        public void AddEnemy()
        {
            int randX, randY;
            bool flag = false;

            do
            {
                randX = r.Next(width);
                randY = r.Next(height);
                if (maze[randY, randX].Type == MazeObjectType.Hall)
                {
                    maze[randY, randX].ChangeBackgroundImage(MazeObjectType.Enemy);
                    enemies.Add(new Enemy(new Point(randX, randY)));
                    flag = true;
                }
            } while (!flag);
        }

        public void DelEnemy(Point p)
        {
            enemies.Remove(GetEnemyByLoacation(p));
            ShowInfo();
        }

        public Enemy GetEnemyByLoacation(Point p)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.Location == p)
                {
                    return enemy;
                }
            }
            return null;
        }


        public bool CheckEndGame()
        {
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

        private void GameRestart(string text)
        {
            MessageBox.Show(text, "Message");
            StartSettings();
            Show();
        }
    }
}
