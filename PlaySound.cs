using System.Media;

namespace Maze
{
    public static class GameSound
    {
        public const string DIR = "sound";  // название папки 
        private static SoundPlayer sound = new SoundPlayer();

        private static void PlaySound(string path)
        {
            using (sound)
            {
                // указываем путь к файлу и воспроизводим
                sound.SoundLocation = path;
                sound.Play();
            }
        }

        public static void TakeMedal()
        {
            PlaySound($@"{DIR}\medal.wav");
        }

        public static void HitEnemy()
        {
            PlaySound($@"{DIR}\enemy.wav");
        }

        public static void DrinkEnergy()
        {
            PlaySound($@"{DIR}\energy.wav");
        }

        public static void DrinkPill()
        {
            PlaySound($@"{DIR}\pill.wav");
        }

        public static void Winner()
        {
            PlaySound($@"{DIR}\winner.wav");
        }

        public static void Loss()
        {
            PlaySound($@"{DIR}\loss.wav");
        }

        public static void PlayerStep()
        {
            PlaySound($@"{DIR}\step.wav");
        }

        public static void Detonation()
        {
            PlaySound($@"{DIR}\detonation.wav");
        }
    }
}
