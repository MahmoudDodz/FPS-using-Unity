using UnityEngine;

public class ZombieIdlestate : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0f;
    Transform player;
    public float detectAreaRedius = 18f;



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;


    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            animator.SetBool("isPatroling", true);
        }
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < detectAreaRedius)
        {
            animator.SetBool("isChasing", true);
        }

    }




}
