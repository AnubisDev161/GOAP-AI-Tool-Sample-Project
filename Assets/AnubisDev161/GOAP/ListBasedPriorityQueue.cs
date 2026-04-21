using System.Collections.Generic;

namespace GOAP
{
    public class ListBasedPriorityQueue
    {
        private List<GOAPNode> insertedNodes = new List<GOAPNode>();
        Dictionary<WorldState, GOAPNode> insertedStates = new Dictionary<WorldState, GOAPNode>();

        public float count => insertedNodes.Count;

        public GOAPNode Pop()
        {
            GOAPNode cheapestNode = insertedNodes[0];

            foreach (var node in insertedNodes)
            {
                if (node.fCost < cheapestNode.fCost || node.fCost == cheapestNode.fCost && node.hCost < cheapestNode.hCost)
                {
                    cheapestNode = node;
                }
            }

            insertedNodes.Remove(cheapestNode);
            insertedStates.Remove(cheapestNode.requiredWorldState);

            return cheapestNode;
        }

        public GOAPNode GetMin()
        {
            GOAPNode cheapestNode = null;

            foreach (var node in insertedNodes)
            {
                if (node.fCost < cheapestNode.fCost || node.fCost == cheapestNode.fCost && node.hCost < cheapestNode.hCost)
                {
                    cheapestNode = node;
                }
            }

            return cheapestNode;
        }

        public GOAPNode GetItem(WorldState worldState)
        {
            foreach (var node in insertedNodes)
            {
                if (node.requiredWorldState == worldState)
                {
                    return node;
                }
            }

            return null;
        }

        public bool Contains(WorldState worldStateToFind)
        {
            foreach (var node in insertedNodes)
            {
                if (node.requiredWorldState == (worldStateToFind))
                {
                    return true;
                }
            }

            return false;
        }

        public void Push(GOAPNode nodeToAdd)
        {
            insertedStates.Add(nodeToAdd.requiredWorldState, nodeToAdd);
            insertedNodes.Add(nodeToAdd);
        }
    }
}
