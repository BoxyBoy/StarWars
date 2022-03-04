using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemySpawners;
    public GameObject squadSpawner;
    float spawnTimer = 0.2f;

    void Start()
    {
        
    }
    
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            // spawn an enemy at one of the spawn points, probably calling a
            // function called "SpawnEnemy()" or something that does that logic
        }
    }

    public void SpawnEnemy()
    {

    }
}
