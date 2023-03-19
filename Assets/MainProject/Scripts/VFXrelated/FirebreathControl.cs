using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebreathControl : MonoBehaviour
{

    void OnParticleCollision(GameObject other)
    {
        // This gets triggered when the firebreath interacts with the target
        if (other.tag == "target")
        {
            other.GetComponent<TargetControl>().TargetHit();
        }
    }
}
