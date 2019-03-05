using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    Vector2 joystickCenter = Vector2.zero;
    public GameObject highlightCircle;
    public bool useLerp = true;
    public float lerpTime = 2;
    private bool pointerDown;

    private float lerpCoef;
    private float lerp;

    void Start()
    {
        background.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (pointerDown)
        {
            if (lerp < lerpTime)
            {
                lerp += Time.deltaTime;
            } else
            {
                lerp = lerpTime;
            }
            lerpCoef = lerp / lerpTime;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        inputVector = Vector3.Normalize(inputVector);
        if (useLerp)
        {
            inputVector *= lerpCoef;
        }
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        highlightCircle.SetActive(true);
        background.gameObject.SetActive(true);
        background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        joystickCenter = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        highlightCircle.SetActive(false);
        background.gameObject.SetActive(false);
        inputVector = Vector2.zero;
        lerp = 0;
    }
}