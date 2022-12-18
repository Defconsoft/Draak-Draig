using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;



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











