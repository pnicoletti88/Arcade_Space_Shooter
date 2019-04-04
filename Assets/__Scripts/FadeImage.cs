using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : FadeText
{
    public FadeText parentFade;
    private IEnumerator _imageCoroutine;
    private bool _imageOnce = true;
    private bool _imageExit = false;
    
    //inherits from FadeText UI class, with minor updates to work for an image.
    void Update()
    {
        // reference to a parent class object limits the need for MenuButton to have a reference every time to a FadeImage script - the only UI image that needs this is the twitter button, so a more robust system is unneccessary, if not counterproductive.
        _imageExit = parentFade.exit; 
        if(_imageExit && _imageOnce)
        {
            _imageOnce = false;
            _imageCoroutine = FadeImageToZeroAlpha(1f, GetComponent<Image>());
            StartCoroutine(_imageCoroutine);
        }
    }

    // similar to parent class fade function, however this is adapted to an image UI object.
    public IEnumerator FadeImageToZeroAlpha(float rate, Image image )
    { 
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        while (image.color.a > 0.0f)
        {
            if (image.color.a - (Time.deltaTime / rate) <= 0.0f)
            {
                StopCoroutine(_imageCoroutine);
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime / rate));
            yield return null;
        }
    }
}
