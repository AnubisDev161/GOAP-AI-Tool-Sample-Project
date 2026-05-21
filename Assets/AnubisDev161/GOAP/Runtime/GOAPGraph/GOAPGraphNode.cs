using System;
using System.Collections.Generic;
using UnityEngine;
using GOAP.Core;

namespace GOAP.GOAPGraph
{
    [Serializable]
    public class GOAPGraphNode
    {
        [SerializeField]
        private string guid;
        [SerializeField]
        private Rect pos;

        public Action<GOAPGraphAsset, WorldState, bool> executeFinished;
        
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

        public virtual void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            OnExecuteFinished(currentGraph, worldState, success);
        }

        public virtual void OnExecuteFinished(GOAPGraphAsset currentGraph, WorldState worldState, bool success)
        {
             executeFinished?.Invoke(currentGraph, worldState, success);
        }

        public List<WorldFactNode> GetPreconditionNodes(GOAPGraphAsset currentGraph)
        {
            List<WorldFactNode> nodesConnectedToInput = new List<WorldFactNode>();
            foreach (var index in portsIndices)
            {
                // Check if connection to a blackboard node exists
                var outputNodes = currentGraph.GetOutputNodes(id, index);
                if (outputNodes == null) continue;
                foreach (var outputNode in outputNodes)
                {
                    if (outputNode != null && outputNode is WorldFactNode)
                    {
                        nodesConnectedToInput.Add((WorldFactNode)outputNode);
                    }
                }
            }

            return nodesConnectedToInput;
        }

        public List<WorldFactNode> GetEffectNodes(GOAPGraphAsset currentGraph)
        {
            List<WorldFactNode> nodesConnectedToOutput = new List<WorldFactNode>();
            foreach (var index in portsIndices)
            {
                // Check if connection to a blackboard node exists

                var inputNodes = currentGraph.GetInputNodes(id, index);
                if (inputNodes == null) continue;
                foreach (var inputNode in inputNodes)
                {
                    if (inputNode != null && inputNode is WorldFactNode)
                    {
                        nodesConnectedToOutput.Add((WorldFactNode)inputNode);
                    }
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