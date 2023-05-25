using System;
using System.Windows.Forms;
using System.Drawing;
using static Maze.MazeObject;

namespace Maze
{
    public class Labirint
    {
        public enum GameValue { MaxHealth = 100, LossHealth = 25, AddHealth = 10, MaxEnergy = 500, LossEnergy = 1, AddEnergy = 25 };
        public readonly Point finalPoint;  // координаты конца лабиринта

        private int height;  // высота лабиринта
        private int width;  // ширина лабиринта

        private Player player;
        private Form parent;
        private MazeObject[,] maze;
        private PictureBox[,] images;


        public Labirint(Form parent, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.parent = parent;

            player = new Player();
            maze = new MazeObject[height, width];
            images = new PictureBox[height, width];
            finalPoint = new Point(width - 1, height - 3);

            StartSettings();
        }


        // Свойства
        public MazeObject[,] Maze => maze;
        public PictureBox[,] Images => images;
        public Point FinalPoint => finalPoint;


        private void StartSettings()
        {
            parent.Controls.Clear();
            player.Location = new Point(0, 2);
            player.StartSettings();
            Generate();
        }

        private void Generate()
        {
            Random r = new Random();

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
                        if (r.Next(175) == 0)  // враг
                        {
                            current = MazeObjectType.Enemy;
                        }

                        else if (r.Next(175) == 0)  // энергетик
                        {
                            current = MazeObjectType.Energy;
                        }

                        else if (r.Next(175) == 0)  // лекарство
                        {
                            current = MazeObjectType.Pill;
                        }

                        else if (r.Next(150) == 0)  // медаль
                        {
                            current = MazeObjectType.Medal;
                            player.AllPlayersMedal++;
                        }

                        else if (r.Next(5) == 0)  // стена
                        {
                            current = MazeObjectType.Wall;
                        }
                    }

                    maze[y, x] = new MazeObject(current);
                    images[y, x] = new PictureBox();
                    images[y, x].Location = new Point(x * maze[y, x].Width, y * maze[y, x].Height);
                    images[y, x].Parent = parent;
                    images[y, x].Size = new Size(maze[y, x].Width, maze[y, x].Height);
                    images[y, x].BackgroundImage = maze[y, x].Texture;
                    images[y, x].Visible = false;
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
                    images[y, x].Visible = true;
                }
            }
        }

        public void ShowInfo()
        {
            parent.Text = $"Maze   (Медалей:  {player.PlayersMedal}/{player.AllPlayersMedal},  Здоровье: {player.PlayersHealth}%,  Энергия: {player.PlayersEnergy})";
        }


        public void MovePLayer(KeyEventArgs e)
        {
            int playerX = player.Location.X;
            int playerY = player.Location.Y;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (player.CheckCollision(this, playerX, playerY - 1))
                    {
                        player.Move(this, playerX, playerY - 1);
                        player.Location = new Point(playerX, playerY - 1);
                    }
                    break;

                case Keys.Down:
                    if (player.CheckCollision(this, playerX, playerY + 1))
                    {
                        player.Move(this, playerX, playerY + 1);
                        player.Location = new Point(playerX, playerY + 1);
                    }
                    break;

                case Keys.Left:
                    if (player.CheckCollision(this, playerX - 1, playerY))
                    {
                        player.Move(this, playerX - 1, playerY);
                        player.Location = new Point(playerX - 1, playerY);
                    }
                    break;

                case Keys.Right:
                    if (player.CheckCollision(this, playerX + 1, playerY))
                    {
                        player.Move(this, playerX + 1, playerY);
                        player.Location = new Point(playerX + 1, playerY);
                    }
                    break;
            }

            CheckEndGame();
        }

        private void CheckEndGame()
        {
            if (finalPoint.X == player.Location.X && finalPoint.Y == player.Location.Y)
            {
                GameSound.Winner();
                GameRestart("Победа - найден выход!");
            }

            else if (player.PlayersMedal == player.AllPlayersMedal)
            {
                GameSound.Winner();
                GameRestart("Победа - медали собраны!");
            }

            else if (player.PlayersHealth <= 0)
            {
                GameSound.Loss();
                GameRestart("Поражение - закончилось здоровье!");
            }

            else if (player.PlayersEnergy <= 0)
            {
                GameSound.Loss();
                GameRestart("Поражение - закончилась энергия!");
            }
        }

        private void GameRestart(string text)
        {
            MessageBox.Show(text, "Message");
            StartSettings();
            Show();
        }
    }
}
