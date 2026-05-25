using GOAP.Core;
using System.Collections.Generic;

namespace GOAP.Tree
{
    /// <summary>
    /// I've choosen a list as data structure because the open queue usually holds a very small amount of items in this implementation
    /// </summary>
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

        public void ReplaceItem(GOAPNode newItemValue)
        {
            for (int i = 0; i < insertedNodes.Count; i++)
            {
                if (insertedNodes[i].requiredWorldState == newItemValue.requiredWorldState)
                {
                    insertedNodes[i] = newItemValue;
                }
            }
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
