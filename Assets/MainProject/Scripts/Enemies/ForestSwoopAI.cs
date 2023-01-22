using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

[RequireComponent(typeof(NavMeshAgent))]
public class ForestSwoopAI : MonoBehaviour
{

    public float moveRadius;
    private NavMeshAgent agent;
    private Vector3 currentDestination;
    public bool caught;
    public CinemachineVirtualCamera KillCam;
    public GameObject dragonModel;
    public GameObject pigModel;
    public GameObject endSpot;
    private Animator pigAnimator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindNewDestination();
        pigAnimator = gameObject.transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!caught){
            //Check if arrived at destination
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        FindNewDestination();
                    }
                }
            }


            agent.SetDestination (currentDestination);

        } else { //stop it when its caught
            agent.isStopped = true;
        }


    }
    
    
    
    
    void FindNewDestination() {
        
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;
        NavMeshHit rayHit;
        NavMesh.SamplePosition (randomDirection, out rayHit, moveRadius, 1);
        Vector3 newDestination = rayHit.position;
        currentDestination = newDestination;
    
    }


    public void SetAnim(int AnimNo) {
        pigAnimator.SetInteger("number", AnimNo);
        pigAnimator.Play("BaseLayer");
    }



}
