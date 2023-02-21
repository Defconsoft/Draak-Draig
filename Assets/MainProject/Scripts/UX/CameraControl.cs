using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    
    public CinemachineVirtualCamera[] cams;
    
    // Start is called before the first frame update
    public void RefocusCamera(int camIdx)
    {
        for (int i = 0; i < cams.Length; i++)
        {
            if (i == camIdx)
            {
                cams[i].m_Priority = 10;
            }
            else
            {
                cams[i].m_Priority = 1;
            }
        }
    }
}
