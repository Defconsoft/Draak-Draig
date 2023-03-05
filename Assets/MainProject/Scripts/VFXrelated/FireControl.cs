using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireControl : MonoBehaviour
{
    
    public VisualEffect fire;
    public float fadeOutAmount = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        fire = gameObject.GetComponent<VisualEffect>();
        StartCoroutine(FadeOutFire());
    }

    IEnumerator FadeOutFire()
    {
        float flameHeight = fire.GetFloat("FlameHeight");
        yield return new WaitForSeconds(2f);
        while (flameHeight > 1)
        {
            flameHeight -= fadeOutAmount;
            fire.SetFloat("FlameHeight", flameHeight);

            yield return new WaitForSeconds(1f);
        }
        fire.Stop();        
    }
}
