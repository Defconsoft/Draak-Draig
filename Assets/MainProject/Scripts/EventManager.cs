using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> MinigameImpact;

    public static void TriggerImpact(int id)
    {
        MinigameImpact?.Invoke(id);
    }
}
