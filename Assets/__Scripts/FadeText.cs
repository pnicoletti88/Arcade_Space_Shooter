using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    // Script that allows all text UI elements to fade to zero alpha (become completely transparent) before a scene transition - this makes the transition look better and less abrupt.
    public bool exit = false;
    public bool isMenu;
    public LoadScene sceneLoader;
    private IEnumerator _coroutine;
    protected bool _once = true;

    void Update()
    {
        if (isMenu)
        {
            exit = sceneLoader.exitScene; // if the object is a plain UI Text element, then exit can simply be set from a reference to the sceneLoader singleton.
        }
        if (exit && _once)
        {
            _once = false;  // prevents the coroutine from running multiple times (e.g. if the user held down the 'submit' button, etc. This way, the UI Elements only fade to zero alpha once.
            if (isMenu)
            { 
                Invoke("WaitBeforeFade", 2.0f); // if the object is a plain UI text element, then Invoke forces the Coroutine to wait 2 seconds to match up with the menubutton coroutine, which is called immediately.
            }
            else
            {
                WaitBeforeFade(); //If the object is a menubutton with animations, the coroutine to fade is called immediately - the coroutine on MenuButton has a wait function for 2 seconds, to ensure the completion of all animations before disabling the animator that controls them.
            }
        }
    }

    public void WaitBeforeFade()
    {
        // calls the corutine to fade the text away before loading a new scene
        _coroutine = FadeTextToZeroAlpha(1f, GetComponent<Text>());
        StartCoroutine(_coroutine);
    }

    public IEnumerator FadeTextToZeroAlpha(float rate, Text text)
    {
        //sets the text colour to its current colour, with 100% alpha
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            if (text.color.a - (Time.deltaTime / rate) <= 0.0f)
            {
                StopCoroutine(_coroutine); // if the alpha would become negative, then the coroutine is stopped before this occurs.
            }
            // alpha element of the UI object's colour is decreased with time, until it hits 0 and is completely transparent (or very close, depending on the rate variable).
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / rate));
            yield return null;
        }
    }
}
