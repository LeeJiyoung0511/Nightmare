using NAudio.Wave;

namespace Nightmare
{
    internal class SoundManager
    {
        private static MediaFoundationReader audioFile;

        private static DirectSoundOut outputDevice;

        private static string currentFile = "";

        public static void PlayBGM(string fileName)
        {
            string filePath = GetFilePath(fileName,"BGM");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("파일을 찾을 수 없습니다: " + filePath);
                return;
            }

            // 같은 노래가 재생중이라면 안끊기고 재생
            if (currentFile == filePath && outputDevice != null && outputDevice.PlaybackState == PlaybackState.Playing)
            {
                return;
            }

            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }

            if (outputDevice != null)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = null;
            }

            audioFile = new MediaFoundationReader(filePath);

            outputDevice = new DirectSoundOut();
            outputDevice.Init(audioFile);
            outputDevice.Play();
            currentFile = filePath;

            outputDevice.PlaybackStopped += (sender, args) =>
            {
                audioFile.Position = 0;
                outputDevice.Play();
            };
        }

        private static string GetFilePath(string fileName, string folderName)
        {
            var paths = AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            var newPath = "";

            for (int i = 0; i < paths.Length - 4; i++)
            {
                newPath += paths[i] + "\\";
            }

            newPath += $"{folderName}\\{fileName}.mp3";

            return newPath;
        }
    }
}
