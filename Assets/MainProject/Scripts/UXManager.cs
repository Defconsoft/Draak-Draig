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
    [SerializeField] private CanvasGroup VillageAttackGrp;

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
    public GameObject[] selectionBorders;
    public Image[] attackCharges;

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

        /////////////////////////////////////////
        //SO I CAN REMEMBER
        //
        // 0 - Intro
        // 1 - Main Menu
        // 2 - Customisation
        // 3 - Opening Scene
        // 4 - Resource Gathering
        // 5 - Village Daytime
        // 6 - Animal Chase
        // 7 - Castle Battle
        // 8 - Forest Swoop 
        // 9 - Village Attack
        // 10 - Anouk Test
        //////////////////////////////////////////

        //Set the quote and instructions text;
        DebugMenu.enabled = false;
        SetQuoteText(SceneNo);
        Debug.Log (SceneNo);
        //Sorts main menu interaction
        if (SceneNo == 1) {
            MainMenuGrp.blocksRaycasts = true;
        } else {
            MainMenuGrp.blocksRaycasts = false;
        }        



        StartCoroutine(LoadYourAsyncScene(SceneNo));
    }

    public IEnumerator LoadYourAsyncScene (int SceneNo) {

        //Fade in the background canvas
        FadeInCanvasGrp(BGCanvasGrp, 1f);

        //Fade out the main menu canvas
        FadeOutCanvasGrp(MainMenuGrp, 1f);

        //Wait for 3 seconds then fade in the quote
        yield return new WaitForSeconds (1.5f);
        FadeInCanvasGrp (QuoteTextGrp, 1.5f);

        AsyncOperation sceneDone = SceneManager.LoadSceneAsync (SceneNo);
        // Wait until the asynchronous scene fully loads
        while (!sceneDone.isDone)
        {
            yield return null;
        }
        
        //wait 3 seconds then fade out the quote
        yield return new WaitForSeconds (3f);
        FadeOutCanvasGrp(QuoteTextGrp, 1f);

        //This is the level specific 



        //if Main menu fade out the Top Bar Group
        if (SceneNo > 2 && SceneNo < 10) {
            FadeInCanvasGrp(TopBarGrp, 1f);
        } else {
            FadeOutCanvasGrp(TopBarGrp, 0.1f);
        }


        //Main menu scene
        if (SceneNo == 1) {
            FadeInCanvasGrp(MainMenuGrp, 1f);
        } 

        //customisation scene
        if (SceneNo == 2) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
        }  

        if (SceneNo == 3) {
            StartCoroutine(SetInstructions());
        }       


        //loads the daytime timer into the resource scene
        if (SceneNo == 4) {
            FadeInCanvasGrp (DayTimerGrp, 1.5f);
            FadeInCanvasGrp (ResourceGrp, 1.5f);
            StartDaytime();
            StartCoroutine(SetInstructions());
        }

        //Village scene
        if (SceneNo == 5) {
            FadeInCanvasGrp (ResourceGrp, 1.5f);
            FadeOutCanvasGrp (DayTimerGrp, 0.1f);
            StopDaytime();
            StartCoroutine(SetInstructions());
        }

        //animal chase scene
        if (SceneNo == 6) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            StartCoroutine(SetInstructions());
            //StartCoroutine (TempSceneWait(SceneNo + 1)); //REMOVE ME WHEN DONE
        }

        //castle attack scene
        if (SceneNo == 7) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            StartCoroutine(SetInstructions());
        }

        //forest swoop scene
        if (SceneNo == 8) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            FadeInCanvasGrp(DragonGrp, 1f);
            StartCoroutine(SetInstructions());
        }

        //city battle scene
        if (SceneNo == 9) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            FadeInCanvasGrp(VillageAttackGrp, 1f);
            // StartCoroutine (TempSceneWait(3)); //REMOVE ME WHEN DONE
        }   

        //wait for 2 seconds and fade out the BG
        yield return new WaitForSeconds (1f);
        FadeOutCanvasGrp(BGCanvasGrp, 1.5f);
    }


    IEnumerator SetInstructions() {
        //Fades in the instruction bar at the top
        FadeInCanvasGrp(InstructionGrp, 1f);


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
            FadeOutCanvasGrp (DayTimerGrp, 0.1f);
            StartCoroutine(LoadYourAsyncScene (5));
        }   
    
    }

    //////////////////////////////////////
    // DEBUG MENU
    //////////////////////////////////////    

    private IEnumerator TempSceneWait(int sceneNo) {
        yield return new WaitForSeconds (10f);
        LoadScene(sceneNo);
    }


    //////////////////////////////////////
    // USE TO FADE CANVAS GROUPS
    //////////////////////////////////////

    public void SetQuoteText(int SceneNo) {
        //SET THE INSTRUCTION TEXT FOR THE INSTRUCTIONS BOX
        switch (SceneNo)
        {
            
            case 0: //Intro
                QuoteTextBox.text = TextQuotes[0];
                break;

            case 1: //Move to Main Menu
                QuoteTextBox.text = TextQuotes[1];
                instructionText.text = instructions[1];
                break;

            case 2: //Move to Customisation
                QuoteTextBox.text = TextQuotes[2];
                instructionText.text = instructions[2];
                break;

            case 3: //Move to Opening Scene
                QuoteTextBox.text = TextQuotes[3];
                instructionText.text = instructions[3];
                break;
            case 4: //Move to Resource Gathering
                QuoteTextBox.text = TextQuotes[4];
                instructionText.text = instructions[4];
                break;
            case 5: //Move to Village Day
                QuoteTextBox.text = TextQuotes[5];
                instructionText.text = instructions[5];
                break;
            case 6: //Move to Animal Chase
                QuoteTextBox.text = TextQuotes[6];
                instructionText.text = instructions[6];
                break;
            case 7: //Move to Castle Battle
                QuoteTextBox.text = TextQuotes[7];
                instructionText.text = instructions[7];
                break;
            case 8: //Move to Forest Swoop
                QuoteTextBox.text = TextQuotes[8];
                instructionText.text = instructions[8];
                break;
            case 9: //Move to Village Attack
                QuoteTextBox.text = TextQuotes[9];
                instructionText.text = instructions[9];
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

    public void SetAttackType(int idx)
    {
        for (int i = 0; i < selectionBorders.Length; i++)
        {
            if (i == idx)
            {
                selectionBorders[i].SetActive(true);
            }
            else
            {
                selectionBorders[i].SetActive(false);
            }
        }
    }

    public void SetAttackCharge(int attack, float fill)
    {
        attackCharges[attack].fillAmount = fill;
    }

}
