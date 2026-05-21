using UnityEngine;
using UnityEngine.AI;

namespace GOAP.Core.Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class DefaultGOAPNavigationAgent : GOAPNavigation
    {
        private NavMeshAgent navMeshAgent;

        bool destinationReached;
        private void OnEnable()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (destinationReached == false && navMeshAgent.remainingDistance <= 2)
            {
                destinationReached = true;
                OnDestinationReached();
            }
        }

        public override void SetDestination(Vector3 destination)
        {
            destinationReached = false;
            navMeshAgent.SetDestination(destination);
        }
    }
}