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
    private BoundsCheck bound;

    void Awake()
    {
        bound = GetComponent<BoundsCheck>();
    }

    void Update()
    {
        Move();
        timeCounter += Time.deltaTime;
        if (bound != null && bound.offScreenDown)
        {
            Destroy(gameObject);
        }
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
        temporaryPosition.y += -(float)System.Math.Abs(Random.Range(0, 2) * speed * (float)System.Math.Sin(timeCounter));
        position = temporaryPosition;
    }
}
