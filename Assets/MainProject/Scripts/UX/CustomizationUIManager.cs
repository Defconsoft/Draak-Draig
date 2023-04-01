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
    public bool tailEnabled = true;
    public Toggle tailToggle;
    private GameManager gameManager = null;



    public void Start(){

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        // Set starting values correctly
        shapeControls[0].SetShapes(
            gameManager.hornSize,
            gameManager.hornSqueeze,
            gameManager.hornCurve
        );
        shapeControls[1].SetShapes(
            gameManager.tailSize,
            gameManager.tailSqueeze
        );
        tailToggle.isOn = gameManager.tailSpikeEnabled;
        tailEnabled = gameManager.tailSpikeEnabled;

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
        List<float> hornValues = shapeControls[0].GetShapes();
        List<float> tailValues = shapeControls[1].GetShapes();
        gameManager.hornSize = hornValues[0];
        gameManager.hornSqueeze = hornValues[1];
        gameManager.hornCurve = hornValues[2];
        gameManager.tailSize = tailValues[0];
        gameManager.tailSqueeze = tailValues[1];
        gameManager.tailSpikeEnabled = tailEnabled; 
        gameManager.GetComponent<UXManager>().LoadScene(3);
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

    public void SetTailActive(System.Boolean isActive)
    {
        tailEnabled = isActive;
    }
}
