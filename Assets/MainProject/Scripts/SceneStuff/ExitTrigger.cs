using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{

    public LevelSceneManager OSManager;
    public GameObject MessageCanvas;
    public int SceneToLoad;
    public bool isNight = false;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (isNight)
            {
                GameObject.Find("GameManager").GetComponent<UXManager>().LoadScene(SceneToLoad);
            }
            else
            {
                PlayerController playerCtrl = other.gameObject.GetComponent<PlayerController>();
                playerCtrl.nextScene = true;
                playerCtrl.SceneToLoad = SceneToLoad;
                playerCtrl.exitTrigger = MessageCanvas;
                OSManager.SceneCanvasGrp = MessageCanvas.GetComponent<CanvasGroup>();
                OSManager.FadeInInstruction();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            if (!isNight)
            {
                other.gameObject.GetComponent<PlayerController>().nextScene = false;
                other.gameObject.GetComponent<PlayerController>().exitTrigger = null;
                OSManager.FadeOutInstruction();
            }
        }
    }

}
