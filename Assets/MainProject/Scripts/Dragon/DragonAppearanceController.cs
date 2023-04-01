using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAppearanceController : MonoBehaviour
{
    private GameManager gameManager;
    public SkinnedMeshRenderer horns;
    public SkinnedMeshRenderer tail;
    public Material glowMat;
    public Material dragonMat;
    public float glowRate = 1f;
    public float refreshRate = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        horns.SetBlendShapeWeight(1, gameManager.hornSqueeze);
        horns.SetBlendShapeWeight(0, gameManager.hornSize);
        horns.SetBlendShapeWeight(2, gameManager.hornCurve);
        tail.gameObject.SetActive(gameManager.tailSpikeEnabled);
        tail.SetBlendShapeWeight(0, gameManager.tailSqueeze);
        tail.SetBlendShapeWeight(1, gameManager.tailSize);
        glowMat.SetFloat("_HueChange", dragonMat.GetFloat("_HueChange"));
        glowMat.SetFloat("_GlowPower", 200f);
    }

    public void Glow()
    {
        StartCoroutine(StartGlow());
    }

    private IEnumerator StartGlow()
    {
        glowMat.SetFloat("_GlowPower", 70f);
        float counter = 70f;
        while(counter > 10f)
        {
            counter -= 3*glowRate;
            glowMat.SetFloat("_GlowPower", counter);            
            yield return new WaitForSeconds(refreshRate);
        }
        while(counter > 1f)
        {
            counter -= glowRate;
            glowMat.SetFloat("_GlowPower", counter);            
            yield return new WaitForSeconds(refreshRate);
        }
    }

    private void OnDisable()
    {
        glowMat.SetFloat("_GlowPower", 200f);
    }
}
