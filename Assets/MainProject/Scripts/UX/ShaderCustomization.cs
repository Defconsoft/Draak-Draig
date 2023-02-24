using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderCustomization : MonoBehaviour
{
    // public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material material; 
    public Texture[] textures;
    public GameObject[] frames;
    public Slider hueSlider;
    public Slider satSlider;
    private float startingHue;
    private float startingSat;
    private Texture startingTex;
    private int startingTexIdx;

    
    public void Awake()
    {
        // Get current values to be able to reset
        startingHue = material.GetFloat("_HueChange");
        startingSat = material.GetFloat("_Saturation");
        startingTex = material.GetTexture("_baseTexture");
        // Ensure selection frame is set up properly
        for (int i=0; i< textures.Length; i++)
        {
            if (textures[i] == startingTex)
            {
                startingTexIdx = i;
                ChangeTexture(i);
                break;
            }
        }
        hueSlider.value = startingHue;
        satSlider.value = startingSat;

    }
    
    public void ApplyHueChange(System.Single val)
    {
        material.SetFloat("_HueChange", val);
    }

    public void ApplySaturationChange(System.Single val)
    {
        material.SetFloat("_Saturation", val);
    }

    public void ChangeTexture(int idx)
    {
        material.SetTexture("_baseTexture", textures[idx]);
        // Update selection frame
        foreach (GameObject go in frames)
        {
            if (go == frames[idx])
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }

    public void ResetValues()
    {
        material.SetFloat("_HueChange", startingHue);
        material.SetFloat("_Saturation", startingSat);
        ChangeTexture(startingTexIdx);
        hueSlider.value = startingHue;
        satSlider.value = startingSat;
    }
}
