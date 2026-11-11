
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private float spawnAheadDistance = 28f;
    [SerializeField] private float spawnBehindDistance = 12f;
    [SerializeField] private float firstSpawnDistance = 8f;
    [SerializeField] private float rowSpacing = 5f;

    [Header("Track")]
    [SerializeField] private float trackHalfWidth = 1.2f;
    [SerializeField] private int laneCount = 3;
    [SerializeField] private int maxObstaclesPerRow = 2;

    private Transform player;
    private GameObject obstaclePrefab;
    private Transform obstaclesRoot;

    private float nextSpawnY;

    [Inject]
    private void Construct(
        [Inject(Id = "PlayerTransform")] Transform playerTransform,
        [Inject(Id = "ObstaclePrefab")] GameObject obstaclePrefab,
        [Inject(Id = "ObstaclesRoot")] Transform obstaclesRoot)
    {
        this.player = playerTransform;
        this.obstaclePrefab = obstaclePrefab;
        this.obstaclesRoot = obstaclesRoot;
    }

    private void Start()
    {
        nextSpawnY = player.position.y + firstSpawnDistance;
    }

    private void Update()
    {
        SpawnAhead();
        RemoveOldObstacles();
    }

    private void SpawnAhead()
    {
        float spawnLimit = player.position.y + spawnAheadDistance;

        while (nextSpawnY < spawnLimit)
        {
            SpawnRow(nextSpawnY);
            nextSpawnY += rowSpacing;
        }
    }

    private void SpawnRow(float yPosition)
    {
        List<int> availableLanes = new List<int>();

        for (int i = 0; i < laneCount; i++)
        {
            availableLanes.Add(i);
        }

        int count = Random.Range(
            1,
            Mathf.Clamp(maxObstaclesPerRow, 1, laneCount) + 1
        );

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableLanes.Count);
            int lane = availableLanes[randomIndex];

            availableLanes.RemoveAt(randomIndex);

            float xPosition = laneCount == 1
                ? 0f
                : Mathf.Lerp(
                    -trackHalfWidth,
                    trackHalfWidth,
                    (float)lane / (laneCount - 1)
                );

            Vector3 spawnPosition = new Vector3(
                xPosition,
                yPosition,
                0f
            );

            Instantiate(
                obstaclePrefab,
                spawnPosition,
                Quaternion.identity,
                obstaclesRoot
            );
        }
    }

    private void RemoveOldObstacles()
    {
        for (int i = obstaclesRoot.childCount - 1; i >= 0; i--)
        {
            Transform obstacle = obstaclesRoot.GetChild(i);

            if (obstacle.position.y <
                player.position.y - spawnBehindDistance)
            {
                Destroy(obstacle.gameObject);
            }
        }
    }
}
