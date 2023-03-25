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
    public GameObject firePoint;
    public GameObject projectile;
    public float fireDelay;
    private GameObject Trashcan;

    bool Shootable;
    bool canShoot;

    public bool Dead;
    public bool deadOnce;

    [Header ("Animation stuff")]
    private Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        dragonPos = GameObject.Find("dragonPerched").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination (destination.position);
        Trashcan = GameObject.Find("Trashcan");
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("IsSprinting", true);
    }

    // Update is called once per frame
    void Update()
    {
        //ANIMATION//////////////////////////////////////
        //Needs to be walking/running by default
        /////////////////////////////////////////////////
        

        //Walking
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //ANIMATION//////////////////////////////////////
                //Need to stop walking and stand in idle.
                /////////////////////////////////////////////////
                anim.SetBool("IsSprinting", false);

                RotateTowards(dragonPos);
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.isStopped = true;
                    Shootable = true;
        
                }
            }
        }

        //Dieing
        if (Dead && !deadOnce) {
            deadOnce = true;
            Debug.Log ("DEAD");
            StartCoroutine(Death());
        } 


        //Shooting

        if (Shootable && canShoot == false) {
            StartCoroutine(ShootArrow());
        }



    }


    private void RotateTowards (Transform target) {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    IEnumerator Death() {
        Dead = false;
        StopCoroutine(ShootArrow());
        canShoot = true;
        enemyManager.hasEnemy = false;
        //ANIMATION//////////////////////////////////////
        //Play death anim. Alter the delay below to the length.
        /////////////////////////////////////////////////   
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds (1.6f);
        Destroy(gameObject);
    }


    IEnumerator ShootArrow(){
        canShoot = true;
        yield return new WaitForSeconds (Random.Range (2f, 6f));
        GameObject arrow = Instantiate (projectile, firePoint.transform.position, Quaternion.identity);
        arrow.transform.parent = Trashcan.transform;
        //ANIMATION//////////////////////////////////////
        //Can play the fire arrow animation here. Alter the fireDelay to be the length of the animation.
        /////////////////////////////////////////////////
        anim.SetBool("IsShooting", true);        
        yield return new WaitForSeconds (fireDelay);
        anim.SetBool("IsShooting", false);
        canShoot = false;
    }

}


