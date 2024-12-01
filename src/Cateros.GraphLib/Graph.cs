namespace Cateros.GraphLib
{
    public abstract class Graph<E, N> where N : Node<N, E> where E : Edge<E, N>
    {
        public Dictionary<int, Node<N,E>> Nodes { get; } = [];
        public Dictionary<int, E> Edges { get; } = [];
    }
}
