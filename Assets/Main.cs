using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    [Header("Set in Inspector")]
    public GameObject prefabEnemies;
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
        GameObject spawnedEn = Instantiate<GameObject>(prefabEnemies);

        float enemyPad = enemyPadding;
        if(spawnedEn.GetComponent<BoundsCheck>() != null)
        {
            enemyPad = Mathf.Abs(spawnedEn.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMinimum = -boundM.camWidth + enemyPad;
        float xMaximum = boundM.camWidth - enemyPad;
        pos.x = Random.Range(xMinimum, xMaximum);
        pos.y = boundM.camHeight + enemyPad;
        spawnedEn.transform.position = pos;

        Invoke("SpawnEnemy", 1f / enemySpawnRate);
    }
}
