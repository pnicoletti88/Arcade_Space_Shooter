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
    

    void Update()
    {
        _imageExit = parentFade.exit;
        if(_imageExit && _imageOnce)
        {
            _imageOnce = false;
            _imageCoroutine = FadeImageToZeroAlpha(1f, GetComponent<Image>());
            StartCoroutine(_imageCoroutine);
        }
    }

    public IEnumerator FadeImageToZeroAlpha(float t, Image image )
    { 
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        while (image.color.a > 0.0f)
        {
            if (image.color.a - (Time.deltaTime / t) <= 0.0f)
            {
                StopCoroutine(_imageCoroutine);
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
