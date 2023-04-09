using UnityEngine;
using DG.Tweening;

public class InteractableNPCManager : MonoBehaviour
{

    [Header("UX Objects")]
    public Canvas overallCanvas;
    public CanvasGroup narrativeCanvas;
    public TMPro.TMP_Text NPCName;
    public TMPro.TMP_Text Textbox;
    public TMPro.TMP_Text Outcome;
    public GameObject Yesbutton, Nobutton, Exitbutton;


    public int ConversationState;
    private Animator anim;
    
    public bool FishVendor, RockVendor;

    [Header("Speech Stuff")]
    public string npcName;
    [TextArea(3,10)]
    public string outComeText;
    [TextArea(5,10)]
    public string[] Conversation;
    public AudioClip greeting;


    public void StartConversation() {
        NPCName.text = npcName;
        Outcome.text = outComeText;
        overallCanvas.enabled = true;
        narrativeCanvas.DOFade(1f,1f);
        Textbox.text = Conversation[0];
        anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("Talk");
        GetComponent<AudioSource>().PlayOneShot(greeting, 0.25f);

        //check to see if you have resources
        if (RockVendor && GameObject.Find("GameManager").GetComponent<GameManager>().totalRock == 0){
            ConversationState = 4;
        }

        if (FishVendor && GameObject.Find("GameManager").GetComponent<GameManager>().totalFish == 0){
            ConversationState = 4;
        }


    }

    private void Update() {
        
        switch (ConversationState){
            case 0: //Start
                break;

            case 1: //Are you sure
                Textbox.text = Conversation[1];
                anim.SetTrigger("Talk");
                break;

            case 2: //SuccessExit
                Textbox.text = Conversation[2];
                anim.SetTrigger("Talk");
                Yesbutton.SetActive (false);
                Nobutton.SetActive (false);
                Exitbutton.SetActive (true);

                if (RockVendor) {
                    GameObject.Find("GameManager").GetComponent<GameManager>().totalRock = 0;
                    GameObject.Find("GameManager").GetComponent<UXManager>().SetItemAmounts();
                }

                if (FishVendor) {
                    GameObject.Find("GameManager").GetComponent<GameManager>().totalFish = 0;
                    GameObject.Find("GameManager").GetComponent<UXManager>().SetItemAmounts();
                }


                break;

            case 3: //FailExit
                Textbox.text = Conversation[3];
                anim.SetTrigger("Talk");
                Yesbutton.SetActive (false);
                Nobutton.SetActive (false);
                Exitbutton.SetActive (true);
                break;

            case 4: //No resources
                Textbox.text = Conversation[4];
                anim.SetTrigger("Talk");
                Yesbutton.SetActive (false);
                Nobutton.SetActive (false);
                Exitbutton.SetActive (true);
                break;

        }
    }


    public void ButtonYes() {
        ConversationState++;
    }

    public void ButtonNo(){
        ConversationState = 3;
    }

    public void ButtonExit(){
        narrativeCanvas.DOFade(0,1f);
        GameObject.Find("Player").GetComponent<PlayerController>().Endinteracting();
        overallCanvas.enabled = false;
        ConversationState = 0;
    }

    public void TurnToPlayer(Transform playerPos)
    {
        Vector3 relativePos = playerPos.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }



}
