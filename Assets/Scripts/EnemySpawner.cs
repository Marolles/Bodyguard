using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPool;
    public List<GameObject> spawners;

    [Space(20)]
    [Header("Settings")]

    [Range(0f,1f)]
    public float spawnRate;


    private float cd;
    private List<GameObject> instantiatedObjects;
    public List<GameObject> pool;

    private bool spawnEnabled;

    private void Start()
    {
        pool = new List<GameObject>();
        foreach (Transform t in enemyPool.transform)
        {
            pool.Add(t.gameObject);
        }
        cd = 3;
    }

    public void ToggleSpawn(bool b)
    {
        spawnEnabled = b;
        if (b == false)
        {
            ResetEnemies();
        }
    }

    private void ResetEnemies()
    {
        pool.Clear();
        foreach (EnemyAnimator enemyAnimator in FindObjectsOfType<EnemyAnimator>())
        {
            enemyAnimator.linkedEnemy.ForceKill();
            enemyAnimator.transform.position = enemyPool.transform.position;
            pool.Add(enemyAnimator.gameObject);
        }
    }

    public void AddToPool(GameObject gameObject)
    {
        pool.Add(gameObject);
    }

    private void Update()
    {
        if (!spawnEnabled) { return; }
        if (cd > 0)
        {
            cd -= Time.deltaTime;
        } else
        {
            cd = spawnRate * 3;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (pool.Count <= 0) { return; }
        GameObject spawnedObject = pool[0];
        pool.RemoveAt(0);
        int spawnerIndex = Mathf.RoundToInt(Random.Range(0f, spawners.Count-1));
        spawnedObject.transform.position = spawners[spawnerIndex].transform.position;
        spawnedObject.transform.rotation = spawners[spawnerIndex].transform.rotation;

        EnemyAnimator enemyAnimator = spawnedObject.GetComponent<EnemyAnimator>();
        enemyAnimator.Spawn();
        enemyAnimator.LinkToSpawner(this);
    }
}
