using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExplosionController : MonoBehaviour
{

    public float lifeTime; //this is how long the explosion is alive for

    private float _startTime; //holds the time that the system starts
    
 
    void Start()
    {
        _startTime = Time.time; //set to start time
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime < Time.time - _startTime)//if object has been alive for longer than life destroy it (animation is over)
        {
            Destroy(gameObject);
        }   
    }
}
