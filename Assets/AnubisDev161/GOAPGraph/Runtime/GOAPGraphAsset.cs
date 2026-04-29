using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAPGraph
{
    [CreateAssetMenu(menuName = "GOAPGraph/New Graph")]
    public class GOAPGraphAsset : ScriptableObject
    {
        [SerializeReference]
        private List<GOAPGraphNode> nodes;
        public List<GOAPGraphNode> Nodes => nodes;
        
        [SerializeField]
        private List<GOAPGraphConnection> connections;
        public List<GOAPGraphConnection> Connections => connections;

        private Dictionary<string, GOAPGraphNode> nodeDictionary;

        public GOAPGraphObject goapGraphObject;
        public GOAPGraphAsset()
        {
            nodes = new List<GOAPGraphNode>();
            connections = new List<GOAPGraphConnection>();
        }

        public void Initialize(GOAPGraphObject graphObject)
        {
            this.goapGraphObject = graphObject;
            nodeDictionary = new Dictionary<string, GOAPGraphNode>();
         
            foreach (var node in nodes)
            {
                nodeDictionary.Add(node.id, node);
            }
        }
            
        public GOAPGraphNode GetStartNode()
        {
            StartNode[] startNodes = nodes.OfType<StartNode>().ToArray();
            if (startNodes.Length == 0)
            {
                Debug.LogError("There is no start node in this graph");
                return null;
            }

            return startNodes[0];
        }

        internal GOAPGraphNode GetNode(string nextNodeId)
        {
            if (nodeDictionary.TryGetValue(nextNodeId, out GOAPGraphNode node))
            {
                return node;
            }
            
            return null;
        }

        public GOAPGraphNode GetInputNode(string outpuNodeId, int index)
        {
            foreach (var connection in connections)
            {
                if (connection.outputPort.nodeId == outpuNodeId && connection.outputPort.portIndex == index)
                {
                    string nodeId = connection.inputPort.nodeId;
                    GOAPGraphNode inputNode = nodeDictionary[nodeId];   
                    return inputNode;
                }
            }

            return null;
        }

        public GOAPGraphNode GetOutputNode(string inputNodeId, int index)
        {
            foreach (var connection in connections)
            {
                if (connection.inputPort.nodeId == inputNodeId && connection.inputPort.portIndex == index)
                {
                    string nodeId = connection.outputPort.nodeId;
                    GOAPGraphNode outputNode = nodeDictionary[nodeId];
                    return outputNode;
                }
            }

            return null;
        }
    }
}