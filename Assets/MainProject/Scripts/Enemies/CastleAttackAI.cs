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
    bool rotateDone;

    [Header ("Animation & audio stuff")]
    public AudioClip dieSound;
    public AudioClip walksound;
    public AudioClip arrowSound;
    private Animator anim;
    private AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        dragonPos = GameObject.Find("dragonPerched").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination (destination.position);
        Trashcan = GameObject.Find("Trashcan");
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("IsSprinting", true);
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySoundAfterDelay());
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
                Debug.Log ("here");
                anim.SetBool("IsSprinting", false);
                audioSource.Stop();

                if (!rotateDone){
                    RotateTowards(dragonPos);
                }
                
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

        if (Quaternion.Angle(transform.rotation, lookRotation) <= 10f) {
            rotateDone = true;
        }
    }

    IEnumerator Death() {
        Dead = false;
        audioSource.PlayOneShot(dieSound, 0.4f);
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
        GameObject arrow = Instantiate (projectile, firePoint.transform.position, firePoint.transform.rotation);
        arrow.GetComponent<CastleProjectile>().origEnemy = this;
        Vector3 direction = dragonPos.position - arrow.transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, direction);
        arrow.transform.rotation = toRotation;
        arrow.transform.parent = Trashcan.transform;
        //ANIMATION//////////////////////////////////////
        //Can play the fire arrow animation here. Alter the fireDelay to be the length of the animation.
        /////////////////////////////////////////////////
        anim.SetBool("IsShooting", true);
        audioSource.PlayOneShot(arrowSound, 1.2f);        
        yield return new WaitForSeconds (fireDelay);
        anim.SetBool("IsShooting", false);
        canShoot = false;
    }

    private IEnumerator PlaySoundAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        audioSource.clip = walksound;
        audioSource.pitch = Random.Range(0.7f, 1.4f); // for some variation
        audioSource.Play();
    }

}


