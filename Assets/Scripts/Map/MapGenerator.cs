using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;
    public int seed = 10;

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
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
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
                newTile.localScale = Vector3.one * (1f - outlinePercent);
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
                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
            }
            else
            {
                obstacleMap[randomCoordinate.x, randomCoordinate.y] = false;
                currentObstacleCount--;
            }
        }
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
