using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    public bool exit = false;
    public bool isMenu;
    public LoadScene sceneLoader;
    private IEnumerator _coroutine;
    protected bool _once = true;

    void Update()
    {
        if (isMenu)
        {
            exit = sceneLoader.exitScene;
        }
        if (exit && _once)
        {
            _once = false;
            if (isMenu)
            { 
                Invoke("WaitBeforeFade", 2.0f);
            }
            else
            {
                WaitBeforeFade();
            }
        }
    }

    public void WaitBeforeFade()
    {
        
        _coroutine = FadeTextToZeroAlpha(1f, GetComponent<Text>());
        StartCoroutine(_coroutine);
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            if (text.color.a - (Time.deltaTime / t) <= 0.0f)
            {
                StopCoroutine(_coroutine);
            }
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
