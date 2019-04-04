using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{

    [Header("Set in Inspector")]
    public GameObject player;
    public GameObject[] panels;
    public float scrollingSpeed = -30f;
    public float motionMult = 0.25f;

    private float _panelHeight;
    private float _panelDepth;

    // Start is called before the first frame update
    void Start()
    {
        _panelHeight = panels[0].transform.localScale.y;
        _panelDepth = panels[0].transform.position.z;
 

        // sets height and depth of the star panels
        panels[0].transform.position = new Vector3(0, 0, _panelDepth);
        panels[1].transform.position = new Vector3(0, _panelHeight, _panelDepth);
        
    }

    // Update is called once per frame
    void Update()
    {
        float tY, tX = 0;

        tY = Time.time * scrollingSpeed % _panelHeight + (_panelHeight * 0.5f);

        if(player != null && player.CompareTag("Hero"))
        {
            tX = -player.transform.position.x * motionMult;
        }

        panels[0].transform.position = new Vector3(tX, tY, _panelDepth);

        if(tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - _panelHeight, _panelDepth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + _panelHeight, _panelDepth);
        }
        
    }
}
