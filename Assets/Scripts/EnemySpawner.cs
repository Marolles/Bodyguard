using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { paparazzi, interviewer }
public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPool;
    public List<GameObject> spawners;

    [Space(20)]
    [Header("Settings")]


    public float cd;
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
        cd = GameManager.i.timeBeforeFirstEnemy;
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
        cd = GameManager.i.timeBeforeFirstEnemy;
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
            cd = Random.Range(GameManager.i.enemyspawnRate*0.8f, GameManager.i.enemyspawnRate * 1.2f);
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
        enemyAnimator.linkedEnemy.PreSpawn();
        enemyAnimator.LinkToSpawner(this);
    }
}
