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

        // additional logic added to allow for star movement on any scene, even without the player - note that the 'player' item must be assigned in the inspector - however, if it is an object without the 'Hero' tag, this will not run. 
        if(player != null && player.CompareTag("Hero"))
        {
            // player != null logic comes before the CompareTag function call - if the first term evaluates false, then comparetag will not try and find the tag of an object that does not exist. && operator only evaluates the second condition in the event the first one is true.
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
