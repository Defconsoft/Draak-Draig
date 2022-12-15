using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DinoFracture;



public class InteractableGameManager : MonoBehaviour
{

    public enum GameType { rock, tree, fish};
    public GameType gameType;
    public GameObject [] RockModels;
    public GameObject [] TreeModels;
    private int Choice;
    public int InteractableState = 0;

    public float slideTime = 1f;
    public Canvas aimCanvas;
    public Slider aimSlider;
    public TMPro.TMP_Text aimReact;
    private Tween aimTween;

    public Canvas powerCanvas;
    public Slider powerSlider;
    public TMPro.TMP_Text powerReact;
    private Tween powerTween;

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

        }  
    }

    public void StartTheGame() {

        armsAnim = player.GetComponentInChildren<Animator>();

        if (gameType == GameType.rock){
            StartCoroutine(RunRockGame());
        }

        if (gameType == GameType.tree){
            StartCoroutine(RunTreeGame());
        }

        if (gameType == GameType.fish){
            // armsAnim.SetBool("isFishing", true);
        }


    }


    public void KillTheAimTween() {

        if (gameType == GameType.rock){
            StartCoroutine(SecondPhaseRockGame());
        }

        if (gameType == GameType.tree){
            StartCoroutine(SecondPhaseTreeGame());
        }

        if (gameType == GameType.fish){
            
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
            // armsAnim.SetBool("isFishin", false);
        }
    }


    ////////////////////////////////////
    //ROCK GAME
    ////////////////////////////////////

    public IEnumerator RunRockGame() {
        yield return new WaitForSeconds(2f);
        aimCanvas.enabled = true;
        aimTween = aimSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        InteractableState = 1;
    }


    public IEnumerator SecondPhaseRockGame() {
        aimTween.Kill();
        float attempt = aimSlider.value;
        //check the attempt and animate the text
        if (attempt >= 0.41f && attempt <=0.59f) {
            aimReact.text = "Excellent";
        } else {
            aimReact.text = "Good";
        } 
        aimReact.enabled = true;
        aimReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (-241, 100), 1f).SetEase (Ease.InOutQuad);

        
        yield return new WaitForSeconds(1f);
        aimCanvas.enabled = false;
        InteractableState = 2;
        powerCanvas.enabled = true;
        powerTween = powerSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
    } 

    public IEnumerator EndRockGame() {
        powerTween.Kill();
        float attempt = powerSlider.value;

        //check the attempt and animate the text
        if (attempt >= 0.8f ) {
            powerReact.text = "Excellent";
        } else {
            powerReact.text = "Good";
        } 
        powerReact.enabled = true;
        powerReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (-2.6f, 147.8f), 1f).SetEase (Ease.InOutQuad);


        yield return new WaitForSeconds(1f);
        powerCanvas.enabled = false;
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
        aimCanvas.enabled = true;
        aimTween = aimSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        InteractableState = 1;
    }


    public IEnumerator SecondPhaseTreeGame() {
        aimTween.Kill();
        float attempt = aimSlider.value;
        //check the attempt and animate the text
        if (attempt >= 0.41f && attempt <=0.59f) {
            aimReact.text = "Excellent";
        } else {
            aimReact.text = "Good";
        } 
        aimReact.enabled = true;
        aimReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (-241, 100), 1f).SetEase (Ease.InOutQuad);

        
        yield return new WaitForSeconds(1f);
        aimCanvas.enabled = false;
        InteractableState = 2;
        powerCanvas.enabled = true;
        powerTween = powerSlider.DOValue(1f, slideTime).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
    } 

    public IEnumerator EndTreeGame() {
        powerTween.Kill();
        float attempt = powerSlider.value;

        //check the attempt and animate the text
        if (attempt >= 0.8f ) {
            powerReact.text = "Excellent";
        } else {
            powerReact.text = "Good";
        } 
        powerReact.enabled = true;
        powerReact.gameObject.GetComponent<RectTransform>().DOAnchorPos (new Vector2 (-2.6f, 147.8f), 1f).SetEase (Ease.InOutQuad);


        yield return new WaitForSeconds(1f);
        powerCanvas.enabled = false;
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



    //Change objects to single array and make fracture function public.





}
