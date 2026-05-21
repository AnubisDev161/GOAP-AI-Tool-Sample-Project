using System.Collections.Generic;

namespace GOAP
{
    //public class GOAPMinHeap
    //{
    //    MinHeapNode rootNode;
    //    MinHeapNode cheapestNode;
    //    public int elementsCount { get; private set; }

    //    Dictionary<GOAPBlackboard, MinHeapNode> insertedNodes = new Dictionary<GOAPBlackboard, MinHeapNode> ();
    //    public GOAPNode Pop()
    //    {
    //        if (cheapestNode == null) return null;

    //        var value = cheapestNode.value;
    //        insertedNodes.Remove(cheapestNode.value.requiredWorldState);
           
    //        if (cheapestNode == rootNode) rootNode = null;
    //        cheapestNode = null;

    //        elementsCount--;
    //        return value;
    //    }

    //    public bool IsEmpty()
    //    {
    //        return rootNode == null;
    //    }

    //    public void PrintAllNodes()
    //    {
    //        rootNode.PrintAllNodes();
    //    }

    //    public bool Push(GOAPNode nodeToAdd)
    //    {
    //        if (IsEmpty())
    //        {
    //            rootNode = new MinHeapNode(nodeToAdd);
    //            cheapestNode = rootNode;
    //            insertedNodes.Add(nodeToAdd.requiredWorldState, cheapestNode);
    //            elementsCount++;
    //            return true;
    //        }
            
    //        var node = rootNode;

    //        var possibleDouble = Find(nodeToAdd);
    //        if (possibleDouble != null && !possibleDouble.IsMoreExpansiveThan(nodeToAdd)) return false; // There can't be two nodes with the same world state and same fCost

    //        while (nodeToAdd != null)
    //        {
    //            if (node.IsMoreExpansiveThan(nodeToAdd))
    //            {
    //                if (node.chidlLeft != null)
    //                {
    //                    node = node.chidlLeft;
    //                }
    //                else
    //                {
    //                    node.chidlLeft = new MinHeapNode(nodeToAdd);
                 
    //                    cheapestNode = node.chidlLeft;
    //                    insertedNodes.Add(nodeToAdd.requiredWorldState, node.chidlLeft);
    //                    nodeToAdd = null;
    //                    elementsCount++;
    //                }
    //            }
    //            else
    //            {
    //                if (node.childRight != null)
    //                {
    //                    node = node.childRight;
    //                }
    //                else
    //                {
    //                    node.childRight = new MinHeapNode(nodeToAdd);

    //                    insertedNodes.Add(nodeToAdd.requiredWorldState, node.childRight);
    //                    nodeToAdd = null;
    //                    elementsCount++;
    //                }
    //            }
    //        }

    //        return true;
    //    }

    //    public void ReplaceExistingIfCheaper(GOAPNode node)
    //    {
    //       var existing = Find(node);

    //        if (existing == null) return;

    //        if (existing.IsMoreExpansiveThan(node))
    //        {
    //            existing.value = node;
    //        }

    //    }
    //    public MinHeapNode Find(GOAPNode nodeToFind)
    //    {
    //        insertedNodes.TryGetValue(nodeToFind.requiredWorldState, out var value);

    //        if (value == null) return null;

    //        return value;
    //    }
    //    public bool Contains(GOAPNode nodeToFind)
    //    {
    //        return Find(nodeToFind) != null;
    //    }

    //    public bool Remove(GOAPNode nodeToRemove)
    //    {
    //       return insertedNodes.Remove(nodeToRemove.requiredWorldState);
    //    }
    //}

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
}
