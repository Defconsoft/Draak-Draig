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
    [SerializeField] public CanvasGroup BGCanvasGrp;
    [SerializeField] private CanvasGroup MainMenuGrp;
    [SerializeField] private CanvasGroup QuoteTextGrp;
    [SerializeField] private CanvasGroup DayTimerGrp;
    [SerializeField] private CanvasGroup TopBarGrp;
    [SerializeField] private CanvasGroup ResourceGrp;
    [SerializeField] private CanvasGroup InstructionGrp;
    [SerializeField] private CanvasGroup DragonGrp;

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

    [Header ("Dragon Stuff")]
    public Image eagleEyeFill;

    [Header ("Health Stuff")]
    public Image HealthBar;
    public Image EnergyBar;




    private void Awake() {

    }

    private void Start() {
        RockCount.text = "Rock: " + gameManager.totalRock;
        FishCount.text = "Fish: " + gameManager.totalFish;
        WoodCount.text = "Wood: " + gameManager.totalWood;
        HealthBar.fillAmount = gameManager.HealthAmount;
        EnergyBar.fillAmount = gameManager.EnergyAmount;
    }

    //////////////////////////////////////
    // USE TO LOAD SCENES
    //////////////////////////////////////
    public void LoadScene(int SceneNo){
        //Set the quote and instructions text;
        DebugMenu.enabled = false;
        SetQuoteText(SceneNo);
        StartCoroutine(LoadYourAsyncScene(SceneNo));
    }

    public IEnumerator LoadYourAsyncScene (int SceneNo) {

        //Fade in the background canvas
        FadeInCanvasGrp(BGCanvasGrp, 2f);

        //Fade out the main menu canvas
        FadeOutCanvasGrp(MainMenuGrp, 2f);

        //Wait for 3 seconds then fade in the quote
        yield return new WaitForSeconds (3f);
        FadeInCanvasGrp (QuoteTextGrp, 3f);

        AsyncOperation sceneDone = SceneManager.LoadSceneAsync (SceneNo);
        // Wait until the asynchronous scene fully loads
        while (!sceneDone.isDone)
        {
            yield return null;
            Debug.Log ("HERE");
        }
        
        //wait 3 seconds then fade out the quote
        yield return new WaitForSeconds (3f);
        FadeOutCanvasGrp(QuoteTextGrp, 2f);

        //This is the level specific 

        //if Main menu fade out the Top Bar Group
        if (SceneNo > 0 && SceneNo < 8) {
            FadeInCanvasGrp(TopBarGrp, 2f);
        } else {
            FadeOutCanvasGrp(TopBarGrp, 0.1f);
        }

        //loads the daytime timer into the resource scene
        if (SceneNo == 2) {
            FadeInCanvasGrp (DayTimerGrp, 3f);
            FadeInCanvasGrp (ResourceGrp, 3f);
            StartDaytime();
        }

        //Village scene
        if (SceneNo == 3) {
            FadeInCanvasGrp (ResourceGrp, 3f);
            FadeOutCanvasGrp (DayTimerGrp, 0.1f);
            StopDaytime();
        }

        //animal chase scene
        if (SceneNo == 4) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            StartCoroutine (TempSceneWait(SceneNo + 1)); //REMOVE ME WHEN DONE
        }

        //castle attack scene
        if (SceneNo == 5) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            StartCoroutine (TempSceneWait(SceneNo + 1)); //REMOVE ME WHEN DONE
        }

        //forest swoop scene
        if (SceneNo == 6) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            FadeInCanvasGrp(DragonGrp, 2f);
        }

        //city battle scene
        if (SceneNo == 7) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            StartCoroutine (TempSceneWait(SceneNo + 1)); //REMOVE ME WHEN DONE
        }   

        //customisation scene
        if (SceneNo == 8) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
        }             



        //wait for 2 seconds and fade out the BG
        yield return new WaitForSeconds (2f);
        FadeOutCanvasGrp(BGCanvasGrp, 2f);

        //Fades in the instruction bar at the top
        FadeInCanvasGrp(InstructionGrp, 2f);


        //Fades out the instruction bar after 5 seconds
        yield return new WaitForSeconds (5f);
        FadeOutCanvasGrp(InstructionGrp, 5f);

    }


    private void Update() {
        
    
        Keyboard kboard = Keyboard.current;

        if (kboard.lKey.wasPressedThisFrame) {
            if (DebugMenu.isActiveAndEnabled == true) {
                DebugMenu.enabled = false;
            } else {
                DebugMenu.enabled = true;
            }
        }


        if (daytimeActive) {
            daytimeSlider.value += Time.deltaTime;
        }

        //NEED TO PUT IN A DAYTIME ENDING SOON TIMER AND MAKE THE SUN MOVE


        if (daytimeActive == true && daytimeSlider.value >= daytimeSlider.maxValue && !dayComplete) {
            StartCoroutine(LoadYourAsyncScene (3));
        }   
    
    }

    //////////////////////////////////////
    // DEBUG MENU
    //////////////////////////////////////    

    private IEnumerator TempSceneWait(int sceneNo) {
        yield return new WaitForSeconds (10f);
        StartCoroutine(LoadYourAsyncScene(sceneNo));
    }


    //////////////////////////////////////
    // USE TO FADE CANVAS GROUPS
    //////////////////////////////////////

    public void SetQuoteText(int SceneNo) {
        //SET THE INSTRUCTION TEXT FOR THE INSTRUCTIONS BOX
        switch (SceneNo)
        {
            
            case 0: //Main
                QuoteTextBox.text = TextQuotes[0];
                break;

            case 1: //Move to openingscene
                QuoteTextBox.text = TextQuotes[1];
                instructionText.text = instructions[1];
                break;

            case 2: //Move to resource gathering
                QuoteTextBox.text = TextQuotes[2];
                instructionText.text = instructions[2];
                break;

            case 3: //Move to village
                QuoteTextBox.text = TextQuotes[3];
                instructionText.text = instructions[3];
                break;
            case 4: //Move to Animal Chase
                QuoteTextBox.text = TextQuotes[4];
                instructionText.text = instructions[4];
                break;
            case 5: //Move to Castle Attack
                QuoteTextBox.text = TextQuotes[5];
                instructionText.text = instructions[5];
                break;
            case 6: //Move to Forest Swoop
                QuoteTextBox.text = TextQuotes[6];
                instructionText.text = instructions[6];
                break;
            case 7: //Move to Village Attack
                QuoteTextBox.text = TextQuotes[7];
                instructionText.text = instructions[7];
                break;
            case 8: //Go to customisation
                QuoteTextBox.text = TextQuotes[8];
                instructionText.text = instructions[8];
                break;


        }


    }


    public void FadeOutCanvasGrp(CanvasGroup current, float fadeTime){
        current.DOFade (0, fadeTime);
    }

    public void FadeInCanvasGrp(CanvasGroup current, float fadeTime){
        current.DOFade (1f, fadeTime);
    }




    public void StartDaytime() {
        daytimeSlider.maxValue = gameManager.DaytimeTimerAmount;
        daytimeActive = true;
    }

    public void StopDaytime() {
        daytimeActive = false;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SetItemAmounts() {
        RockCount.text = "Rock: " + gameManager.totalRock;
        FishCount.text = "Fish: " + gameManager.totalFish;
        WoodCount.text = "Wood: " + gameManager.totalWood;
    }


    public void SetEagleEyeAmount(float value){
        eagleEyeFill.fillAmount = value;
    }

    public void DragonGroupFade(float endValue) {
        Debug.Log (endValue);
        if (endValue == 0) {
            FadeOutCanvasGrp(DragonGrp, 0.1f);
        } else {
            FadeInCanvasGrp(DragonGrp, 0.1f);
        }
    }

}
