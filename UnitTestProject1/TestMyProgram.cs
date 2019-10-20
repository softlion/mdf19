using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpContestProject
{
    [TestClass]
    public class TestConcours
    {
        #region old tests
        [TestMethod]
        public void TestAnneeBisextile()
        {
            var inputs = new string[]
            {
                "2",
                "2019",
                "2020"
            };
            var console = new MyConsoleFake(inputs);
            new MyProgramAnneeBisextile().Run(console);
            int i = 0;
        }

        [TestMethod]
        public void TestXml()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"TestXml"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new MyProgramXml().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i}");
            }
        }


        [TestMethod]
        public void TestDesert()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"TestDesert"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new MyProgramDesert().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i}");
            }
        }

        [TestMethod]
        public void TestMyProgramCodeSport()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"TestCodeSport"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new MyProgramCodeSport().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i}");
            }
        }

        [TestMethod]
        public void TestChoco()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"TestChoco"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new MyProgramChoco().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i}");
            }
        }
        #endregion

        [TestMethod]
        public void TestPowerPlant()
        {
            var folder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..", @"TestPowerPlant"));
            var inputFiles = Directory.EnumerateFiles(folder, "input*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(5))).ToList();
            var ouputFiles = Directory.EnumerateFiles(folder, "output*.txt").OrderBy(s => Int32.Parse(Path.GetFileNameWithoutExtension(s).Substring(6))).ToList();

            for (var i = 0; i < inputFiles.Count; i++)
            {
                var inputs = File.ReadAllLines(inputFiles[i]);
                var console = new MyConsoleFake(inputs);
                new MyProgramPowerPlant().Run(console);

                var outputs = File.ReadAllText(ouputFiles[i]);
                Assert.AreEqual(outputs, console.Output, $"input:{i+1}\nerror:\n{console.ErrorOutput}");
            }
        }
    }

    public class MyProgramPowerPlant
    {
        private int max = 9999;

        public void Run(IConsole console)
        {
            var values = console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
            var nLines = values[0];
            var nCols = values[1];

            var terrain = new string[nLines]; //X=sapin .=piste
            for (var i = 0; i < nLines; i++)
                terrain[i] = console.ReadLine();

            //var parcourues = new bool[nLines, nCols];
            var poids = new int[nLines, nCols];
            //chaque poids représente le nombre de déplacement qu'on doit faire pour qu'il n'y ait pas de sapin en dessous,
            //sous réserve que ce soit possible (ie: qu'un sapin bloque à droite ou à gauche)
            //  => si ce n'est pas possible, on met Int32.MaxValue

            //Pour chaque ligne, en commencant par la 1ere
            //  Pour chaque colonne
            //      si case sapin, mettre Int32.MaxValue
            //      si case terrain,
            //          valeurGauche = poids(case de gauche+1)
            //          valeurDroite = DeplacementsDroit(case)
            //          mettre Min(valeurGauche, valeurDroite)

            int DeplacementsDroit(int line, int col)
            {
                if (line == nLines - 1)
                    return 0;

                if (col == nCols - 1)
                    return terrain[line + 1][col] == '.' ? 0 : max;

                var deplacements = 0;
                do
                {
                    if (terrain[line+1][col] == '.')
                        return deplacements;

                    if (col<nCols-1 && terrain[line][col + 1] == 'X')
                        return max;

                    deplacements++;
                    col++;

                } while (col<nCols);

                return max;
            }

            for (var line = 0; line < nLines; line++)
            {
                for (var col = 0; col < nCols; col++)
                {
                    if (terrain[line][col] == 'X')
                        poids[line, col] = max;
                    else
                    {
                        var g = col > 0 ? poids[line, col - 1]+1 : max;
                        var d = DeplacementsDroit(line,col);
                        poids[line, col] = Math.Min(g,d);
                    }
                }
            }

            console.Error("avant");
            console.Dump(poids);

            //for (var line = nLines-2; line >= 0; line--)
            //{
            //    for (var col = 0; col < nCols; col++)
            //        poids[line, col] += poids[line+1, col];
            //}

            //console.Error("apres");
            //console.Dump(poids);

            int MinCol(int[,] table, int line, int nearCol)
            {
                var minCols = new List<int>();
                var minCol = 0;
                var value = Int32.MaxValue;
                for(var col=0; col<nCols; col++)
                {
                    if (table[line, col] < value)
                    {
                        value = table[line, col];
                        minCol = col;
                        minCols.Clear();
                        minCols.Add(col);
                    }
                    else if (table[line, col] == value)
                    {
                        minCols.Add(col);
                    }
                }

                return minCols.OrderBy(m => Math.Abs(m - nearCol)).First();
            }

            //On a les poids.
            //Prends la case de la 1ere ligne avec le poids le plus faible.
            var startCol = MinCol(poids, 0, 0);
            var mouvements = 1;
            //Puis celle de la ligne suivante avec le poids le plus faible et la plus proche (en calculant la différence d'index de colonne)
            for (var line = 1; line < nLines; line++)
            {
                var col = MinCol(poids, line, startCol);
                mouvements += Math.Abs(col - startCol); //droite ou gauche
                startCol = col;
                if (line != nLines - 1)
                    mouvements++; //descend
            }


            console.WriteLine($"{mouvements}");
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
        var folder = @"C:\Users\benja\source\repos\UnitTestProject1\TestXXX";
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


    #region old concours
    public class MyProgramAnneeBisextile
    {
        public void Run(IConsole console)
        {
            var N = Int32.Parse(console.ReadLine());
            var years = new int[N];
            for (var i = 0; i < N; i++)
                years[i] = Int32.Parse(console.ReadLine());

            foreach (var year in years)
            {

                if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
                    console.WriteLine("BISSEXTILE");
                else
                    console.WriteLine("NON BISSEXTILE");
            }
        }
    }
    #endregion

    #region old concours2
    class Balise
    {
        public char Letter;
        public Balise Parent;
        public int Profondeur;
        public List<Balise> Childs = new List<Balise>();
    }

    public class MyProgramXml
    {
        public void Run(IConsole console)
        {
            var xml = console.ReadLine();
            console.Error(xml);
            var root = new Balise();

            var poids = new Dictionary<char, double>();

            var current = root;
            var skipNext = false;
            var profondeur = 1;
            foreach (var c in xml)
            {
                if (skipNext)
                {
                    skipNext = false;
                    continue;
                }

                var isClose = c == '-';

                if (!isClose)
                {
                    var b = new Balise { Letter = c, Parent = current, Profondeur = profondeur };
                    current.Childs.Add(b);
                    current = b;
                    if (!poids.TryGetValue(c, out var poid))
                        poid = 0;
                    poid += 1.0 / profondeur;
                    poids[c] = poid;
                    profondeur++;
                }
                else
                {
                    current = current.Parent;
                    skipNext = true;
                    profondeur--;
                }
            }

            var letter = poids.OrderByDescending(p => p.Value).ThenBy(p => p.Key).Select(p => p.Key).First();
            console.WriteLine($"{letter}");
            var i = 0;
        }
    }
    #endregion

    #region old concours3
    public class MyProgramDesert
    {
        public void Run(IConsole console)
        {
            var sizes = console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
            var nLines = sizes[0];
            var nCols = sizes[1];

            var table = new string[nLines];
            for (var line = 0; line < nLines; line++)
                table[line] = console.ReadLine();
            //.=terre
            //#=sable

            var profondeurs = new int[nLines, nCols];
            var parcourues = new bool[nLines, nCols];
            //Pour chaque case '.' non parcourue,
            //   a) assigner aux 4 cases '#' voisines Min(valeur de cette case voisine,1)
            //   b) pour chacune des 4 cases voisines non parcourues
            //        b1) réexécuter le code a) (récursif)

            for (var line = 0; line < nLines; line++)
            {
                for (var col = 0; col < nCols; col++)
                {
                    if (!parcourues[line, col] && table[line][col] == '.')
                        SetProfondeur(line, col, 0);
                }
            }

            void SetProfondeur(int line, int col, int profondeur)
            {
                var parcourue = parcourues[line, col];
                if (parcourue)
                {
                    if (profondeurs[line, col] <= profondeur)
                        return;
                }

                parcourues[line, col] = true;
                profondeurs[line, col] = profondeur;

                if (line > 0 && table[line - 1][col] == '#')
                    SetProfondeur(line - 1, col, profondeur + 1);
                if (line < nLines - 1 && table[line + 1][col] == '#')
                    SetProfondeur(line + 1, col, profondeur + 1);
                if (col > 0 && table[line][col - 1] == '#')
                    SetProfondeur(line, col - 1, profondeur + 1);
                if (col < nCols - 1 && table[line][col + 1] == '#')
                    SetProfondeur(line, col + 1, profondeur + 1);
            }

            var maxProfondeur = 0;
            for (var line = 0; line < nLines; line++)
            {
                for (var col = 0; col < nCols; col++)
                    maxProfondeur = Math.Max(maxProfondeur, profondeurs[line, col]);
            }

            console.WriteLine($"{maxProfondeur}");
        }
    }
    #endregion

    #region old concours4
    public class MyProgramCodeSport
    {
        public void Run(IConsole console)
        {
            var A = Int32.Parse(console.ReadLine());
            var B = Int32.Parse(console.ReadLine());
            var D = Int32.Parse(console.ReadLine());

            for (var i = A; i <= B; i++)
            {
                if (i % D == 0)
                {
                    console.WriteLine($"{i}");
                    break;
                }
            }
        }
    }
    #endregion

    #region old concours5
    public class MyProgramChoco
    {
        public void Run(IConsole console)
        {
            var N = Int32.Parse(console.ReadLine());
            var n = 0;
            for (var i = 0; i < N; i++)
            {
                var date = Sum(Int64.Parse(console.ReadLine()));
                if (date == 42)
                {
                    n++;
                }
            }

            console.WriteLine($"{n}");
        }

        long Sum(long date)
        {
            while (date > 99)
                date = date.ToString().Select(c => Int32.Parse(c.ToString())).Sum();
            return date;
        }
    }
    #endregion
}
