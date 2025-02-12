using Nightmare;
using System;

namespace Nightmare
{
    internal class Game
    {
        static void Main(string[] args)
        {
            // 게임 강제 종료시에도 게임 저장
            AppDomain.CurrentDomain.ProcessExit += GameManager.Instance.GameSave;


            SoundManager.PlayBGM("Intro");

            Console.SetWindowSize(120, 40);
            //var titlelines = ASCIIManager.Getlines("Title");
            //var booklines = ASCIIManager.Getlines("Book");
            //var posterlines = ASCIIManager.Getlines("Poster");


            //ASCIIManager.DisplayAlignASCIIArt(titlelines, Align.Center, VerticalAlign.Top);
            //ASCIIManager.DisplayAlignASCIIArt(booklines, Align.Center, VerticalAlign.Middle);
            //ASCIIManager.DisplayAlignASCIIArt(posterlines, Align.Center, VerticalAlign.Top);

            var lines = new string[] { "1. 악몽 속으로 들어가기", "2. 게임 종료" };
            //ASCIIManager.AlignText(lines, Align.Left, VerticalAlign.Bottom, titlelines.Length + booklines.Length);
            //ASCIIManager.AlignText(lines, Align.Left, VerticalAlign.Bottom, posterlines.Length);
            Console.WriteLine(lines[0]);
            Console.WriteLine(lines[1]);

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (input == 1)
                {
                    GameManager.Instance.GameStart();
                }
                else if (input == 2)
                {
                    Console.WriteLine("진짜 종료할 거야?");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }
    }
}
