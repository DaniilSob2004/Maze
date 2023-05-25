using System.Drawing;
using static Maze.MazeObject;
using static Maze.Labirint;

namespace Maze
{
    public class Player
    {
        private Point location;
        private int allPlayersMedal;  // все медали которые есть в лабиринте
        private int playersMedal;  // медали игрока
        private int playersHealth;  // здоровье игрока
        private int playersEnergy;  // энергия игрока
        private bool usePill;  // выпил ли лекарство
        private int stepAfterPill;  // кол-во перемещений после принятия лекарства

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


        public void StartSettings()
        {
            playersMedal = allPlayersMedal = 0;
            playersHealth = (int)GameValue.MaxHealth;
            playersEnergy = (int)GameValue.MaxEnergy;
            stepAfterPill = 0;
            usePill = false;
        }

        public void Move(Labirint l, int newX, int newY)
        {
            // очищаем
            l.Maze[location.Y, location.X].Type = MazeObjectType.Hall;
            l.Images[location.Y, location.X].BackgroundImage = l.Maze[location.Y, location.X].Texture;

            // отображаем
            l.Maze[newY, newX].Type = MazeObjectType.Player;
            l.Images[newY, newX].BackgroundImage = l.Maze[newY, newX].Texture;

            playersEnergy--;  // отнимаем энергию

            // если игрок выпил лекарство, то прибавляем перемещение
            if (usePill) stepAfterPill++;

            l.ShowInfo();
        }

        public bool CheckCollision(Labirint l, int newX, int newY)
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
                    UpdateEnergy();
                    break;
            }

            l.ShowInfo();
            return true;
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

        private void UpdateEnergy()
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
    }
}
