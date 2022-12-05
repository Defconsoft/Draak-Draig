using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public GameObject[] tools;

    public void EquipPickAxe()
    {
        tools[0].SetActive(true);
    }

    public void EquipAxe()
    {
        tools[1].SetActive(true);
    }

    public void UnEquipPickAxe()
    {
        tools[0].SetActive(false);
    }

    public void UnEquipAxe()
    {
        tools[1].SetActive(false);
    }
}
