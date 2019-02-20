using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Script : MonoBehaviour
{
    public static Hero_Script S;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float radius = 1f;

    [Header("Set in Dynamically")]
    public float sheildLevel = 1;
    public float camWidth;
    public float camHeight;


    void Awake()
    {
        if (S == null)
        {
            S = this;
            camHeight = Camera.main.orthographicSize;
            camWidth = camHeight * Camera.main.aspect;
        }
        else
        {
            Debug.LogError("Attempted Creation of Second Hero");
        }
    }


    // Update is called once per frame
    void Update()
    {
        //these method used input based on the user defined axis and return a value between 1- and 1 depending on which direction is push
        //starts at 0 and builds to 1 the longer you hold it
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //handles moving
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //handles ship tilt
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        if (pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
        }else if(pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
        }

        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
        }
        else if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
        }

        transform.position = pos;
    }
}
