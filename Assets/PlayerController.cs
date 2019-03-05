using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 8f;
    public float timeToRun = 2f;

    [Space(5)]
    [Header("References")]
    public Joystick joystick;

    public GameObject HighlightCircle;
    public GameObject HighlightArrow;

    [HideInInspector]
    public Rigidbody rb;
    private StarBehaviour star;
    private WalkableArea walkableArea;
    private float sprintCD = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        walkableArea = FindObjectOfType<WalkableArea>();
        if (walkableArea == null)
        {
            Debug.LogWarning("ERROR : No movable zone found, please add one");
        }
        star = FindObjectOfType<StarBehaviour>();
        if (star == null)
        {
            Debug.LogWarning("ERROR : Star not found, please add one");
        }
        HighlightArrow.SetActive(false);
        HighlightCircle.SetActive(false);
    }

    void Update()
    {
        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);

        if (moveVector != Vector3.zero)
        {
            sprintCD += Time.deltaTime;
            HighlightArrow.SetActive(true);
            transform.rotation = Quaternion.LookRotation(moveVector);
            transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);

            if (!walkableArea.IsInWalkableZone(this.gameObject))
            {
                transform.position = walkableArea.GetClosestPoint(transform.position);
            }
        } else
        {
            transform.position += star.GetMovement();
            HighlightArrow.SetActive(false);
            sprintCD = 0;
        }
    }

    public void ShowHighlightCircle()
    {
        HighlightCircle.SetActive(true);
    }

    public void HideHighlightCircle()
    {
        HighlightCircle.SetActive(false);
    }
}
