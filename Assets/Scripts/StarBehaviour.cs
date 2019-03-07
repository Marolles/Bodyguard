using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarBehaviour : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;
    public float minTalkTime;
    public float maxTalkTime;

    [Header("References")]
    public GameObject positionIndicator;
    private GameObject positionIndicatorArrow;
    public float angle;
    [HideInInspector]
    public GameObject visuals;
    private Animator animator;
    private Vector3 defaultPos;

    [Header("Emojis")]
    public GameObject emojiPrefab;

    [Space(10)]
    public Sprite emojiLove;
    public Sprite emojiFear;
    public Sprite emojiLike;

    public Vector3 movement;
    private Camera camera;
    private Vector3 lastPositionOnViewport;
    private float indicatorBounds;
    private Vector2 screenSize;
    private List<GameObject> activeEmoji;
    private bool talking;
    private float talkingCD;

    private bool isEnabled;

    private StarPoint actualStarPoint;
    public StarCollider starCollider;

    private void Start()
    {
        defaultPos = transform.position;
        visuals = transform.Find("Visuals").gameObject;
        starCollider = visuals.GetComponent<StarCollider>();
        activeEmoji = new List<GameObject>();
        camera = Camera.main;
        positionIndicatorArrow = positionIndicator.transform.Find("Arrow").gameObject;
        animator = visuals.GetComponent<Animator>();
        animator.speed = 0;
    }

    public void ToggleStar(bool b)
    {
        isEnabled = b;
        if (b == false) { ResetStar(); }
    }

    void ResetStar()
    {
        lastPositionOnViewport = Vector3.zero;
        animator.SetBool("talking", false);
        actualStarPoint = null;
        talking = false;
        talkingCD = 0;
        transform.position = defaultPos;
        visuals.transform.position = transform.position;
        visuals.transform.rotation = Quaternion.identity;
        animator.speed = 1;
    }

    private void Update()
    {
        if (!isEnabled) { return; }
        if (actualStarPoint == null)
        {
            DefaultMovement();
        } else
        {
            PointMovement();
        }

        UpdatePositionIndicator();
        indicatorBounds = positionIndicator.GetComponent<RectTransform>().sizeDelta.x / 2;
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;

        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateEmoji(emojiLove);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateEmoji(emojiFear);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            GenerateEmoji(emojiLike);
        }
        UpdateEmojiPosition();
        UpdateTalkingCD();
    }

    void DefaultMovement()
    {
        if (talkingCD > 0) { return; }
        movement = new Vector3(0, 0, 0.01f * moveSpeed);
        transform.position += movement;
        visuals.transform.position = Vector3.Lerp(visuals.transform.position, transform.position, Time.deltaTime * moveSpeed * 0.1f);
        visuals.transform.rotation = Quaternion.Lerp(visuals.transform.rotation, Quaternion.identity, Time.deltaTime);
    }

    void PointMovement()
    {
        movement = new Vector3(0, 0, 0);
        if (Vector3.Distance(visuals.transform.position, actualStarPoint.transform.position) > 1.5f)
        {
            Vector3 pointPosition = actualStarPoint.transform.position;
            pointPosition.y = visuals.transform.position.y;
            visuals.transform.position = Vector3.MoveTowards(visuals.transform.position, pointPosition, Time.deltaTime * moveSpeed * 0.4f);
            visuals.transform.LookAt(pointPosition);
        } else
        {
            actualStarPoint = null;
            talkingCD = Random.Range(minTalkTime, maxTalkTime);
        }
    }

    void UpdateTalkingCD()
    {
        if (talkingCD > 0)
        {
            animator.SetBool("talking", true);
            talkingCD -= Time.deltaTime;
        } else
        {
            talkingCD = 0;
            animator.SetBool("talking", false);
        }
    }

    public void TriggerEntered(Collider other)
    {
        StarPoint potentialStarpoint = other.GetComponent<StarPoint>();
        if (potentialStarpoint != null)
        {
            actualStarPoint = potentialStarpoint;
        }
        if (other.tag == "FinishLine")
        {
            GameManager.i.WinGame();
        }
    }

    public void CollisionEntered(Collision other)
    {
        EnemyBehaviour potentialEnemy = other.gameObject.GetComponent<EnemyBehaviour>();
        if (other.gameObject.tag == "Enemy" && potentialEnemy != null && potentialEnemy.alive)
        {
            GameManager.i.LoseGame();
        }
    }

    void UpdateEmojiPosition()
    {
        if (activeEmoji.Count <= 0) { return; }
        Vector3 emojiPosition;
        if (!IsSeenByCamera(gameObject))
        {
            emojiPosition = positionIndicator.transform.position;
        } else
        {
            emojiPosition = camera.WorldToScreenPoint(visuals.transform.position);
        }
        foreach (GameObject emoji in activeEmoji)
        {
            emoji.transform.position = emojiPosition;
        }
    }

    public void GenerateEmoji(Sprite sprite)
    {
        GameObject newEmoji = Instantiate(emojiPrefab, GameManager.i.canvas.transform);
        newEmoji.transform.Find("Visuals").GetComponent<Image>().sprite = sprite;
        activeEmoji.Add(newEmoji);
    }

    public Vector3 GetMovement()
    {
        return movement;
    }

    public Vector3 GetViewPortPosition(Transform position)
    {
        Vector3 result = camera.WorldToViewportPoint(position.position);
        return result;
    }

    public Vector3 GetScreenPosition(Transform position)
    {
        Vector3 result = camera.WorldToScreenPoint(position.position);
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
            if (indicatorPosition.z > 0)
            {
                indicatorPosition.x = posX;
                indicatorPosition.y = posY;
            } else
            {
                indicatorPosition.x = posX;
                indicatorPosition.y = positionIndicator.transform.position.y;
            }
            positionIndicator.transform.position = indicatorPosition;
        }
        else
        {
            positionIndicator.SetActive(false);
        }
    }
}
