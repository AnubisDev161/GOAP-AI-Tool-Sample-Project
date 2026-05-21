using GOAP;
using GOAP.Data;
using GOAPGraph;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP.Core.Agent
{
    public abstract class GOAPNavigation : MonoBehaviour
    {
        public Action<GOAPGraphAsset, WorldState> desinationReached;

        public virtual void SetDestination(Vector3 destination)
        {

        }

        // Call OnDestinationReached to inform the tree when you agent reached its destination, the graph handle the rest for you
        public virtual void OnDestinationReached()
        {
            var brain = GetComponent<GOAPAgent>().goapBrain;
            desinationReached?.Invoke(brain.graphInstance, brain.currentWorldState);
        }
    }
}
