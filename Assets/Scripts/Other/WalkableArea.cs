using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableArea : MonoBehaviour
{
    private Collider objCollider;

    private void Start()
    {
        objCollider = GetComponent<Collider>();    
    }

    //Check if the object is still in the walkable area
    public bool IsInWalkableZone(GameObject obj)
    {
        if (objCollider.bounds.Contains(obj.transform.position))
        {
            return true;
        } else
        {
            return false;
        }
    }

    //Returs the closest point in the walkable area
    public Vector3 GetClosestPoint(Vector3 position)
    {
        Vector3 closestPoint = objCollider.bounds.ClosestPoint(position);
        closestPoint.y = position.y;
        return closestPoint;
    }

    public void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.i.starBehaviour.transform.position.z);
    }
}
