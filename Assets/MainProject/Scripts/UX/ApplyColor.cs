using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyColor : MonoBehaviour
{
    // Start is called before the first frame update
    public FlexibleColorPicker fcp;
    public Material material;
    private Color startingColor;

    void Awake()
    {
        startingColor = material.color;
        fcp.color = startingColor;
    }

    void OnDisable()
    {
        fcp.bufferedColor = new FlexibleColorPicker.BufferedColor(material.color);
    }
    
    // Update is called once per frame
    public void UpdateColor(Color col)
    {
        material.color = col;
    }

    public void ResetColor()
    {
        fcp.color = startingColor;
    }

    void OnEnable()
    {
        material.color = fcp.color;
    }
}
