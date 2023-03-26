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
    public Material eyesMat;
    public Material baseMat;
    public Material webbingMat;
    public Material platesMat;
    public Color webbingCol;
    public Color platesCol;
    public float startingHueEyes = 0f;
    public float startingSatEyes = 1f;
    public Texture startingTexEyes;
    public float startingHueBase = 0f;
    public float startingSatBase = 1f;
    public float hornSize = 0f;
    public float hornSqueeze = 0f;
    public float hornCurve = 0f;
    public float tailSize = 0f;
    public float tailSqueeze = 0f;
    public bool tailSpikeEnabled = true;



    [Header ("Health & Energy")]
    public float HealthAmount;
    public float EnergyAmount; 
    public float ForestSwoopReplenish;
    public float acEnergyReductionAmount;


    void Awake()
    {

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        eyesMat.SetFloat("_HueChange", startingHueEyes);
        eyesMat.SetFloat("_Saturation", startingSatEyes);
        eyesMat.SetTexture("_baseTexture", startingTexEyes);
        baseMat.SetFloat("_HueChange", startingHueBase);
        baseMat.SetFloat("_Saturation", startingSatBase);
        webbingMat.color = webbingCol;
        platesMat.color = platesCol;
    }


    public void MinusHealth(float amount){
        HealthAmount = HealthAmount - amount;
        NumberClean();
        Mathf.Clamp(HealthAmount, 0f, 1f);
        uxManager.HealthBar.fillAmount = HealthAmount;
    }

    public void PlusHealth(float amount){
        HealthAmount = HealthAmount + amount;
        NumberClean();
        Mathf.Clamp(HealthAmount, 0f, 1f);
        uxManager.HealthBar.fillAmount = HealthAmount;
    }

    public void MinusEnergy(float amount){
        EnergyAmount = EnergyAmount - amount;
        NumberClean();
        Mathf.Clamp(EnergyAmount, 0f, 1f);
        uxManager.EnergyBar.fillAmount = EnergyAmount;
    }

    public void PlusEnergy(float amount){
        EnergyAmount = EnergyAmount + amount;
        NumberClean();
        Mathf.Clamp(EnergyAmount, 0f, 1f);
        uxManager.EnergyBar.fillAmount = EnergyAmount;
    }

    public void NumberClean(){
        HealthAmount = Mathf.Round(HealthAmount *1000f) /1000f;
        EnergyAmount = Mathf.Round(EnergyAmount *1000f) /1000f;
    }




}











