namespace Cateros.GraphLib
{
    public abstract class Edge<E, N>(int id, Node<N, E> startNode, Node<N, E> endNode) where N : Node<N, E> where E: Edge<E, N>
    {
        public int Id { get; } = id;
        public Node<N, E> StartNode { get; } = startNode;
        public Node<N, E> EndNode { get; } = endNode;
    }
}
