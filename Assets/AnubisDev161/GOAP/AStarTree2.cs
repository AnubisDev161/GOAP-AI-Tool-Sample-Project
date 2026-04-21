using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AStarTree2 : MonoBehaviour
{
    public class TestNode
    {

        public int nodeId { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public Dictionary<int, double> neighbours { get; set; }


    }

    public class Map
    {
        public HashSet<TestNode> nodes { get; set; }
        public TestNode GetNodeById(int nodeId)
        {
            return nodes.FirstOrDefault(node => node.nodeId == nodeId);
        }
    }

    public class AStarTree
    {
        double Heuristic(TestNode current, TestNode goal)
        {
            return Mathf.Abs(current.x - goal.x) + Mathf.Abs(current.y - goal.y);
        }

        public List<TestNode> GeneratePath(TestNode start, TestNode goal, Map map)
        {
            var openList = new List<TestNode> { start };
            var closedList = new HashSet<TestNode>();

            // Dictionarries to hold g(n), h(n), and parent pointers

            var gScore = new Dictionary<int, double> { [start.nodeId] = 0 };
            var hScore = new Dictionary<int, double> { [start.nodeId] = Heuristic(start, goal) };
            var parentMap = new Dictionary<int, TestNode>();

            while (openList.Count > 0)
            {
                // Find node in open list with the lowest F score
                var current = openList.OrderBy(node => gScore[node.nodeId] + hScore[node.nodeId]).First();

                if (current.nodeId == goal.nodeId)
                {
                    return ReconstructPath(parentMap, current);
                }

                // Treat Node as explored
                openList.Remove(current);
                closedList.Add(current);

                AddNeighboursIfPossible(current, gScore, hScore, openList, closedList, goal, map, parentMap);

            }

            return null; // No path found
        }

        private List<TestNode> ReconstructPath(Dictionary<int, TestNode> parentMap, TestNode current)
        {
            var path = new List<TestNode> { current };

            while (parentMap.ContainsKey(current.nodeId))
            {
                current = parentMap[current.nodeId];
                path.Add(current);
            }

            path.Reverse();
            return path;
        }
        private void AddNeighboursIfPossible(TestNode current, Dictionary<int, double> gScore, Dictionary<int, double> hScore, List<TestNode> openList, HashSet<TestNode> closedList, TestNode goal, Map map, Dictionary<int, TestNode> parentMap)
        {
            foreach (var neigbourId in current.neighbours.Keys)
            {
                var neighbour = map.GetNodeById(neigbourId);
                if (neighbour == null || closedList.Contains(neighbour)) continue;

                // Tentative gScore (current gScore + distance to neighbour
                double tentativeGScore = gScore[current.nodeId] + gScore[neighbour.nodeId];

                if (!gScore.ContainsKey(neighbour.nodeId) || tentativeGScore < gScore[neighbour.nodeId])
                {
                    // Update gScore and hScore
                    gScore[neighbour.nodeId] = tentativeGScore;
                    hScore[neighbour.nodeId] = Heuristic(neighbour, goal);

                    // Set the current node as the parent of the neighbour
                    parentMap[neighbour.nodeId] = current;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
    }
}
