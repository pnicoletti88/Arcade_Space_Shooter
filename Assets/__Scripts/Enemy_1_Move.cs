using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Move : MonoBehaviour
{
    public bool flag;
    // Start is called before the first frame update
    void Start()
    {
        flag = (Random.Range(0, 2) == 0);

    }
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
        if (flag)
        {
            temporaryPosition.x -= 10f * Time.deltaTime;
        }
        else
        {
            temporaryPosition.x += 10f * Time.deltaTime;
        }
        temporaryPosition.y -= 10f * Time.deltaTime;
        position = temporaryPosition;
    }
}
