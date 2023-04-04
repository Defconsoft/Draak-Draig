using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    private bool isMuted = false;
    public GameObject muteIcon;
    public GameObject volumeIcon;
    private float currentVolume = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        muteIcon.SetActive(false);
        volumeIcon.SetActive(true);
    }

    // Update is called once per frame
    public void ChangeAudioStatus()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            muteIcon.SetActive(true);
            mixer.SetFloat("Volume", -80f);
            volumeIcon.SetActive(false);
        }
        else
        {
            muteIcon.SetActive(false);
            mixer.SetFloat("Volume", 0f);
            volumeIcon.SetActive(true);
        }
    }
}
