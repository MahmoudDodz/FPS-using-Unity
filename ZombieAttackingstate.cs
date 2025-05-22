using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackingstate : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    public float StopAttackDistance = 2.5f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookaAtPlayer();
        float distancefromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distancefromPlayer > StopAttackDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }
    private void LookaAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        var yRotation = lookRotation.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

}
