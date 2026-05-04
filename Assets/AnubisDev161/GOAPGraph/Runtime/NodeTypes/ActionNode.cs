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
            //currentGraph.gameObject.transform.position += direction;
            Debug.Log("Action executed");

            var connectedOutputParams = GetConnectedOutputParams(currentGraph);
            var connectedInputParams = GetConnectedInputParams(currentGraph);

            // TODO Replace this with actual World State from other project
         
            debugInfo.terminationReason = TerminationReason.preconditionsNotMet;

            debugInfo.terminationReason = connectedOutputParams.Count > 0 ? TerminationReason.preconditionsNotMet : TerminationReason.None;
           // debugInfo.success = connectedInputParams.Count > 0 ? false : true;

            foreach (var outputParam in connectedOutputParams)
            {
                var data = outputParam.GetData();
                Debug.Log("Data: " + data);
                

                if (currentGraph.goapGraphObject.worldFact == data)
                {
                    Debug.Log("Preconditions met " + data);
                    debugInfo.success = true;
                    debugInfo.terminationReason = TerminationReason.None;
                }
            }

            foreach (var inputParam in connectedInputParams)
            {
                var data = inputParam.GetData();
                Debug.Log("Data: " + data);

                if (currentGraph.goapGraphObject.worldFact.name == data.name && currentGraph.goapGraphObject.worldFact.value != data.value)
                {
                    currentGraph.goapGraphObject.worldFact.value = data.value;

                    Debug.Log("Effects applied " + data);
                }
            }

           base.OnProcess(currentGraph, debugInfo);
        }

        private List<BlackbaordKeyNode> GetConnectedOutputParams(GOAPGraphAsset currentGraph)
        {
            List<BlackbaordKeyNode> outputParamNodes = new List<BlackbaordKeyNode>();
            foreach (var index in portsIndices)
            {
                if (index < 2) continue;
                // Check if connection to a blackboard node exists
                var outputNode = currentGraph.GetOutputNode(id, index);
                if (outputNode != null && outputNode is BlackbaordKeyNode)
                {
                    outputParamNodes.Add((BlackbaordKeyNode)outputNode);
                }
            }

            return outputParamNodes;
        }

        private List<BlackbaordKeyNode> GetConnectedInputParams(GOAPGraphAsset currentGraph)
        {
            List<BlackbaordKeyNode> inputParamNodes = new List<BlackbaordKeyNode>();
            foreach (var index in portsIndices)
            {
                if (index < 2) continue;
                // Check if connection to a blackboard node exists
                var inputtNode = currentGraph.GetInputNode(id, index);
                if (inputtNode != null && inputtNode is BlackbaordKeyNode)
                {
                    inputParamNodes.Add((BlackbaordKeyNode)inputtNode);
                }
            }

            return inputParamNodes;
        }
    }
}

