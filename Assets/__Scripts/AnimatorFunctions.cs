using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    public MenuButtonController menuButtonController;
    
    void PlaySound(AudioClip sound)
    {
        menuButtonController.audioSource.PlayOneShot(sound);
    }
}
