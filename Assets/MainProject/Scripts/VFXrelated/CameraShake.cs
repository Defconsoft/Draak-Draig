using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    
    public float amplitudeGain;
    public Camera cam;
    public float shakeDuration;
    private Vector3 orignalPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.ShakeCam += DoShake;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         StartCoroutine(Shake());
    //     }
        
    // }

    public void DoShake()
    {
        StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {
        orignalPosition = transform.localPosition;
        float elapsed = 0f;
        
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * amplitudeGain;
            float y = Random.Range(-1f, 1f) * amplitudeGain;

            transform.localPosition = orignalPosition + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.localPosition = orignalPosition;
    }
}
