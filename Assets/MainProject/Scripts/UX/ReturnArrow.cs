using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnArrow : MonoBehaviour
{

    public GameObject target;

    private void Update() {

        var targetPosLocal = Camera.main.transform.InverseTransformPoint(target.transform.position);
        var targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg - 90;
        this.transform.eulerAngles = new Vector3(0, 0, targetAngle);

    }


}
