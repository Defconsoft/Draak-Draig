using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarningText: MonoBehaviour
{

    public TMPro.TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.DOFade (1, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

}
