using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelSceneManager : MonoBehaviour
{

    public CanvasGroup SceneCanvasGrp;
    // Start is called before the first frame update


    public void FadeInInstruction() {
        SceneCanvasGrp.DOFade(1f, 0.5f);
    }

    public void FadeOutInstruction() {
        SceneCanvasGrp.DOFade(0f, 0.5f);
    }


}
