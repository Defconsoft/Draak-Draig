using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpeningSceneManager : MonoBehaviour
{

    public CanvasGroup OpenSceneCanvasGrp;
    // Start is called before the first frame update


    public void FadeInInstruction() {
        OpenSceneCanvasGrp.DOFade(1f, 0.5f);
    }

    public void FadeOutInstruction() {
        OpenSceneCanvasGrp.DOFade(0f, 0.5f);
    }


}
