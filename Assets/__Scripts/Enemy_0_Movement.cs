using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0_Movement : MonoBehaviour
{

    [Header("Set in Inspector: Enemy")]


    private BoundsCheck bound;

    void Awake()
    {
        bound = GetComponent<BoundsCheck>();
    }


    // Update is called once per frame
    void Update()
    {
        Move();
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
        temporaryPosition.y -= 10f * Time.deltaTime;
        position = temporaryPosition;
    }
}
