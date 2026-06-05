using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class ExampleAnimationController : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private NavMeshAgent navMeshAgent;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }
}
