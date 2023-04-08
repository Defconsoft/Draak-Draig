using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebreathControl : MonoBehaviour
{
    public Vector3 target;
    public bool aimAtTarget = true;
    // private float timeEnabled = 0f;

  
    void Update()
    {
        if (aimAtTarget)
        {
            transform.LookAt(target);
        }
    }

    void OnEnable()
    {
        // This is to make sure the firebreath cannot get stuck in an active state
        StartCoroutine(ShutDownAfterWait());
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Firebreath collided with: " + other.name);
        //Debug.Log("The tag is: " + other.tag);
        string otherTag = other.tag;
        // This gets triggered when the firebreath interacts with the target
        if (otherTag == "target")
        {
            other.GetComponent<TargetControl>().TargetHit();
        }
        else if (otherTag == "CastleEnemy"){
            other.gameObject.GetComponent<CastleAttackAI>().Dead = true;
            Debug.Log ("FIRE");
        }
        else if (otherTag == "bird")
        {
            other.GetComponent<BirdsFlying>().KillBird();
        }
        else if (other.name.Contains("BirdTower"))
        {
            Debug.Log("Burn tower");
            other.GetComponent<BirdTower>().Burn();
        }
        
    }

    private IEnumerator ShutDownAfterWait()
    {
        yield return new WaitForSeconds(2.2f);
        gameObject.SetActive(false);
    }



}
