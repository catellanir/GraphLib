using System.Collections.Generic;

namespace Cateros.GraphLib
{
    public interface IEdgeVisitor<E, N, W> where N : Node<N, E> where E : Edge<E, N>
    {
        bool CanVisit(E edge);
        W VisitCost(E edge);
        Maybe<W> Visit(E edge);
    }
}


