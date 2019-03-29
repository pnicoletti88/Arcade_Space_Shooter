using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_TitleScreen : MonoBehaviour
{
    static public Main_TitleScreen scriptReference; //Singleton
    
    void Awake()
    {
        if (scriptReference == null)
        {
            scriptReference = this; //sets up singleton so that only 1 main script can be created.
        }
        else
        {
            Debug.LogError("Attempted Creation of Second Main Script (Title Screen)");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
