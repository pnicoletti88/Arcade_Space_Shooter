using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    void Awake()
    {
        // Array of music items - if the length is greater than one, the new music gameobject is destroyed 
        // (the object is created each time the title screen is loaded, so this prevents multiple instances of the song object while also keeping the old one loaded).
        GameObject[] musicItems = GameObject.FindGameObjectsWithTag("Music");
        if (musicItems.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //Prevents the music gameobject from being destroyed as Scenes transition - this allows for uninterrupted music throughout the game.
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
