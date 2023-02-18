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
    private bool oneHit;
    private CastleBattleController castleController;

    void Start()
    {
        explosion = GetComponent<VisualEffect>();
        rb = GetComponent<Rigidbody>();
        castleController = GameObject.Find("CastleManager").GetComponent<CastleBattleController>();
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "CastleEnemy" && !oneHit){
            oneHit = true;
            other.gameObject.GetComponent<CastleAttackAI>().Dead = true;
        } 
       
        Explode();       
    }
    
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Barrel") {
            castleController.barrelExplosionPoint = collision.gameObject.transform;    
            StartCoroutine (castleController.BarrelNuke());
            Destroy(collision.gameObject);

        }

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
