using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Movement : Enemy_Parent
{
    public float speed;
    public float timeCounter = 0;

    //keyword new to hide update of parent class
    new void Update()
    {
        Move();
        timeCounter += Time.deltaTime;
        if (_bound != null && (_bound.offScreenDown || _bound.offScreenLeft || _bound.offScreenRight))
        {
            Destroy(gameObject);
        }
    }
    
    protected override void Move()
    {
        Vector3 temporaryPosition = position;
        temporaryPosition.x += Random.Range(0,2)*speed * (float)System.Math.Cos(timeCounter);
        temporaryPosition.y += -(float)System.Math.Abs(Random.Range(0, 2) * speed * (float)System.Math.Sin(timeCounter));
        position = temporaryPosition;
    }
}
