using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] impactAudioClips;
    public float audioVolume = 0.5f;
    
    void Start()
    {
        EventManager.MinigameImpact += PlayImpactAudio;
    }
    
    private void PlayImpactAudio(int idx)
    {
        PlayClip(impactAudioClips[idx]);
    }

    public void PlayClip(AudioClip audio)
    {
        audioSource.PlayOneShot(audio, audioVolume);
    }
}
