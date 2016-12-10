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

    public void GenerateMap()
    {
        tileCoordinates = GenerateTileCoordinates(mapSize.x, mapSize.y);
        shuffledTileCoordinates = new Queue<Coordinate>(Shuffle.ShuffleArray(tileCoordinates.ToArray(), seed));

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

        int obstacleCount = 10;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coordinate randomCoordinate = GetRandomCoordinate();
            Vector3 obstaclePosition = CoordinateToPosition(randomCoordinate.x, randomCoordinate.y);
            Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
            newObstacle.parent = mapHolder;
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
    }
}
