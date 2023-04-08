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
    public int currentScene;
    public bool isPaused;
    public Button resumeBtn;

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
    [SerializeField] private CanvasGroup PauseMenuGrp;
    [SerializeField] private CanvasGroup HintMenuGrp;
    [SerializeField] private CanvasGroup TutorialGrp;

    [Header ("Quote Stuff")]
    [SerializeField] private TMPro.TMP_Text QuoteTextBox;   
    public string[] TextQuotes;


    [Header ("Timer Stuff")]
    public Slider daytimeSlider;
    private bool loadResourceLevelOnce;
    public bool daytimeActive;
    private bool dayComplete;
    private Quaternion _targetRotation;
    float angleToRotate = 160f;
    float stepAngle;

    [Header ("Item Stuff")]
    public TMPro.TMP_Text RockCount;
    public TMPro.TMP_Text FishCount;
    public TMPro.TMP_Text WoodCount; 
    public GameObject MinimapContainer;


    [Header ("Instruction Stuff")]
    public TMPro.TMP_Text instructionText;
    public string[] instructions;

    [Header ("Dragon Stuff")]
    public GameObject[] selectionBorders;
    public Image eagleEyeFill, firebreathFill;
    public Image[] attackCharges;
    public GameObject DragonEyeBar, FireBreathBar;
    public Image destructionBar;
    public GameObject powerText;

    [Header ("Health Stuff")]
    public Image HealthBar;
    public Image EnergyBar;
    public Sprite Human, Dragon;
    public Image IconHolder;


    [Header ("Hint Stuff")]
    public float hintTime;
    public float hintFadeTime;
    public string[] hints;
    public TMPro.TMP_Text hintTextArea;
    public float showHintTime = 20f;
    private float prayHintTime;
    private float hintTimer;
    private float prayerTimer;
    private bool hasPrayed;
    private bool HintShow, PrayShow;
    public bool interacted, spoken, birddead, archerdead, pigdead, targetdone;
    public int spokeTo;


    [Header ("Tutorial Stuff")]
    public TMPro.TMP_Text Directions;
    public TMPro.TMP_Text Instructions; 
    public bool firstInteracted; 

    [Header ("MusicStuff")]    
    public AudioSource MenuMusic;
    public AudioSource DayTimeMusic;    
    public AudioSource NightTimeMusic;

    private void Awake() {

    }

    private void Start() {
        RockCount.text = "Rock: " + gameManager.totalRock;
        FishCount.text = "Fish: " + gameManager.totalFish;
        WoodCount.text = "Wood: " + gameManager.totalWood;
        HealthBar.fillAmount = gameManager.HealthAmount;
        EnergyBar.fillAmount = gameManager.EnergyAmount;
        prayHintTime = showHintTime;


        _targetRotation = Quaternion.Euler(186f, -30f, 0);
        stepAngle = angleToRotate / gameManager.DaytimeTimerAmount;
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
        currentScene = SceneNo;
        
        HintMenuGrp.DOFade (0, 0);
        SetQuoteText(SceneNo);
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
        hintTimer = 0;
        HintShow = false;
        //wait 3 seconds then fade out the quote
        yield return new WaitForSeconds (3f);
        FadeOutCanvasGrp(QuoteTextGrp, 1f);

        //This is the level specific 



        //if Main menu fade out the Top Bar Group
        if (SceneNo > 3 && SceneNo < 9) {
            FadeInCanvasGrp(TopBarGrp, 1f);
        } else {
            FadeOutCanvasGrp(TopBarGrp, 0.1f);
        }


        //if Main menu fade out the Top Bar Group
        if (SceneNo > 2 && SceneNo < 6) {
            MinimapContainer.SetActive (true);
        } else {
            MinimapContainer.SetActive (false);
        }

        //Main menu scene
        if (SceneNo == 1) {
            FadeInCanvasGrp(MainMenuGrp, 1f); 
            MenuMusic.Play();
            DayTimeMusic.Stop();
            NightTimeMusic.Stop();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } 

        //customisation scene
        if (SceneNo == 2) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            MenuMusic.Stop();
            DayTimeMusic.Play();
            NightTimeMusic.Stop();
        }  

        if (SceneNo == 3) { //Opening Scene
            StartCoroutine(StartGameFade());
            gameManager.HealthAmount = 1f;
            gameManager.EnergyAmount = 1f;
            gameManager.UpdateBars();
            FadeOutCanvasGrp(VillageAttackGrp, 0.1f);
            IconHolder.sprite = Human;

        }       


        //loads the daytime timer into the resource scene
        if (SceneNo == 4) {
            FadeInCanvasGrp (DayTimerGrp, 1.5f);
            FadeInCanvasGrp (ResourceGrp, 1.5f);
            StartDaytime();
            StartCoroutine(SetInstructions());
            IconHolder.sprite = Human;
        }

        //Village scene
        if (SceneNo == 5) {
            FadeInCanvasGrp (ResourceGrp, 1.5f);
            FadeOutCanvasGrp (DayTimerGrp, 0.1f);
            StopDaytime();
            StartCoroutine(SetInstructions());
            IconHolder.sprite = Human;
        }

        //animal chase scene
        if (SceneNo == 6) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            FadeInCanvasGrp(DragonGrp, 1f);
            DragonEyeBar.SetActive (false);
            FireBreathBar.SetActive (true);
            StartCoroutine(SetInstructions());
            IconHolder.sprite = Dragon;
            MenuMusic.Stop();
            DayTimeMusic.Stop();
            NightTimeMusic.Play();
        }

        //castle attack scene
        if (SceneNo == 7) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            FadeInCanvasGrp(DragonGrp, 1f);
            DragonEyeBar.SetActive (false);
            FireBreathBar.SetActive (true);
            StartCoroutine(SetInstructions());
            IconHolder.sprite = Dragon;
        }

        //forest swoop scene
        if (SceneNo == 8) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            FadeInCanvasGrp(DragonGrp, 1f);
            DragonEyeBar.SetActive (true);
            FireBreathBar.SetActive (false);
            StartCoroutine(SetInstructions());
            IconHolder.sprite = Dragon;
        }

        //city battle scene
        if (SceneNo == 9) {
            FadeOutCanvasGrp (ResourceGrp, 0.1f);
            FadeOutCanvasGrp(TopBarGrp, 0.1f);
            DragonEyeBar.SetActive (false);
            FadeInCanvasGrp(VillageAttackGrp, 1f);
            // StartCoroutine (TempSceneWait(3)); //REMOVE ME WHEN DONE
            IconHolder.sprite = Dragon;
        }   

        //wait for 2 seconds and fade out the BG
        yield return new WaitForSeconds (1f);
        FadeOutCanvasGrp(BGCanvasGrp, 1.5f);
    }

    IEnumerator StartGameFade() {
        yield return new WaitForSeconds(12f);
        StartCoroutine(SetInstructions());
        FadeInCanvasGrp(TopBarGrp, 1f);
    }


    IEnumerator SetInstructions() {
        //Fades in the instruction bar at the top
        FadeInCanvasGrp(InstructionGrp, 1f);


        //Fades out the instruction bar after 5 seconds
        yield return new WaitForSeconds (5f);
        //FadeOutCanvasGrp(InstructionGrp, 5f);
    }   

    public void FadeVillageAttack() {
        FadeOutCanvasGrp(VillageAttackGrp, 1f);
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
            GameObject.Find("SunDirectional").transform.rotation = Quaternion.RotateTowards(GameObject.Find("SunDirectional").transform.rotation, _targetRotation, stepAngle * Time.deltaTime);
        }

        //NEED TO PUT IN A DAYTIME ENDING SOON TIMER AND MAKE THE SUN MOVE


        if (daytimeActive == true && daytimeSlider.value >= daytimeSlider.maxValue && !dayComplete) {
            dayComplete = true;
            FadeOutCanvasGrp (DayTimerGrp, 0.1f);
            StartCoroutine(LoadYourAsyncScene (5));
        } 

        //Pause Menu Stuff

        
        if (Input.GetKeyDown(KeyCode.Escape) && currentScene >=3) {
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            PauseMenuGrp.blocksRaycasts = true;
            PauseMenuGrp.interactable = true;
            PauseMenuGrp.alpha = 1f;
            Time.timeScale = 0f;
        }
  
        if (currentScene >=3) {
            hintTimer += Time.deltaTime;
            CheckToDisplayHints();
        }

        if (spokeTo >= 2){
            prayerTimer += Time.deltaTime;
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

    public void SetFireBreathAmount(float value){
        firebreathFill.fillAmount = value;
    }

    public void DragonGroupFade(float endValue) {
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

    public void SetDestructionAmount(float amount)
    {
        destructionBar.fillAmount = amount;
    }



    public void PauseResume() {
        resumeBtn.interactable = false;
        resumeBtn.interactable = true;
        isPaused = false;
        PauseMenuGrp.alpha = 0f;
        Time.timeScale = 1f;
        PauseMenuGrp.interactable = false;
        PauseMenuGrp.blocksRaycasts = false;
        //Sort out the cursor

        switch (currentScene) 
        {
            case 0:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case 1:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case 2: 
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case 3: 
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;  

            case 4: 
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case 5: 
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case 6: 
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case 7: 
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
                break;

            case 8: 
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case 9: 
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;


        }        





    }

    public void PauseMainMenu() {
        isPaused = false;
        Time.timeScale = 1f;
        LoadScene(1);
        PauseMenuGrp.interactable = false;
        PauseMenuGrp.blocksRaycasts = false;
        FadeOutCanvasGrp(PauseMenuGrp, 0.5f);
        Debug.Log (Cursor.lockState);
    }

    public void PauseExitGame() {
        Application.Quit();
    }


    public void ShowHint(int hintNo) {
        hintTextArea.text = hints[hintNo];
        StartCoroutine(AnimateHint());
    }

    IEnumerator AnimateHint() {
        HintMenuGrp.DOFade (1, hintFadeTime);
        yield return new WaitForSeconds (hintTime);
        HintMenuGrp.DOFade (0, hintFadeTime);
    }

    void CheckToDisplayHints(){
        //Yes I know this is a very bad bunch of if statements but whatever.

        if (!PrayShow){
            if (currentScene == 5 && prayerTimer>= prayHintTime) {
                if (spokeTo >= 2 && !hasPrayed){
                    hasPrayed = true;
                    PrayShow = true;
                    ShowHint (3);
                }
            } 
        }




        if (!HintShow){
            if (currentScene == 3 && hintTimer >= showHintTime) {
                HintShow = true;
                ShowHint (0);
            } 
            
            if (currentScene == 4 && hintTimer >= showHintTime) {
                if (!interacted){
                    HintShow = true;
                    ShowHint (1);
                }
            } 
            
            if (currentScene == 5 && hintTimer >= showHintTime) {
                if (!spoken){
                    HintShow = true;
                    ShowHint (2);
                }
            } 
 
                     
            if (currentScene == 6 && hintTimer >= showHintTime){
                if (!birddead){
                    HintShow = true;
                    ShowHint (4);
                }
            } 
            
            if (currentScene == 7 && hintTimer >= showHintTime){
                if (!archerdead){
                    HintShow = true;
                    ShowHint (5);
                }
            } 
            
            if (currentScene == 8 && hintTimer >= showHintTime){
                if (!pigdead){
                    HintShow = true;
                    ShowHint (6);
                }
            }

            if (currentScene == 9 && hintTimer >= showHintTime){
                if (!targetdone){
                    HintShow = true;
                    ShowHint (7);
                }
            }

        }

    }


    public void ShowTutorial() {
        TutorialGrp.alpha = 1f;
        TutorialGrp.blocksRaycasts = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;


    }

    public void ExitTutorial() {
        TutorialGrp.alpha = 0f;
        TutorialGrp.blocksRaycasts = false;
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


}
