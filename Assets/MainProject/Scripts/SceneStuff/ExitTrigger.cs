using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{

    public LevelSceneManager OSManager;
    public GameObject MessageCanvas;
    public int SceneToLoad;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().nextScene = true;
            other.gameObject.GetComponent<PlayerController>().SceneToLoad = SceneToLoad;
            other.gameObject.GetComponent<PlayerController>().exitTrigger = MessageCanvas;
            OSManager.FadeInInstruction();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().nextScene = false;
            other.gameObject.GetComponent<PlayerController>().exitTrigger = null;
            OSManager.FadeOutInstruction();
        }
    }

}
