using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform navMeshFloor;
    public Transform navMeshMaskPrefab;

    public Vector2 mapSize;
    public Vector2 maxMapSize;

    public int seed = 10;
    public float tileSize = 1f;

    [Range(0, 1)]
    public float outlinePercent;

    [Range(0, 1)]
    public float obstaclePercent;

    Coordinate mapCentre;
    List<Coordinate> tileCoordinates;
    Queue<Coordinate> shuffledTileCoordinates;

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
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y) * tileSize;
    }

    private bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] visited = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coordinate> queue = new Queue<Coordinate>();

        queue.Enqueue(mapCentre);
        visited[mapCentre.x, mapCentre.y] = true;

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
        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);

        return targetAccessibleTileCount == accessibleTileCount;
    }

    public void GenerateMap()
    {
        tileCoordinates = GenerateTileCoordinates(mapSize.x, mapSize.y);
        shuffledTileCoordinates = new Queue<Coordinate>(Shuffle.ShuffleArray(tileCoordinates.ToArray(), seed));

        mapCentre = new Coordinate((int)(mapSize.x / 2), (int)(mapSize.y / 2));

        string holderName = "Generated Map";
        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordinateToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1f - outlinePercent) * tileSize;
                newTile.parent = mapHolder;
            }
        }

        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
        int currentObstacleCount = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            Coordinate randomCoordinate = GetRandomCoordinate();
            obstacleMap[randomCoordinate.x, randomCoordinate.y] = true;
            currentObstacleCount++;

            if (randomCoordinate != mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                Vector3 obstaclePosition = CoordinateToPosition(randomCoordinate.x, randomCoordinate.y);
                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * tileSize / 2, Quaternion.identity) as Transform;
                newObstacle.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newObstacle.parent = mapHolder;
            }
            else
            {
                obstacleMap[randomCoordinate.x, randomCoordinate.y] = false;
                currentObstacleCount--;
            }
        }

        Transform navMeshMaskLeft = Instantiate(navMeshMaskPrefab, Vector3.left * (mapSize.x + maxMapSize.x) / 4 * tileSize, Quaternion.identity) as Transform;
        navMeshMaskLeft.parent = mapHolder;
        navMeshMaskLeft.localScale = new Vector3((maxMapSize.x - mapSize.x) / 2, 1f, mapSize.y) * tileSize;

        Transform navMeshMaskRight = Instantiate(navMeshMaskPrefab, Vector3.right * (mapSize.x + maxMapSize.x) / 4 * tileSize, Quaternion.identity) as Transform;
        navMeshMaskRight.parent = mapHolder;
        navMeshMaskRight.localScale = new Vector3((maxMapSize.x - mapSize.x) / 2, 1f, mapSize.y) * tileSize;

        Transform navMeshMaskTop = Instantiate(navMeshMaskPrefab, Vector3.forward * (mapSize.y + maxMapSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
        navMeshMaskTop.parent = mapHolder;
        navMeshMaskTop.localScale = new Vector3(maxMapSize.x, 1f, (maxMapSize.y - mapSize.y) / 2) * tileSize;

        Transform navMeshMaskBottom = Instantiate(navMeshMaskPrefab, Vector3.back * (mapSize.y + maxMapSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
        navMeshMaskBottom.parent = mapHolder;
        navMeshMaskBottom.localScale = new Vector3(maxMapSize.x, 1f, (maxMapSize.y - mapSize.y) / 2) * tileSize;

        navMeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y, 0) * tileSize;
    }

    public Coordinate GetRandomCoordinate()
    {
        Coordinate randomCoordinate = shuffledTileCoordinates.Dequeue();
        shuffledTileCoordinates.Enqueue(randomCoordinate);

        return randomCoordinate;
    }

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
}
