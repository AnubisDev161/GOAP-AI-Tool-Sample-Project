using System.Collections.Generic;

namespace GOAP
{
    public class ListBasedPriorityQueue
    {
        private List<GOAPNode> insertedNodes = new List<GOAPNode>();

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

        public void Push(GOAPNode nodeToAdd)
        {
            GOAPNode nodeToReplace = null;
            foreach (var node in insertedNodes)
            {
                if (node == nodeToAdd && (node.fCost < nodeToAdd.fCost || node.fCost == nodeToAdd.fCost && node.hCost < nodeToAdd.hCost))
                {
                    nodeToReplace = nodeToAdd;
                }
            }

            if (nodeToReplace != null)
            {
                nodeToReplace = nodeToAdd;
            }
            else
            {
                insertedNodes.Add(nodeToAdd);
            }
        }
    }
}
//public class MinHeapNode
//{
//    internal MinHeapNode chidlLeft;
//    internal MinHeapNode childRight;
//    public GOAPNode value;
//    internal MinHeapNode(GOAPNode value)
//    {
//        this.value = value;
//    }

//    internal bool IsMoreExpansiveThan(GOAPNode other)
//    {
//        return value.fCost > other.fCost || value.fCost == other.fCost && value.hCost > other.hCost;
//    }

//    public void PrintAllNodes()
//    {
//        if (value.action != null)
//        {
//            MessageBus.PrintToUnityLog($"Node value {value.action.name}");
//        }

//        if (chidlLeft != null)
//        {
//            chidlLeft.PrintAllNodes();
//        }

//        if (childRight != null)
//        {
//            childRight.PrintAllNodes();
//        }
//    }
//}
