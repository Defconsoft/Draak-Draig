using UnityEngine;
using DG.Tweening;

public class InteractableNPCManager : MonoBehaviour
{

    public Canvas overallCanvas;
    public CanvasGroup narrativeCanvas;
    public TMPro.TMP_Text Textbox;
    public int ConversationState;
    public GameObject Yesbutton, Nobutton, Exitbutton;
    
    
    [TextArea(5,10)]
    public string[] Conversation;


    public void StartConversation() {
        overallCanvas.enabled = true;
        narrativeCanvas.DOFade(1f,1f);
        Textbox.text = Conversation[0];
    }

    private void Update() {
        
        switch (ConversationState){
            case 0: //Start
                break;

            case 1: //Are you sure
                Textbox.text = Conversation[1];
                break;

            case 2: //SuccessExit
                Textbox.text = Conversation[2];
                Yesbutton.SetActive (false);
                Nobutton.SetActive (false);
                Exitbutton.SetActive (true);
                break;

            case 3: //FailExit
                Textbox.text = Conversation[3];
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
    }



}
