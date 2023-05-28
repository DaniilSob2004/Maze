using System.Drawing;
using System.Windows.Forms;

namespace Maze
{
    public partial class Form1 : Form
    {
        private int sizeX = 40;
        private int sizeY = 20;
        private int sizeElem = 16;
        private Labirint l;

        public Form1()
        {
            InitializeComponent();
            Options();
            l = Labirint.GetInstance(this, sizeX, sizeY, sizeElem);
            InitialLabirintObjects();
            GameSound.BackgroundMusic();
        }

        private void Options()
        {
            Text = "Maze";

            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = Color.FromArgb(255, 92, 118, 137);

            Width = sizeX * sizeElem + (Size.Width - ClientSize.Width);
            Height = sizeY * sizeElem + (Size.Height - ClientSize.Height);
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitialLabirintObjects()
        {
            Player.InitialLabirint();
            Enemy.InitialLabirint();
            Bomb.InitialLabirint();
        }

        private void StartGame()
        {
            l.Show();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                l.BombPlanted();  // вызываем атаку игрока
            }
            else if (e.KeyCode == Keys.Escape)
            {
                l.GameRestart("Перезапуск игры");  // перезапуск лабиринта
            }
            else
            {
                l.MovePLayer(e);  // двигаем персонажа
            }
        }

        private void StartBtn_Click(object sender, System.EventArgs e)
        {
            ClearMenu();
            System.GC.Collect();
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

        private void ClearMenu()
        {
            BackgroundImage = null;
            Controls.Remove(startBtn);
            Controls.Remove(exitBtn);
        }
    }
}
