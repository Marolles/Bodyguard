using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public float cameraSpeed;


    public GameObject target;
    public StarBehaviour star;

    public float maxDistance;

    public bool isSeen = false;

    private Camera camera;


    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (target == null) { return; }

        if (star.transform.position.z - this.transform.position.z < -maxDistance && target.transform.position.z > transform.position.z)
        {
            Vector3 clampedMovement = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * cameraSpeed);
            clampedMovement.z = transform.position.z + star.GetMovement().z;
            clampedMovement.y = transform.position.y;
            transform.position = clampedMovement;
        }
        else if (star.transform.position.z - this.transform.position.z > maxDistance && target.transform.position.z < transform.position.z)
        {
            Vector3 clampedMovement = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * cameraSpeed);
            clampedMovement.z = transform.position.z + star.GetMovement().z;
            clampedMovement.y = transform.position.y;
            transform.position = clampedMovement;
        }
        else
        {
            Vector3 cameraMovement = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * cameraSpeed);
            cameraMovement.y = transform.position.y;
            transform.position = cameraMovement;
        }
    }

    public bool IsSeenByCamera(GameObject obj)
    {
        Vector3 viewPortPosition = GetViewPortPosition(obj.transform);
        Debug.Log(viewPortPosition);
        if (viewPortPosition.x > 0 && viewPortPosition.x < 1 && viewPortPosition.y > 0 && viewPortPosition.y < 1)
        {
            return true;
        }
        return false;
    }

    public Vector3 GetViewPortPosition(Transform position)
    {
        return camera.WorldToViewportPoint(position.position);
    }
}
