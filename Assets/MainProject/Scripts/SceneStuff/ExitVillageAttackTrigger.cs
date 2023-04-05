using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitVillageAttackTrigger : MonoBehaviour
{

    bool DoOnce;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && !DoOnce) {
            DoOnce = true;
            GameObject.Find("Dragon").GetComponent<VillageAttackController>().GameEnded = true;
        }

    }


}
