using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Movement : MonoBehaviour
{
    public float speed;
    public float timeCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

 
    void Update()
    {
        Move();
        timeCounter += Time.deltaTime;
    }
    public Vector3 position
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    public void Move()
    {
        
        Vector3 temporaryPosition = position;
        temporaryPosition.x += Random.Range(0,2)*speed * (float)System.Math.Cos(timeCounter);
        temporaryPosition.y += -Random.Range(0, 2)*speed * (float)System.Math.Sin(timeCounter);
        position = temporaryPosition;
    }
}
