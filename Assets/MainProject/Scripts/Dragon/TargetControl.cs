using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TargetControl : MonoBehaviour
{
    [SerializeField]
    private float pulseSpeed = 0.8f;
    [SerializeField]
    private float scaleOffset = 0f;
    [SerializeField]
    private float targetValue = 10f;
    public GameObject associatedObject = null;
    private bool activeTarget = true;


    // Update is called once per frame
    void Update()
    {
        Scale();
    }

    private void Scale()
    {
        float sinValue = (Mathf.Sin(Time.time * pulseSpeed) + 3)/4 + scaleOffset;
        Vector3 scaleVec = new Vector3(sinValue, sinValue, sinValue);
        transform.localScale = scaleVec;
    }

    private void OnTriggerEnter(Collider other)
    {
        TargetHit();
        
    }

    public void TargetHit()
    {
        if (activeTarget)
        {
            activeTarget = false;
            // send target hit event
            Debug.Log("Target hit");
            EventManager.HitTarget(targetValue);
            // Turn on the fire
            if (associatedObject != null)
            {
                Debug.Log("Starting fire effect");
                GameObject vfxObject = associatedObject.GetComponentInChildren<VisualEffect>(true).gameObject;
                vfxObject.SetActive(true);
            }
            // Destroy target
            gameObject.SetActive(false); //Destroy didn't seem to react fast enough
        }
    }
}
