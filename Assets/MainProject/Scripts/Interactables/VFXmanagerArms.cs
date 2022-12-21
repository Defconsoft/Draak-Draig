using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXmanagerArms : MonoBehaviour
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

    public IEnumerator Impact(int idx)
    {
        VisualEffect vfx = vfxGraphs[idx];
        vfx.gameObject.SetActive(true);
        vfx.Play();

        yield return new WaitForSeconds(0.6f);

        vfx.Stop();
        vfx.gameObject.SetActive(false);
    }
}
