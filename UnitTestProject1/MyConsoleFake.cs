using System.Text;

namespace CSharpContestProject
{
    public class MyConsoleFake : IConsole
    {
        private readonly string[] lines;
        private int currentLine = 0;
        private readonly StringBuilder output = new StringBuilder();
        private readonly StringBuilder error = new StringBuilder();

        public string Output => output.ToString().TrimEnd('\r').TrimEnd('\n').Replace("\r", "");
        public string ErrorOutput => error.ToString();

        public MyConsoleFake(string[] lines)
        {
            this.lines = lines;
        }

        public string ReadLine() => lines[currentLine++];
        public void WriteLine(string line) => output.AppendLine(line);
        public void Error(string line) => error.AppendLine(line);
    }

    public static class HelperFake
    {
        public static void Dump(this IConsole console, int[,] table)
        {
            var nLines = table.GetLength(0);
            var nCols = table.GetLength(1);

            for (var line = 0; line < nLines; line++)
            {
                var sb = new StringBuilder();
                for (var col = 0; col < nCols; col++)
                    sb.Append(table[line, col].ToString("00000")).Append(" ");
                console.Error(sb.ToString());
            }
        }
    }
}
