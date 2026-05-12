using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using GOAP;

namespace GOAPGraph
{
    public class GOAPGraphObject : MonoBehaviour // TODO Replace with GOAP Brain
    {
        public GOAPNavigation navigation { get; private set; }
        private void OnEnable()
        {
            var navigation = GetComponent<GOAPNavigation>();

            if (navigation == null)
            {
                Debug.LogError("No GOAPNaviagtion component found!");
                return;
            }

            this.navigation = navigation;
        }
        ////  [HideInInspector]
        //  public WorldFact worldFact; //= new WorldFact("DialogueText", false, ValueType.Bool);

        //  public NavMeshAgent navMeshAgent;

        //  public Action<GOAPGraphAsset> destinationReached;

        //  private bool isDestinationReached;

        //  private void Start()
        //  {
        //      navMeshAgent = GetComponent<NavMeshAgent>();
        //  }

        //private void OnEnable()
        //{
        //    graphInstance = (graphAsset);
        //    ExecuteAsset();
        //}


        //private void ExecuteAsset()
        //{
        //    graphInstance.Initialize(this);
        //    GOAPGraphNode startNode = graphInstance.GetStartNode();


        //   // ProcessAndMoveToNextNode(startNode);
        //}

        //private void Update()
        //{
        //    if (!isDestinationReached && navMeshAgent.remainingDistance <= 2)
        //    {
        //        destinationReached?.Invoke(graphInstance);
        //        isDestinationReached = true;
        //    }
        //}

        //private void ProcessAndMoveToNextNode(GOAPGraphNode startNode)
        //{
        //    startNode.processFinished += OnProcessFinished;
        //    startNode.OnProcess(graphInstance, new DebugInfo(true, TerminationReason.None));
        //}

        //public void OnProcessFinished(string lastNodeId, string nextNodeId, GOAPGraphAsset currentGraph)
        //{
        //    graphInstance.GetNode(lastNodeId).processFinished -= OnProcessFinished;

        //    if (!string.IsNullOrEmpty(nextNodeId))
        //    {
        //        GOAPGraphNode node = graphInstance.GetNode(nextNodeId);
        //        ProcessAndMoveToNextNode(node);
        //    }
        //}
    }
}

