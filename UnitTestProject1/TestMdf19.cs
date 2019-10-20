using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"MDF19\Q1"));
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
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"MDF19\Q2"));
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
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"MDF19\Q3"));
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

        [TestMethod]
        public void TestQ4()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"MDF19\Q4"));
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
            var N = Int32.Parse(console.ReadLine());
            var briques = "";
            for (var i = 0; i < N; i++)
                briques += console.ReadLine()[0];

            var r = briques.ToString().Split('X').Max(s => s.Length);
            r = r - 1;

            console.WriteLine($"{r}");
        }
    }

    /// <summary>
    /// Etant donné un échiquier avec uniquement un roi blanc et 0+ tours noires, c'est au roi de jouer,
    /// déterminer si le roi est pat (pat), échec et mat (check-mat), ou s'il peux jouer (still-in-game)
    /// </summary>
    public class MyProgramQ2
    {
        public void Run(IConsole console)
        {
            var table = new string[8];
            (int ligne,int colonne) roi = (0,0);
            var tours = new List<(int ligne,int colonne)>();
            for (var i = 0; i < 8; i++)
            {
                table[i] = console.ReadLine();
                if (table[i].Contains("R"))
                    roi = (i,table[i].IndexOf('R'));

                var start = 0;
                while(table[i].Skip(start).Contains('T'))
                {
                    var tour = (i,table[i].Substring(start).IndexOf('T') + start);
                    start = tour.Item2+1;
                    tours.Add(tour);
                }
            }


            var isEchec = IsEchec(roi, tours, true);
            var isPat = 
                    //4 verticales
                    (roi.ligne<7 ? IsEchec((roi.ligne+1, roi.colonne), tours) : true)
                    && (roi.ligne>0 ? IsEchec((roi.ligne-1, roi.colonne), tours) : true)
                    && (roi.colonne<7 ? IsEchec((roi.ligne, roi.colonne+1), tours) : true)
                    && (roi.colonne>0 ? IsEchec((roi.ligne, roi.colonne-1), tours) : true)
                    //4 diagonales (le roi peut prendre une tour!)
                    && (roi.colonne>0 && roi.ligne>0 ? IsEchec((roi.ligne-1, roi.colonne-1), tours) : true)
                    && (roi.colonne<7 && roi.ligne<7 ? IsEchec((roi.ligne+1, roi.colonne+1), tours) : true)
                    && (roi.colonne < 7 && roi.ligne > 0 ? IsEchec((roi.ligne - 1, roi.colonne + 1), tours) : true)
                    && (roi.colonne >0 && roi.ligne < 7 ? IsEchec((roi.ligne + 1, roi.colonne - 1), tours) : true)
                ;

            //still-in-game : si le roi peut encore se déplacer vers une case où il ne sera pas pris dès le tour suivant.
            //- check-mat : si le roi ne peut se déplacer que dans des positions où il sera pris dès le tour suivant et qu'il est "échec" au tour présent (c'est à dire qu'il serait également pris au tour suivant s'il ne de déplaçait pas).
            //- pat : si le roi ne peut se déplacer que vers des cases où il sera pris au tour suivant et mais qu'il n'est pas "échec" au tour présent (c'est à dire qu'il ne serait pas pris au tour suivant s'il ne se déplaçait pas).
            var result = isEchec && isPat ? "check-mat"
                : isPat ? "pat"
                : "still-in-game";

            //La chaine still-in-game ou check-mat ou pat en fonction de la situation de l’échiquier.
            console.WriteLine($"{result}");

        }

        bool IsEchec((int ligne, int colonne) roi, List<(int ligne, int colonne)> tours, bool searchNext = false)
        {
            var remainingTours = tours.ToList();
            remainingTours.Remove(roi);
            var isEchec = remainingTours.Select(t => t.ligne).Contains(roi.ligne)
                           || remainingTours.Select(t => t.colonne).Contains(roi.colonne);

            if (!searchNext)
                return isEchec;

            //Il a le droit de prendre la tour!!
            if (isEchec)
            {
                var tourACote = tours.Where(t =>
                    (t.ligne == roi.ligne + 1 || t.ligne == roi.ligne - 1 || t.ligne == roi.ligne)
                    && (t.colonne == roi.colonne + 1 || t.colonne == roi.colonne - 1 || t.colonne == roi.colonne)
                ).ToList();

                foreach (var tour in tourACote)
                {
                    var newTours = tours.ToList();
                    newTours.Remove(tour);
                    if (!IsEchec(tour, newTours, false))
                    {
                        return false;
                    }
                }

            }

            return isEchec;
        }
    }

    public class MyProgramQ3
    {
        public void Run(IConsole console)
        {
            var values = console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
            var N = values[0]; //1..1000 //Nombre de PC
            var M = values[1]; //1..N     //ID de la racine [0..N-1]

            //construire graphe
            var parents = new int[N];
            var nodes = Enumerable.Range(0,N).Select(i => new Node {Id=i, IsRoot = i==M}).ToArray();
            for (var i = 0; i < N; i++)
            {
                var parent = parents[i] = Int32.Parse(console.ReadLine()); //-1 si pas de parent
                if (parent != -1)
                {
                    nodes[i].Links.Add((nodes[parents[i]], new LinkProperty()));
                    nodes[parents[i]].Links.Add((nodes[i], new LinkProperty()));
                }
            }

            //lorsqu'il existe un lien entre 2 machines, au moins une des deux doit être infectée.
            //IsInfected
            //Algo: (NOK)
            // 1)infecter un noeud. En déduire tous les noeuds à infecter. Conserver la liste.
            // 2)recommander avec tous les autres noeuds
            // 3)choisir la liste avec le moins de noeuds

            //Algo2:
            // 1) trier les noeuds par ordre de plus de liens.
            // 2) Si 1 seul noeud, le prendre. Si plusieurs, tester toute la suite :)

            var choix = new List<List<int>>();
            foreach(var noeud0 in nodes.OrderByDescending(n => n.Links.Count).ThenByDescending(n => n.Links.Count))
            {
                if(noeud0.IsTested)
                    continue;

                noeud0.IsInfected = true;
                noeud0.IsTested = true;
                foreach (var node in noeud0.Links.Select(l => l.Node))
                    node.IsTested = true;

                //var nextState = false;
                //RecursiveCalcul(noeud0, nextState);
            }
            
            var liste = nodes.Where(n => n.IsInfected).Select(n => n.Id).ToList();
            choix.Add(liste);
            foreach (var node in nodes)
            {
                node.IsInfected = false;
                node.IsTested = false;
            }
            
            //void RecursiveCalcul(Node node, bool isInfected)
            //{
            //    var newNodes = node.Links.Where(l => !l.Node.IsTested).Select(l => l.Node).ToList();

            //    foreach (var noeudLie in newNodes)
            //    {
            //        //Ne pas l'infecter. Le marquer comme parcouru.
            //        noeudLie.IsTested = true;
            //        noeudLie.IsInfected = isInfected;
            //    }

            //    foreach (var noeudLie in newNodes)
            //        RecursiveCalcul(noeudLie, !isInfected);
            //}

            var ordisAInfecter = choix.OrderBy(list => list.Count).First();
            var outputString = ordisAInfecter.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i).Append(' '), sb => sb.ToString());
            console.WriteLine($"{outputString}");       
        }
    }

    public class LinkProperty
    {
        public int Weight { get; set; }
    }

    public class Node
    {
        public int Id;
        public bool IsRoot;
        public bool IsInfected;
        public bool IsTested;
        public int[] Distances; //1 distance per gateway
        public List<(Node Node,LinkProperty Property)> Links = new List<(Node, LinkProperty)>();
        //public List<Node> Links = new List<Node>();

        private bool distanceSetThisPass = false;

        public void ClearPass() => distanceSetThisPass = false;

        /// <summary>
        /// We default to Int32.MaxValue, because a gateway can be isolated from the rest of the board,
        /// and skynet can be on another isolated part, thus leading to cutting a never reachable link.
        /// </summary>
        public void ClearDistances(int gatewayIndex)
        {
            Distances[gatewayIndex] = Int32.MaxValue;
            foreach (var link in Links)
                link.Property.Weight = 0;
        }

        public void AddDistance(int gatewayIndex, int distance=1, int increment = 1)
        {
            Distances[gatewayIndex] = !distanceSetThisPass ? distance : Distances[gatewayIndex] + distance;
            distanceSetThisPass = true;
            foreach (var link in Links.Where(n => !n.Node.distanceSetThisPass))
                link.Node.AddDistance(gatewayIndex, distance + increment, increment);
        }

        public void AddWeights(int distance = 1, int increment = 1)
        {
            foreach (var link in Links)
                link.Property.Weight += distance;
            distanceSetThisPass = true;
            foreach (var link in Links.Where(n => !n.Node.distanceSetThisPass))
                link.Node.AddWeights(distance + increment, increment);
        }

        public void RemoveLink(Node node) => Links.Remove(Links.FirstOrDefault(link => link.Node.Id == node.Id));

        public override bool Equals(object other) => (other as Node)?.Id == Id;
        protected bool Equals(Node other) => Id == other.Id && Equals(Links, other.Links);
        public override int GetHashCode() => Id;
    }

    public class MyProgramQ4
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
