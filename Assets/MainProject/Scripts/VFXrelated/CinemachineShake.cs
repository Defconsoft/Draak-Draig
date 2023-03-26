using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
// using Cinemachine.Editor;
using Cinemachine.Utility;

public class CinemachineShake : MonoBehaviour
{
    public float amplitudeGain;
    public float frequencyGain;
    public CinemachineVirtualCamera cam;
    public float shakeDuration;

    void Start()
    {
        EventManager.TargetHit += DoShake;
    }

    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.L))
    //     {
    //         DoShake();
    //     }
    // }

    private void DoShake(float value)
    {
        StartCoroutine(Shake());
        Debug.Log("Shaking");
    }

    public IEnumerator Shake()
    {
        Noise(amplitudeGain, frequencyGain);
        yield return new WaitForSeconds(shakeDuration);
        Noise(0,0);
    }

    void Noise(float amplitude,float frequency)
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
    }

    private void OnDisable()
    {
        EventManager.TargetHit -= DoShake;
    }
    
}
