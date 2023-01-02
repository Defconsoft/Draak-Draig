using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXControllerDragon : MonoBehaviour
{
    public VisualEffect[] vfxGraphs;

    // Start is called before the first frame update
    void Start()
    {
        foreach(VisualEffect vfx in vfxGraphs)
        {
            vfx.Stop();
            vfx.gameObject.SetActive(false);
        }   
    }

    public void StartVFX(int idx)
    {
        // 0 = firebreath
        VisualEffect vfx = vfxGraphs[idx];
        vfx.gameObject.SetActive(true);
        vfx.Play();
    }

    public void StopVFX(int idx)
    {
        VisualEffect vfx = vfxGraphs[idx];
        vfx.Stop();
        vfx.gameObject.SetActive(false);
    }
}
