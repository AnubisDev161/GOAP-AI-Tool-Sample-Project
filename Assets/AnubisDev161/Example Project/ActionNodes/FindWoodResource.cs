using System.Collections.Generic;
using GOAP.GOAPGraph;
using GOAP.Core;
using UnityEngine;
using Unity.Mathematics;

namespace ExampleProject
{
    [NodeInfo("find wood source", "Example / find wood source", hasInputParams: true, hasOutputParams: true)]
    
    public class FindWoodResource : ActionNode
    {
        [ExposedProperty]
        public LayerMask treeMask;
        [ExposedProperty]
        public float sphereCastRadius;
        [ExposedProperty]
        public string targetPosKeyName;
        [ExposedProperty]
        public string wodSourceKeyName;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            success = false;
            var exampleAgent = currentGraph.agent as ExampleAgent;
            var closestTree = GetClosestTree(currentGraph.agent.transform.position);
            if (closestTree != null)
            {
                var targetPosKey = currentGraph.Blackboard.GetKey(targetPosKeyName);
                var woodSourceKey = currentGraph.Blackboard.GetKey(wodSourceKeyName);
                targetPosKey.value = closestTree.transform.position;
                woodSourceKey.value = closestTree;
                success = true;
            }

            base.OnExecuteFinished(currentGraph, worldState, success);
        }

        private GameObject GetClosestTree(Vector3 position)
        {
            var trees = Physics.OverlapSphere(position, sphereCastRadius, treeMask);
            GameObject closestTree = null;
            float closestDistance = math.INFINITY;

            foreach (var tree in trees)
            {
                if ((position - tree.transform.position).magnitude < closestDistance)
                { 
                    closestDistance = (position - tree.transform.position).magnitude;
                    closestTree = tree.gameObject; 
                }
            }

            return closestTree;
        }

        public override bool IsAchvievable(GOAPGraphAsset currentGraph)
        {
            if (GetClosestTree(currentGraph.agent.transform.position) == null)
            {
                return false;
            }

            return true;
        }
    }
}