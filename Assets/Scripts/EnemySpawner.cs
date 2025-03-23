using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float minSpawnInterval = 4f;
    [SerializeField]
    private float maxSpawnInterval = 6f;

    private float timeSinceLastSpawn;
    private float currentSpawnInterval;

    private void Start()
    {
        SpawnEnemy();
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= currentSpawnInterval)
        {
            SpawnEnemy();
        }
    }

    private void SetRandomSpawnInterval()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPosition = GetRandomEdgePosition();
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.Initialize(playerTransform);
        enemy.OnEnemyDestroyed.RemoveListener(OnEnemyDestroyed);
        enemy.OnEnemyDestroyed.AddListener(OnEnemyDestroyed);

        timeSinceLastSpawn = 0f;
        SetRandomSpawnInterval();
    }

    private void OnEnemyDestroyed()
    {
        timeSinceLastSpawn += Random.Range(minSpawnInterval, maxSpawnInterval) / 2;
    }

    private Vector2 GetRandomEdgePosition()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        int edge = Random.Range(0, 4);
        switch (edge)
        {
            case 0: // Top
                return new Vector2(Random.Range(-cameraWidth / 2, cameraWidth / 2), cameraHeight / 2);
            case 1: // Bottom
                return new Vector2(Random.Range(-cameraWidth / 2, cameraWidth / 2), -cameraHeight / 2);
            case 2: // Left
                return new Vector2(-cameraWidth / 2, Random.Range(-cameraHeight / 2, cameraHeight / 2));
            case 3: // Right
                return new Vector2(cameraWidth / 2, Random.Range(-cameraHeight / 2, cameraHeight / 2));
            default:
                return Vector2.zero;
        }
    }
}
