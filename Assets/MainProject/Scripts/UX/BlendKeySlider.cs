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
    public int curveKeyIdx = -1;
    public Slider sizeSlider;
    public Slider squeezeSlider;
    public Slider curveSlider;
    private float startSize;
    private float startSqueeze;
    private float startCurve;

    void Awake()
    {
        startSize = skinnedMeshRenderer.GetBlendShapeWeight(sizeKeyIdx);
        startSqueeze = skinnedMeshRenderer.GetBlendShapeWeight(squeezeKeyIdx);
        if (curveKeyIdx != -1)
        {
            startCurve = skinnedMeshRenderer.GetBlendShapeWeight(curveKeyIdx);
        }
        else
        {
            startCurve = -1;
        }        
    }

    
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

    public void ResetShapes()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(sizeKeyIdx, startSize);
        sizeSlider.value = startSize / 100f;
        skinnedMeshRenderer.SetBlendShapeWeight(squeezeKeyIdx, startSqueeze);
        squeezeSlider.value = startSqueeze / 100f;
        if (curveSlider != null)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(curveKeyIdx,startCurve);
            curveSlider.value = startCurve / 100f;
        }
    }
}
