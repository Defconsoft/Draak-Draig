    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
     
    public class FirstButton : MonoBehaviour
    {
     
        // Start is called before the first frame update
        void Start()
        {
            GameObject.Find("UI").GetComponent<CustomizationUIManager>().OpenCustomizationDialog(0);
        }
     
     
    }
