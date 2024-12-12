namespace Cateros.GraphLib
{
    public abstract class Edge<E, N> where N : Node<N, E> where E: Edge<E, N>
    {

        public int Id { get; }
        public Node<N, E> StartNode { get; }
        public Node<N, E> EndNode { get; }

        public Edge(int id, Node<N, E> startNode, Node<N, E> endNode)
        {
            Id = id;
            StartNode = startNode;
            EndNode = endNode;
            startNode.OutgoingEdges.Add((E)this);
            EndNode.IncomingEdges.Add((E)this);
        }
        public override string ToString() => $"Edge {Id}";
    }
}
