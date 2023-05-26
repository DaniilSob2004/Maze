using System.Drawing;
using System.Collections.Generic;
using static Maze.MazeObject;
using static Maze.Labirint;
using System.Windows.Forms;

namespace Maze
{
    public class Player
    {
        public static Labirint l;

        private Point location;
        private List<Bomb> bombs;

        private int allPlayersMedal;  // все медали которые есть в лабиринте
        private int playersMedal;  // медали игрока
        private int playersHealth;  // здоровье игрока
        private int playersEnergy;  // энергия игрока
        private bool usePill;  // выпил ли лекарство
        private int stepAfterPill;  // кол-во перемещений после принятия лекарства

        private bool isBombPlanted;

        public Player(Point location = new Point())
        {
            this.location = location;
            StartSettings();
        }

        public Point Location
        {
            get => location;
            set => location = value;
        }
        public List<Bomb> Bombs
        {
            get => bombs;
        }
        public int AllPlayersMedal
        {
            get => allPlayersMedal;
            set => allPlayersMedal = value;
        }
        public int PlayersMedal
        {
            get => playersMedal;
            set => playersMedal = value;
        }
        public int PlayersHealth
        {
            get => playersHealth;
            set => playersHealth = value;
        }
        public int PlayersEnergy
        {
            get => playersEnergy;
            set => playersEnergy = value;
        }
        public bool UsePill
        {
            get => usePill;
            set => usePill = value;
        }
        public int StepAfterPill
        {
            get => stepAfterPill;
            set => stepAfterPill = value;
        }
        public bool IsBombPlanted
        {
            get => isBombPlanted;
            set => isBombPlanted = value;
        }


        public void StartSettings()
        {
            bombs = new List<Bomb>();
            isBombPlanted = false;
            playersMedal = allPlayersMedal = 0;
            playersHealth = (int)GameValue.MaxHealth;
            playersEnergy = (int)GameValue.MaxEnergy;
            stepAfterPill = 0;
            usePill = false;
        }

        public void Move(int newX, int newY)
        {
            l.Maze[location.Y, location.X].ChangeBackgroundImage(MazeObjectType.Hall);  // очищаем
            l.Maze[newY, newX].ChangeBackgroundImage(MazeObjectType.Player);  // отображаем

            /*l.Maze[location.Y, location.X].Type = MazeObjectType.Hall;
            l.Maze[location.Y, location.X].PictureBox.BackgroundImage = l.Maze[location.Y, location.X].Texture;*/

            playersEnergy--;  // отнимаем энергию

            // если игрок выпил лекарство, то прибавляем перемещение
            if (usePill) stepAfterPill++;

            l.ShowInfo();
        }

        public bool CheckCollision(int newX, int newY)
        {
            if (newX < 0) return false;

            switch (l.Maze[newY, newX].Type)
            {
                case MazeObjectType.Wall:
                    return false;

                case MazeObjectType.Hall:
                    GameSound.PlayerStep();
                    break;

                case MazeObjectType.Medal:
                    playersMedal++;
                    GameSound.TakeMedal();
                    break;

                case MazeObjectType.Enemy:
                    LossHealth();
                    break;

                case MazeObjectType.Pill:
                    if (playersHealth == (int)GameValue.MaxHealth) return false;
                    AddHealth();
                    break;

                case MazeObjectType.Energy:
                    // энергетик можно выпить, только после 10 перемещений с момента принятия лекарства
                    if (!usePill || stepAfterPill < 10 || playersEnergy == (int)GameValue.MaxEnergy) return false;
                    AddEnergy();
                    break;
            }
            return true;
        }


        public void BombPlanted()
        {
            Bomb bomb = new Bomb(new Point(location.X, location.Y));
            bombs.Add(bomb);

            IsBombPlanted = true;
        }

        public void DrawingBomb()
        {
            if (LossEnergy())
            {
                int last = bombs.Count - 1;

                // отображаем
                l.Maze[bombs[last].Location.Y, bombs[last].Location.X].ChangeBackgroundImage(MazeObjectType.Bomb);
                
                // запускаем таймер
                bombs[last].StartTimerBeforeDetonation();
            }
            else
            {
                MessageBox.Show("Мало энергии для использования бомбы!", "Message");
            }
        }


        private void LossHealth()
        {
            if (playersHealth - (int)GameValue.LossHealth < 0)
            {
                playersHealth = 0;
            }
            else
            {
                playersHealth -= (int)GameValue.LossHealth;
            }
            GameSound.HitEnemy();
        }

        private void AddHealth()
        {
            if (playersHealth + (int)GameValue.AddHealth > (int)GameValue.MaxHealth)
            {
                playersHealth = (int)GameValue.MaxHealth;
            }
            else
            {
                playersHealth += (int)GameValue.AddHealth;
            }
            usePill = true;
            GameSound.DrinkPill();
        }

        private void AddEnergy()
        {
            if (playersEnergy + (int)GameValue.AddEnergy > (int)GameValue.MaxEnergy)
            {
                playersEnergy = (int)GameValue.MaxEnergy;
            }
            else
            {
                playersEnergy += (int)GameValue.AddEnergy;
            }
            usePill = false;
            stepAfterPill = 0;
            GameSound.DrinkEnergy();
        }

        private bool LossEnergy()
        {
            if (playersEnergy - (int)GameValue.BombPlanted <= 0) return false;
            playersEnergy -= (int)GameValue.BombPlanted;
            return true;
        }
    }
}
