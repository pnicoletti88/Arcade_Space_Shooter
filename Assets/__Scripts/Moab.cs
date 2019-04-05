using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moab : Projectile
{
    // Start is called before the first frame update

    public GameObject explosion;

    private float _startTime;
    private int _boomCount = 0;
    protected override void Start()
    {
        _startTime = Time.time;
        base.Start();
    }

    void makeBoom()
    {
        GameObject boom = Instantiate(explosion);
        boom.transform.position = transform.position;
        _boomCount++;
    }
    
    void DestroyRemainingEnemy()
    {
        Main_MainScene.scriptReference.DeleteAllEnemies();
        Main_MainScene.scriptReference.spawnEnemies = true;
        Destroy(gameObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        transform.Rotate(0, 5f, 0);
        if (Time.time - _startTime > 1.5f && _boomCount == 0) 
        {
            Main_MainScene.scriptReference.spawnEnemies = false;
            makeBoom();
            Invoke("makeBoom", 0.03f);
            Invoke("makeBoom", 0.06f);
            Invoke("makeBoom", 0.09f);
            Invoke("makeBoom", 0.12f);
            Invoke("DestroyRemainingEnemy", 0.2f);
            gameObject.SetActive(false);

        }
        base.Update();
    }
}
