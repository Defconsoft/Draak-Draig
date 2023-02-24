using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderCustomization : MonoBehaviour
{
    // public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material material; 
    public Texture[] textures;
    public GameObject[] frames;

    
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
}
