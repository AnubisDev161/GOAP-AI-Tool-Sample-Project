using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Example trigger that simply changes the InDanger blackbaord key from true to false,
/// depending on whether a threat has entered or exicted the collider
/// </summary>
[RequireComponent (typeof(SphereCollider))]
public class DangerTrigger : GOAPBlackboardTrigger
{
    [SerializeField]
    private string collisionEnterValue;

    [SerializeField]
    private string collisionExitValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent(typeof(ThreatAgent), false) == null) return;

        var blackboardKey = GetSpecifiedWorldFact();

        blackboardKey.value = collisionEnterValue;
        Debug.Log("Threat in sight");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent(typeof(ThreatAgent), false) == null) return;

        var blackboardKey = GetSpecifiedWorldFact();

        blackboardKey.value = collisionExitValue;
        Debug.Log("Threat out of sight");
    }
}
