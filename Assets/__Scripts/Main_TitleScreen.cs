using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_TitleScreen : MonoBehaviour
{
    // class sets up a singleton of the title screen's main script, however because this screen is mainly UI elements, this script is fairly empty. 
    // The script is included to prevent multiple instances of the scene, and add functionality if more complex processess need to be added.
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
}
