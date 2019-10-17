using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class LinkProperty
{
    public int Weight { get; set; }
}

public class Node
{
    public int Id;
    public bool IsGateway;
    public int[] Distances; //1 distance per gateway
    public List<(Node Node, LinkProperty Property)> Links = new List<(Node, LinkProperty)>();

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

    public void AddDistance(int gatewayIndex, int distance = 1, int increment = 1)
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

class Player
{
    static void Main(string[] args)
    {
        var gatewayIds = new List<int>();
        var links = new List<(int a, int b)>();

        #region Parsing
        var inputs = Console.ReadLine().Split(' ');
        var N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
        var L = int.Parse(inputs[1]); // the number of links
        var E = int.Parse(inputs[2]); // the number of exit gateways
        for (int i = 0; i < L; i++)
        {
            inputs = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
            var N2 = int.Parse(inputs[1]);
            links.Add((N1, N2));
        }

        for (int i = 0; i < E; i++)
        {
            var EI = int.Parse(Console.ReadLine()); // the index of a gateway node
            gatewayIds.Add(EI);
        }

        var board = BuildBoard(links, gatewayIds, N);
        var gateways = gatewayIds.Select(id => board[id]).ToList();
        #endregion

        // game loop
        while (true)
        {
            ResetWeights(board, gateways); //Could be optimized as we know which link is removed

            // The index of the node on which the Skynet agent is positioned this turn
            var skynet = board[int.Parse(Console.ReadLine())];

            // Find the 1st shortest path skynet will use to reach the gateway, and cut the last link
            var path1 = FindShortestPath(skynet);

            //1 node can be linked to max 2 gateways
            //Find the 2nd shortest path skynet will use to reach another gateway if the 1st one is not accessible
            //var path2 = FindShortestPath(skynet, gateways.IndexOf(path1.Last()));

            var path = path1;
            /*
            if(path2 != null) //Only path2 can be null
            {
                //Use those to compute link weights
                path1.Last().AddWeights();
                path2.Last().AddWeights();
    
                //And select the path with the lowest weight sum
                var weightPath1 = ComputeWeight(path1);
                var weightPath2 = ComputeWeight(path2);
                Console.Error.WriteLine($"Weights: 1:{weightPath1} 2:{weightPath2}");
                path = weightPath1 <= weightPath2 ? path1 : path2;
            }
            */

            var linkToCut = (((IEnumerable<Node>)path).Reverse().Skip(1).First(), path.Last());

            //Cut 1st the links from nodes that are linked to 2 gateways, and are the nearest from skynet
            if (path.Count > 2)
            {
                var linkedNodesWith2Gateways =
                    gateways.SelectMany(gw => gw.Links)
                    .Where(link => link.Node.Links.Count(link2 => link2.Node.IsGateway) >= 2)
                    .OrderBy(link => link.Node.Distances.Min())
                    .FirstOrDefault();

                if (linkedNodesWith2Gateways.Node != null)
                    linkToCut = (linkedNodesWith2Gateways.Node, gateways.First(gw => gw.Links.Contains(linkedNodesWith2Gateways)));
            }

            links.Remove((linkToCut.Item1.Id, linkToCut.Item2.Id));
            linkToCut.Item1.RemoveLink(linkToCut.Item2);
            linkToCut.Item2.RemoveLink(linkToCut.Item1);


            //Console.Error.WriteLine($"Link cut: {linkToCut.Item1} {linkToCut.Item2} skynet:{skynet}");
            Console.WriteLine($"{linkToCut.Item1.Id} {linkToCut.Item2.Id}"); // Example: 0 1 are the indices of the nodes you wish to sever the link between
        }
    }

    private static int ComputeWeight(List<Node> path)
    {
        var start = path.First();
        var weight = 0;
        foreach (var node in path.Skip(1))
        {
            var link = start.Links.First(l => l.Node.Id == node.Id);
            weight += link.Property.Weight;
            start = node;
        }

        return weight;
    }

    private static List<Node> FindShortestPath(Node skynet, int exceptGatewayIndex = -1)
    {
        var path = new List<Node> { skynet };
        while (true)
        {
            Console.Error.WriteLine($"skynet:{skynet.Id} exceptGatewayIndex:{exceptGatewayIndex} path:{path.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i.Id).Append("-"))} links:{skynet.Links.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i.Node.Id).Append(","))}");
            var bestNextNode = skynet.Links.Select(link => link.Node).Except(path).OrderBy(node =>
            {
                var distances = node.Distances.ToArray();
                if (exceptGatewayIndex >= 0)
                    distances[exceptGatewayIndex] = Int32.MaxValue;
                return distances.Min();
            }).FirstOrDefault();

            if (bestNextNode == null)
                return null; //No path to a gateway from this position

            path.Add(bestNextNode);
            if (bestNextNode.IsGateway)
                break;
            skynet = bestNextNode;
        }

        return path;
    }

    static void ResetWeights(in Node[] board, in List<Node> gateways)
    {
        var gatewayIndex = 0;
        foreach (var gateway in gateways)
        {
            foreach (var node in board)
            {
                node.ClearPass();
                node.ClearDistances(gatewayIndex);
            }

            gateway.AddDistance(gatewayIndex);
            gatewayIndex++;
        }
    }

    static Node[] BuildBoard(List<(int a, int b)> links, List<int> gateways, int nodeCount)
    {
        var board = Enumerable.Range(0, nodeCount).Select(i => new Node { Id = i, IsGateway = gateways.Contains(i), Distances = new int[gateways.Count] }).ToArray();

        foreach (var node in board)
        {
            var id = node.Id;
            node.Links = links
                .Where(item => item.a == id || item.b == id)
                .Select(link => (board[link.a == id ? link.b : link.a], new LinkProperty()))
                .ToList();
        }

        return board;
    }
}
