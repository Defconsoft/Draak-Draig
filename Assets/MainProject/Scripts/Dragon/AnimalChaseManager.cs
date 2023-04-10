using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalChaseManager : MonoBehaviour
{

    private GameManager gameManager;
    private UXManager uxManager;
    bool GameStarted;
    bool GameEnded;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uxManager = GameObject.Find("GameManager").GetComponent<UXManager>();    


        StartCoroutine (WaitforLoad());
    }


    IEnumerator WaitforLoad(){
        yield return new WaitForSeconds (5f);
        GameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameStarted == true){
        
            gameManager.EnergyAmount -= gameManager.acEnergyReductionAmount * Time.deltaTime;
            uxManager.EnergyBar.fillAmount = gameManager.EnergyAmount ;
        }



        if (gameManager.EnergyAmount <= 0.25 && GameEnded == false) {
            uxManager.BirdTowerAmount = 10;
            GameEnded = true;
            GameStarted = false;
            uxManager.LoadScene(7);
        }


    }


}
