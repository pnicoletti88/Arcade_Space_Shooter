using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Set in Dynamically")]
    public bool onScreen = true;
    public float camWidth;
    public float camHeight;

    [HideInInspector]
    public bool offScreenRight, offScreenLeft, offScreenUp, offScreenDown;

    public bool boundsCheckActive = true;

    //determine cam width and height - used for bounds
    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    //late update used as it will check if something is out of bounds after it moves - avoid race conditions
    void LateUpdate()
    {
        if (boundsCheckActive)
        {
            Vector3 pos = transform.position;
            onScreen = true;
            offScreenRight = offScreenLeft = offScreenDown = offScreenUp = false;

            //first if-else block determines off screen right or left
            if (pos.x > camWidth - radius)
            {
                pos.x = camWidth - radius;
                onScreen = false;
                offScreenRight = true;
            }
            else if (pos.x < -camWidth + radius)
            {
                pos.x = -camWidth + radius;
                onScreen = false;
                offScreenLeft = true;
            }

            //second else-if block determines off screen top or bottom
            if (pos.y > camHeight - radius)
            {
                pos.y = camHeight - radius;
                onScreen = false;
                offScreenUp = true;
            }
            else if (pos.y < -camHeight + radius)
            {
                pos.y = -camHeight + radius;
                onScreen = false;
                offScreenDown = true;
            }

            //this will transform the position of an object to put it back on screen, if desired.
            if (keepOnScreen && !onScreen)
            {
                transform.position = pos;
                onScreen = true;
                offScreenRight = offScreenLeft = offScreenDown = offScreenUp = false;
            }
        }
    }


    //function given by prof to draw wire cube in gizmos
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }

}
