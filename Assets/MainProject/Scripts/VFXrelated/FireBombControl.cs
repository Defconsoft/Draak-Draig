using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireBombControl : MonoBehaviour
{
    public Transform particles;
    public Transform orb;
    public GameObject explosion;
    private Rigidbody rb;
    public GameObject fire;
    public Animator anim;
    public AudioClip explosionSound;
    public float explosionVolume = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim.Rebind();
    }

    // IEnumerator Charge()
    // {
        
    //     yield return new WaitForSeconds(0.2f);
    //     while ((orb.localScale.x < endScaleOrb) && (particles.localScale.x > endScaleParticles))
    //     {
    //         orb.localScale += new Vector3(1f,1f,1f) * scaleSpeedOrb;
    //         particles.localScale -= new Vector3(1f,1f,1f) * scaleSpeedPS;
    //         yield return new WaitForSeconds(0.1f);
    //     }   
    // }

    void OnCollisionEnter(Collision collision)
    {        
        GameObject fireClone = Instantiate(fire, transform.position, Quaternion.identity);
        // EventManager.CameraShake();
        Explode();
    }

    private void Explode()
    {
        rb.isKinematic = true; // to ensure the explosion is in one place
        explosion.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(explosionSound, explosionVolume);
        orb.gameObject.SetActive(false);
        particles.gameObject.SetActive(false);
        Destroy(gameObject, 2.5f);
    }
}
