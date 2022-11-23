using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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


