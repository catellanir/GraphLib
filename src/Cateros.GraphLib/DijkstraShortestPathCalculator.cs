namespace Cateros.GraphLib
{
    public class DijkstraShortestPathCalculator<G, E, N, W>: IShortestPathCalculator<G, E, N, W> where G : Graph<E, N> where N : Node<N, E> where E : Edge<E, N>
    {
        public List<(Node<N, E>, int Cost)>? CalculateShortestPath(G graph, N startNode, N endNode, IEdgeVisitor<E, N, W> edgeVisitor)
        {

            // Initialize all the distances to max, and the "previous" city to null
            Dictionary<int, (Node<N, E>? Previous, int Distance)> distances = graph.Nodes.Select((node, i) => (node, details: (Previous: (Node<N,E>?)null, Distance: int.MaxValue)))
                .ToDictionary(x => x.node.Value.Id, x => x.details);

            // priority queue for tracking shortest distance from the start node to each other node
            var queue = new PriorityQueue<Node<N, E>, double>();

            // initialize the start node at a distance of 0
            distances[startNode.Id] = (null, 0);

            // add the start node to the queue for processing
            queue.Enqueue(startNode, 0);

            // as long as we have a node to process, keep looping
            while (queue.Count > 0)
            {
                // remove the node with the current smallest distance from the start node
                var current = queue.Dequeue();

                // if this is the node we want, then we're finished
                // as we must already have the shortest route!
                if (current == endNode)
                {
                    // build the route by tracking back through previous
                    return BuildRoute(distances, endNode);
                }

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
                            distances[edge.EndNode.Id] = (current, newDistance);

                            // if the node is already in the queue, first remove it
                            queue.Remove(edge.EndNode, out _, out _);
                            // now add the node with the new distance
                            queue.Enqueue(edge.EndNode, newDistance);
                        }
                    }
                }
            }

            // if we don't have anything left, then we've processed everything,
            // but didn't find the node we want
            return null;
        }

        public Dictionary<int, (Edge<E, N>? Previous, int Distance)> CalculateShortestPathsFromNode(G graph, N startNode, N endNode, IEdgeVisitor<E, N, W> edgeVisitor)
        {
            // Initialize all the distances to max, and the "previous" city to null
            var distances = graph.Nodes.Select((node, i) => (node, details: (Previous: (Node<N, E>?)null, Distance: int.MaxValue)))
                .ToDictionary(x => x.node.Value.Id, x => x.details);

            // priority queue for tracking shortest distance from the start node to each other node
            var queue = new PriorityQueue<Node<N, E>, double>();

            // initialize the start node at a distance of 0
            distances[startNode.Id] = (null, 0);

            // add the start node to the queue for processing
            queue.Enqueue(startNode, 0);

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
                            distances[edge.EndNode.Id] = (current, newDistance);

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

        private List<(Node<N, E>, int)> BuildRoute(Dictionary<int, (Node<N, E>? previous, int Distance)> distances, N endNode)
        {
            var route = new List<(Node<N, E>, int)>();
            Node<N,E>? prev = endNode;

            // Keep examining the previous version until we
            // get back to the start node
            while (prev is not null)
            {
                var current = prev;
                (prev, var distance) = distances[current.Id];
                route.Add((current, distance));
            }

            // reverse the route
            route.Reverse();
            return route;
        }
    }
}


