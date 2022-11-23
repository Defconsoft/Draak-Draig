using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{

    public OpeningSceneManager OSManager;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            OSManager.FadeInInstruction();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            OSManager.FadeOutInstruction();
        }
    }

}
