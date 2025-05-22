using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingstate : StateMachineBehaviour
{
    float timer;
    public float patrolTime = 10f;
    Transform player;
    NavMeshAgent navmeshagent;
    public float detectAreaRedius = 18f;

    public float patrolSpeed = 2f;
    List<Transform> wayPoints = new List<Transform>();


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navmeshagent = animator.GetComponent<NavMeshAgent>();
        navmeshagent.speed = patrolSpeed;
        timer = 0f;
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("WayPoint");
        foreach (Transform t in waypointCluster.transform)
        {
            wayPoints.Add(t);
        }

        Vector3 nextWaypoint = wayPoints[Random.Range(0, wayPoints.Count)].position;
        navmeshagent.SetDestination(nextWaypoint);


    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navmeshagent.remainingDistance <= navmeshagent.stoppingDistance)
        {
            navmeshagent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }
        timer += Time.deltaTime;
        if (timer > patrolTime)
        {
            animator.SetBool("isPatroling", false);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < detectAreaRedius)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navmeshagent.SetDestination(navmeshagent.transform.position);
    }
}
