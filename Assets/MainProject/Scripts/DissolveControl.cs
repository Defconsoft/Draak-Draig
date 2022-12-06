using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveControl : MonoBehaviour
{
    public Animator anim;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public VisualEffect vfxGraph;
    public float dissolveRate = 0.02f;
    public float refreshRate = 0.05f;
    public float dieDelay = 0.2f;

    private Material[] dissolveMaterials; 
    
    // Start is called before the first frame update
    void Start()
    {
        if (vfxGraph != null)
        {
            vfxGraph.Stop();
            vfxGraph.gameObject.SetActive(false);
        }
        
        if (skinnedMeshRenderer != null)
        {
            dissolveMaterials = skinnedMeshRenderer.materials;
        }
    }

    // For debugging purposes
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(Dissolve());
        }
    }

    public void StartDissolving()
    {
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        if (anim != null)
        {
            anim.SetTrigger("WakeUp");
        }

        yield return new WaitForSeconds(dieDelay);

        if (vfxGraph != null)
        {
            vfxGraph.gameObject.SetActive(true);
            vfxGraph.Play();
        }

        float counter = 0;

        if(dissolveMaterials.Length >0)
        {
            while(dissolveMaterials[0].GetFloat("DissolveAmount_") < 1)
            {
                counter += dissolveRate;

                for(int i=0; i<dissolveMaterials.Length; i++){
                    dissolveMaterials[i].SetFloat("DissolveAmount_", counter);
                }
                
                yield return new WaitForSeconds(refreshRate);
            }
        }

        Destroy(gameObject, 1);
    }
}
