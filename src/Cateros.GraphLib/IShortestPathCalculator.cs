namespace Cateros.GraphLib
{
    public interface IShortestPathCalculator<G, E, N, W> where G: Graph<E, N> where N : Node<N, E> where E : Edge<E, N>
    {
        List<(Node<N, E>, int Cost)>? CalculateShortestPath(G graph, N startNode, N endNode, IEdgeVisitor<E, N, W> edgeVisitor);
        Dictionary<int, (Edge<E, N>? Previous, int Distance)> CalculateShortestPathsFromNode(G graph, N startNode, N endNode, IEdgeVisitor<E, N, W> edgeVisitor);
    }
}


