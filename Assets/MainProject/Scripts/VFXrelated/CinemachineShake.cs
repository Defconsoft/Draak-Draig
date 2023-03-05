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
    public CinemachineFreeLook cmFreeCam;
    public float shakeDuration;

    // void Start()
    // {
    //     EventManager.BrokeObject += DoShake;
    // }

    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.L))
    //     {
    //         DoShake();
    //     }
    // }

    public void DoShake()
    {
        StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {
        Noise(amplitudeGain, frequencyGain);
        yield return new WaitForSeconds(shakeDuration);
        Noise(0,0);
    }

    void Noise(float amplitude,float frequency)
    {
        cmFreeCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cmFreeCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cmFreeCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cmFreeCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        cmFreeCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        cmFreeCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
    }
    
}
