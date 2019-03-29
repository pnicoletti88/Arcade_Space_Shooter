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
    private float _degreePerFrame = 2.7f;

    protected override void Start()
    {
        _targetEnemy = Main_MainScene.scriptReference.getClosestsEnemy();
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
            _targetEnemy = Main_MainScene.scriptReference.getClosestsEnemy();
            //note after getting target missile will not target it until next frame
            //this is done to keep frame rate high as getting a target and then aiming at it are both slow operations
        }
        else //if missle has target make it aim towards it - updating aim each frame
        { 
            Vector3 enemyLocation = _targetEnemy.transform.position;
            float deltaX = enemyLocation.x - transform.position.x;
            float deltaY = enemyLocation.y - transform.position.y;

            Vector2 targetDir = new Vector2(deltaX, deltaY);

            //use arctan to get angle using deltaX and Y between missile and enemy
             //convert from radians to degrees

            //flip angle is missle is above enemy
            

            //update angle and velocity of ship
            //SPEED HAS BEEN HARDCODED - change this?

            Vector3 vel = GetComponent<Rigidbody>().velocity;
            Vector2 currDir = new Vector2(vel.x, vel.y);

            float targetAngle = getAngle(targetDir);
            float currentAngle = getAngle(currDir);
            float finalAngle = targetAngle;

            if (targetAngle - currentAngle < 180.0f && targetAngle - currentAngle > 0.0f)
            {
                finalAngle = Mathf.Min(targetAngle, currentAngle + _degreePerFrame);
            }
            else if(currentAngle - targetAngle < 180.0f && currentAngle - targetAngle > 0.0f)
            {
                finalAngle = Mathf.Max(targetAngle, currentAngle - _degreePerFrame);
            }
            else if(targetAngle + 360.0f - currentAngle < 180.0f && targetAngle + 360.0f - currentAngle > 0.0f)
            {
                finalAngle = Mathf.Min(targetAngle+360, currentAngle + _degreePerFrame);
            }
            else if (currentAngle + 360.0f - targetAngle < 180.0f && currentAngle+360.0f - targetAngle > 0.0f)
            {
                finalAngle = Mathf.Max(targetAngle, currentAngle - _degreePerFrame + 360.0f);
            }



            transform.rotation = Quaternion.AngleAxis(finalAngle, Vector3.back);
            GetComponent<Rigidbody>().velocity = transform.rotation * (Vector3.up * 40f);

        }
    }
    private float getAngle(Vector2 vector)
    {
        float angle = Mathf.Atan(vector.x / vector.y);
        angle = angle / (2 * Mathf.PI) * 360;

        if (vector.x >= 0 && vector.y >= 0)
        {
            return angle;
        }
        else if (vector.x < 0 && vector.y >= 0)
        {
            return angle+360;
        }
        else if (vector.x >= 0 && vector.y < 0)
        {
            return angle+180;
        }
        else if (vector.x < 0 && vector.y < 0)
        {
            return angle+180;
        }
        return 0;
    }
}
