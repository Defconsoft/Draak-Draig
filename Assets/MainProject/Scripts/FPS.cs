using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public TMPro.TMP_Text fpsCounter;
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;


    private void Awake() {
        frameDeltaTimeArray = new float[50];
    }

    // Update is called once per frame
    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        fpsCounter.text = "FPS = " + Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;
        foreach (float deltaTime in frameDeltaTimeArray) {
            total += deltaTime;
        }

        return frameDeltaTimeArray.Length / total;
    }
}
