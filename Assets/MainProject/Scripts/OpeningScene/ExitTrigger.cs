using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GameObject.Find("OpeningScene").GetComponent<OpeningSceneManager>().FadeInInstruction();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            GameObject.Find("OpeningScene").GetComponent<OpeningSceneManager>().FadeOutInstruction();
        }
    }

}
