using UnityEngine;
using UnityEngine.AI;

public class ZombieChasingstate : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    public float chaseSpeed = 6f;
    public float StopchaseDistance = 21;
    public float attackingDistance = 2.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);
        float distancefromPlayer = Vector3.Distance(player.position, animator.transform.position);


        if (distancefromPlayer > StopchaseDistance)
        {
            animator.SetBool("isChasing", false);

        }

        if (distancefromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }


    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);

    }
}
