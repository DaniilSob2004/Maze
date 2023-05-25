using System.Drawing;
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
            StartGame();
        }

        public void Options()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = Color.FromArgb(255, 92, 118, 137);

            Width = sizeX * 16 + (Size.Width - ClientSize.Width);
            Height = sizeY * 16 + (Size.Height - ClientSize.Height);
            StartPosition = FormStartPosition.CenterScreen;
        }

        public void StartGame() {
            l = new Labirint(this, sizeX, sizeY);
            l.Show();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // двигаем персонажа
            l.MovePLayer(e);
        }
    }
}
