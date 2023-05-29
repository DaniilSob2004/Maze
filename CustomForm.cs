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

        private void BackToMenu()
        {
            var answer = MessageBox.Show("Хотите вернуться на главное меню?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                panel1.Visible = true;
                Text = "Maze";
                l.EndMovingEnemies();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!panel1.Visible)  // если не главное меню, то обработчик клавиатуры будет срабатывать
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        BackToMenu();  // обратно на главное меню
                        return true;

                    case Keys.Enter:
                        l.BombPlanted();  // устанавливаем бомбу
                        return true;

                    default:
                        l.MovePLayer(keyData);  // двигаем персонажа
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void StartBtn_Click(object sender, System.EventArgs e)
        {
            if (Labirint.isFirstGen)
            {
                StartGame();  // первый запуск
                Labirint.isFirstGen = false;
            }
            else
            {
                l.GameRestart("");  // перезапуск лабиринта
            }
            panel1.Visible = false;
        }

        private void ExitBtn_Click(object sender, System.EventArgs e)
        {
            var answer = MessageBox.Show("Вы действительно хотите выйти?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes) Close();
        }
    }
}
