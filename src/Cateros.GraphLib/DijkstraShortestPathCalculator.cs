using Microsoft.Extensions.ObjectPool;
using System.Data.SqlTypes;
using System.Numerics;

namespace Cateros.GraphLib
{
    public class DijkstraShortestPathCalculator<G, E, N, W> : IShortestPathCalculator<G, E, N, W>
        where G : Graph<E, N>
        where N : Node<N, E>
        where E : Edge<E, N>
        where W : struct, IAdditionOperators<W, W, W>, IComparable<W>, IMinMaxValue<W>, IComparisonOperators<W, W, bool>
    {
        public IEnumerable<(Edge<E, N>, W Cost)>? CalculateShortestPath(G graph, N startNode, N endNode, IEdgeVisitor<E, N, W> edgeVisitor)
        {

            var cost1 = new ObjectPool<Dictionary<int, (Edge<E, N>? Previous, W Cost)>>(() => graph.Nodes
                .Select((node, i) => (node, details: (Previous: (Edge<E, N>?)null, Cost: W.MaxValue)))
                .ToDictionary(x => x.node.Value.Id, x => x.details));

            // Initialize all the distances to max
            Dictionary<int, (Edge<E, N>? Previous, W Cost)> costs =  graph.Nodes
                .Select((node, i) => (node, details: (Previous: (Edge<E, N>?)null, Cost: W.MaxValue)))
                .ToDictionary(x => x.node.Value.Id, x => x.details);

            // priority queue for tracking shortest distance from the start node to each other node
            var queue = new PriorityQueue<Node<N, E>, W>();

            // initialize the start node at a distance of 0
            costs[startNode.Id] = (null, W.MinValue);

            // add the start node to the queue for processing
            queue.Enqueue(startNode, W.MinValue);

            // as long as we have a node to process, keep looping
            while (queue.Count > 0)
            {
                // remove the node with the current smallest distance from the start node
                var current = queue.Dequeue();

                // if this is the node we want, then we're finished
                // as we must already have the shortest route!
                if (current == endNode)
                {
                    if (costs.TryGetValue(current.Id, out var cost) && cost.Previous is not null)
                        // build the route by tracking back through previous
                        return IShortestPathCalculator<G, E, N, W>.BuildRoute(costs, endNode);
                    else return null;
                }

                // add the node to the "visited" list
                var currentNodeCost = costs[current.Id].Cost;

                foreach (var edge in current.OutgoingEdges)
                {
                    if (edgeVisitor.CanVisit(edge))
                    {

                        var cost = edgeVisitor.VisitCost(edge);
                        // get the current shortest distance to the connected node
                        var currentCost = costs[edge.EndNode.Id].Cost;

                        // calculate the new cumulative distance to the edge
                        var newCost = currentNodeCost + cost;

                        // if the new distance is shorter, then it represents a new 
                        // shortest-path to the connected edge
                        if (newCost < currentCost)
                        {
                            // update the shortest distance to the connection
                            // and record the "current" node as the shortest
                            // route to get there 
                            costs[edge.EndNode.Id] = (edge, newCost);

                            // if the node is already in the queue, first remove it
                            queue.Remove(edge.EndNode, out _, out _);
                            // now add the node with the new distance
                            queue.Enqueue(edge.EndNode, newCost);
                        }
                    }
                }
            }

            // if we don't have anything left, then we've processed everything,
            // but didn't find the node we want
            return null;
        }

        public Dictionary<int, (Edge<E, N>? Previous, W Cost)> CalculateShortestPathsFromNode(G graph, N startNode, IEdgeVisitor<E, N, W> edgeVisitor)
        {
            // Initialize all the distances to max, and the "previous" city to null
            var distances = graph.Nodes
                .Select((node, i) => (node, details: (Previous: (Edge<E, N>?)null, Distance: W.MaxValue)))
                .ToDictionary(x => x.node.Value.Id, x => x.details);

            // priority queue for tracking shortest distance from the start node to each other node
            var queue = new PriorityQueue<Node<N, E>, W>();

            // initialize the start node at a distance of 0
            distances[startNode.Id] = (null, W.MinValue);

            // add the start node to the queue for processing
            queue.Enqueue(startNode, W.MinValue);

            // as long as we have a node to process, keep looping
            while (queue.Count > 0)
            {
                // remove the node with the current smallest distance from the start node
                var current = queue.Dequeue();

                // add the node to the "visited" list
                var currentNodeDistance = distances[current.Id].Distance;

                foreach (var edge in current.OutgoingEdges)
                {
                    if (edgeVisitor.CanVisit(edge))
                    {

                        var cost = edgeVisitor.VisitCost(edge);
                        // get the current shortest distance to the connected node
                        var distance = distances[edge.EndNode.Id].Distance;

                        // calculate the new cumulative distance to the edge
                        var newDistance = currentNodeDistance + cost;

                        // if the new distance is shorter, then it represents a new 
                        // shortest-path to the connected edge
                        if (newDistance < distance)
                        {
                            // update the shortest distance to the connection
                            // and record the "current" node as the shortest
                            // route to get there 
                            distances[edge.EndNode.Id] = (edge, newDistance);

                            // if the node is already in the queue, first remove it
                            queue.Remove(edge.EndNode, out _, out _);
                            // now add the node with the new distance
                            queue.Enqueue(edge.EndNode, newDistance);
                        }
                    }
                }
            }
            return distances;
        }

    }
}


