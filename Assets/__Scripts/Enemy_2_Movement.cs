using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Movement : Enemy_Parent
{
    public float speed;
    public float timeCounter = 0;
    private int _directionFlag;
    private int _count = 0;
    private float _slide;
    private float _timeCounter = 0;

    void Start()
    {
        _slide = Random.Range(15, 20);
        _directionFlag = 0;
    }

    //keyword new to hide update of parent class
    new void Update()
    {
        _timeCounter += 4*Time.deltaTime;
        Move();
        timeCounter += Time.deltaTime;
        if (_bound != null && (_bound.offScreenDown || _bound.offScreenLeft || _bound.offScreenRight))
        {
            Destroy(gameObject);
        }
    }
    
    protected override void Move()
    {
        //Vector3 temporaryPosition = position;
        /* if (_directionFlag == 0 || _directionFlag == 2)
         {
             temporaryPosition.y -= 10f * Time.deltaTime;
         }
         else if (_directionFlag == 1)
         {
             temporaryPosition.x += _slide * Time.deltaTime;
         }
         else if(_directionFlag == 3)
         {
             temporaryPosition.x -= _slide * Time.deltaTime;
         }
         position = temporaryPosition;
         _count++;
         if (_count >= 25)
         {
             _directionFlag = ++_directionFlag % 4;
             _count = 0;
         }*/
        Vector3 temporaryPosition = position;
        temporaryPosition.y -= 10f * Time.deltaTime;
        temporaryPosition.x -= _slide * (float)System.Math.Sin(_timeCounter) * Time.deltaTime;
        position = temporaryPosition;
    }
}
