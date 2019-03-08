using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiController : MonoBehaviour
{
    public Sprite emojiFear;
    public Sprite emojiLove;
    public Sprite emojiAngry;

    public GameObject emojiPrefab;

    public Dictionary<GameObject, Transform> activeEmojis = new Dictionary<GameObject, Transform>();

    void Update()
    {
        UpdateEmojiPosition();
    }

    void UpdateEmojiPosition()
    {
        if (activeEmojis.Count <= 0) { return; }
        foreach (KeyValuePair<GameObject, Transform> emoji in activeEmojis)
        {
            Vector3 emojiPosition;
            emojiPosition = Camera.main.WorldToScreenPoint(emoji.Value.position);
            emoji.Key.transform.position = emojiPosition;
        }
    }

    public void GenerateEmoji(Transform parent, Sprite sprite)
    {
        GameObject newEmoji = Instantiate(emojiPrefab, GameManager.i.canvas.transform);
        newEmoji.transform.Find("Visuals").GetComponent<Image>().sprite = sprite;
        activeEmojis.Add(newEmoji, parent);
    }
}
