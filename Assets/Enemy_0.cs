using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
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
        temporaryPosition.y -= 10f * Time.deltaTime;
        position = temporaryPosition;
    }
}
