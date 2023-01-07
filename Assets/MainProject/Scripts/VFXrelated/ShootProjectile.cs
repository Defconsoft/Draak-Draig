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

    void Start()
    {
        if (direction == new Vector3(0,0,0))
        {
            direction = throwpoint.forward;
        }
    }

    void OnEnable()
    {
        Debug.Log("enabled");
        StartCoroutine(ThrowAfterWait(throwDelay));
    }

    IEnumerator ThrowAfterWait(float waitTime)
    {
        GameObject objClone = Instantiate(obj, throwpoint.position, Quaternion.identity);
        rb = objClone.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(waitTime);
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }
}
