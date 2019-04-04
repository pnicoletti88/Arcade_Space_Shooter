using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    void Awake()
    {
        GameObject[] musicItems = GameObject.FindGameObjectsWithTag("Music");
        if (musicItems.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
