using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Transform player;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Area")]
    [SerializeField] private float spawnRadius = 50f;

    [Tooltip("Distancia mínima al jugador")]
    [SerializeField] private float safeRadiusFromPlayer = 15f;

    [Header("Spawn Settings")]
    [SerializeField] private bool passiveSpawn = true;

    [SerializeField] private float spawnInterval = 5f;

    [SerializeField] private int enemiesPerWave = 3;

    [SerializeField] private int maxAliveEnemies = 20;

    [Header("Runtime")]
    [SerializeField] private List<GameObject> aliveEnemies = new();

    private void Start()
    {
        if (passiveSpawn)
        {
            StartCoroutine(PassiveSpawnLoop());
        }
    }

    IEnumerator PassiveSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            CleanupDeadEnemies();

            if (aliveEnemies.Count >= maxAliveEnemies)
                continue;

            SpawnWave(enemiesPerWave);
        }
    }

    public void SpawnWave(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos = GenerateSpawnPosition();

            GameObject prefab =
                enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject enemy =
                Instantiate(prefab, spawnPos, Quaternion.identity);

            aliveEnemies.Add(enemy);

            enemy.GetComponent<EnemyPair>().player = player;
            enemy.transform.parent = transform;
        }
    }

    Vector3 GenerateSpawnPosition()
    {
        const int maxAttempts = 30;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 random2D =
                Random.insideUnitCircle * spawnRadius;

            Vector3 candidate =
                centerPoint.position +
                new Vector3(random2D.x, 0f, random2D.y);

            float distToPlayer =
                Vector3.Distance(candidate, player.position);

            if (distToPlayer >= safeRadiusFromPlayer)
            {
                return candidate;
            }
        }

        // fallback
        return centerPoint.position;
    }

    void CleanupDeadEnemies()
    {
        aliveEnemies.RemoveAll(e => e == null);
    }

    [Header("Debug")]
    [SerializeField] private bool drawDebug = true;

    private void OnDrawGizmosSelected()
    {
        if (!drawDebug)
            return;

        if (centerPoint != null)
        {
            // Radio máximo de spawn
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                centerPoint.position,
                spawnRadius
            );
        }

        if (player != null)
        {
            // Zona segura alrededor del jugador
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                player.position,
                safeRadiusFromPlayer
            );
        }
    }
}
