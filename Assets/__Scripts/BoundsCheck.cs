using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keep = true;

    [Header("Set in Dynamically")]
    public bool onScreen = true;
    public float camWidth;
    public float camHeight;

    [HideInInspector]
    public bool offScreenRight, offScreenLeft, offScreenUp, offScreenDown;

    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        onScreen = true;
        offScreenRight = offScreenLeft = offScreenDown = offScreenUp = false;

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
        if (keep && !onScreen)
        {
            transform.position = pos;
            onScreen = true;
            offScreenRight = offScreenLeft = offScreenDown = offScreenUp = false;
        }
    }
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
