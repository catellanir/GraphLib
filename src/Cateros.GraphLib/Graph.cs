using System.Collections.ObjectModel;

namespace Cateros.GraphLib
{
    public abstract class Graph<E, N> where N : Node<N, E> where E : Edge<E, N>
    {
        private readonly Dictionary<int, E> _edges = [];
        private readonly Dictionary<int, N> _nodes = [];

        public ReadOnlyDictionary<int, N> Nodes => _nodes.AsReadOnly();
        public ReadOnlyDictionary<int, E> Edges => _edges.AsReadOnly();


        public void AddEdge(E edge)
        {
            _edges.Add(edge.Id, edge);
        }

        public void AddNode(N node)
        {
            _nodes.Add(node.Id, node);
        }
    }
}
