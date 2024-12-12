namespace Cateros.GraphLib
{
    public abstract class Node<N, E>(int id) 
        where N: Node<N, E>
        where E: Edge<E, N> 
    {
        public int Id {  get; } = id;
        public IList<E> OutgoingEdges{ get; } = [];
        public IList<E> IncomingEdges{ get; } = [];
    }
}
