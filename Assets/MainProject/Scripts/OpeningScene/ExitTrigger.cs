using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{

    public OpeningSceneManager OSManager;
    public int SceneToLoad;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().nextScene = true;
            other.gameObject.GetComponent<PlayerController>().SceneToLoad = SceneToLoad;
            OSManager.FadeInInstruction();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().nextScene = false;
            OSManager.FadeOutInstruction();
        }
    }

}
