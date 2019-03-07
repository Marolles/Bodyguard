using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;
    public GameObject target;

    [HideInInspector]
    public Rigidbody rb;
    public bool alive = true;
    private Collider cd;
    public EnemySpawner linkedSpawner;



    private void Start()
    {
        cd = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        if (target != null && alive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerFound = collision.gameObject.GetComponent<PlayerController>();
        if (playerFound != null && alive)
        {
            Kill();
            rb.AddForce(new Vector3(playerFound.rb.velocity.x * 3000, 15000, playerFound.rb.velocity.z  *3000));
        }
    }

    public void Kill()
    {
        //Add score too
        ForceKill();
        linkedSpawner.AddToPool(this.transform.parent.gameObject);
    }

    public void ForceKill()
    {
        alive = false;
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void Spawn()
    {
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        alive = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
}
