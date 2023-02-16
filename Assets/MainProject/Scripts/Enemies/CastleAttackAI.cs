using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CastleAttackAI : MonoBehaviour
{
    
    private NavMeshAgent agent;
    public Transform destination;
    private Transform dragonPos;
    private float rotationSpeed = 10f;
    public CastleBattleEnemyManager enemyManager;

    bool Shootable;

    public bool Dead;



    // Start is called before the first frame update
    void Start()
    {
        dragonPos = GameObject.Find("dragonPerched").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination (destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        


        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                RotateTowards(dragonPos);
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.isStopped = true;
                    Shootable = true;
                }
            }
        }

        if (Dead) {
            StartCoroutine(Death());
        }        

    }


    private void RotateTowards (Transform target) {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    IEnumerator Death() {
        Dead = false;
        enemyManager.hasEnemy = false;
        yield return new WaitForSeconds (1f);
        Destroy(gameObject);
    }

}


