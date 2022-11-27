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
    [SerializeField] private CanvasGroup QuoteTextGrp;
    [SerializeField] private TMPro.TMP_Text QuoteTextBox;   


    public string[] TextQuotes;

    private void Awake() {

    }





    //////////////////////////////////////
    // USE TO LOAD SCENES
    //////////////////////////////////////
    public void LoadScene(int SceneNo){
        StartCoroutine("LoadYourAsyncScene", SceneNo);
    }

    public IEnumerator LoadYourAsyncScene (int SceneNo) {

        var scene = SceneManager.LoadSceneAsync (SceneNo);
        scene.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads

        FadeInCanvasGrp(BGCanvasGrp, 2f);
        SetQuoteText(SceneNo);
        if (SceneNo == 1){
            FadeOutCanvasGrp(MainMenuGrp, 2f);
        }
        yield return new WaitForSeconds (3f);
        scene.allowSceneActivation = true; //Loads the scene in
        FadeInCanvasGrp (QuoteTextGrp, 3f);
        yield return new WaitForSeconds (3f);
        FadeOutCanvasGrp(QuoteTextGrp, 2f);
        yield return new WaitForSeconds (2f);
        FadeOutCanvasGrp(BGCanvasGrp, 2f);
    }



    //////////////////////////////////////
    // USE TO FADE CANVAS GROUPS
    //////////////////////////////////////

    private void SetQuoteText(int SceneNo) {
        switch (SceneNo)
        {
            
            case 0: //Main
                break;

            case 1: //Move to openingscene
                QuoteTextBox.text = TextQuotes[0];
                break;

            case 2: //Move to resource gathering
                QuoteTextBox.text = TextQuotes[1];
                break;

            case 3: //Move to village
                QuoteTextBox.text = TextQuotes[2];
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
