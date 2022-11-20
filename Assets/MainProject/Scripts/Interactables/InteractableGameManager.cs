
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class InteractableGameManager : MonoBehaviour
{


    public GameObject [] RockModels;
    public int InteractableState = 0;

    public Canvas aimCanvas;
    public Slider aimSlider;
    private Tween aimTween;

    public Canvas powerCanvas;
    public Slider powerSlider;
    private Tween powerTween;


    private void Start() {
        
        //Select a rock
        int rockChoice = Random.Range (0, RockModels.Length);
        for (int i = 0; i < RockModels.Length; i++)
        {
            if (i == rockChoice) {
                RockModels[i].SetActive (true);
                RockModels[i].transform.rotation = Random.rotation;
            }
        }



    }

    public void StartTheGame() {
        aimCanvas.enabled = true;
        aimTween = aimSlider.DOValue(1f, 1f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        InteractableState = 1;
    }


    public void KillTheAimTween() {
        aimTween.Kill();
        aimCanvas.enabled = false;
        InteractableState = 2;
        powerCanvas.enabled = true;
        powerTween = powerSlider.DOValue(1f, 1f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);

    }

    public void KillThePowerTween() {
        powerTween.Kill();
        powerCanvas.enabled = false;
        InteractableState = 3;
        Destroy(this.gameObject.transform.parent.gameObject);
    }

}
