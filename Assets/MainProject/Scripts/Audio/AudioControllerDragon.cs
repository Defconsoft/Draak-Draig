using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerDragon : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] attackAudioClips;
    public AudioClip[] wingflapClips;
    public float audioVolume = 0.5f;
    
    // void Start()
    // {
    //     EventManager.MinigameImpact += PlayImpactAudio;
    // }
    
    private void PlayAttackAudio(int idx)
    {
        // 0 = firebreath, 1=fireball, 2=firebomb
        PlayClip(attackAudioClips[idx]);
    }

    public void PlayClip(AudioClip audio)
    {
        audioSource.PlayOneShot(audio, audioVolume);
    }

    private void PlayWingFlapSound()
    {
        int randomIdx = Random.Range(0, wingflapClips.Length);
        PlayClip(wingflapClips[randomIdx]);
    }


    // private void OnDisable()
    // {
    //     EventManager.MinigameImpact -= PlayImpactAudio;
    // }
}
