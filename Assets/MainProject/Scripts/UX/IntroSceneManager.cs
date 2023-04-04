using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IntroSceneManager : MonoBehaviour
{
    public TextMeshProUGUI[] paragraphs;
    public TextMeshProUGUI continueText;
    public float fadeSpeed = 2.0f;
    private Vector3 originalScale;
    private Vector3 scaleTo;
    public float dissolveRate = 0.02f;
    public float refreshRate = 0.05f;
    public Image storyBackground;
    private Material mat;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeTextToFullAlpha(paragraphs, fadeSpeed));
        StartCoroutine(FadeInContinueText(continueText, 1.0f));   
        originalScale = continueText.transform.localScale;
        scaleTo = 1.05f * originalScale;
        mat = storyBackground.material;
        mat.SetFloat("DissolveAmount_", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) {
            StartDissolving();
        }
    }

    public IEnumerator FadeTextToFullAlpha(TextMeshProUGUI[] txts, float t)
    {
        foreach(TextMeshProUGUI txt in txts)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
            while (txt.color.a < 1.0f)
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }
    }

    public IEnumerator FadeInContinueText(TextMeshProUGUI txt, float t)
    {
        yield return new WaitForSeconds(5f);
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        while (txt.color.a < 1.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime / t));
            yield return null;
        }
        OnScale();
    }

    private void OnScale()
    {
        continueText.transform.DOScale(scaleTo, 2.0f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StartDissolving()
    {
        StartCoroutine(DissolveAndLoad());
    }

    IEnumerator DissolveAndLoad()
    {
        // GetComponent<AudioSource>().Play();
        float counter = 0f;
        while(mat.GetFloat("DissolveAmount_") < 0.7f)
        {
            counter += dissolveRate;
            mat.SetFloat("DissolveAmount_", counter);            
            yield return new WaitForSeconds(refreshRate);
        }
        SceneManager.LoadScene(1);
    }
}
