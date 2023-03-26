using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebreathControl : MonoBehaviour
{
    public Vector3 target;
    public bool aimAtTarget = true;

  
    void Update()
    {
        if (aimAtTarget)
        {
            transform.LookAt(target);
        }
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



}
