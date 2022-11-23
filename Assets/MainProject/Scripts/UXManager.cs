using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UXManager : MonoBehaviour
{

    [SerializeField] private Canvas MainMenu;
    [SerializeField] private CanvasGroup BGCanvasGrp;
    [SerializeField] private CanvasGroup MainMenuGrp;
    [SerializeField] private CanvasGroup OpeningSceneGrp;



    ///Fade Targets
    private CanvasGroup FadeIn, Fadeout;


    private void Awake() {

    }





    //////////////////////////////////////
    // USE TO LOAD SCENES
    //////////////////////////////////////
    public void LoadScene(int SceneNo){
        StartCoroutine("LoadYourAsyncScene", SceneNo);
    }

    public IEnumerator LoadYourAsyncScene (int SceneNo) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNo);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SetFadeTargets(SceneNo);
        FadeOutCanvasGrp(Fadeout, 2f);
        yield return new WaitForSeconds (3f);
        FadeInCanvasGrp (FadeIn, 3f);
        yield return new WaitForSeconds (3f);
        FadeOutCanvasGrp(FadeIn, 2f);
        yield return new WaitForSeconds (2f);
        FadeOutCanvasGrp(BGCanvasGrp, 2f);
    }



    //////////////////////////////////////
    // USE TO FADE CANVAS GROUPS
    //////////////////////////////////////

    private void SetFadeTargets(int SceneNo) {
        switch (SceneNo)
        {
            
            case 0: //Main
                break;

            case 1: //Move to openingscene
                FadeIn = OpeningSceneGrp;
                Fadeout = MainMenuGrp;
                break;
        }


    }


    private void FadeOutCanvasGrp(CanvasGroup current, float fadeTime){
        current.DOFade (0, fadeTime);
    }

    private void FadeInCanvasGrp(CanvasGroup current, float fadeTime){
        current.DOFade (1f, fadeTime);
    }


}
