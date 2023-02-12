using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UXManager uxManager;

    [Header ("Timer Stuff")]
    public float DaytimeTimerAmount;

    [Header ("Movement Stuff")]
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float dragonSpeed = 2.0f;
    public float dragonRotateSpeed = 2.0f;

    [Header ("Resources stuff")]
    public int baseResourceAmount;
    public int totalWood;
    public int totalRock;
    public int totalFish;


    [Header ("Dragon stuff")]
    public float EagleEyeAmount;


    [Header ("Health & Energy")]
    public float HealthAmount;
    public float EnergyAmount; 
    public float ForestSwoopReplenish;



    void Awake()
    {

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }


    public void MinusHealth(float amount){
        HealthAmount = HealthAmount - amount;
        Mathf.Clamp(HealthAmount, 0f, 1f);
        uxManager.HealthBar.fillAmount = HealthAmount;
    }

    public void PlusHealth(float amount){
        HealthAmount = HealthAmount + amount;
        Mathf.Clamp(HealthAmount, 0f, 1f);
        uxManager.HealthBar.fillAmount = HealthAmount;
    }

    public void MinusEnergy(float amount){
        EnergyAmount = EnergyAmount - amount;
        Mathf.Clamp(EnergyAmount, 0f, 1f);
        uxManager.EnergyBar.fillAmount = EnergyAmount;
    }

    public void PlusEnergy(float amount){
        EnergyAmount = EnergyAmount + amount;
        Mathf.Clamp(EnergyAmount, 0f, 1f);
        uxManager.EnergyBar.fillAmount = EnergyAmount;
    }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

}











