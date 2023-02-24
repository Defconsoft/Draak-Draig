using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomizationUIManager : MonoBehaviour
{
    public GameObject[] dialogList;
    public GameObject[] buttonList;
    public Color normalColour;
    public Color selectedColour;
    public ShaderCustomization[] shaderControls;
    public ApplyColor[] colorControls;
    public BlendKeySlider[] shapeControls;
    private GameObject gameManager;

    public void Start(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameManager = GameObject.Find("GameManager");
    }

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

    public void SaveButton()
    {
        // TODO send values to game manager
        gameManager.GetComponent<UXManager>().LoadScene(1);
    }

    public void CancelButton()
    {
        foreach (ShaderCustomization shader in shaderControls)
        {
            shader.ResetValues();
        }
        foreach (ApplyColor color in colorControls)
        {
            color.ResetColor();
        }
        foreach (BlendKeySlider shape in shapeControls)
        {
            shape.ResetShapes();
        }
        gameManager.GetComponent<UXManager>().LoadScene(1);
        
    }
}
