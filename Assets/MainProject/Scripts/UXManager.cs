using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;


public class UXManager : MonoBehaviour
{

    public GameManager gameManager;

    [Header ("Canvas Stuff")]
    [SerializeField] private Canvas MainMenu;
    [SerializeField] private Canvas DebugMenu;
    [SerializeField] private CanvasGroup BGCanvasGrp;
    [SerializeField] private CanvasGroup MainMenuGrp;
    [SerializeField] private CanvasGroup QuoteTextGrp;
    [SerializeField] private CanvasGroup DayTimerGrp;
    [SerializeField] private CanvasGroup TopBarGrp;
    [SerializeField] private CanvasGroup InstructionGrp;

    [Header ("Quote Stuff")]
    [SerializeField] private TMPro.TMP_Text QuoteTextBox;   
    public string[] TextQuotes;


    [Header ("Timer Stuff")]
    public Slider daytimeSlider;
    private bool loadResourceLevelOnce;
    public bool daytimeActive;
    private bool dayComplete;

    [Header ("Item Stuff")]
    public TMPro.TMP_Text RockCount;
    public TMPro.TMP_Text FishCount;
    public TMPro.TMP_Text WoodCount;    

    [Header ("Instruction Stuff")]
    public TMPro.TMP_Text instructionText;
    public string[] instructions;



    private void Awake() {

    }

    private void Start() {
        RockCount.text = "Rock: " + gameManager.totalRock;
        FishCount.text = "Fish: " + gameManager.totalFish;
        WoodCount.text = "Wood: " + gameManager.totalWood;
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
        } else if (SceneNo == 2 && !loadResourceLevelOnce) {
            loadResourceLevelOnce = true;
        }
        yield return new WaitForSeconds (3f);
        scene.allowSceneActivation = true; //Loads the scene in
        FadeInCanvasGrp (QuoteTextGrp, 3f);
        yield return new WaitForSeconds (3f);
        FadeOutCanvasGrp(QuoteTextGrp, 2f);
        yield return new WaitForSeconds (2f);
        FadeOutCanvasGrp(BGCanvasGrp, 2f);
        FadeInCanvasGrp(TopBarGrp, 2f);
        FadeInCanvasGrp(InstructionGrp, 2f);
        if (loadResourceLevelOnce && !daytimeActive) {
            FadeInCanvasGrp (DayTimerGrp, 3f);
            StartDaytime();
        }

        yield return new WaitForSeconds (5f);
        FadeOutCanvasGrp(InstructionGrp, 5f);

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
                instructionText.text = instructions[0];
                break;

            case 2: //Move to resource gathering
                QuoteTextBox.text = TextQuotes[1];
                instructionText.text = instructions[1];
                break;

            case 3: //Move to village
                QuoteTextBox.text = TextQuotes[2];
                instructionText.text = instructions[2];
                break;


        }


    }


    private void FadeOutCanvasGrp(CanvasGroup current, float fadeTime){
        current.DOFade (0, fadeTime);
    }

    private void FadeInCanvasGrp(CanvasGroup current, float fadeTime){
        current.DOFade (1f, fadeTime);
    }


    //////////////////////////////////////
    // DEBUG MENU
    //////////////////////////////////////   

    private void Update() {
        Keyboard kboard = Keyboard.current;

        if (kboard.mKey.wasPressedThisFrame) {
            if (DebugMenu.isActiveAndEnabled == true) {
                DebugMenu.enabled = false;
            } else {
                DebugMenu.enabled = true;
            }
        }


        if (daytimeActive) {
            daytimeSlider.value += Time.deltaTime;
        }

        if (daytimeActive == true && daytimeSlider.value >= daytimeSlider.maxValue && !dayComplete) {
            dayComplete = true;
            FadeOutCanvasGrp (DayTimerGrp, 1f);
            StartCoroutine(LoadYourAsyncScene (3));
        }



    }


    public void LoadDebugLevel (int debugLevel) {

        DebugMenu.enabled = false;
        SceneManager.LoadScene (debugLevel);
        FadeOutCanvasGrp(MainMenuGrp, 2f);
        FadeOutCanvasGrp(BGCanvasGrp, 2f);
        FadeInCanvasGrp(TopBarGrp, 2f);
    }



    public void StartDaytime() {
        daytimeSlider.maxValue = gameManager.DaytimeTimerAmount;
        daytimeActive = true;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SetItemAmounts() {
        RockCount.text = "Rock: " + gameManager.totalRock;
        FishCount.text = "Fish: " + gameManager.totalFish;
        WoodCount.text = "Wood: " + gameManager.totalWood;
    }



}
