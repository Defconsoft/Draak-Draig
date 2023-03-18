using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXControllerDragon : MonoBehaviour
{
    public GameObject[] effects;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in effects)
        {
            obj.SetActive(false);
        }   
    }

    public void StartVFX(int idx)
    {
        // 0 = firebreath, 1=fireball, 2=firebomb
        GameObject fx = effects[idx];
        fx.SetActive(true);
        if(idx == 0)
        {
            fx.GetComponent<ParticleSystem>().Play();
        }
    }

    public void StopVFX(int idx)
    {
        GameObject fx = effects[idx];
        if (idx == 0)
        {
            fx.GetComponent<ParticleSystem>().Stop();
        }
        fx.SetActive(false);
    }
}
