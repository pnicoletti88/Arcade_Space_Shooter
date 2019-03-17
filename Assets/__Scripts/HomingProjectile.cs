using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLEASE IGNORE THIS FILE - it is for phase 3 and was just done now for fun
//It is not implemented in the game right now as there is no way to switch to homing missile in game

/*
 * Purpose: This class controls the homing missles
 * It extends the Porjectile class - allowing the projectiles to be made in the same way
*/

public class HomingProjectile : Projectile
{

    private GameObject _targetEnemy;
    // Start is called before the first frame update
    protected override void Start()
    {
        _targetEnemy = Main.scriptReference.getClosestsEnemy();
        _bounds = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    protected override void Update() 
    {
        if (!_bounds.onScreen) //destroy projectile when off screen
        {
            Destroy(gameObject);
        }
        //first make sure that there is an enemy to target - if not find one
        if (_targetEnemy == null)
        {
            _targetEnemy = Main.scriptReference.getClosestsEnemy();
            //note after getting target missile will not target it until next frame
            //this is done to keep frame rate high as getting a target and then aiming at it are both slow operations
        }
        else //if missle has target make it aim towards it - updating aim each frame
        { 
            Vector3 enemyLocation = _targetEnemy.transform.position;
            float deltaX = enemyLocation.x - transform.position.x;
            float deltaY = enemyLocation.y - transform.position.y;

            //use arctan to get angle using deltaX and Y between missile and enemy
            float angle = Mathf.Atan(deltaX / deltaY);
            angle = angle / (2 * Mathf.PI) * 360; //convert from radians to degrees

            //flip angle is missle is above enemy
            if (deltaY < 0)
            {
                angle -= 180;
            }

            //update angle and velocity of ship
            //SPEED HAS BEEN HARDCODED - change this?
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            GetComponent<Rigidbody>().velocity = transform.rotation * (Vector3.up * 40f);

        }
    }
}
