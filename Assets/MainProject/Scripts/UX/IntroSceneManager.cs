using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroSceneManager : MonoBehaviour
{
    public TextMeshProUGUI[] paragraphs;
    public TextMeshProUGUI continueText;
    public float fadeSpeed = 2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeTextToFullAlpha(paragraphs, fadeSpeed));
        StartCoroutine(FadeInContinueText(continueText, 1.0f));   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) {
            SceneManager.LoadScene(1);
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
        yield return new WaitForSeconds(2f);
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        while (txt.color.a < 1.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
}
