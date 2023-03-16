using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> MinigameImpact;
    public static event Action ShakeCam;
    public static event Action<float> TargetHit;

    public static void TriggerImpact(int id)
    {
        MinigameImpact?.Invoke(id);
    }

    public static void ExplosionHappened()
    {
        ShakeCam?.Invoke();
    }

    public static void HitTarget(float targetValue)
    {
        TargetHit?.Invoke(targetValue);
    }
}
