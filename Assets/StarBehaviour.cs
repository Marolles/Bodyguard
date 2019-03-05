using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehaviour : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;

    [Header("References")]
    public GameObject positionIndicator;
    private GameObject positionIndicatorArrow;
    public float angle;

    private Vector3 movement;
    private Camera camera;
    private Vector3 lastPositionOnViewport;
    public float indicatorBounds;
    public Vector2 screenSize;

    private void Start()
    {
        camera = Camera.main;
        positionIndicatorArrow = positionIndicator.transform.Find("Arrow").gameObject;
    }

    private void Update()
    {
        movement = new Vector3(0, 0, 0.01f * moveSpeed);
        transform.position += movement;
        UpdatePositionIndicator();
        indicatorBounds = positionIndicator.GetComponent<RectTransform>().sizeDelta.x / 2;
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;
    }

    public Vector3 GetMovement()
    {
        return movement;
    }

    public Vector3 GetViewPortPosition(Transform position)
    {
        return camera.WorldToViewportPoint(transform.position);
    }

    public Vector3 GetScreenPosition(Transform position)
    {
        Vector3 result = camera.WorldToScreenPoint(transform.position);
        result.z = 0;
        return result;
    }

    public bool IsSeenByCamera(GameObject obj)
    {
        Vector3 viewPortPosition = GetViewPortPosition(obj.transform);
        if (viewPortPosition.x > 0 && viewPortPosition.x < 1 && viewPortPosition.y > 0 && viewPortPosition.y < 1)
        {
            return true;
        }
        return false;
    }

    void UpdatePositionIndicator()
    {
        if (positionIndicator != null && !IsSeenByCamera(gameObject))
        {
            positionIndicator.SetActive(true);
            positionIndicatorArrow.transform.LookAt(GetScreenPosition(transform), new Vector3(1, 0, 0));
            Vector3 indicatorPosition = GetScreenPosition(transform);
            float posX = Mathf.Clamp(indicatorPosition.x, indicatorBounds, Screen.width - indicatorBounds);
            float posY = Mathf.Clamp(indicatorPosition.y, indicatorBounds, Screen.height - indicatorBounds);
            indicatorPosition.x = posX;
            indicatorPosition.y = posY;
            positionIndicator.transform.position = indicatorPosition;
        }
        else
        {
            positionIndicator.SetActive(false);
        }
    }
}
