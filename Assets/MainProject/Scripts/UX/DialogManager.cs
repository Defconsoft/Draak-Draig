using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject[] dialogList;
    public GameObject[] buttonList;
    public Color normalColour;
    public Color selectedColour;

    public void OpenCustomizationDialog(int idx)
    {
        foreach (GameObject go in dialogList)
        {
            if (go == dialogList[idx])
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }

        foreach (GameObject go in buttonList)
        {
            TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
            Image image = go.GetComponentsInChildren<Image>()[1];
            Debug.Log(image.gameObject.name);

            if (go == buttonList[idx])
            {
                ChangeButtonColour(text, selectedColour);
                ChangeIconColour(image, selectedColour);
            }
            else
            {
                ChangeButtonColour(text, normalColour);
                ChangeIconColour(image, normalColour);
            }
        }
    }

    public void ChangeButtonColour(TextMeshProUGUI txt, Color col)
    {
        txt.color = col;
    }

    public void ChangeIconColour(Image img, Color col)
    {
        img.color = col;
    }
}
