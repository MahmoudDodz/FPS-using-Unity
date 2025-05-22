using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;
    private NavMeshAgent agent;
    public bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;
        HP -= damageAmount;
        if (HP <= 0)
        {
            isDead = true;

            int rondomValue = Random.Range(0, 2);
            if (rondomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }
        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }


}
