using ShortRouteApi.Models;

namespace ShortRouteApi.Services
{
    public class GraphService
    {
        private Dictionary<string, Node> _graph;

        public GraphService()
        {
            InitializeGraph();
        }

        private void InitializeGraph()
        {
            _graph = new Dictionary<string, Node>();

            // Create all nodes
            var nodes = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
            foreach (var nodeName in nodes)
            {
                _graph[nodeName] = new Node { Name = nodeName };
            }

            // Add edges as per the graph
            _graph["A"].Edges.Add(new Edge { To = "B", Weight = 4 });
            _graph["A"].Edges.Add(new Edge { To = "C", Weight = 6 });
            _graph["C"].Edges.Add(new Edge { To = "D", Weight = 8 });
            _graph["D"].Edges.Add(new Edge { To = "E", Weight = 4 });
            _graph["D"].Edges.Add(new Edge { To = "G", Weight = 1 });
            _graph["E"].Edges.Add(new Edge { To = "F", Weight = 3 });
            _graph["E"].Edges.Add(new Edge { To = "B", Weight = 2 });
            _graph["E"].Edges.Add(new Edge { To = "H", Weight = 5 });
            _graph["F"].Edges.Add(new Edge { To = "B", Weight = 2 });
            _graph["F"].Edges.Add(new Edge { To = "H", Weight = 6 });
            _graph["G"].Edges.Add(new Edge { To = "I", Weight = 5 });
            _graph["G"].Edges.Add(new Edge { To = "E", Weight = 5 });
            _graph["H"].Edges.Add(new Edge { To = "I", Weight = 8 });
        }

        public List<string> GetAllNodes()
        {
            return _graph.Keys.OrderBy(k => k).ToList();
        }

        public RouteResponse FindShortestPath(string start, string end)
        {
            if (!_graph.ContainsKey(start) || !_graph.ContainsKey(end))
            {
                return new RouteResponse
                {
                    ShortestPath = new List<string>(),
                    TotalDistance = -1,
                    Message = "Invalid start or end node"
                };
            }

            var distances = new Dictionary<string, int>();
            var previous = new Dictionary<string, string>();
            var unvisited = new HashSet<string>();

            // Initialize
            foreach (var node in _graph.Keys)
            {
                distances[node] = int.MaxValue;
                previous[node] = null;
                unvisited.Add(node);
            }
            distances[start] = 0;

            // Dijkstra's Algorithm
            while (unvisited.Count > 0)
            {
                // Get node with minimum distance
                string current = null;
                int minDistance = int.MaxValue;
                foreach (var node in unvisited)
                {
                    if (distances[node] < minDistance)
                    {
                        minDistance = distances[node];
                        current = node;
                    }
                }

                if (current == null || distances[current] == int.MaxValue)
                    break;

                unvisited.Remove(current);

                // Check if we reached the destination
                if (current == end)
                    break;

                // Update distances to neighbors
                foreach (var edge in _graph[current].Edges)
                {
                    if (unvisited.Contains(edge.To))
                    {
                        int newDistance = distances[current] + edge.Weight;
                        if (newDistance < distances[edge.To])
                        {
                            distances[edge.To] = newDistance;
                            previous[edge.To] = current;
                        }
                    }
                }
            }

            // Build path
            if (distances[end] == int.MaxValue)
            {
                return new RouteResponse
                {
                    ShortestPath = new List<string>(),
                    TotalDistance = -1,
                    Message = "No path found between the nodes"
                };
            }

            var path = new List<string>();
            var currentNode = end;
            while (currentNode != null)
            {
                path.Insert(0, currentNode);
                currentNode = previous[currentNode];
            }

            return new RouteResponse
            {
                ShortestPath = path,
                TotalDistance = distances[end],
                Message = "Success"
            };
        }
    }
}