using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public float speed = 40f;
    public GameObject obj;
    private Rigidbody rb;
    public Transform throwpoint;
    public Vector3 direction = new Vector3(0,0,0);
    public float throwDelay = 0.4f;
    public bool dragonIsMoving = false;
    public GameObject parent = null;
    public Vector3 target;

    void Start()
    {
/*         if (direction == new Vector3(0,0,0))
        {
            direction = throwpoint.forward;
        } */
    }

    void OnEnable()
    {
        if (dragonIsMoving)
        {
            StartCoroutine(ThrowAfterWaitWhileMoving(throwDelay));
        }
        else
        {
            StartCoroutine(ThrowAfterWait(throwDelay));
        }
    }

    IEnumerator ThrowAfterWait(float waitTime)
    {
        GameObject objClone = Instantiate(obj, throwpoint.position, Quaternion.identity);
        rb = objClone.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(waitTime);
        direction = (target - objClone.transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    IEnumerator ThrowAfterWaitWhileMoving(float waitTime)
    {
        // Add object parent to make sure object moves with dragon while charging
        GameObject objClone = Instantiate(obj, throwpoint.position, Quaternion.identity, parent.transform);
        rb = objClone.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(waitTime);
        // unparent so object can be shot off properly
        objClone.transform.parent = null;
        // Setting direction when actually shooting to help aim
        direction = (target - objClone.transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }
}
