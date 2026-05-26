using UnityEngine;
using UnityEngine.AI;

namespace GOAP.Core.Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class DefaultGOAPNavigationAgent : GOAPNavigation
    {
        private NavMeshAgent navMeshAgent;
        private bool destinationReached;

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

        public override void SetDestination(Vector3 destination)
        {
            destinationReached = false;
            navMeshAgent.SetDestination(destination);
        }
    }
}