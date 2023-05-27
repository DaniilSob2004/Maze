using System;
using System.Media;
using System.Threading;
using NAudio.Wave;

namespace Maze
{
    public static class GameSound
    {
        public const string DIR = "sound";  // название папки 
        private static SoundPlayer sound = new SoundPlayer();
        private static AudioFileReader audioFile;
        private static WaveOutEvent waveOut;

        static GameSound()
        {
            audioFile = new AudioFileReader($@"{DIR}\music.mp3");
            waveOut = new WaveOutEvent();
            waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
            waveOut.Volume = 0.7f;
            waveOut.Init(audioFile);
        }

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

        public static void CreateBomb()
        {
            PlaySound($@"{DIR}\create_bomb.wav");
        }

        public static void BackgroundMusic()
        {
            audioFile.Position = 0;
            waveOut.Play();
        }

        private static void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            BackgroundMusic();
        }
    }
}
