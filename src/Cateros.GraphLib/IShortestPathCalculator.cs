using System.Numerics;

namespace Cateros.GraphLib
{
    public interface IShortestPathCalculator<G, E, N, W> 
        where G: Graph<E, N> 
        where N : Node<N, E> 
        where E : Edge<E, N>
        where W : struct, IAdditionOperators<W, W, W>, IComparable<W>, IMinMaxValue<W>, IComparisonOperators<W, W, bool>

    {
        IEnumerable<(Edge<E, N>, W Cost)>? CalculateShortestPath(G graph, N startNode, N endNode, IEdgeVisitor<E, N, W> edgeVisitor);
        Dictionary<int, (Edge<E, N>? Previous, W Cost)> CalculateShortestPathsFromNode(G graph, N startNode, IEdgeVisitor<E, N, W> edgeVisitor);
        
        static IEnumerable<(Edge<E, N>, W)> BuildRoute(Dictionary<int, (Edge<E, N>? previous, W cost)> costs, Node<N, E> targetNode)
        {
            return BuildBackRoute(costs, targetNode).Reverse();
        }

        static IEnumerable<(Edge<E, N>, W)> BuildBackRoute(Dictionary<int, (Edge<E, N>? previous, W cost)> costs, Node<N, E> targetNode)
        {
            // Keep examining the previous version until we get back to the start node
            (var prev, var cost) = costs[targetNode.Id];
            while (prev is not null)
            {
                yield return (prev, cost);
                (prev, cost) = costs[prev.StartNode.Id];
            }
        }
    }
}


