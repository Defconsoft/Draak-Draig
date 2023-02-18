using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CastleCarryAI : MonoBehaviour
{

    private NavMeshAgent agent;
    private Transform destination;
    public GameObject Barrel;
    bool oneHit;

    // Start is called before the first frame update
    void Start()
    {
        destination = GameObject.Find("BarrelCarryDestination").transform;
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
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "BarrelTrigger" && !oneHit) {
            oneHit = true;
            Barrel.GetComponent<Rigidbody>().isKinematic = false;
            //Barrel.GetComponent<Rigidbody>().AddForce(0,5f,0, ForceMode.VelocityChange);
            Barrel.GetComponent<Rigidbody>().AddExplosionForce (10f, Barrel.transform.position, 3f, -.25f, ForceMode.Impulse);
            Barrel.transform.parent = GameObject.Find("Trashcan").transform;
        }
    }


}
