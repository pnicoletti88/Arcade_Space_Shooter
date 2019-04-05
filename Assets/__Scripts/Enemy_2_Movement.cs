using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Movement : Enemy_Parent
{
    public float speed;
    private float _slide;
    private float _timeCounter = 0;

    void Start()
    {
        _slide = Random.Range(15, 20); //this determines the amplitude of sine wave
        _health = 95; // sets health from parent class to the appropriate value, depending on the enemy type.
        powerUpDropChance = 0.35f;
    }

    //keyword new to hide update of parent class
    protected override void Update()
    {
        _timeCounter += 4*Time.deltaTime; //time counter used for sin wave movement pattern
        base.Update();
    }
    
    //handles movement of the enemy
    protected override void Move()
    {
        Vector3 temporaryPosition = position;
        temporaryPosition.y -= 10f * Time.deltaTime * _speedFactor;
        temporaryPosition.x -= _slide * (float)System.Math.Sin(_timeCounter) * Time.deltaTime * _speedFactor; //sine function to control x position
        position = temporaryPosition; //update the position 
    }
}
