﻿using System.Drawing;
using System.Windows.Forms;

namespace Maze
{
    public partial class Form1 : Form
    {
        private int sizeX = 40;
        private int sizeY = 20;
        private Labirint l;

        public Form1()
        {
            InitializeComponent();
            Options();
            GameSound.BackgroundMusic();
        }

        public void Options()
        {
            Text = "Maze";

            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = Color.FromArgb(255, 92, 118, 137);

            Width = sizeX * 16 + (Size.Width - ClientSize.Width);
            Height = sizeY * 16 + (Size.Height - ClientSize.Height);
            StartPosition = FormStartPosition.CenterScreen;
        }

        public void StartGame()
        {
            l = Labirint.GetInstance(this, sizeX, sizeY);
            l.Show();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                l.BombPlanted();  // вызываем атаку игрока
            }
            else
            {
                l.MovePLayer(e);  // двигаем персонажа
            }
        }

        private void StartBtn_Click(object sender, System.EventArgs e)
        {
            BackgroundImage = null;
            StartGame();
        }

        private void ExitBtn_Click(object sender, System.EventArgs e)
        {
            var answer = MessageBox.Show("Вы действительно хотите выйти?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (answer)
            {
                case DialogResult.Yes:
                    Close();
                    break;

                case DialogResult.No:
                    break;
            }
        }
    }
}
