using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    [Header("Set in Inspector")]
    public GameObject Enemy_0, Enemy_1, Enemy_2;
    public float enemySpawnRate = 1;
    public float enemyPadding = 1.5f;

    private BoundsCheck boundM;

    void Awake()
    {
        S = this;
        boundM = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnRate);
    }

    public void SpawnEnemy()
    {
        int n = 0; //Random.Range(0, prefabEnemies.Length);
        GameObject spawnedEn_0 = Instantiate<GameObject>(Enemy_0);
        GameObject spawnedEn_1 = Instantiate<GameObject>(Enemy_1);
        GameObject spawnedEn_2 = Instantiate<GameObject>(Enemy_2);

        /*
         * This is for enemy 0
         */
        float enemyPad = enemyPadding;
        if(spawnedEn_0.GetComponent<BoundsCheck>() != null)
        {
            enemyPad = Mathf.Abs(spawnedEn_0.GetComponent<BoundsCheck>().radius);
        }
        /*
         * This is for enemy 1
         */
        if (spawnedEn_1.GetComponent<BoundsCheck>() != null)
        {
            enemyPad = Mathf.Abs(spawnedEn_1.GetComponent<BoundsCheck>().radius);
        }
        /*
         * This is for enemy 2
         */
        if (spawnedEn_2.GetComponent<BoundsCheck>() != null)
        {
            enemyPad = Mathf.Abs(spawnedEn_2.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;
        float xMinimum = -boundM.camWidth + enemyPad;
        float xMaximum = boundM.camWidth - enemyPad;
        //Enemy_0
        pos.x = Random.Range(xMinimum, xMaximum);
        pos.y = boundM.camHeight + enemyPad;
        spawnedEn_0.transform.position = pos;
        //Enemy_1
        pos.x = Random.Range(xMinimum, xMaximum);
        pos.y = boundM.camHeight + enemyPad;
        spawnedEn_1.transform.position = pos;
        //Enemy_2
        pos.x = Random.Range(xMinimum, xMaximum);
        pos.y = boundM.camHeight + enemyPad;
        spawnedEn_2.transform.position = pos;



        Invoke("SpawnEnemy", 1f / enemySpawnRate);
    }
}
