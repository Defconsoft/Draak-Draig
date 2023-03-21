using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DinoFracture;



public class InteractableGameManager : MonoBehaviour
{



    public enum GameType { rock, tree, fish};
    private GameManager gameManager;
    private UXManager uXManager;
    private int baseAmount;
    private int addAmount;
    private float Bonus1, Bonus2;


    [Header ("UX Animation Stuff")] 
    public float moveUp;

    [Header ("Interact Type Setup")]
    public GameType gameType;
    public GameObject [] RockModels;
    public GameObject [] TreeModels;
    public GameObject FishPool;


    [Header ("Game Tracking")]
    private int Choice;
    public int InteractableState = 0;
    public float slideTime = 1f;


    [Header ("Rock UX")]
    public Canvas rockAimCanvas;
    public CircleSlider rockAimSlider;
    public TMPro.TMP_Text rockAimReact;
    private Tween rockAimTween;

    public Canvas rockPowerCanvas;
    public Slider rockPowerSlider;
    public TMPro.TMP_Text rockPowerReact;
    public TMPro.TMP_Text rockAmountReact;
    private Tween rockPowerTween;

    [Header ("Tree UX")]
    public Canvas treeAimCanvas;
    public CircleSlider treeAimSlider;
    public TMPro.TMP_Text treeAimReact;
    private Tween treeAimTween;

    public Canvas treePowerCanvas;
    public Slider treePowerSlider;
    public TMPro.TMP_Text treePowerReact;
    public TMPro.TMP_Text treeAmountReact;
    private Tween treePowerTween;


    [Header ("Fish UX")]
    public Canvas fishAimCanvas;
    public Slider fishAimSlider;
    public TMPro.TMP_Text fishAimReact;
    private Tween fishAimTween;

    public Canvas fishPowerCanvas;
    public Slider fishPowerSlider;
    public TMPro.TMP_Text fishPowerReact;
    public TMPro.TMP_Text fishAmountReact;
    private Tween fishPowerTween;


    [Header ("Animation related")]
    public Animator armsAnim;
    public float animDelayTree = 1f;
    public float animDelayRock = 1f;
    public float animDelayFish = 1f;
    private GameObject player;
    public GameObject fractureContainer;
    public GameObject fishBobber;
    private Tween bobberTween;
    private GameObject fishingLineStart;


    private void Start() {

        player = GameObject.FindWithTag("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uXManager = GameObject.Find("GameManager").GetComponent<UXManager>();

        if (gameType == GameType.rock){
            //Select a rock
            Choice = Random.Range (0, RockModels.Length);
            for (int i = 0; i < RockModels.Length; i++)
            {
                if (i == Choice) {
                    RockModels[i].SetActive (true);
                    RockModels[i].transform.rotation = Random.rotation;
                }
            }
        }

        if (gameType == GameType.tree){
            //Select a tree
            Choice = Random.Range (0, TreeModels.Length);
            for (int i = 0; i < TreeModels.Length; i++)
            {
                if (i == Choice) {
                    TreeModels[i].SetActive (true);
                    TreeModels[i].transform.Rotate (Random.Range (0, 360), Random.Range (0, 360), 0);
                    //TreeModels[i].transform.rotation = new Quaternion (Random.Range (0, 360), Random.Range (0, 360), TreeModels[i].transform.rotation.z, 0);
                }
            }
        }  


        if (gameType == GameType.fish){
            //Select a fish
            FishPool.SetActive (true);
        }  
    }

    public void StartTheGame() {

        armsAnim = player.GetComponentInChildren<Animator>();

        if (gameType == GameType.rock){
            Debug.Log ("rock");
            StartCoroutine(RunRockGame());
            armsAnim.SetTrigger("minePhase1");
        }

        if (gameType == GameType.tree){
            Debug.Log ("tree");
            StartCoroutine(RunTreeGame());
            armsAnim.SetTrigger("chopPhase1");
        }

        if (gameType == GameType.fish){
            Debug.Log ("fish");
            armsAnim.SetBool("isFishing", true);
            StartCoroutine(RunFishGame());
        }


    }


    public void KillTheAimTween() {

        if (gameType == GameType.rock){
            StartCoroutine(SecondPhaseRockGame());
            armsAnim.SetTrigger("minePhase2");
        }

        if (gameType == GameType.tree){
            StartCoroutine(SecondPhaseTreeGame());
            armsAnim.SetTrigger("chopPhase2");
        }

        if (gameType == GameType.fish){
            StartCoroutine(SecondPhaseFishGame());
            armsAnim.SetTrigger("rodOut");
        }
    }

    public void KillThePowerTween() {

        if (gameType == GameType.rock){
            StartCoroutine(EndRockGame());
        }

        if (gameType == GameType.tree){
            StartCoroutine(EndTreeGame());
        }

        if (gameType == GameType.fish){
            StartCoroutine(EndFishGame());
            armsAnim.SetBool("isFishing", false);
        }
    }


    ////////////////////////////////////
    //ROCK GAME
    ////////////////////////////////////

    public IEnumerator RunRockGame() {
        yield return new WaitForSeconds(2f);
        rockAimCanvas.enabled = true;
        rockAimTween = DOTween.To (     ()=> rockAimSlider.value, 
                                        x=> rockAimSlider.value = x, 
                                        1f, 
                                        slideTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        InteractableState = 1;
    }


    public IEnumerator SecondPhaseRockGame() {
        rockAimTween.Kill();
        float attempt = rockAimSlider.value;
        //check the attempt and animate the text
        if (attempt >= 0.45f && attempt <=0.55f) {
            rockAimReact.text = "Excellent";
            Bonus1 = 0.75f;
        } else {
            rockAimReact.text = "Good";
            Bonus1 = 0.25f;
        } 
        rockAimReact.enabled = true;
        rockAimReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (rockAimReact.gameObject.GetComponent<RectTransform>().anchoredPosition.x, rockAimReact.gameObject.GetComponent<RectTransform>().anchoredPosition.y + moveUp), 1f).SetEase (Ease.InOutQuad);

        
        yield return new WaitForSeconds(1f);
        rockAimCanvas.enabled = false;
        InteractableState = 2;
        rockPowerCanvas.enabled = true;
        rockPowerTween = rockPowerSlider.DOValue(1f, slideTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
    } 

    public IEnumerator EndRockGame() {
        rockPowerTween.Kill();
        float attempt = rockPowerSlider.value;

        //check the attempt and animate the text
        if (attempt >= 0.8f ) {
            rockPowerReact.text = "Excellent";
            Bonus2 = 0.75f;
        } else {
            rockPowerReact.text = "Good";
            Bonus2 = 0.25f;
        } 


        TotalResources(1);


        rockPowerReact.enabled = true;
        rockPowerReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (rockPowerReact.gameObject.GetComponent<RectTransform>().anchoredPosition.x, rockPowerReact.gameObject.GetComponent<RectTransform>().anchoredPosition.y + moveUp), 1f).SetEase (Ease.InOutQuad);
        rockAmountReact.text = addAmount.ToString() + " Rock";
        rockAmountReact.enabled = true;
        rockAmountReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (rockAmountReact.gameObject.GetComponent<RectTransform>().anchoredPosition.x, rockAmountReact.gameObject.GetComponent<RectTransform>().anchoredPosition.y + moveUp), 1f).SetEase (Ease.InOutQuad);

        yield return new WaitForSeconds(1f);
        rockPowerCanvas.enabled = false;
        //PLAY THE PICK AXE SWIN ANIM
        armsAnim.SetTrigger("mine");
        yield return new WaitForSeconds(animDelayRock);

        RockModels[Choice].GetComponent<FractureGeometry>().Fracture();

        GameObject.Find ("Player").GetComponent<PlayerController>().Endinteracting();
        yield return new WaitForSeconds(2f);
        InteractableState = 3;
        fractureContainer.transform.parent = null;
        Destroy(this.gameObject.transform.parent.gameObject);
        yield return new WaitForSeconds(1f);

    }

    ////////////////////////////////////
    //TREE GAME
    ////////////////////////////////////

    public IEnumerator RunTreeGame() {
        yield return new WaitForSeconds(2f);
        treeAimCanvas.enabled = true;
        //treeAimTween = treeAimSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);

        treeAimCanvas.enabled = true;
        treeAimTween = DOTween.To (     ()=> treeAimSlider.value, 
                                        x=> treeAimSlider.value = x, 
                                        1f, 
                                        slideTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        InteractableState = 1;
    }


    public IEnumerator SecondPhaseTreeGame() {
        treeAimTween.Kill();
        float attempt = treeAimSlider.value;
        //check the attempt and animate the text
        if (attempt >= 0.71f && attempt <=0.79f) {
            treeAimReact.text = "Excellent";
            Bonus1 = 0.75f;
        } else {
            treeAimReact.text = "Good";
            Bonus1 = 0.25f;
        } 
        treeAimReact.enabled = true;
        treeAimReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (treeAimReact.gameObject.GetComponent<RectTransform>().anchoredPosition.x, treeAimReact.gameObject.GetComponent<RectTransform>().anchoredPosition.y + moveUp), 1f).SetEase (Ease.InOutQuad);

        
        yield return new WaitForSeconds(1f);
        treeAimCanvas.enabled = false;
        InteractableState = 2;
        treePowerCanvas.enabled = true;
        treePowerTween = treePowerSlider.DOValue(1f, slideTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
    } 

    public IEnumerator EndTreeGame() {
        treePowerTween.Kill();
        float attempt = treePowerSlider.value;

        //check the attempt and animate the text
        if (attempt >= 0.8f ) {
            treePowerReact.text = "Excellent";
            Bonus2 = 0.75f;
        } else {
            treePowerReact.text = "Good";
            Bonus2 = 0.25f;
        } 

        TotalResources(2);

        treePowerReact.enabled = true;
        treePowerReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (treePowerReact.gameObject.GetComponent<RectTransform>().anchoredPosition.x, treePowerReact.gameObject.GetComponent<RectTransform>().anchoredPosition.y + moveUp), 1f).SetEase (Ease.InOutQuad);
        treeAmountReact.text = addAmount.ToString() + " Wood";
        treeAmountReact.enabled = true;
        treeAmountReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (treeAmountReact.gameObject.GetComponent<RectTransform>().anchoredPosition.x, treeAmountReact.gameObject.GetComponent<RectTransform>().anchoredPosition.y + moveUp), 1f).SetEase (Ease.InOutQuad);

        yield return new WaitForSeconds(1f);
        treePowerCanvas.enabled = false;
        //PLAY THE AXE SWING ANIM
        armsAnim.SetTrigger("chop");
        yield return new WaitForSeconds(animDelayTree);

        TreeModels[Choice].GetComponent<FractureGeometry>().Fracture();

        GameObject.Find ("Player").GetComponent<PlayerController>().Endinteracting();
        yield return new WaitForSeconds(4f);
        InteractableState = 3;
        fractureContainer.transform.parent = null;
        Destroy(this.gameObject.transform.parent.gameObject);
        yield return new WaitForSeconds(1f);

    }

    ////////////////////////////////////
    //FISH GAME
    ////////////////////////////////////

    public IEnumerator RunFishGame() {
        yield return new WaitForSeconds(2f);
        fishAimCanvas.enabled = true;
        fishAimTween = fishAimSlider.DOValue(1f, slideTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        InteractableState = 1;
    }


    public IEnumerator SecondPhaseFishGame() {
        fishAimTween.Kill();
        float attempt = fishAimSlider.value;
        //check the attempt and animate the text
        if (attempt >= 0.8f) {
            fishAimReact.text = "Super Cast";
            Bonus1 = 0.75f;
        } else {
            fishAimReact.text = "Good Cast";
            Bonus1 = 0.25f;
        } 
        fishAimReact.enabled = true;
        fishAimReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (fishAimReact.gameObject.GetComponent<RectTransform>().anchoredPosition.x, fishAimReact.gameObject.GetComponent<RectTransform>().anchoredPosition.y + moveUp), 1f).SetEase (Ease.InOutQuad);

        //Do some fishing here
        fishingLineStart = GameObject.FindWithTag("fishingLineTip"); // getting it here as otherwise the GO is inactive
        fishBobber.transform.position = fishingLineStart.transform.position;
        fishBobber.SetActive (true);
        fishBobber.GetComponent<UpdateLineScript>()._childTransform = fishingLineStart.transform; // player.transform;    

        fishBobber.transform.DOJump (FishPool.transform.position, 2f, 1, 0.4f).SetEase(Ease.InExpo);

        yield return new WaitForSeconds(1f);
        fishAimCanvas.enabled = false;
        InteractableState = 2;

        yield return new WaitForSeconds(0.4f);
        bobberTween = fishBobber.transform.DOShakePosition(1f, new Vector3 (0,0.2f, 0), 2, 45f).SetLoops (-1, LoopType.Restart);
        yield return new WaitForSeconds(2f);



        fishPowerCanvas.enabled = true;
        fishPowerTween = fishPowerSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
    } 

    public IEnumerator EndFishGame() {
        fishPowerTween.Kill();
        bobberTween.Kill();
        fishBobber.transform.DOJump (fishingLineStart.transform.position, 2f, 1, 0.4f).SetEase(Ease.InExpo);
        fishBobber.SetActive(false);

        float attempt = fishPowerSlider.value;

        //check the attempt and animate the text
        if (attempt >= 0.8f ) {
            fishPowerReact.text = "Big Fish";
            Bonus2 = 0.75f;
        } else {
            fishPowerReact.text = "Medium Fish";
            Bonus2 = 0.25f;
        } 


        TotalResources(3);


        fishPowerReact.enabled = true;
        fishPowerReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (fishPowerReact.gameObject.GetComponent<RectTransform>().anchoredPosition.x, fishPowerReact.gameObject.GetComponent<RectTransform>().anchoredPosition.y + moveUp), 1f).SetEase (Ease.InOutQuad);
        fishAmountReact.text = addAmount.ToString() + " Fish";
        fishAmountReact.enabled = true;
        fishAmountReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (fishAmountReact.gameObject.GetComponent<RectTransform>().anchoredPosition.x, fishAmountReact.gameObject.GetComponent<RectTransform>().anchoredPosition.y + moveUp), 1f).SetEase (Ease.InOutQuad);


        yield return new WaitForSeconds(1f);
        fishPowerCanvas.enabled = false;

        yield return new WaitForSeconds(animDelayFish);

        FishPool.SetActive (false);

        GameObject.Find ("Player").GetComponent<PlayerController>().Endinteracting();
        yield return new WaitForSeconds(4f);
        InteractableState = 3;
        Destroy(this.gameObject.transform.parent.gameObject);
        yield return new WaitForSeconds(1f);

    }

    void TotalResources(int type) {

        baseAmount = gameManager.baseResourceAmount;

        addAmount = Mathf.RoundToInt (baseAmount + (baseAmount + (baseAmount * Bonus1 + baseAmount * Bonus2)));

        switch (type)
        {
            
            case 0: //no interactable
                break;

            case 1: //rock
                gameManager.totalRock = gameManager.totalRock + addAmount;
                break;

            case 2: //tree
                gameManager.totalWood = gameManager.totalWood + addAmount;
                break;

            case 3: //fish
                gameManager.totalFish = gameManager.totalFish + addAmount;
                break;


        }

        uXManager.SetItemAmounts();

    }



    //Change objects to single array and make fracture function public.





}
