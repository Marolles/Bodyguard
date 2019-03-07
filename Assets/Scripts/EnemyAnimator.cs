using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    Animator animator;
    GameObject visuals;
    public EnemyBehaviour linkedEnemy;

    private void Start()
    {
        animator = GetComponent<Animator>();
        visuals = transform.Find("Visuals").gameObject;
        linkedEnemy = visuals.GetComponent<EnemyBehaviour>();
        visuals.SetActive(false);
        animator.speed = 0;

        StartCoroutine("Spawn_C");
    }
    public void Spawn()
    {
        animator.enabled = true;
        visuals.SetActive(true);
        visuals.transform.localPosition = Vector3.zero;
        animator.SetTrigger("reset");
        animator.speed = 1;
    }

    public void LinkToSpawner(EnemySpawner spawner)
    {
        linkedEnemy.linkedSpawner = spawner;
    }

    public void EndSpawn()
    {
        animator.enabled = false;
        linkedEnemy.Spawn();
    }

    IEnumerable Spawn_C()
    {
        yield return new WaitForSeconds(Random.Range(0,1));
        Spawn();
        yield return null;
    }
}
