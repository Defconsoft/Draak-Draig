using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Fireball : MonoBehaviour
{
    private VisualEffect explosion;
    public GameObject projectile;
    public VisualEffect fireTrail;
    private Rigidbody rb;

    void Start()
    {
        explosion = GetComponent<VisualEffect>();
        rb = GetComponent<Rigidbody>();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        rb.isKinematic = true; // to ensure the explosion is in one place
        explosion.enabled = true;
        fireTrail.enabled = false; // stop firetrail effect
        projectile.SetActive(false);
        Destroy(gameObject, 1f);
    }
}
