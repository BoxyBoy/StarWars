using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Wave[] waves;
    public Enemy enemy;
    public event System.Action<int> OnNewWave;

    GameEntity playerEntity;
    Transform playerTransform;
    MapGenerator map;
    Wave currentWave;
    Vector3 previousCampingPosition;

    int currentWaveNumber;
    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;
    float timeBetweenCamping = 2f;
    float campingThresholdDistance = 1.5f;
    float nextCampingCheckTime;

    bool isCamping;
    bool isDisabled = false;

    private void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerEntity.OnDeath += OnPlayerDeath;
        playerTransform = playerEntity.transform;

        nextCampingCheckTime = Time.time + timeBetweenCamping;
        previousCampingPosition = playerTransform.position;

        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }

    private void Update()
    {
        if (isDisabled) return;

        if (Time.time > nextCampingCheckTime)
        {
            nextCampingCheckTime = Time.time + timeBetweenCamping;
            isCamping = (Vector3.Distance(playerTransform.position, previousCampingPosition) < campingThresholdDistance);

            previousCampingPosition = playerTransform.position;
        }

        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        float spawnTimer = 0f;
        float spawnDelay = 1f;
        float tileFlashSpeed = 4f;

        Transform randomOpenTile = map.GetRandomOpenTile();
        if (isCamping)
        {
            randomOpenTile = map.GetTileFromPosition(playerTransform.position);
        }

        Material tileMaterial = randomOpenTile.GetComponent<Renderer>().material;
        Color originalColor = tileMaterial.color;
        Color flashColor = Color.red;

        while (spawnTimer < spawnDelay)
        {
            tileMaterial.color = Color.Lerp(originalColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1f));

            spawnTimer += Time.deltaTime;
            yield return null;
        }
        

        Enemy spawnedEnemy = Instantiate(enemy, randomOpenTile.position, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    private void OnPlayerDeath()
    {
        isDisabled = true;
    }

    private void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    private void ResetPlayerPosition()
    {
        playerTransform.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    private void NextWave()
    {
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }
            ResetPlayerPosition();
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
