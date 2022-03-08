using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float spawnTimer = 1.25f;

    public GameObject[] enemyList;
    public GameObject[] enemySpawners;

    void Start()
    {
    }
    
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = 1.25f;
        }
    }

    // after an amount of time has passed, spawn an enemy from one of the points on the map
    public void SpawnEnemy()
    {
        int selectedSpawner = Random.Range(0, enemySpawners.Length - 1);
        Transform enemySpawnTransform = enemySpawners[selectedSpawner].transform;
        Instantiate(enemyList[Random.Range(0, enemyList.Length - 1)], enemySpawnTransform);
    }
}
