using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [Header ("Timer Stuff")]
    public float DaytimeTimerAmount;

    [Header ("Movement Stuff")]
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;


    [Header ("Resources stuff")]
    public int baseResourceAmount;
    public int totalWood;
    public int totalRock;
    public int totalFish;






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






}











