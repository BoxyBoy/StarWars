using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public bool developerMode;

    public Wave[] waves;
    public Enemy[] enemies;
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

        if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            StartCoroutine("SpawnEnemy");
        }

        // Developer Mode Magic
        if (developerMode)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StopCoroutine("SpawnEnemy");

                foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    GameObject.Destroy(enemy.gameObject);
                }

                NextWave();
            }
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
            if (originalColor != flashColor)
            {
                tileMaterial.color = Color.Lerp(originalColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1f));
            } 

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        int enemyPrefabIndex = Random.Range(0, enemies.Length);
        Enemy spawnedEnemy = Instantiate(enemies[enemyPrefabIndex], randomOpenTile.position, Quaternion.identity) as Enemy;

        spawnedEnemy.transform.LookAt(playerTransform);
        spawnedEnemy.OnDeath += OnEnemyDeath;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColor);
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

            // When to start spawning enemies
            nextSpawnTime = Time.time + currentWave.levelStartDelay;
            nextCampingCheckTime = Time.time + currentWave.levelStartDelay;

            ResetPlayerPosition();
        }
    }

    [System.Serializable]
    public class Wave
    {
        public bool infinite;
        public int enemyCount;
        public float timeBetweenSpawns;

        public float moveSpeed;
        public int hitsToKillPlayer;
        public float enemyHealth;
        public Color skinColor;

        public float levelStartDelay = 3f;
    }
}
