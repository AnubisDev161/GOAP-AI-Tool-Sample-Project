using GOAPGraph;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Action", "Action / Action", hasInputParams: true, hasOutputParams: true)]
    public class ActionNode : GOAPGraphNode
    {
        [ExposedProperty]
        public string name;
        public override void OnProcess(GOAPGraphAsset currentGraph, DebugInfo debugInfo)
        {
           // //currentGraph.gameObject.transform.position += direction;
           // Debug.Log("Action executed");

           // var paramsConnectedToInput = GetPreconditionNodes(currentGraph);
           // var paramesConnectedToOutput = GetEffectNodes(currentGraph);

           // // TODO Replace this with actual World State from other project
         
           // debugInfo.terminationReason = TerminationReason.preconditionsNotMet;

           // debugInfo.terminationReason = paramsConnectedToInput.Count > 0 ? TerminationReason.preconditionsNotMet : TerminationReason.None;
           //// debugInfo.success = connectedInputParams.Count > 0 ? false : true;

           // foreach (var param in paramsConnectedToInput)
           // {
           //     var data = param.GetData();
           //     Debug.Log("Data: " + data);
                

           //     if (currentGraph.goapGraphObject.worldFact == data)
           //     {
           //         Debug.Log("Preconditions met " + data);
           //         debugInfo.success = true;
           //         debugInfo.terminationReason = TerminationReason.None;
           //     }
           //     else
           //     {
           //         Debug.Log("Preconditions not  met " + data);
           //         debugInfo.success = false;
           //         base.OnProcess(currentGraph, debugInfo);
           //         return;
           //     }
           // }

           // foreach (var param in paramesConnectedToOutput)
           // {
           //     var data = param.GetData();
           //     Debug.Log("Data: " + data);

           //     if (currentGraph.goapGraphObject.worldFact.name == data.name && currentGraph.goapGraphObject.worldFact.value != data.value)
           //     {
           //         currentGraph.goapGraphObject.worldFact.value = data.value;

           //         Debug.Log("Effects applied " + data);
           //     }
           // }

            base.OnProcess(currentGraph, debugInfo);
        }

        public List<BlackbaordKeyNode> GetPreconditionNodes(GOAPGraphAsset currentGraph)
        {
            List<BlackbaordKeyNode> nodesConnectedToInput = new List<BlackbaordKeyNode>();
            foreach (var index in portsIndices)
            {
                if (index < 2) continue;
                // Check if connection to a blackboard node exists
                var outputNode = currentGraph.GetOutputNode(id, index);
                if (outputNode != null && outputNode is BlackbaordKeyNode)
                {
                    nodesConnectedToInput.Add((BlackbaordKeyNode)outputNode);
                }
            }

            return nodesConnectedToInput;
        }

        public List<BlackbaordKeyNode> GetEffectNodes(GOAPGraphAsset currentGraph)
        {
            List<BlackbaordKeyNode> nodesConnectedToOutput = new List<BlackbaordKeyNode>();
            foreach (var index in portsIndices)
            {
                if (index < 2) continue;
                // Check if connection to a blackboard node exists
                var inputtNode = currentGraph.GetInputNode(id, index);
                if (inputtNode != null && inputtNode is BlackbaordKeyNode)
                {
                    nodesConnectedToOutput.Add((BlackbaordKeyNode)inputtNode);
                }
            }

            return nodesConnectedToOutput;
        }
    }
}

