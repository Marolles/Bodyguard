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
    private bool alive = true;
    private Collider cd;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<Collider>();
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
        alive = false;
        rb.constraints = RigidbodyConstraints.None;
      // cd.enabled = false;
    }
}
