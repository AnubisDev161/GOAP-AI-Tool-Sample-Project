using GOAP.Core.Agent;
using UnityEngine;

namespace ExampleProject
{
    /// <summary>
    /// Example trigger that simply changes the InDanger blackbaord key from true to false,
    /// depending on whether a threat has entered or exicted the collider
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class DangerTrigger : GOAPBlackboardTrigger
    {
        [SerializeField]
        private string collisionEnterValue;

        [SerializeField]
        private string collisionExitValue;

        private void OnEnable()
        {
            AddSpecifiedWorldFactWithValue(collisionExitValue, false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponentInParent(typeof(ThreatAgent), false) == null) return;

            var worldFact = GetSpecifiedWorldFact();
            AddSpecifiedWorldFactWithValue(collisionEnterValue, true);
            Debug.Log("Threat in sight");
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponentInParent(typeof(ThreatAgent), false) == null) return;

            var worldFact = GetSpecifiedWorldFact();
            AddSpecifiedWorldFactWithValue(collisionExitValue, false);
            Debug.Log("Threat out of sight");
        }
    }
}