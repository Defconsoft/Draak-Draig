using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class pulse : MonoBehaviour
{

    private TMPro.TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshProUGUI>();
        tmp.DOFade(0.25f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
}


