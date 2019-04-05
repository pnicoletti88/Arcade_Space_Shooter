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

    // this functiom instantiates the explosion game object, sets its postion and increases the count by one
    void MakeBoom()
    {
        GameObject boom = Instantiate(explosion);
        boom.transform.position = transform.position;
        _boomCount++;
    }

    //calls DeleteAllEnemies function from main scene when moab comes and restarts the spawning in SpawnEnemy function
    void DestroyRemainingEnemy()
    {
        Main_MainScene.scriptReference.DeleteAllEnemies();
        Main_MainScene.scriptReference.spawnEnemies = true;
        Destroy(gameObject);
    }

    // Update is called once per frame
    //Stops spawning using spawnEnemies field
    //Instatiates 5 explosions for visual effect
    //SetActive to false to hide the game object after everything is destroyed
    protected override void Update()
    {
        transform.Rotate(0, 5f, 0);
        if (Time.time - _startTime > 1.5f && _boomCount == 0) 
        {
            Main_MainScene.scriptReference.spawnEnemies = false;
            MakeBoom();
            Invoke("MakeBoom", 0.03f);
            Invoke("MakeBoom", 0.06f);
            Invoke("MakeBoom", 0.09f);
            Invoke("MakeBoom", 0.12f);
            Invoke("DestroyRemainingEnemy", 0.2f);
            gameObject.SetActive(false);

        }
        base.Update();
    }
}
