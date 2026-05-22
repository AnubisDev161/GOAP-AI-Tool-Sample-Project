using GOAP.Core;
using GOAP.GOAPGraph;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ExampleProject
{
    [NodeInfo("Find grow spot", "Example / Find grow spot", hasInputParams: true, hasOutputParams: true)]
    public class FindGrowSpot : ActionNode
    {
        [ExposedProperty]
        public float minSpaceBetweenTree;

        [ExposedProperty]
        public string growSpotKeyName;

        [ExposedProperty]
        public float searchRange;

        [ExposedProperty]
        public LayerMask treeMask;

        [ExposedProperty]
        public int maxIterationSafetyStop = 50;

        public override void OnExecute(GOAPGraphAsset currentGraph, WorldState worldState, Dictionary<string, WorldFact> preconditions = null, Dictionary<string, WorldFact> effects = null, bool success = true)
        {
            var blackbaordKey = currentGraph.Blackboard.GetKey(growSpotKeyName);
            var validPosition = GetValidPosition(currentGraph);

            blackbaordKey.value = validPosition;
            base.OnExecute(currentGraph, worldState);
        }

        private Vector3 GetValidPosition(GOAPGraphAsset currentGraph, int iterations = 0)
        {
            var positionToValidate = GetSamplePosition(currentGraph);

            if (!ValidatePosition(positionToValidate) && iterations < maxIterationSafetyStop)
            {
                iterations++;
                return GetValidPosition(currentGraph, iterations);
            }

            return positionToValidate;
        }

        private bool ValidatePosition(Vector3 position)
        {
            var colliders = Physics.OverlapSphere(position, minSpaceBetweenTree, treeMask);
            if (colliders == null)
            {
                Debug.Log("<color=yellow> Found valid grow position");
                return true;
            }

            return false;
        }

        private Vector3 GetSamplePosition(GOAPGraphAsset currentGraph)
        {
            var randomPointInsideUnitSphere = Random.insideUnitSphere;
            var randomPos = (randomPointInsideUnitSphere * searchRange) + currentGraph.agent.gameObject.transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, searchRange, 1);

            return hit.position;
        }
    }
}
