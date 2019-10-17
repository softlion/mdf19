using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpContestProject
{
    [TestClass]
    public class TestMdf19
    {
        [TestMethod]
        public void TestQ1()
        {
            var folder = @"C:\dev\repos\mdf19\UnitTestProject1\MDF19\Q1";
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new MyProgramQ1().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i+1}\nerror:\n{console.ErrorOutput}");
            }
        }

        [TestMethod]
        public void TestQ2()
        {
            var folder = @"C:\dev\repos\mdf19\UnitTestProject1\MDF19\Q2";
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new MyProgramQ2().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i+1}\nerror:\n{console.ErrorOutput}");
            }
        }

        [TestMethod]
        public void TestQ3()
        {
            var folder = @"C:\dev\repos\mdf19\UnitTestProject1\MDF19\Q3";
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new MyProgramQ3().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i+1}\nerror:\n{console.ErrorOutput}");
            }
        }
    }

    public class MyProgramQ1
    {
        public void Run(IConsole console)
        {
            //var N = Int32.Parse(console.ReadLine());
            //console.WriteLine($"{xxx}");
        }
    }

    public class MyProgramQ2
    {
        public void Run(IConsole console)
        {
            //var N = Int32.Parse(console.ReadLine());
            //console.WriteLine($"{xxx}");
        }
    }

    public class MyProgramQ3
    {
        public void Run(IConsole console)
        {
            //var N = Int32.Parse(console.ReadLine());
            //console.WriteLine($"{xxx}");
        }
    }


    /* TEMPLATE

    public class MyProgramXXX
    {
        public void Run(IConsole console)
        {
            //var N = Int32.Parse(console.ReadLine());
            //console.WriteLine($"{xxx}");
        }
    }

    [TestMethod]
    public void TestXXX()
    {
        var folder = @"C:\dev\repos\mdf19\UnitTestProject1\TestXXX";
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

        for (var i = 0; i < inputFiles.Count; i++)
        {
            var inputs = File.ReadAllLines(inputFiles[i]);
            var console = new MyConsoleFake(inputs);
            new MyProgramXXXXX().Run(console);

            var outputs = File.ReadAllText(ouputFiles[i]);
            Assert.AreEqual(outputs, console.Output, $"input:{i+1}\nerror:\n{console.ErrorOutput}");
        }
    }
    */
}
