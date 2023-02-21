using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendKeySlider : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;

    public int sizeKeyIdx = 1;
    public int squeezeKeyIdx = 0;
    public int curveKeyIdx = 2;

    
    public void ApplySliderValueToBlendShapeSize(System.Single val)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(sizeKeyIdx, val * 100f);
        // Debug.Log("slider val: " + val);
    }

    public void ApplySliderValueToBlendShapeSqueeze(System.Single val)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(squeezeKeyIdx, val * 100f);
    }

    public void ApplySliderValueToBlendShapeCurve(System.Single val)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(curveKeyIdx, val * 100f);
    }
}
