using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StormWarningMesage : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CanvasGroup>().DOFade (1, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

}
