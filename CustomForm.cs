using System.Drawing;
using System.Windows.Forms;

namespace Maze
{
    public partial class CustomForm : Form
    {
        private int sizeX = 40;
        private int sizeY = 20;
        private int sizeElem = 16;
        private Labirint l;

        public CustomForm()
        {
            InitializeComponent();
            Options();

            // ПАТТЕРН ОДИНОЧКА
            l = Labirint.GetInstance(this, sizeX, sizeY, sizeElem);
            InitialLabirintObjects();  // инициализация объекта лабаринта для всех классов

            GameSound.BackgroundMusic();
        }

        private void Options()
        {
            Text = "Maze";

            FormBorderStyle = FormBorderStyle.FixedSingle;  // запрещено изменять размер окна
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
            l.Show();  // вывод лабиринта
        }

        private void CustomForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                l.BombPlanted();  // устанавливаем бомбу
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
            StartGame();
        }

        private void ExitBtn_Click(object sender, System.EventArgs e)
        {
            var answer = MessageBox.Show("Вы действительно хотите выйти?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes) Close();
        }

        private void ClearMenu()
        {
            // удаляем элементы меню
            BackgroundImage = null;
            Controls.Remove(startBtn);
            Controls.Remove(exitBtn);
        }
    }
}
