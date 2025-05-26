using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("A reference to the tile we want to spawn")]
    public Transform tile;
    [Tooltip("A reference to the obstacle we want to spawn")]
    public Transform obstacle;

    [Tooltip("Where the first tile should be placed at")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 15)]
    public int initSpawnNum = 10;
    [Tooltip("How many tiles to spawn in with no obstaccles")]
    public int initNoObstacles = 4;

    private Vector3 nextTileLocation;
    private Quaternion nextTileRotation;
    // Start is called before the first frame update
    void Start()
    {
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        for (int i = 0; i < initSpawnNum; i++)
        {
            SpawnNextTile(i >= initNoObstacles);
        }
    }
    public void SpawnNextTile(bool spawnObstacles = true)
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);

        var nextTile = newTile.Find("NewSpawnPoint");
        nextTileLocation = nextTile.transform.position;
        nextTileRotation = nextTile.rotation;
        if(spawnObstacles )
        {
            SpawnObstacle(newTile);
        }
    }
    private void SpawnObstacle(Transform newTile)
    {
        var obstaclesSpawnPoints = new List<GameObject>();
        foreach(Transform child in newTile)
        {
            if (child.CompareTag("ObstacleSpawn"))
            {
                obstaclesSpawnPoints.Add(child.gameObject);
            }
        }
        if(obstaclesSpawnPoints.Count > 0)
        {
            int index = Random.Range(0, obstaclesSpawnPoints.Count);
            var spawnPoint = obstaclesSpawnPoints[index];
            var spawnPos = spawnPoint.transform.position;
            var newObstacle = Instantiate(obstacle, spawnPos, Quaternion.identity);
            newObstacle.SetParent(spawnPoint.transform);
        }
    }
}
