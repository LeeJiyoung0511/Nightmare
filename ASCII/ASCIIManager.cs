namespace Nightmare
{
    public enum Align
    {
        Left,
        Center,
        Right
    }

    public enum VerticalAlign
    {
        Top,
        Middle,
        Bottom
    }

    public class ASCIIManager
    {
        /// <summary>
        /// 아스키 아트를 정렬하고 표시하는 함수
        /// </summary>
        /// <param name="fileName">아스키 아트 파일이름</param>
        /// <param name="hori">가로 정렬</param>
        /// <param name="verti">수직 정렬</param>
        static public void DisplayAlignASCIIArt(string fileName, Align hori, VerticalAlign verti)
        {
            string path = GetFilePath(fileName);
            string[] lines = File.ReadAllLines(path);

            AlignASCIIText(lines, hori, verti);
        }

        /// <summary>
        /// 텍스트 정렬 함수
        /// </summary>
        /// <param name="lines">정렬할 텍스트</param>
        /// <param name="hori">가로 정렬</param>
        /// <param name="verti">수직 정렬</param>
        static public void AlignASCIIText(string[] lines, Align hori, VerticalAlign verti)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;
            int textHeight = lines.Length;

            int startY = verti switch
            {
                VerticalAlign.Middle => Math.Max((consoleHeight - textHeight) / 2, 0),
                VerticalAlign.Bottom => Math.Max(consoleHeight - textHeight - 1, 0),
                _ => 0
            };

            if (startY + textHeight >= consoleHeight)
            {
                startY = Math.Max(consoleHeight - textHeight, 0);
            }

            Console.SetCursorPosition(0, startY);

            foreach (string line in lines)
            {
                int padding = hori switch
                {
                    Align.Center => (consoleWidth - line.Length) / 2,
                    Align.Right => consoleWidth - line.Length,
                    _ => 0
                };

                Console.WriteLine(new string(' ', padding) + line);
            }
        }

        static public void DisplayAlignASCIIArt(string[] art, Align horizontalAlign, VerticalAlign verticalAlign)
        {
            AlignASCIIText(art, horizontalAlign, verticalAlign, 0);
        }

        static public void AlignASCIIText(string[] text, Align horizontalAlign, VerticalAlign verticalAlign, int reservedHeight)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;
            int textHeight = text.Length;

            int startY = (verticalAlign == VerticalAlign.Middle) ? (consoleHeight - textHeight) / 2 :
                         (verticalAlign == VerticalAlign.Bottom) ? Math.Max(consoleHeight - textHeight - 1, 0) : 0;


            foreach (string line in text)
            {
                int padding = (horizontalAlign == Align.Center) ? (consoleWidth - line.Length) / 2 :
                              (horizontalAlign == Align.Right) ? (consoleWidth - line.Length) : 0;

                Console.WriteLine(new string(' ', padding) + line);
            }
        }

        static public void AlignText(string[] text, Align horizontalAlign, VerticalAlign verticalAlign, int y = 8)
        {
            int textHeight = text.Length;
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            int startY = (verticalAlign == VerticalAlign.Middle) ? (consoleHeight - textHeight) / 2 :
                         (verticalAlign == VerticalAlign.Bottom) ? Math.Max(consoleHeight - textHeight - 1, 0) : 0;

            Console.SetCursorPosition(0, startY - y);

            foreach (string line in text)
            {
                int textWidth = GetTextWidth(line);

                int padding = (horizontalAlign == Align.Center) ? (consoleWidth - textWidth) / 2 :
                              (horizontalAlign == Align.Right) ? (consoleWidth - textWidth) : 0;

                Console.WriteLine(new string(' ', Math.Max(padding, 0)) + line);
            }
        }

        static int GetTextWidth(string text)
        {
            return text.Sum(c => (c >= '가' && c <= '힣') ? 2 : 1);
        }

        public static string[] Getlines(string fileName)
        {
            string path = GetFilePath(fileName);
            return File.ReadAllLines(path);
        }

        private static string GetFilePath(string fileName)
        {
            var paths = AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            var newPath = "";

            for (int i = 0; i < paths.Length - 4; i++)
            {
                newPath += paths[i] + "\\";
            }

            newPath += $"ASCII\\{fileName}.txt";

            return newPath;
        }
    }
}
