using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebreathControl : MonoBehaviour
{
    public Vector3 target;

  
    void Update()
    {
        transform.LookAt(target);
    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log("Firebreath collided with: " + other.name);
        //Debug.Log("The tag is: " + other.tag);
        // This gets triggered when the firebreath interacts with the target
        if (other.tag == "target")
        {
            other.GetComponent<TargetControl>().TargetHit();
        }
        else if (other.tag == "CastleEnemy"){
            other.gameObject.GetComponent<CastleAttackAI>().Dead = true;
            Debug.Log ("FIRE");
        }
        
    }



}
