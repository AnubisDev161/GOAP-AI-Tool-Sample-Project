using GOAP.GOAPGraph;
using System;
using UnityEngine;

namespace GOAP.Core.Agent
{
    public abstract class GOAPNavigation : MonoBehaviour
    {
        public Action<GOAPGraphAsset, WorldState> desinationReached;

        public virtual void SetDestination(Vector3 destination)
        {

        }

        /// <summary>
        /// Call OnDestinationReached to inform the tree when your agent reached its destination, the graph will handle the rest for you
        /// </summary> 
        public virtual void OnDestinationReached()
        {
            var brain = GetComponent<GOAPAgent>().goapBrain;
            desinationReached?.Invoke(brain.graphInstance, brain.currentWorldState);
        }
    }
}
