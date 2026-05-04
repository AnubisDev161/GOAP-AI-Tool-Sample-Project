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

        public const int OUTPUT_PORT_INDEX = 0;
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
            GOAPGraphNode nextNodeInFlow = currentGraph.GetInputNode(guid, OUTPUT_PORT_INDEX);
            if (nextNodeInFlow != null && debugInfo.success)
            {
                processFinished?.Invoke(this.id, nextNodeInFlow.id, currentGraph);
                return;
            }

            if (debugInfo.terminationReason == TerminationReason.None)
            {
                debugInfo.terminationReason = TerminationReason.noSuccessorNodeFound;
            }

            processFinished?.Invoke(this.id, string.Empty, currentGraph);
            Debug.Log("Graph process terminated at Node: " + this + " Reason: " + debugInfo.terminationReason);
        }

        public virtual void OnWorldFactPropertyChanged(SerializedPropertyChangeEvent evt)
        {
        
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