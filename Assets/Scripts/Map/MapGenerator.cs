using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Map[] maps;
    public int mapIndex;

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform navMeshFloor;
    public Transform navMeshMaskPrefab;
    public Vector2 maxMapSize;

    public float tileSize = 1f;

    [Range(0, 1)]
    public float outlinePercent;

    List<Coordinate> tileCoordinates;
    Queue<Coordinate> shuffledTileCoordinates;

    Map currentMap;

    private void Start()
    {
        GenerateMap();
    }

    private List<Coordinate> GenerateTileCoordinates(float width, float height)
    {
        List<Coordinate> coordinates = new List<Coordinate>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                coordinates.Add(new Coordinate(x, y));
            }
        }
        return coordinates;
    }

    private Vector3 CoordinateToPosition(int x, int y)
    {
        return new Vector3(-currentMap.mapSize.x / 2f + 0.5f + x, 0, -currentMap.mapSize.y / 2f + 0.5f + y) * tileSize;
    }

    private bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] visited = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coordinate> queue = new Queue<Coordinate>();

        queue.Enqueue(currentMap.mapCentre);
        visited[currentMap.mapCentre.x, currentMap.mapCentre.y] = true;

        int accessibleTileCount = 1;
        while (queue.Count > 0)
        {
            Coordinate tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++) {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;

                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!visited[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                visited[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coordinate(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }
        int targetAccessibleTileCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount);

        return targetAccessibleTileCount == accessibleTileCount;
    }

    public void GenerateMap()
    {
        currentMap = maps[mapIndex];
        System.Random randomGenerator = new System.Random(currentMap.seed);
        GetComponent<BoxCollider>().size = new Vector3(currentMap.mapSize.x * tileSize, 0.1f, currentMap.mapSize.y * tileSize);

        // Generating Coordinates
        tileCoordinates = GenerateTileCoordinates(currentMap.mapSize.x, currentMap.mapSize.y);
        shuffledTileCoordinates = new Queue<Coordinate>(Shuffle.ShuffleArray(tileCoordinates.ToArray(), currentMap.seed));

        // Creating Map Holder object
        string holderName = "Generated Map";
        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        // Spawning tiles
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                Vector3 tilePosition = CoordinateToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1f - outlinePercent) * tileSize;
                newTile.parent = mapHolder;
            }
        }

        // Spawning obstacles
        bool[,] obstacleMap = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];

        int obstacleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
        int currentObstacleCount = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            Coordinate randomCoordinate = GetRandomCoordinate();
            obstacleMap[randomCoordinate.x, randomCoordinate.y] = true;
            currentObstacleCount++;

            if (randomCoordinate != currentMap.mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight, currentMap.maxObstacleHeight, (float)randomGenerator.NextDouble());
                Vector3 obstaclePosition = CoordinateToPosition(randomCoordinate.x, randomCoordinate.y);

                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight / 2f, Quaternion.identity) as Transform;
                newObstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize);
                newObstacle.parent = mapHolder;

                Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);

                float colorPercent = randomCoordinate.y / (float)currentMap.mapSize.y;

                obstacleMaterial.color = Color.Lerp(currentMap.foregroundColor, currentMap.backgroundColor, colorPercent);
                obstacleRenderer.sharedMaterial = obstacleMaterial;
            }
            else
            {
                obstacleMap[randomCoordinate.x, randomCoordinate.y] = false;
                currentObstacleCount--;
            }
        }

        // Creating NavMesh Mask
        Transform navMeshMaskLeft = Instantiate(navMeshMaskPrefab, Vector3.left * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        navMeshMaskLeft.parent = mapHolder;
        navMeshMaskLeft.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1f, currentMap.mapSize.y) * tileSize;

        Transform navMeshMaskRight = Instantiate(navMeshMaskPrefab, Vector3.right * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        navMeshMaskRight.parent = mapHolder;
        navMeshMaskRight.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1f, currentMap.mapSize.y) * tileSize;

        Transform navMeshMaskTop = Instantiate(navMeshMaskPrefab, Vector3.forward * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        navMeshMaskTop.parent = mapHolder;
        navMeshMaskTop.localScale = new Vector3(maxMapSize.x, 1f, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;

        Transform navMeshMaskBottom = Instantiate(navMeshMaskPrefab, Vector3.back * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        navMeshMaskBottom.parent = mapHolder;
        navMeshMaskBottom.localScale = new Vector3(maxMapSize.x, 1f, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;

        navMeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y, 0) * tileSize;
    }

    public Coordinate GetRandomCoordinate()
    {
        Coordinate randomCoordinate = shuffledTileCoordinates.Dequeue();
        shuffledTileCoordinates.Enqueue(randomCoordinate);

        return randomCoordinate;
    }

    [System.Serializable]
    public struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate && this == (Coordinate)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Coordinate c1, Coordinate c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Coordinate c1, Coordinate c2)
        {
            return !(c1 == c2);
        }
    }

    [System.Serializable]
    public class Map
    {
        public Coordinate mapSize;
        [Range(0, 1)]
        public float obstaclePercent;
        public int seed;
        public float minObstacleHeight;
        public float maxObstacleHeight;
        public Color foregroundColor;
        public Color backgroundColor;

        public Coordinate mapCentre
        {
            get
            {
                return new Coordinate(mapSize.x / 2, mapSize.y / 2);
            }
        }
    }
}
