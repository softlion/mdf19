using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpContestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            new MyProgramQ1().Run(new MyConsoleReal());
        }
    }
}

#region helpers
namespace CSharpContestProject
{
    public interface IConsole
    {
        string ReadLine();
        void WriteLine(string line);
        string Output { get; }
        string ErrorOutput { get; }
        void Error(string text);
    }

    public class MyConsoleReal : IConsole
    {
        public string ReadLine() => Console.ReadLine();
        public void WriteLine(string line) => Console.WriteLine(line);
        public void Error(string text) => Console.Error.WriteLine(text);
        public string Output => null;
        public string ErrorOutput => null;
    }
}
#endregion

