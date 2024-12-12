// See https://aka.ms/new-console-template for more information
using Cateros.GraphLib;
using System.Numerics;

Console.WriteLine("Hello, World!");


var n1 = new TestNode(1);
var n2 = new TestNode(2);
var n3 = new TestNode(3);
var n4 = new TestNode(4);


var graph = new TestGraph();
graph.AddNode(n1);
graph.AddNode(n2);
graph.AddNode(n3);
graph.AddNode(n4);
graph.AddEdge(new TestEdge(1, n1, n2) { Length = 3000, AdditionalWeight = 3 });
graph.AddEdge(new TestEdge(2, n2, n3) { Length = 3000, AdditionalWeight = 9 });
graph.AddEdge(new TestEdge(3, n1, n3) { Length = 5000, AdditionalWeight = 7 });
graph.AddEdge(new TestEdge(4, n3, n4) { Length = 3000 });

var vistor = new TestVisitor() {Speed = 1000 };

var djkstra = new DijkstraShortestPathCalculator<TestGraph, TestEdge, TestNode, TestWeight>();
 var res = djkstra.CalculateShortestPath(graph, n1, n4, vistor);
if (res is null)
{
    Console.WriteLine("No route found");
}
else
{

foreach(var e in res)
{
    Console.WriteLine($"Edge: {e.Item1.Id} - Cost: {e.Cost.TravelCost}, Time: {e.Cost.TravelTime}, Distance: {e.Cost.TravelDistance}");
}
}
Console.WriteLine("calculation completed");
Console.ReadLine();
internal class TestGraph: Graph<TestEdge, TestNode>
{

}

internal class TestNode(int id) : Node<TestNode, TestEdge>(id)
{
    public override string ToString()=> $"Node {Id}";
}

internal class TestEdge(int id, Node<TestNode, TestEdge> startNode, Node<TestNode, TestEdge> endNode) : Edge<TestEdge, TestNode>(id, startNode, endNode)
{
    public bool Enabled { get; set; } = true;
    public uint Length { get; set; }
    public uint AdditionalWeight {  get; set; } 
}

internal class TestVisitor : IEdgeVisitor<TestEdge, TestNode, TestWeight>
{
    public uint Speed { get; set; }

    public bool CanVisit(TestEdge edge)
    {
        return edge.Enabled;
    }

    public Maybe<TestWeight> Visit(TestEdge edge)
    {
        throw new NotImplementedException();
    }

    public TestWeight VisitCost(TestEdge edge)
    {
        return new() 
        {
            TravelCost =  (uint)(edge.Length / Speed + edge.AdditionalWeight) , 
            TravelDistance = edge.Length, 
            TravelTime = (uint)(edge.Length / Speed) 
        };
    }
}

internal struct TestWeight : IAdditionOperators<TestWeight, TestWeight, TestWeight>, IComparable<TestWeight>, IMinMaxValue<TestWeight>, IComparisonOperators<TestWeight, TestWeight, bool>

{

    public static TestWeight MaxValue => new(){TravelCost = uint.MaxValue, TravelTime = uint.MaxValue, TravelDistance = uint.MaxValue };

    public static TestWeight MinValue => new() { TravelCost = uint.MinValue, TravelTime = uint.MinValue, TravelDistance = uint.MinValue};

    public uint TravelTime { get; set; }
    public uint TravelCost { get; set;}
    public uint TravelDistance { get; set;}

    public readonly int CompareTo(TestWeight other)
    {
        return this.TravelCost.CompareTo(other.TravelCost);
    }

    public readonly bool Equals(TestWeight other)
    {
        return this.TravelCost.Equals(other.TravelCost);
    }


    public static TestWeight operator +(TestWeight value)
    {
        return new() 
        {
            TravelTime = value.TravelTime,
            TravelDistance = value.TravelDistance,
            TravelCost = value.TravelCost
        };
    }

    public static TestWeight operator +(TestWeight left, TestWeight right)
    {
        return new() 
        {
            TravelTime = left.TravelTime+ right.TravelTime,
            TravelDistance = left.TravelDistance + right.TravelDistance,
            TravelCost = left.TravelCost + right.TravelCost
        };
    }

    public static bool operator ==(TestWeight left, TestWeight right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TestWeight left, TestWeight right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(TestWeight left, TestWeight right)
    {
        return left.TravelCost < right.TravelCost;
    }

    public static bool operator >(TestWeight left, TestWeight right)
    {
        return right.TravelCost > left.TravelCost;
    }

    public static bool operator <=(TestWeight left, TestWeight right)
    {
        return left.TravelCost <= right.TravelCost;
    }

    public static bool operator >=(TestWeight left, TestWeight right)
    {
        return left.TravelCost >= right.TravelCost;
    }
}