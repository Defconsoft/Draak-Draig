using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Playables;

public class LevelSceneManager : MonoBehaviour
{

    public PlayableDirector director;
    public CanvasGroup SceneCanvasGrp;
    // Start is called before the first frame update

    void Start()
    {
        if (director != null)
        {
            StartCoroutine(PlayCutscene());
        }
    }


    public void FadeInInstruction() {
        SceneCanvasGrp.DOFade(1f, 0.5f);
    }

    public void FadeOutInstruction() {
        SceneCanvasGrp.DOFade(0f, 0.5f);
    }

    private IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(2f);
        director.Play();
    }


}
