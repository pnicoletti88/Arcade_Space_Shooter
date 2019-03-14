using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Script : MonoBehaviour
{
    public static Hero_Script S;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;

    [Header("Set in Dynamically")]
    [SerializeField]
    private float _shieldLevel = 4;

    private GameObject lastTriggerGo = null;


    void Awake()
    {
        if (S == null)
        {
            S = this; //sets up singleton so that only 1 hero can be created.
        }
        else
        {
            Debug.LogError("Attempted Creation of Second Hero");
        }
    }


    // Update is called once per frame
    void Update()
    {
        //these method used input based on the user defined axis and return a value between 1- and 1 depending on which direction is push
        //starts at 0 and builds to 1 the longer you hold it
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //handles moving
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //handles ship tilt
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }

    //checks for collision between hero and enemies
    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (go == lastTriggerGo)
        {
            return;
        }
        //sets the last triggered game object to the "other" gameobject
        lastTriggerGo = go;
        //decreases shield level upon trigger with an enemy
        //destroys enemy
        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        else
        {
            print("Triggered by non-enemy: " + go.name);
        }
    }

    //shield level property allows _shieldLevel to be set as private and is only accessed through get and set
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            //sets shield level to minimum of the current value and 4
            _shieldLevel = Mathf.Min(value, 4);
            //destroys ship and restarts game when no shields remain on the hero
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.scriptReference.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
