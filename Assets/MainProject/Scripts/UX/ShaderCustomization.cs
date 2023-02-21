using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderCustomization : MonoBehaviour
{
    // public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material material; 

    
    public void ApplyHueChange(System.Single val)
    {
        material.SetFloat("_HueChange", val);
    }

    public void ApplySaturationChange(System.Single val)
    {
        material.SetFloat("_Saturation", val);
    }
}
