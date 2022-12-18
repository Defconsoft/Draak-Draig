using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DinoFracture;



public class InteractableGameManager : MonoBehaviour
{



    public enum GameType { rock, tree, fish};

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
    private Tween rockPowerTween;

    [Header ("Tree UX")]
    public Canvas treeAimCanvas;
    public Slider treeAimSlider;
    public TMPro.TMP_Text treeAimReact;
    private Tween treeAimTween;

    public Canvas treePowerCanvas;
    public Slider treePowerSlider;
    public TMPro.TMP_Text treePowerReact;
    private Tween treePowerTween;


    [Header ("Fish UX")]
    public Canvas fishAimCanvas;
    public Slider fishAimSlider;
    public TMPro.TMP_Text fishAimReact;
    private Tween fishAimTween;

    public Canvas fishPowerCanvas;
    public Slider fishPowerSlider;
    public TMPro.TMP_Text fishPowerReact;
    private Tween fishPowerTween;


    [Header ("Animation related")]
    public Animator armsAnim;
    public float animDelay = 4f;
    private GameObject player;


    private void Start() {

        player = GameObject.FindWithTag("Player");

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
        } else {
            rockAimReact.text = "Good";
        } 
        rockAimReact.enabled = true;
        rockAimReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (0, 100), 1f).SetEase (Ease.InOutQuad);

        
        yield return new WaitForSeconds(1f);
        rockAimCanvas.enabled = false;
        InteractableState = 2;
        rockPowerCanvas.enabled = true;
        rockPowerTween = rockPowerSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
    } 

    public IEnumerator EndRockGame() {
        rockPowerTween.Kill();
        float attempt = rockPowerSlider.value;

        //check the attempt and animate the text
        if (attempt >= 0.8f ) {
            rockPowerReact.text = "Excellent";
        } else {
            rockPowerReact.text = "Good";
        } 
        rockPowerReact.enabled = true;
        rockPowerReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (-2.6f, 147.8f), 1f).SetEase (Ease.InOutQuad);


        yield return new WaitForSeconds(1f);
        rockPowerCanvas.enabled = false;
        //PLAY THE PICK AXE SWIN ANIM
        armsAnim.SetTrigger("mine");
        yield return new WaitForSeconds(animDelay);

        RockModels[Choice].GetComponent<FractureGeometry>().Fracture();

        GameObject.Find ("Player").GetComponent<PlayerController>().Endinteracting();
        yield return new WaitForSeconds(4f);
        InteractableState = 3;
        Destroy(this.gameObject.transform.parent.gameObject);
        yield return new WaitForSeconds(1f);

    }

    ////////////////////////////////////
    //TREE GAME
    ////////////////////////////////////

    public IEnumerator RunTreeGame() {
        yield return new WaitForSeconds(2f);
        treeAimCanvas.enabled = true;
        treeAimTween = treeAimSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        InteractableState = 1;
    }


    public IEnumerator SecondPhaseTreeGame() {
        treeAimTween.Kill();
        float attempt = treeAimSlider.value;
        //check the attempt and animate the text
        if (attempt >= 0.41f && attempt <=0.59f) {
            treeAimReact.text = "Excellent";
        } else {
            treeAimReact.text = "Good";
        } 
        treeAimReact.enabled = true;
        treeAimReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (-241, 100), 1f).SetEase (Ease.InOutQuad);

        
        yield return new WaitForSeconds(1f);
        treeAimCanvas.enabled = false;
        InteractableState = 2;
        treePowerCanvas.enabled = true;
        treePowerTween = treePowerSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
    } 

    public IEnumerator EndTreeGame() {
        treePowerTween.Kill();
        float attempt = treePowerSlider.value;

        //check the attempt and animate the text
        if (attempt >= 0.8f ) {
            treePowerReact.text = "Excellent";
        } else {
            treePowerReact.text = "Good";
        } 
        treePowerReact.enabled = true;
        treePowerReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (-2.6f, 147.8f), 1f).SetEase (Ease.InOutQuad);


        yield return new WaitForSeconds(1f);
        treePowerCanvas.enabled = false;
        //PLAY THE AXE SWING ANIM
        armsAnim.SetTrigger("chop");
        yield return new WaitForSeconds(animDelay);

        TreeModels[Choice].GetComponent<FractureGeometry>().Fracture();

        GameObject.Find ("Player").GetComponent<PlayerController>().Endinteracting();
        yield return new WaitForSeconds(4f);
        InteractableState = 3;
        Destroy(this.gameObject.transform.parent.gameObject);
        yield return new WaitForSeconds(1f);

    }

    ////////////////////////////////////
    //FISH GAME
    ////////////////////////////////////

    public IEnumerator RunFishGame() {
        yield return new WaitForSeconds(2f);
        fishAimCanvas.enabled = true;
        fishAimTween = fishAimSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        InteractableState = 1;
    }


    public IEnumerator SecondPhaseFishGame() {
        fishAimTween.Kill();
        float attempt = fishAimSlider.value;
        //check the attempt and animate the text
        if (attempt >= 0.41f && attempt <=0.59f) {
            fishAimReact.text = "Excellent";
        } else {
            fishAimReact.text = "Good";
        } 
        fishAimReact.enabled = true;
        fishAimReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (-241, 100), 1f).SetEase (Ease.InOutQuad);

        
        yield return new WaitForSeconds(1f);
        fishAimCanvas.enabled = false;
        InteractableState = 2;
        fishPowerCanvas.enabled = true;
        fishPowerTween = fishPowerSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
    } 

    public IEnumerator EndFishGame() {
        fishPowerTween.Kill();
        float attempt = fishPowerSlider.value;

        //check the attempt and animate the text
        if (attempt >= 0.8f ) {
            fishPowerReact.text = "Excellent";
        } else {
            fishPowerReact.text = "Good";
        } 
        fishPowerReact.enabled = true;
        fishPowerReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (-2.6f, 147.8f), 1f).SetEase (Ease.InOutQuad);


        yield return new WaitForSeconds(1f);
        fishPowerCanvas.enabled = false;

        yield return new WaitForSeconds(animDelay);

        FishPool.SetActive (false);

        GameObject.Find ("Player").GetComponent<PlayerController>().Endinteracting();
        yield return new WaitForSeconds(4f);
        InteractableState = 3;
        Destroy(this.gameObject.transform.parent.gameObject);
        yield return new WaitForSeconds(1f);

    }

    //Change objects to single array and make fracture function public.





}
