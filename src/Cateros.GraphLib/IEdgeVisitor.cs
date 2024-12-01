using System.Collections.Generic;

namespace Cateros.GraphLib
{
    public interface IEdgeVisitor<E, N, W> where N : Node<N, E> where E : Edge<E, N>
    {
        public bool CanVisit(E edge);
        public W VisitCost(E edge);
        public Maybe<W> Visit(E edge);
    }
}


