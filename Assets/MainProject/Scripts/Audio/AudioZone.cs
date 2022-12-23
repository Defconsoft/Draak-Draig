using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour
{
    private string col;
    public AudioSource audio;
    public float fadeOutAmount = 0.1f;
    public float fadeInAmount = 0.05f;
    public float desiredVolume = 0.3f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audio.Play();
            audio.volume = 0f;
            audio.loop = true;
            StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            audio.loop = false;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        float currentVolume = audio.volume;
        while (currentVolume > 0f)
        {
            currentVolume -= fadeOutAmount;
            audio.volume = currentVolume;
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator FadeIn()
    {
        float currentVolume = audio.volume;
        while (currentVolume < desiredVolume)
        {
            currentVolume += fadeInAmount;
            audio.volume = currentVolume;
            yield return new WaitForSeconds(0.5f);
        }
    }


}
