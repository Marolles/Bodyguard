using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 8f;
    public float timeToRun = 2f;

    public float comboTime = 3f;
    public int comboForExplosion = 3;
    public int maxCombo = 3;

    public float explosionRadius = 10;
    public float explosionForce = 1000;

    private Vector3 defaultPosition;

    [Space(10)]
    public float rotationSpeed;

    [Space(10)]
    public float maxFootstepParticles;

    [Space(10)]
    public float maxAnimatorSpeed;

    [Space(10)]
    public float minPowerFXEmission;
    public float maxPowerFXEmission;


    [Space(5)]
    [Header("References")]
    public GameObject explosionFXPrefab;

    public GameObject hitFXPrefab;
    public Joystick joystick;

    public GameObject highlightCircle;
    public GameObject highlightArrow;

    public ParticleSystem footstepFX;
    public ParticleSystem powerFX;

    public Animator animator;

    public Vector3 moveVector;
    public bool canMove;

    [HideInInspector]
    public Rigidbody rb;
    private StarBehaviour star;
    private WalkableArea walkableArea;
    private ParticleSystem.EmissionModule footStepEmissionModule;
    private ParticleSystem.EmissionModule powerFXEmissionModule;
    private float sprintCD = 0;
    private float actualSpeed;

    public int comboCount;
    private float comboCD;

    private void Start()
    {
        defaultPosition = transform.position;
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
        powerFXEmissionModule = powerFX.emission;
    }

    void Update()
    {
        if (canMove)
        {
            moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);
        }
        UpdateComboCD();
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

    public void ResetPlayer()
    {
        powerFXEmissionModule = powerFX.emission;
        ResetCombo();
        transform.position = defaultPosition;
    }

    public void AddCombo()
    {
        comboCount++;
        comboCD = comboTime;
        if (comboCount == comboForExplosion)
        {
            Explosion();
            ResetCombo();
        }
        powerFXEmissionModule.rateOverTimeMultiplier = Mathf.Lerp(minPowerFXEmission, maxPowerFXEmission, (float)comboCount / (float)maxCombo);
    }

    void ResetCombo()
    {
        powerFXEmissionModule.rateOverTimeMultiplier = 0;
        comboCD = 0;
        comboCount = 0;
    }

    public void Explosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rbFound = hit.GetComponent<Rigidbody>();

            if (rbFound != null && rbFound != rb && rbFound != GameManager.i.starBehaviour.starCollider.rb)
            {
                rbFound.AddExplosionForce(explosionForce, explosionPos + new Vector3(0,-1,0), explosionRadius, 50f);
                EnemyBehaviour potentialEnemy = rbFound.gameObject.GetComponent<EnemyBehaviour>();
                if (potentialEnemy != null)
                {
                    potentialEnemy.Kill();
                }
            }
        }
        GameObject explosionFX = Instantiate(explosionFXPrefab);
        explosionFX.transform.position = this.transform.position + new Vector3(0,1,0);
        GameManager.i.starBehaviour.GenerateEmoji(GameManager.i.starBehaviour.emojiLove);
    }

    private void UpdateComboCD()
    {
        if (comboCD >0)
        {
            comboCD -= Time.deltaTime;
        } else
        {
            ResetCombo();
        }
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
