using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Settings")]
    public List<Transform> spawnPoints;
    public GameObject poolParent;
    public float prespawnDistance;

    public float minDistance;
    public float spawnRate; //Entre 0 et 1

    public float passedDistance;
    public float savedDistance;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        passedDistance = 0;
        savedDistance = 0;
        foreach (Transform t in poolParent.transform)
        {
            pool.Add(t.gameObject);
        }
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        transform.position += new Vector3(0, 0, minDistance);
        for (int i = 0; i < prespawnDistance / minDistance; i++)
        {
            transform.position += new Vector3(0,0,minDistance);
            yield return null;
            TrySpawn();
        }
        yield return null;
    }

    void TrySpawn()
    {
        float chance = Random.Range(0f, 1f);
        if (chance < spawnRate)
        {
            SpawnObject();
        }
    }

    void Update()
    {
        passedDistance = GameManager.i.starBehaviour.transform.position.z;
        if (passedDistance > savedDistance + minDistance)
        {
            savedDistance = passedDistance;
            TrySpawn();
        }
    }

    void SpawnObject()
    {
        if (pool.Count > 0)
        {
            GameObject freeObject = pool[0];
            pool.RemoveAt(0);
            pool.Add(freeObject);
            freeObject.transform.position = spawnPoints[Mathf.RoundToInt(Random.Range(0f, spawnPoints.Count - 1))].position;
            if (freeObject.GetComponent<EnemyBehaviour>() != null)
            {
                freeObject.GetComponent<EnemyBehaviour>().Spawn();
            }
        }
    }
}
