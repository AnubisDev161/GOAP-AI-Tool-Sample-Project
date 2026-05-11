using Codice.Client.GameUI.Update;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace GOAPGraph
{
    [Serializable]
    public class GOAPGraphNode 
    {
        [SerializeField]
        private string guid;
        [SerializeField]
        private Rect pos;

        public Action<string, string, GOAPGraphAsset> processFinished;
        public Action valueUpdated;

        public string typeName;
        
        [SerializeField]
        protected List<int> portsIndices = new List<int>();

        public string id => guid;
        public Rect position => pos;

        public GOAPGraphNode()
        { 
            NewGUID();
        }

        private void NewGUID()
        {
            guid = Guid.NewGuid().ToString();
        }

        public void SetPosition(Rect newPosition)
        {
            pos = newPosition;
        }

        public void SetPorts(List<int> portsIndices)
        {
            this.portsIndices = portsIndices;
        }

        public virtual void OnProcess(GOAPGraphAsset currentGraph, DebugInfo debugInfo)
        {
            //GOAPGraphNode nextNodeInFlow = currentGraph.GetInputNode(guid, OUTPUT_PORT_INDEX);
            //if (nextNodeInFlow != null && debugInfo.success)
            //{
            //    processFinished?.Invoke(this.id, nextNodeInFlow.id, currentGraph);
            //    return;
            //}

            //if (debugInfo.terminationReason == TerminationReason.None)
            //{
            //    debugInfo.terminationReason = TerminationReason.noSuccessorNodeFound;
            //}

            //processFinished?.Invoke(this.id, string.Empty, currentGraph);
            //Debug.Log("Graph process terminated at Node: " + this + " Reason: " + debugInfo.terminationReason);
        }

        public List<BlackbaordKeyNode> GetPreconditionNodes(GOAPGraphAsset currentGraph)
        {
            List<BlackbaordKeyNode> nodesConnectedToInput = new List<BlackbaordKeyNode>();
            foreach (var index in portsIndices)
            {
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

    public struct DebugInfo
    {
        public bool success;
        public TerminationReason terminationReason;

        public DebugInfo(bool success, TerminationReason terminationReason)
        {
            this.success = success;
            this.terminationReason = terminationReason;
        }
    }

    public enum TerminationReason
    {
        None,
        noSuccessorNodeFound,
        preconditionsNotMet
    }
}