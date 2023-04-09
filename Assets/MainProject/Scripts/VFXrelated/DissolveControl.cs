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
    public PlayerController player;
    private GameManager gameManager;
    private UXManager uxManager;

    private Material[] dissolveMaterials; 
    public Material dragonMat;
    
    // Start is called before the first frame update
    void Start()
    {
        if (vfxGraph != null)
        {
            vfxGraph.Stop();
            vfxGraph.gameObject.SetActive(false);
            vfxGraph.SetFloat("hue", dragonMat.GetFloat("_HueChange"));
        }
        
        if (skinnedMeshRenderer != null)
        {
            dissolveMaterials = skinnedMeshRenderer.materials;
        }

        foreach (Material mat in dissolveMaterials)
        {
            // Make sure color of transform effect is same as dragon
            mat.SetFloat("_HueChange", dragonMat.GetFloat("_HueChange"));
        }
        uxManager = GameObject.Find("GameManager").GetComponent<UXManager>();

        StartCoroutine(Dissolve());

    }

    // For debugging purposes
    // void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.X))
    //     {
    //         StartCoroutine(Dissolve());
    //     }
    // }

    public void StartDissolving()
    {
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        player.Interacting = true;
        StartCoroutine(player.StopFollow());
        // yield return new WaitForSeconds(4f);


        if (anim != null)
        {
            anim.SetTrigger("wakeUp");
        }

        yield return new WaitForSeconds(dieDelay);

        if (vfxGraph != null)
        {
            vfxGraph.gameObject.SetActive(true);
            vfxGraph.Play();
        }

        float counter = .9f;

        if(dissolveMaterials.Length >0)
        {
            while(dissolveMaterials[0].GetFloat("DissolveAmount_") > 0)
            {
                counter -= dissolveRate;

                for(int i=0; i<dissolveMaterials.Length; i++){
                    dissolveMaterials[i].SetFloat("DissolveAmount_", counter);
                }
                
                yield return new WaitForSeconds(refreshRate);
            }
        }

        yield return new WaitForSeconds(2f);


        yield return new WaitForSeconds(1f);
        player.Interacting = false;
        StartCoroutine(player.StartFollow());
        gameObject.SetActive(false);
        uxManager.ShowStartInstructions();
    }

    void OnDisable()
    {
        vfxGraph.Stop();
        vfxGraph.gameObject.SetActive(false);
    }
}
