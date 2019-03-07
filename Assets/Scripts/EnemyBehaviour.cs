using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;
    public GameObject target;
    public EnemyType enemyType;

    [HideInInspector]
    public Rigidbody rb;
    public Rigidbody ragdollrb;
    public bool alive = true;
    private Collider cd;
    public EnemySpawner linkedSpawner;

    public GameObject[] bodyPartObjects;
    public Vector3[] bodyPartDefaultPosition;
    public Quaternion[] bodyPartDefaultRotation;



    private void Start()
    {
        cd = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        GetBodyPositions();
    }

    private void Update()
    {
        if (target != null && alive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
            transform.LookAt(target.transform.position);
            if (transform.position.y < -1000)
            {
                ForceKill();
                AddToPool(this.transform.parent.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerFound = collision.gameObject.GetComponent<PlayerController>();
        if (playerFound != null && alive)
        {
            Kill();
            rb.AddForce(new Vector3(0, 5000, 0));
            rb.AddForce(new Vector3(playerFound.moveVector.x * 10000, 2000, playerFound.moveVector.z  *10000));
            GameObject hitParticle = Instantiate(GameManager.i.playerController.hitFXPrefab);
            Vector3 hitPosition = collision.transform.position;
            hitPosition.y = transform.position.y;
            hitParticle.transform.position = hitPosition;
        }
    }

    private void GetBodyPositions()
    {
        bodyPartDefaultPosition = new Vector3[bodyPartObjects.Length];
        bodyPartDefaultRotation = new Quaternion[bodyPartObjects.Length];
        for (int i = 0; i < bodyPartObjects.Length; i++)
        {
            bodyPartDefaultRotation[i] = bodyPartObjects[i].transform.localRotation;
            bodyPartDefaultPosition[i] = bodyPartObjects[i].transform.localPosition;
        }
    }

    private void ResetBodyPositions()
    {
        if (bodyPartObjects == null || bodyPartDefaultPosition == null || bodyPartDefaultRotation == null) { return; }
        for (int i = 0; i < bodyPartObjects.Length; i++)
        {
            bodyPartObjects[i].transform.localPosition = bodyPartDefaultPosition[i];
            bodyPartObjects[i].transform.localRotation = bodyPartDefaultRotation[i];
        }
    }

    public void Kill()
    {
        GameManager.i.playerController.AddCombo();
        switch (enemyType)
        {
            case EnemyType.interviewer:
                GameManager.i.interviewerKilled++;
                break;
            case EnemyType.paparazzi:
                GameManager.i.paparazziKilled++;
                break;
        }
        ForceKill();
        AddToPool(this.transform.parent.gameObject);
        SetRagdoll();
    }

    public void ForceKill()
    {
        alive = false;
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void AddToPool(GameObject obj)
    {
        if (linkedSpawner != null)
        {
            linkedSpawner.AddToPool(obj);
        }
    }

    void SetRagdoll()
    {
        List<Rigidbody> bodies = new List<Rigidbody>();
        foreach (GameObject obj in bodyPartObjects)
        {
            obj.layer = LayerMask.NameToLayer("Ragdoll");
            if (obj.GetComponent<Rigidbody>() != null)
            {
                obj.GetComponent<Rigidbody>().isKinematic = false;
                obj.GetComponent<Rigidbody>().GetComponent<Collider>().enabled = true;
            }
        }
        GetComponent<Collider>().enabled = false;
    }

    void UnsetRagdoll()
    {
        List<Rigidbody> bodies = new List<Rigidbody>();
        foreach (GameObject obj in bodyPartObjects)
        {
            if (obj.GetComponent<Rigidbody>() != null)
            {
                obj.GetComponent<Rigidbody>().isKinematic = true;
                obj.GetComponent<Rigidbody>().GetComponent<Collider>().enabled = false;
            }
        }
        GetComponent<Collider>().enabled = true;
        ResetBodyPositions();
    }

    public void PreSpawn()
    {
        UnsetRagdoll();
        target = null;
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Spawn()
    {
        target = GameManager.i.starBehaviour.visuals;
        rb.isKinematic = false;
        transform.rotation = Quaternion.identity;
        alive = true;
        UnsetRagdoll();
    }
}
