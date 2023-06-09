﻿using System.Drawing;
using System.Collections.Generic;
using static Maze.MazeObject;
using static Maze.Labirint;
using System.Windows.Forms;

namespace Maze
{
    public class Player
    {
        private static Labirint l;

        private Point location;  // координаты
        private List<Bomb> bombs;  // коллекция всех установленных бомб

        private int allPlayersMedal;  // все медали которые есть в лабиринте
        private int totalMedal;  // медали игрока
        private int totalHealth;  // здоровье игрока
        private int totalEnergy;  // энергия игрока
        private bool usePill;  // выпил ли лекарство
        private int stepAfterPill;  // кол-во перемещений после принятия лекарства
        private int stepAfterEnemy;  // кол-во перемещений после добавления нового врага
        private bool isHitEnemy;  // попал ли персонаж на врага
        private bool isBombPlanted;  // установлена ли была бомба

        public Player(Point location = new Point())
        {
            bombs = new List<Bomb>();
            StartSettings();
        }

        public Point Location
        {
            get => location;
            set => location = value;
        }
        public List<Bomb> Bombs => bombs;
        public int AllPlayersMedal
        {
            get => allPlayersMedal;
            set => allPlayersMedal = value;
        }
        public int PlayersMedal => totalMedal;
        public int PlayersHealth
        {
            get => totalHealth;
            set => totalHealth = value;
        }
        public int PlayersEnergy => totalEnergy;
        public bool UsePill => usePill;
        public int StepAfterPill => stepAfterPill;
        public bool IsHitEnemy
        {
            get => isHitEnemy;
            set => isHitEnemy = value;
        }
        public bool IsBombPlanted
        {
            get => isBombPlanted;
            set => isBombPlanted = value;
        }

        public void SetX(int x)
        {
            location.X = x;
        }

        public void SetY(int y)
        {
            location.Y = y;
        }

        public static void InitialLabirint()
        {
            l = Labirint.GetInstance();  // получаем объект Labirint, паттерн Одиночка
        }


        public void StartSettings()
        {
            // начальные настройки
            bombs.Clear();
            isBombPlanted = false;
            totalMedal = allPlayersMedal = 0;
            totalHealth = (int)GameValue.MaxHealth;
            totalEnergy = (int)GameValue.MaxEnergy;
            stepAfterPill = 0;
            stepAfterEnemy = 0;
            usePill = false;
            isHitEnemy = false;
        }

        public void Move(int newX, int newY)
        {
            l.Maze[location.Y, location.X].ChangeBackgroundImage(MazeObjectType.Hall);  // очищаем
            l.Maze[newY, newX].ChangeBackgroundImage(MazeObjectType.Player);  // отображаем

            totalEnergy--;  // отнимаем энергию

            // если игрок выпил лекарство, то прибавляем перемещение
            if (usePill) stepAfterPill++;

            // если игрок сделал 20 перемещений
            if (++stepAfterEnemy == 20)
            {
                l.AddEnemy();  // добавляем нового врага
                stepAfterEnemy = 0;
            }
        }

        public bool CheckCollision(int newX, int newY)
        {
            if (newX < 0) return false;  // если это стартовая точка

            switch (l.Maze[newY, newX].Type)
            {
                case MazeObjectType.Wall:  // если стена, то столкновение
                    return false;

                case MazeObjectType.Bomb:
                    return false;

                case MazeObjectType.Hall:
                    GameSound.PlayerStep();
                    break;

                case MazeObjectType.Medal:
                    totalMedal++;
                    GameSound.TakeMedal();
                    break;

                case MazeObjectType.Enemy:
                    LossHealth();  // отнимаем здоровье
                    isHitEnemy = true;
                    break;

                case MazeObjectType.Pill:
                    if (totalHealth == (int)GameValue.MaxHealth) return false;  // если здоровье максимально, то нельзя выпить лекарство
                    AddHealth();  // добавляем здоровье
                    break;

                case MazeObjectType.Energy:
                    // энергетик можно выпить, только после 10 перемещений с момента принятия лекарства
                    if (!usePill || stepAfterPill < 10 || totalEnergy == (int)GameValue.MaxEnergy) return false;
                    AddEnergy();  // добавляем энергию
                    break;
            }
            return true;
        }


        public void BombPlanted()
        {
            bombs.Add(new Bomb(location));  // создаём бомбу
            IsBombPlanted = true;
        }

        public void DrawingBomb()
        {
            if (LossEnergy())  // если энергии хватает
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


        public void LossHealth()
        {
            if (totalHealth - (int)GameValue.LossHealth < 0)
            {
                totalHealth = 0;
            }
            else
            {
                totalHealth -= (int)GameValue.LossHealth;
            }
            GameSound.HitEnemy();
        }

        private void AddHealth()
        {
            if (totalHealth + (int)GameValue.AddHealth > (int)GameValue.MaxHealth)
            {
                totalHealth = (int)GameValue.MaxHealth;
            }
            else
            {
                totalHealth += (int)GameValue.AddHealth;
            }
            usePill = true;  // выпил ли лекарство
            GameSound.DrinkPill();
        }

        private void AddEnergy()
        {
            if (totalEnergy + (int)GameValue.AddEnergy > (int)GameValue.MaxEnergy)
            {
                totalEnergy = (int)GameValue.MaxEnergy;
            }
            else
            {
                totalEnergy += (int)GameValue.AddEnergy;
            }
            usePill = false;
            stepAfterPill = 0;
            GameSound.DrinkEnergy();
        }

        private bool LossEnergy()
        {
            if (totalEnergy - (int)GameValue.BombPlanted <= 0) return false;
            totalEnergy -= (int)GameValue.BombPlanted;
            return true;
        }
    }
}
