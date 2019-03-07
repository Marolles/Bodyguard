using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 8f;
    public float timeToRun = 2f;

    [Space(10)]
    public float rotationSpeed;

    [Space(10)]
    public float maxFootstepParticles;

    [Space(10)]
    public float maxAnimatorSpeed;


    [Space(5)]
    [Header("References")]
    public Joystick joystick;

    public GameObject highlightCircle;
    public GameObject highlightArrow;

    public ParticleSystem footstepFX;
    public Animator animator;

    [HideInInspector]
    public Rigidbody rb;
    private StarBehaviour star;
    private WalkableArea walkableArea;
    private ParticleSystem.EmissionModule footStepEmissionModule;
    private float sprintCD = 0;
    private float actualSpeed;

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
        highlightArrow.SetActive(false);
        highlightCircle.SetActive(false);

        footStepEmissionModule = footstepFX.emission;
    }

    void Update()
    {
        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);

        if (moveVector != Vector3.zero)
        {
            sprintCD += Time.deltaTime;
            highlightArrow.SetActive(true);
            transform.rotation = Quaternion.LookRotation(moveVector);
            transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);

            if (!walkableArea.IsInWalkableZone(this.gameObject))
            {
                transform.position = walkableArea.GetClosestPoint(transform.position);
            }
        } else
        {
            transform.position += star.GetMovement();
            highlightArrow.SetActive(false);
            sprintCD = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), Time.deltaTime * rotationSpeed);
        }

        actualSpeed = Mathf.Clamp(moveVector.magnitude * maxAnimatorSpeed, 0, maxAnimatorSpeed);
        if (GameManager.i.starBehaviour.GetMovement().z > 0) {
            actualSpeed += 1;
        }
        footStepEmissionModule.rateOverTimeMultiplier = Mathf.Clamp(actualSpeed * maxFootstepParticles, 0, maxFootstepParticles);
        animator.SetFloat("runSpeed", actualSpeed);
    }

    public void ShowHighlightCircle()
    {
        highlightCircle.SetActive(true);
    }

    public void HideHighlightCircle()
    {
        highlightCircle.SetActive(false);
    }
}
