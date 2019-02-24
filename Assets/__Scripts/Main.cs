using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main scriptReference; //What is the point of this? Singleton has not been set up.
    [Header("Set in Inspector")]
    public GameObject[] preFabEnemies = new GameObject[3];
    public float enemySpawnRate = 1;
    public float enemyPadding = 1.5f;

    private BoundsCheck boundM;

    void Awake()
    {
        scriptReference = this;
        boundM = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnRate);
    }

 
    public void SpawnEnemy()
    {
        int randEnemy = Random.Range(0, preFabEnemies.Length);
        GameObject spawned = Instantiate<GameObject>(preFabEnemies[randEnemy]);

        float enemyPad = enemyPadding;
        if(spawned.GetComponent<BoundsCheck>() != null)
        {
            enemyPad = Mathf.Abs(spawned.GetComponent<BoundsCheck>().radius);
        }

        Vector3 startPos = Vector3.zero;
        float xMinimum = -boundM.camWidth + enemyPad;
        float xMaximum = boundM.camWidth - enemyPad;
        

        startPos.x = Random.Range(xMinimum, xMaximum);
        startPos.y = boundM.camHeight + enemyPad;
        spawned.transform.position = startPos;
        
        Invoke("SpawnEnemy", 1f / enemySpawnRate);
    }
}
