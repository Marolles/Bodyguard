using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [Header("Settings")]
    public float moveSpeed = 8f;

    [Space(5)]
    [Header("References")]
    public Joystick joystick;

    public GameObject HighlightCircle;
    public GameObject HighlightArrow;


    private WalkableArea walkableArea;

    private void Start()
    {
        walkableArea = FindObjectOfType<WalkableArea>();
        if (walkableArea == null)
        {
            Debug.LogWarning("ERROR : No movable zone found, please add one");
        }
        HighlightArrow.SetActive(false);
        HighlightCircle.SetActive(false);
    }

    void Update()
    {
        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);

        if (moveVector != Vector3.zero)
        {
            HighlightArrow.SetActive(true);
            transform.rotation = Quaternion.LookRotation(moveVector);
            transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);

            if (!walkableArea.IsInWalkableZone(this.gameObject))
            {
                transform.position = walkableArea.GetClosestPoint(transform.position);
            }
        } else
        {
            HighlightArrow.SetActive(false);
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("hy");
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        HighlightCircle.SetActive(true);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        HighlightCircle.SetActive(false);
    }
}
