using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;

namespace GOAPGraph
{
    public class GOAPGraphObject : MonoBehaviour
    {
        [field: SerializeField]
        public GOAPGraphAsset graphAsset; // Replace with Blackboard class

        private GOAPGraphAsset graphInstance;

        [HideInInspector]
        public WorldFact worldFact = new WorldFact("DialogueText", false, ValueType.Bool);

        public NavMeshAgent navMeshAgent;

        public Action<GOAPGraphAsset> destinationReached;

        private bool isDestinationReached;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            graphInstance =  Instantiate(graphAsset);
            ExecuteAsset();
        }

        private void ExecuteAsset()
        {
            graphInstance.Initialize(this);
            GOAPGraphNode startNode = graphInstance.GetStartNode();
            ProcessAndMoveToNextNode(startNode);
        }

        private void Update()
        {
            if (!isDestinationReached && navMeshAgent.remainingDistance <= 2)
            {
                destinationReached?.Invoke(graphInstance);
                isDestinationReached = true;
            }
        }

        private void ProcessAndMoveToNextNode(GOAPGraphNode startNode)
        {
            startNode.processFinished += OnProcessFinished;
            startNode.OnProcess(graphInstance, new DebugInfo(true, TerminationReason.None));
        }

        public void OnProcessFinished(string lastNodeId, string nextNodeId, GOAPGraphAsset currentGraph)
        {
            graphInstance.GetNode(lastNodeId).processFinished -= OnProcessFinished;

            if (!string.IsNullOrEmpty(nextNodeId))
            {
                GOAPGraphNode node = graphInstance.GetNode(nextNodeId);
                ProcessAndMoveToNextNode(node);
            }
        }
    }
}

