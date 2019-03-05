using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public float cameraSpeed;


    public GameObject target;
    


    private void Update()
    {
        if (target != null)
        {
            Vector3 cameraMovement = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime* cameraSpeed);
           // cameraMovement.x = transform.position.x;
            cameraMovement.y = transform.position.y;
            transform.position = cameraMovement;
        }
    }
}
