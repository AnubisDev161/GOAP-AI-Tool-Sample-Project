using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GOAP.Core.Agent;

namespace GOAP.GOAPGraph
{
    [CreateAssetMenu(menuName = "GOAPGraph/New Graph")]
    public class GOAPGraphAsset : ScriptableObject
    {
        [SerializeReference]
        private List<GOAPGraphNode> nodes = new List<GOAPGraphNode>();
        public List<GOAPGraphNode> Nodes => nodes;

        [SerializeField]
        private List<GOAPGraphConnection> connections;
        public List<GOAPGraphConnection> Connections => connections;

        private Dictionary<string, GOAPGraphNode> nodeDictionary;

        public GOAPAgent agent;

        [SerializeField]
        private GOAPBlackbaord blackboard = new GOAPBlackbaord();

        public GOAPBlackbaord Blackboard => blackboard;

        public void Initialize(GOAPAgent graphObject)
        {
            this.agent = graphObject;
            nodeDictionary = new Dictionary<string, GOAPGraphNode>();

            foreach (var node in nodes)
            {
                nodeDictionary.Add(node.id, node);
            }
        }

        public List<ActionNode> GetActionNodes()
        {
            List<ActionNode> actionNodes = new List<ActionNode>();
            foreach (var node in nodes)
            {
                if (node is ActionNode)
                {
                    actionNodes.Add((ActionNode)node);
                }
            }

            return actionNodes;
        }

        public GoalWorldStateNode[] GetGoalNodes()
        {
            GoalWorldStateNode[] goalNodes = nodes.OfType<GoalWorldStateNode>().ToArray();
            if (goalNodes.Length == 0)
            {
                Debug.LogError("There is no goal node in this graph");
                return null;
            }

            return goalNodes;
        }

        public GOAPGraphNode GetStartNode()
        {
            StartWorldStateNode[] startNodes = nodes.OfType<StartWorldStateNode>().ToArray();
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

        public List<GOAPGraphNode> GetInputNodes(string outpuNodeId, int index)
        {
            List <GOAPGraphNode> inputNodes = null;
            foreach (var connection in connections)
            {
                if (connection.outputPort.nodeId == outpuNodeId && connection.outputPort.portIndex == index)
                {
                    if (inputNodes == null)
                    {
                        inputNodes = new List <GOAPGraphNode>();
                    }

                    string nodeId = connection.inputPort.nodeId;
                    GOAPGraphNode inputNode = nodeDictionary[nodeId];
                    inputNodes.Add(inputNode);
                }
            }

            return inputNodes;
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

        public List<GOAPGraphNode> GetOutputNodes(string inputNodeId, int index)
        {
            List<GOAPGraphNode> outputNodes = null;
            foreach (var connection in connections)
            {
                if (connection.inputPort.nodeId == inputNodeId && connection.inputPort.portIndex == index)
                {
                    if (outputNodes == null)
                    {
                        outputNodes = new List<GOAPGraphNode>();
                    }

                    string nodeId = connection.outputPort.nodeId;
                    GOAPGraphNode outputNode = nodeDictionary[nodeId];
                    outputNodes.Add(outputNode);
                }
            }

            return outputNodes;
        }
    }
}