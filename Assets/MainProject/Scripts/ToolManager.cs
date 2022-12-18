using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public GameObject[] tools;

    public void Equip(int toolIdx)
    {
        // 0 = pick axe, 1 = wood axe
        tools[toolIdx].SetActive(true);
    }

    public void UnEquip(int toolIdx)
    {
        // 0 = pick axe, 1 = wood axe
        tools[toolIdx].SetActive(false);
    }
}
