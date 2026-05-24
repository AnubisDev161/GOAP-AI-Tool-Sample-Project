using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Animator))]
public class WorkerAnimationController : MonoBehaviour
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
        var velocity = navMeshAgent.velocity;

        animator.SetFloat("SpeedX", velocity.x);
        animator.SetFloat("SpeedY", velocity.z);
    }
}
