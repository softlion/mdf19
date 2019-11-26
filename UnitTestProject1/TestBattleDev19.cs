using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpContestProject
{
    [TestClass]
    public class TestBattleDev19
    {
        [TestMethod]
        public void TestQ1()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"BattleDev\Q1"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new BdQ1().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i+1}\nerror:\n{console.ErrorOutput}");
            }
        }

        [TestMethod]
        public void TestQ2()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"BattleDev\Q2"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new BdQ2().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i + 1}\nerror:\n{console.ErrorOutput}");
            }
        }

        [TestMethod]
        public void TestQ3()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"BattleDev\Q3"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new BdQ3().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i + 1}\nerror:\n{console.ErrorOutput}");
            }
        }

        [TestMethod]
        public void TestQ4()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"BattleDev\Q4"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new BdQ4().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i + 1}\nerror:\n{console.ErrorOutput}");
            }
        }

        [TestMethod]
        public void TestQ5()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"BattleDev\Q5"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new BdQ5().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i + 1}\nerror:\n{console.ErrorOutput}");
            }
        }
    }

    public class BdQ5
    {
        public void Run(IConsole console)
        {
            //A-N
            var carteString = @"A FJM
B CKH
C DG
D GL
E GMN
F KI
H IJ
I L
J N
K L
M N
";

            var carte = new List<Node>();
            for (var i = 'A'; i <= 'N'; i++)
            {
                carte.Add(new Node()
                {
                    Id = i,
                });
            }

            foreach (var line in carteString.Split('\n'))
            {
                var c = line[0];
                var links = line.Skip(2);
                var node = carte.First(n => n.Id == c);
                foreach (var link in links)
                {
                    var targetNode = carte.First(n => n.Id == link);
                    node.Links.Add((targetNode, new LinkProperty()));
                    targetNode.Links.Add((node, new LinkProperty()));
                }
            }

            var temple = console.ReadLine();
            var newCarte = new List<Node>();
            for (var i = 'A'; i <= 'N'; i++)
            {
                var names = console.ReadLine().Split(' ');

                var node = carte.FirstOrDefault(n => n.Id2 == names[0]);
                if (node == null)
                {
                    node = new Node {Id2 = names[0]};
                    carte.Add(node);
                }

                var node2 = carte.FirstOrDefault(n => n.Id2 == names[1]);
                if (node2 == null)
                {
                    node2 = new Node { Id2 = names[1] };
                    carte.Add(node2);
                }

                node.Links.Add((node2, null));
                node2.Links.Add((node, null));
            }

            //console.WriteLine($"{r}");
        }
    }

    public class BdQ4
    {
        public void Run(IConsole console)
        {
            var l1 = console.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
            var NPierres = l1[0];
            var MTypesDePoudre = l1[1];
            var CapaciteLampe = l1[2]; //Grammes

            var tout = new List<(int type, int valeurParGramme, int valeurTotaleSiPierre, int poids)>();
            var pierres = new List<(int valeur, int poids)>();
            for (var i = 0; i < NPierres; i++)
            {
                var vals = console.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
                pierres.Add((vals[0], vals[1]));
                tout.Add((0, vals[0]/ vals[1], vals[0], vals[1]));
            }

            var poudres = new List<(int valeurParGramme, int poids)>();
            for (var i = 0; i < MTypesDePoudre; i++)
            {
                var vals = console.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
                poudres.Add((vals[0], vals[1]));
                tout.Add((1, vals[0], 0, vals[1]));
            }

            tout = (from item in tout
                    orderby item.valeurParGramme descending
                    select item
                   ).ToList();

            for (var i = 0; i < tout.Count-1; i++)
            {
                if(tout[i].type != 0)
                    continue;
                if (tout[i+1].type != 0)
                    continue;

                if (tout[i].valeurTotaleSiPierre < tout[i+1].valeurTotaleSiPierre)
                {
                    var t = tout[i + 1];
                    tout[i + 1] = tout[i];
                    tout[i] = t;
                }
            }

            var capaciteRestante = CapaciteLampe;
            var valeurLampe = 0;
            foreach (var item in tout)
            {
                if (item.type == 0 && capaciteRestante >= item.poids)
                {
                    valeurLampe += item.valeurTotaleSiPierre;
                    capaciteRestante -= item.poids;
                }
                else if (item.type == 1)
                {
                    var quantitePoudre = Math.Min(capaciteRestante, item.poids);
                    valeurLampe += item.valeurParGramme*quantitePoudre;
                    capaciteRestante -= quantitePoudre;
                }
            }

            console.WriteLine($"{valeurLampe}");
        }
    }

    public class BdQ3
    {
        public void Run(IConsole console)
        {
            var l1 = console.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
            var NRJ11 = l1[0];
            var MRequetes = l1[1];

            var dic = new List<(int pos, int start,int end)>();
            for (var i = 0; i < MRequetes; i++)
            {
                var vals = console.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
                dic.Add((i, vals[0], vals[1]));
            }

            dic = dic.OrderBy(kp => kp.start).ThenBy(kp => kp.end).ToList();

            var maxCables = NRJ11;
            var cables = Enumerable.Range(1,maxCables).ToDictionary(i => i, i=> 0);
            var attributions = new List<(int cableIndex, int requetePos)>();
            foreach (var requete in dic)
            {
                var freeCable = cables.FirstOrDefault(c => c.Value == 0 || requete.start >= c.Value).Key;
                if (freeCable > 0)
                {
                    attributions.Add((freeCable, requete.pos));
                    cables[freeCable] = requete.end;
                }
                else
                {
                    console.WriteLine("pas possible");
                    return;
                }
            }

            var r = attributions.OrderBy(a => a.requetePos).Aggregate(new StringBuilder(), (sb, i) => sb.Append(i.cableIndex).Append(' ')).ToString().TrimEnd();
            console.WriteLine($"{r}");
        }
    }

    public class BdQ2
    {
        public void Run(IConsole console)
        {
            var dic = new List<int>();
            for (var i = 0; i < 4; i++)
            {
                var s = console.ReadLine();
                dic.Add(int.Parse(s));
            }

            var min = dic.Min();
            var r = dic.Sum(i => i - min);

            console.WriteLine($"{r}");
        }
    }

    public class BdQ1
    {
        public void Run(IConsole console)
        {
            var N = Int32.Parse(console.ReadLine());
            var dic = new Dictionary<string, int>();
            for (var i = 0; i < N; i++)
            {
                var s = console.ReadLine().Split(' ');
                dic.Add(s[0], int.Parse(s[1]));
            }

            var r = dic.OrderBy(kp => kp.Value).First().Key;

            console.WriteLine($"{r}");
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
