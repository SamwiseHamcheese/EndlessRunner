using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [Tooltip("A refernce to the tile we want to spawn")]
    public Transform tile;

    [Tooltip("A reference to the obstacle we want to spawn")]
    public Transform obstacle;

    [Tooltip("Where the first tile should be placed at")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 15)]
    public int initSpwanNum = 10;

    [Tooltip("How many tiles to spawn with no obstacles")]
    public int initNoObstacles = 4;

    private Vector3 nextTileLocation;
    private Quaternion nextTileRotation;

    // Start is called before the first frame update
    void Start()
    {
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        for( int i = 0; i < initSpwanNum; i++)
        {
            SpawnNextTile(i >= initNoObstacles);
        }
    }
    public void SpawnNextTile(bool spawnObstacles = true)
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);

        var nextTile = newTile.Find("NextSpawnPoint");
        nextTileLocation = nextTile.transform.position;
        nextTileRotation = nextTile.transform.rotation;

        if (spawnObstacles)
        {
            SpawnObstacle(newTile);
        }
    }
    private void SpawnObstacle(Transform newTile)
    {
        var obstacleSpawnPoints = new List<GameObject>();

        foreach(Transform child in newTile)
        {
            if (child.CompareTag("ObstacleSpawn"))
            {
                obstacleSpawnPoints.Add(child.gameObject);
            }
        }
        if(obstacleSpawnPoints.Count > 0)
        {
            int index = Random.Range(0, obstacleSpawnPoints.Count);
            var spawnPoint = obstacleSpawnPoints[index];

            var spawnPos = spawnPoint.transform.position;

            var newObstacle = Instantiate(obstacle, spawnPos, Quaternion.identity);

            newObstacle.SetParent(spawnPoint.transform);
        }
    }
}
