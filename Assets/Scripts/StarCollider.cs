using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollider : MonoBehaviour
{
    StarBehaviour star;
    public Rigidbody rb;

    private void Start()
    {
        star = transform.parent.GetComponent<StarBehaviour>();
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        star.TriggerEntered(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        star.CollisionEntered(other);
    }
}
