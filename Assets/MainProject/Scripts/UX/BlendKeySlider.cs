using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendKeySlider : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;

    
    public void ApplySliderValueToBlendShapeSize(System.Single val)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(1, val * 100f);
        // Debug.Log("slider val: " + val);
    }

    public void ApplySliderValueToBlendShapeSqueeze(System.Single val)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(0, val * 100f);
    }
}
