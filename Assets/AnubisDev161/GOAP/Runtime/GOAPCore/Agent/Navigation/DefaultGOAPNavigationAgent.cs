using UnityEngine;
using UnityEngine.AI;

namespace GOAP.Core.Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class DefaultGOAPNavigationAgent : GOAPNavigation
    {
        private NavMeshAgent navMeshAgent;
        private bool destinationReached = true;

        [SerializeField]
        private float maxRemainingDistance = 2;

        private void OnEnable()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (destinationReached == false && navMeshAgent.remainingDistance <= maxRemainingDistance)
            {
                destinationReached = true;
                OnDestinationReached();
            }
        }

        public override bool SetDestination(Vector3 destination)
        {
            if (navMeshAgent == null) return false;
            destinationReached = false;
           return navMeshAgent.SetDestination(destination);
        }
    }
}