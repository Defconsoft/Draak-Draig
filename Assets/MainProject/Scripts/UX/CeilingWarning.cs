using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingWarning : MonoBehaviour
{

    public Canvas warningCanvas;

    private void OnTriggerEnter(Collider other) {
        warningCanvas.enabled = true;
    }

    private void OnTriggerExit(Collider other) {
        warningCanvas.enabled = false;
    } 

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
